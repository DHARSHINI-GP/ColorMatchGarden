"""
3 Flex Sensors for Color Match Garden
=====================================
Run this on the Raspberry Pi Pico using Thonny

CIRCUIT SETUP: PICO "OFF-BOARD" (Floating)
==========================================
In this setup, the Pico sits separate from the breadboard.
You will use 5 Jumper Wires to connect them.

REQUIRED WIRES:
- 5x Male-to-Female Jumpers (if Pico has pins)
  OR 5x Male-to-Male (if soldering/clipping)

BREADBOARD (SENSORS ONLY) SETUP:
--------------------------------
1. POWER RAILS:
   - Connect 1000nF (1uF) Capacitor across Red (+) and Blue (-) rails.

2. SENSOR 1 (RED) - Row 25:
   - Flex Leg 1 -> Red Rail (+)
   - Flex Leg 2 -> Row 25
   - 10k Resistor -> Row 25 to Blue Rail (-)
   - 100nF Cap    -> Row 25 to Blue Rail (-)

3. SENSOR 2 (GREEN) - Row 30:
   - Flex Leg 1 -> Red Rail (+)
   - Flex Leg 2 -> Row 30
   - 10k Resistor -> Row 30 to Blue Rail (-)
   - 100nF Cap    -> Row 30 to Blue Rail (-)

4. SENSOR 3 (BLUE) - Row 35:
   - Flex Leg 1 -> Red Rail (+)
   - Flex Leg 2 -> Row 35
   - 10k Resistor -> Row 35 to Blue Rail (-)
   - 100nF Cap    -> Row 35 to Blue Rail (-)

PICO TO BREADBOARD CONNECTIONS:
-------------------------------
Connect these 5 wires from the loose Pico to the Breadboard:

1. POWER:  Pico Pin 36 (3V3) -> To Breadboard RED Rail (+)
2. GROUND: Pico Pin 38 (GND) -> To Breadboard BLUE Rail (-)
3. SIGNAL: Pico Pin 31 (GP26) -> To Breadboard Row 25 (Sensor 1)
4. SIGNAL: Pico Pin 32 (GP27) -> To Breadboard Row 30 (Sensor 2)
5. SIGNAL: Pico Pin 34 (GP28) -> To Breadboard Row 35 (Sensor 3)

NOTE: Do not use high pass filters.
"""

from machine import ADC, Pin
import time

# Setup ADC pins (GP26, GP27, GP28)
sensor_red = ADC(26)    # Red - Sensor 1
sensor_green = ADC(27)  # Green - Sensor 2
sensor_blue = ADC(28)   # Blue - Sensor 3

# LED for visual feedback (optional)
led = Pin(25, Pin.OUT)

print("🌸 Color Match Garden - 3 Flex Sensors")
print("=" * 40)
print("Sending RGB values to computer...")
print("=" * 40)

led_state = False

while True:
    # Read all 3 sensors (0-65535)
    r = sensor_red.read_u16()
    g = sensor_green.read_u16()
    b = sensor_blue.read_u16()
    
    # Send as comma-separated values
    # Format: "R:12345,G:23456,B:34567"
    print(f"R:{r},G:{g},B:{b}")
    
    # Blink LED to show it's working
    led_state = not led_state
    led.value(led_state)
    
    # Send 20 times per second
    time.sleep(0.05)
