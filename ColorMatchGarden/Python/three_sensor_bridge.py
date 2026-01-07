"""
3 Flex Sensors to Unity Bridge for Color Match Garden
=====================================================
Reads 3 flex sensors (RGB) from Raspberry Pi Pico and sends to Unity

WIRING (on Pico):
- Sensor 1 (RED):   GP26 (ADC0)
- Sensor 2 (GREEN): GP27 (ADC1)  
- Sensor 3 (BLUE):  GP28 (ADC2)

Run this on your COMPUTER (not Pico), reads serial from Pico
"""

import serial
import socket
import time
import sys

# ============ CONFIGURATION ============
SERIAL_PORT = "COM5"      # Your Pico's COM port
BAUD_RATE = 115200
UNITY_HOST = "127.0.0.1"
UNITY_PORT = 5005         # Must match ThreeSensorInput.cs

# Calibration values (adjust based on YOUR sensors)
FLAT_VALUE = 50000        # ADC value when sensor is flat
BENT_VALUE = 20000        # ADC value when sensor is fully bent
# =======================================

def normalize_value(raw):
    """Convert raw ADC (0-65535) to 0.0-1.0"""
    normalized = (FLAT_VALUE - raw) / (FLAT_VALUE - BENT_VALUE)
    return max(0.0, min(1.0, normalized))

def main():
    print("=" * 55)
    print("  🌸 Color Match Garden - 3 Sensor RGB Bridge 🌸")
    print("=" * 55)
    print(f"  Serial Port: {SERIAL_PORT}")
    print(f"  Unity Target: {UNITY_HOST}:{UNITY_PORT}")
    print("=" * 55)
    
    # Setup UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    
    try:
        ser = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=1)
        print(f"\n✅ Connected to {SERIAL_PORT}")
    except serial.SerialException as e:
        print(f"\n❌ Cannot open {SERIAL_PORT}: {e}")
        print("\n💡 Make sure:")
        print("   1. Pico is connected via USB")
        print("   2. Correct COM port is set")
        print("   3. Thonny is NOT connected to the same port")
        print("\nStarting SIMULATION mode instead...")
        run_simulation(sock)
        return
    
    print("\n🎮 Sending RGB sensor data to Unity!")
    print("   Bend sensors to mix colors!\n")
    
    try:
        while True:
            if ser.in_waiting:
                line = ser.readline().decode('utf-8').strip()
                try:
                    # Expected format from Pico: "R:12345,G:23456,B:34567"
                    # Or just: "12345,23456,34567"
                    
                    if "," in line:
                        parts = line.replace("R:", "").replace("G:", "").replace("B:", "").split(",")
                        
                        if len(parts) >= 3:
                            raw_r = float(parts[0])
                            raw_g = float(parts[1])
                            raw_b = float(parts[2])
                            
                            # Normalize to 0-1
                            r = normalize_value(raw_r)
                            g = normalize_value(raw_g)
                            b = normalize_value(raw_b)
                            
                            # Send to Unity: "R:0.5,G:0.3,B:0.8"
                            message = f"R:{r:.2f},G:{g:.2f},B:{b:.2f}"
                            sock.sendto(message.encode(), (UNITY_HOST, UNITY_PORT))
                            
                            # Visual display
                            print(f"\r🔴 {r:.0%} 🟢 {g:.0%} 🔵 {b:.0%}  ", end="")
                    
                except ValueError as e:
                    pass  # Ignore parse errors
                    
            time.sleep(0.05)
            
    except KeyboardInterrupt:
        print("\n\n👋 Bridge stopped")
    finally:
        ser.close()
        sock.close()

def run_simulation(sock):
    """Simulate 3 sensors for testing without hardware"""
    import math
    
    print("\n🎮 SIMULATION MODE")
    print("   Sending fake sensor values to Unity")
    print("   Press Ctrl+C to stop\n")
    
    try:
        t = 0
        while True:
            # Simulate varying values
            r = 0.5 + 0.5 * math.sin(t * 0.5)
            g = 0.5 + 0.5 * math.sin(t * 0.7 + 1)
            b = 0.5 + 0.5 * math.sin(t * 0.3 + 2)
            
            message = f"R:{r:.2f},G:{g:.2f},B:{b:.2f}"
            sock.sendto(message.encode(), (UNITY_HOST, UNITY_PORT))
            
            print(f"\r🔴 {r:.0%} 🟢 {g:.0%} 🔵 {b:.0%}  ", end="")
            
            t += 0.1
            time.sleep(0.1)
            
    except KeyboardInterrupt:
        print("\n\n👋 Simulation stopped")
    finally:
        sock.close()

if __name__ == "__main__":
    main()
