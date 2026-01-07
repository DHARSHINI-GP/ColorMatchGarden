# Flex Sensor "Snap-to-Zero" Test
from machine import ADC
import time

# --- CONFIGURATION ---
flex = ADC(26)
FLAT_VALUE = 51000  # Updated based on your logs
BENT_VALUE = 65000  # Updated based on your logs
SAMPLE_SIZE = 5     # Size of the smoothing buffer
# ---------------------

buffer = [FLAT_VALUE] * SAMPLE_SIZE
last_avg = FLAT_VALUE

def read_smart_flex():
    global last_avg, buffer
    
    raw = flex.read_u16()
    
    # SMART LOGIC:
    # 1. If bending (Value going UP): Smooth it (Standard Buffer)
    # 2. If flattening (Value going DOWN): SNAP immediately (Fill Buffer)
    
    if raw < (last_avg - 500): # If dropping significantly (released)
        # SNAP DOWN! Clear history to reach 0% instantly
        buffer = [raw] * SAMPLE_SIZE
    else:
        # SMOOTH UP. Keep the history to filter noise
        buffer.pop(0)
        buffer.append(raw)
        
    # Calculate Average
    avg = sum(buffer) // SAMPLE_SIZE
    last_avg = avg
    
    # Percent Calculation
    # Clamp range first
    if avg <= FLAT_VALUE:
        percent = 0.0
    elif avg >= BENT_VALUE:
        percent = 100.0
    else:
        percent = ((avg - FLAT_VALUE) / (BENT_VALUE - FLAT_VALUE)) * 100.0
        
    return percent, avg

print("⚡ Fast-Return Flex Sensor Test ⚡")
print("   Bending = Smooth")
print("   Releasing = INSTANT")
print("=" * 40)

while True:
    percent, adc = read_smart_flex()
    
    # Visual Bar
    bar_len = int(percent / 5)
    bar = "█" * bar_len + "░" * (20 - bar_len)
    
    print(f"Flex: {percent:6.1f}% [{bar}]  ADC:{adc}")
    time.sleep(0.05)
