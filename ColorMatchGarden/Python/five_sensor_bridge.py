"""
5 Flex Sensors to Unity Bridge for Color Match Garden
======================================================
Reads 5 flex sensors from Raspberry Pi Pico (via multiplexer) and sends to Unity

FINGER MAPPING:
- Thumb  -> RED
- Index  -> GREEN
- Middle -> BLUE
- Ring   -> BRIGHTNESS
- Pinky  -> MAGIC/SPARKLE

Run this on your COMPUTER (not Pico), reads serial from Pico
"""

import serial
import socket
import time
import sys
import json
import math

# ============ CONFIGURATION ============
SERIAL_PORT = "COM5"      # Your Pico's COM port
BAUD_RATE = 115200
UNITY_HOST = "127.0.0.1"
UNITY_PORT = 5006         # Must match FiveSensorInput.cs
# =======================================

def main():
    print("=" * 60)
    print("  🖐️  Color Match Garden - 5 Finger Sensor Bridge 🖐️")
    print("=" * 60)
    print(f"  Serial Port: {SERIAL_PORT}")
    print(f"  Unity Target: {UNITY_HOST}:{UNITY_PORT}")
    print("=" * 60)
    print("\n  Finger Mapping:")
    print("  👍 Thumb  = 🔴 RED")
    print("  👆 Index  = 🟢 GREEN")
    print("  🖕 Middle = 🔵 BLUE")
    print("  💍 Ring   = 🌟 BRIGHTNESS")
    print("  🤙 Pinky  = ✨ MAGIC")
    print("=" * 60)
    
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
        print("\n🎮 Starting SIMULATION mode instead...")
        run_simulation(sock)
        return
    
    print("\n🎮 Sending 5-finger sensor data to Unity!")
    print("   Bend your fingers to mix colors!\n")
    
    try:
        while True:
            if ser.in_waiting:
                line = ser.readline().decode('utf-8').strip()
                try:
                    # Try JSON format first: {"thumb": 0.5, "index": 0.3, ...}
                    if line.startswith('{'):
                        data = json.loads(line)
                        thumb = data.get('thumb', {}).get('percent', 0) / 100
                        index = data.get('index', {}).get('percent', 0) / 100
                        middle = data.get('middle', {}).get('percent', 0) / 100
                        ring = data.get('ring', {}).get('percent', 0) / 100
                        pinky = data.get('pinky', {}).get('percent', 0) / 100
                    
                    # Or simple CSV format: 50.0,30.0,80.0,20.0,10.0 (percentages)
                    elif "," in line:
                        parts = line.split(",")
                        if len(parts) >= 5:
                            thumb = float(parts[0]) / 100
                            index = float(parts[1]) / 100
                            middle = float(parts[2]) / 100
                            ring = float(parts[3]) / 100
                            pinky = float(parts[4]) / 100
                        else:
                            continue
                    else:
                        continue
                    
                    # Clamp values
                    thumb = max(0.0, min(1.0, thumb))
                    index = max(0.0, min(1.0, index))
                    middle = max(0.0, min(1.0, middle))
                    ring = max(0.0, min(1.0, ring))
                    pinky = max(0.0, min(1.0, pinky))
                    
                    # Send to Unity: "T:0.5,I:0.3,M:0.8,R:0.2,P:0.1"
                    message = f"T:{thumb:.2f},I:{index:.2f},M:{middle:.2f},R:{ring:.2f},P:{pinky:.2f}"
                    sock.sendto(message.encode(), (UNITY_HOST, UNITY_PORT))
                    
                    # Visual display
                    print(f"\r👍{thumb:.0%} 👆{index:.0%} 🖕{middle:.0%} 💍{ring:.0%} 🤙{pinky:.0%}  ", end="")
                    
                except (ValueError, json.JSONDecodeError) as e:
                    pass  # Ignore parse errors
                    
            time.sleep(0.05)
            
    except KeyboardInterrupt:
        print("\n\n👋 Bridge stopped")
    finally:
        ser.close()
        sock.close()


