# PicoFlexReader.py - MicroPython for Raspberry Pi Pico
# Copy this to your Pico and save as 'main.py'

from machine import ADC
import time

# -----------------------------
# ADC setup
# -----------------------------
# Pins: GP26 (A0), GP27 (A1), GP28 (A2)
flex = [ADC(26), ADC(27), ADC(28)]

# -----------------------------
# YOUR calibration values
# -----------------------------
FLAT = [50000, 51120, 32570]
BENT = [53178, 51736, 32789]

# -----------------------------
# Filtering + stability
# -----------------------------
SAMPLE_SIZE = 5
DEAD_ZONE = 1.5        # %
HYSTERESIS = 2.0       # %

buffers = [[0]*SAMPLE_SIZE for _ in range(3)]
last_percent = [0, 0, 0]

def read_flex(i):
    raw = flex[i].read_u16()

    buffers[i].pop(0)
    buffers[i].append(raw)
    avg = sum(buffers[i]) / SAMPLE_SIZE

    flat = FLAT[i]
    bent = BENT[i]

    if avg <= flat:
        percent = 0.0
    elif avg >= bent:
        percent = 100.0
    else:
        percent = (avg - flat) * 100 / (bent - flat)

    # Dead-zone
    if percent < DEAD_ZONE:
        percent = 0.0
    if percent > 100 - DEAD_ZONE:
        percent = 100.0

    # Hysteresis
    if abs(percent - last_percent[i]) < HYSTERESIS:
        percent = last_percent[i]

    last_percent[i] = percent
    return percent, int(avg)

# -----------------------------
# Main loop
# -----------------------------
print("--- Pico Flex Reader Started (STABLE) ---")

while True:
    percents = []
    
    for i in range(3):
        p, adc = read_flex(i)
        percents.append(p)

    # PRINT CSV FORMAT FOR THE BRIDGE
    # This is exactly what ThonnyUnityBridge.py expects
    print(f"{percents[0]},{percents[1]},{percents[2]}")
    
    time.sleep(0.05)
