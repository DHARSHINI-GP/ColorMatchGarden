"""
SINGLE Flex Sensor Code (GP26) - With Percentage
================================================
Run this on the Raspberry Pi Pico.

CIRCUIT:
- Flex Sensor + 10k Resistor + 100nF Cap (Signal) + 1000nF Cap (Power)
- See previous diagram for Pin/Hole setup.

CALIBRATION:
1. Run the code.
2. Note the 'Raw' value when FLAT. Change FLAT_VAL below.
3. Note the 'Raw' value when fully BENT. Change BENT_VAL below.
"""

from machine import ADC, Pin
import time

# --- USER CALLIBRATION SETTINGS ---
# Adjust these numbers based on what you see in the Shell!
FLAT_VAL = 50000   # Reading when sensor is straight (High Voltage)
BENT_VAL = 20000   # Reading when sensor is bent 90 degrees (Low Voltage)
# ----------------------------------

sensor = ADC(26)
led = Pin(25, Pin.OUT)

print("Started: Flex Sensor Percentage Calculator")
print(f"Calibration: Flat={FLAT_VAL}, Bent={BENT_VAL}")
print("Bend the sensor to see changes!")

while True:
    try:
        raw_value = sensor.read_u16()
        
        # Calculate Percentage
        # Formula assumes value DROPS when bent (Voltage Divider behavior)
        # 0% = Flat, 100% = Bent
        
        # Constrain range to avoid negative numbers or >100
        clipped_value = min(max(raw_value, BENT_VAL), FLAT_VAL)
        
        # Map: (Flat - Current) / (Flat - Bent) * 100
        percent = ((FLAT_VAL - clipped_value) / (FLAT_VAL - BENT_VAL)) * 100
        
        # Output for User and Serial format
        # sending as: R:95,G:95,B:95 (Percentage)
        print(f"R:{percent:.0f},G:{percent:.0f},B:{percent:.0f}  |  (Raw: {raw_value})")
        
        if percent > 50:
            led.on() # LED on if bent more than 50%
        else:
            led.off()
            
        time.sleep(0.05)
        
    except Exception as e:
        print("Error:", e)
        time.sleep(1)
