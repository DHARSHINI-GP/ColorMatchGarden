# ThonnyUnityBridge.py - Run this in Thonny on your COMPUTER
# This bridges the gap between your Pico and Unity

import serial
import socket
import time

# ============ CONFIGURATION ============
SERIAL_PORT = "COM5"      # <-- CHANGE THIS to your Pico's port (e.g., COM3, COM4)
BAUD_RATE = 115200
UNITY_IP = "127.0.0.1"    # Localhost
UNITY_PORT = 5006         # Must match FiveSensorInput.cs
# =======================================

print("--- Unity Flex Bridge Started ---")
print(f"Connecting to Pico on {SERIAL_PORT}...")

# Setup UDP socket for Unity
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

try:
    # Open Serial connection
    ser = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=0.1)
    print("✅ Connected to Pico!")
    print("🎮 Sending data to Unity. Press Ctrl+C to stop.")
except Exception as e:
    print(f"❌ Error: {e}")
    print("💡 Make sure your Pico is plugged in and the SERIAL_PORT is correct.")
    exit()

try:
    while True:
        if ser.in_waiting:
            # Read line from Pico (expected: "p1,p2,p3")
            line = ser.readline().decode('utf-8').strip()
            
            if "," in line:
                try:
                    # Convert percentages (0-100) to normalized values (0.0-1.0)
                    parts = line.split(",")
                    if len(parts) >= 3:
                        r = float(parts[0]) / 100.0
                        g = float(parts[1]) / 100.0
                        b = float(parts[2]) / 100.0
                        
                        # Send to Unity: "T:val,I:val,M:val"
                        # T=Thumb (Red), I=Index (Green), M=Middle (Blue)
                        message = f"T:{r:.2f},I:{g:.2f},M:{b:.2f}"
                        sock.sendto(message.encode(), (UNITY_IP, UNITY_PORT))
                        
                        # Print visual status
                        print(f"\r🔴 {r:.2f} | 🟢 {g:.2f} | 🔵 {b:.2f}   ", end="")
                except ValueError:
                    continue # Skip malformed lines
                    
        time.sleep(0.01)

except KeyboardInterrupt:
    print("\n👋 Bridge stopped.")
finally:
    ser.close()
    sock.close()