def run_simulation(sock):
    """Simulate 5 sensors for testing without hardware"""
    print("\n🎮 SIMULATION MODE - 5 Fingers")
    print("   Sending animated sensor values to Unity")
    print("   Press Ctrl+C to stop\n")
    
    try:
        t = 0
        while True:
            # Simulate varying values with different frequencies
            thumb = 0.5 + 0.5 * math.sin(t * 0.5)          # RED
            index = 0.5 + 0.5 * math.sin(t * 0.7 + 1)      # GREEN
            middle = 0.5 + 0.5 * math.sin(t * 0.3 + 2)     # BLUE
            ring = 0.5 + 0.3 * math.sin(t * 0.2 + 3)       # BRIGHTNESS
            pinky = 0.5 + 0.4 * math.sin(t * 0.9 + 4)      # MAGIC
            
            message = f"T:{thumb:.2f},I:{index:.2f},M:{middle:.2f},R:{ring:.2f},P:{pinky:.2f}"
            sock.sendto(message.encode(), (UNITY_HOST, UNITY_PORT))
            
            print(f"\r👍{thumb:.0%} 👆{index:.0%} 🖕{middle:.0%} 💍{ring:.0%} 🤙{pinky:.0%}  ", end="")
            
            t += 0.1
            time.sleep(0.1)
            
    except KeyboardInterrupt:
        print("\n\n👋 Simulation stopped")
    finally:
        sock.close()


def run_keyboard_mode(sock):
    """Control sensors with keyboard (requires pynput)"""
    try:
        from pynput import keyboard
    except ImportError:
        print("❌ pynput not installed. Run: pip install pynput")
        return
    
    print("\n⌨️  KEYBOARD MODE")
    print("   A/S/D/F/G = Increase Thumb/Index/Middle/Ring/Pinky")
    print("   Press Ctrl+C to stop\n")
    
    values = [0.0, 0.0, 0.0, 0.0, 0.0]  # T, I, M, R, P
    keys_pressed = set()
    
    def on_press(key):
        try:
            keys_pressed.add(key.char.lower())
        except:
            pass
    
    def on_release(key):
        try:
            keys_pressed.discard(key.char.lower())
        except:
            pass
    
    listener = keyboard.Listener(on_press=on_press, on_release=on_release)
    listener.start()
    
    try:
        while True:
            # Update values based on keys
            if 'a' in keys_pressed:
                values[0] = min(values[0] + 0.05, 1.0)
            if 's' in keys_pressed:
                values[1] = min(values[1] + 0.05, 1.0)
            if 'd' in keys_pressed:
                values[2] = min(values[2] + 0.05, 1.0)
            if 'f' in keys_pressed:
                values[3] = min(values[3] + 0.05, 1.0)
            if 'g' in keys_pressed:
                values[4] = min(values[4] + 0.05, 1.0)
            
            # Decay values slowly when not pressed
            for i in range(5):
                if not ['a', 's', 'd', 'f', 'g'][i] in keys_pressed:
                    values[i] = max(values[i] - 0.02, 0.0)
            
            message = f"T:{values[0]:.2f},I:{values[1]:.2f},M:{values[2]:.2f},R:{values[3]:.2f},P:{values[4]:.2f}"
            sock.sendto(message.encode(), (UNITY_HOST, UNITY_PORT))
            
            print(f"\r👍{values[0]:.0%} 👆{values[1]:.0%} 🖕{values[2]:.0%} 💍{values[3]:.0%} 🤙{values[4]:.0%}  ", end="")
            
            time.sleep(0.05)
            
    except KeyboardInterrupt:
        print("\n\n👋 Keyboard mode stopped")
    finally:
        listener.stop()
        sock.close()


if __name__ == "__main__":
    if len(sys.argv) > 1:
        if sys.argv[1] == "--sim":
            sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            run_simulation(sock)
        elif sys.argv[1] == "--keyboard":
            sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            run_keyboard_mode(sock)
        else:
            print(f"Unknown argument: {sys.argv[1]}")
            print("Usage: python five_sensor_bridge.py [--sim|--keyboard]")
    else:
        main()
