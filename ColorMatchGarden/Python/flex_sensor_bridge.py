"""
Flex Sensor Bridge for Color Match Garden
Reads flex sensor values and sends to Unity via UDP
"""

import serial
import socket
import time
import sys

# Configuration
SERIAL_PORT = "COM3"  # Change to your port
BAUD_RATE = 115200
UNITY_HOST = "127.0.0.1"
UNITY_PORT = 5000

# Calibration (adjust based on your sensor)
FLAT_VALUE = 45000
BENT_VALUE = 20000

def normalize_flex_value(raw_value):
    """Convert raw ADC value to 0-100 percentage"""
    normalized = (FLAT_VALUE - raw_value) / (FLAT_VALUE - BENT_VALUE)
    return max(0, min(100, normalized * 100))

def main():
    print("=" * 50)
    print("  Color Match Garden - Flex Sensor Bridge")
    print("=" * 50)
    
    # Setup UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    print(f"[UDP] Sending to {UNITY_HOST}:{UNITY_PORT}")
    
    try:
        ser = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=1)
        print(f"[Serial] Connected to {SERIAL_PORT}")
    except serial.SerialException as e:
        print(f"[Error] Cannot open {SERIAL_PORT}: {e}")
        print("[Info] Running in simulation mode...")
        run_simulation(sock)
        return
    
    print("\n[Ready] Sending flex sensor data to Unity")
    print("        Bend the sensor to change flower brightness!\n")
    
    try:
        while True:
            if ser.in_waiting:
                line = ser.readline().decode('utf-8').strip()
                try:
                    raw_value = float(line)
                    percentage = normalize_flex_value(raw_value)
                    
                    # Send to Unity
                    sock.sendto(str(percentage).encode(), (UNITY_HOST, UNITY_PORT))
                    
                    # Visual feedback
                    level = "Light" if percentage <= 30 else "Medium" if percentage <= 70 else "Bright"
                    bar = "█" * int(percentage / 5) + "░" * (20 - int(percentage / 5))
                    print(f"\r[{bar}] {percentage:5.1f}% ({level})  ", end="")
                    
                except ValueError:
                    pass
            time.sleep(0.05)
    except KeyboardInterrupt:
        print("\n\n[Stopped] Flex sensor bridge closed")
    finally:
        ser.close()
        sock.close()

def run_simulation(sock):
    """Simulate flex sensor for testing without hardware"""
    print("\n[Simulation] Use UP/DOWN arrows or 1/2/3 keys in Unity")
    print("             Press Ctrl+C to stop\n")
    
    import math
    value = 50
    
    try:
        while True:
            # Gentle wave simulation
            value = 50 + 40 * math.sin(time.time() * 0.3)
            sock.sendto(str(value).encode(), (UNITY_HOST, UNITY_PORT))
            
            level = "Light" if value <= 30 else "Medium" if value <= 70 else "Bright"
            bar = "█" * int(value / 5) + "░" * (20 - int(value / 5))
            print(f"\r[{bar}] {value:5.1f}% ({level})  ", end="")
            
            time.sleep(0.1)
    except KeyboardInterrupt:
        print("\n\n[Stopped] Simulation closed")
    finally:
        sock.close()

if __name__ == "__main__":
    main()
