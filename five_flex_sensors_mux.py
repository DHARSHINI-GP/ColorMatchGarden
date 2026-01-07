"""
5 Flex Sensors with CD4051 Multiplexer - Raspberry Pi Pico
============================================================
Reads 5 flex sensors through a single ADC pin using CD4051 multiplexer.

Wiring:
- GP26 (ADC0) -> CD4051 Pin 3 (Common OUT)
- GP10 -> CD4051 Pin 9 (Select A)
- GP11 -> CD4051 Pin 10 (Select B)
- GP12 -> CD4051 Pin 11 (Select C)
"""

from machine import Pin, ADC
import time

# ============== CONFIGURATION ==============

# ADC pin connected to multiplexer output
ADC_PIN = 26

# Multiplexer select pins (directly connected to Pico GPIOs)
SELECT_A_PIN = 10  # GP10 - LSB
SELECT_B_PIN = 11  # GP11
SELECT_C_PIN = 12  # GP12 - MSB

# Calibration values (adjust based on your flex sensors)
# Flat position voltage and bent position voltage
CALIBRATION = {
    0: {"flat": 1.2, "bent": 2.5, "name": "Thumb"},
    1: {"flat": 1.2, "bent": 2.5, "name": "Index"},
    2: {"flat": 1.2, "bent": 2.5, "name": "Middle"},
    3: {"flat": 1.2, "bent": 2.5, "name": "Ring"},
    4: {"flat": 1.2, "bent": 2.5, "name": "Pinky"},
}

# ============== SETUP ==============

# Initialize ADC
adc = ADC(Pin(ADC_PIN))

# Initialize multiplexer select pins
select_a = Pin(SELECT_A_PIN, Pin.OUT)
select_b = Pin(SELECT_B_PIN, Pin.OUT)
select_c = Pin(SELECT_C_PIN, Pin.OUT)


def select_channel(channel):
    """
    Select a channel on the CD4051 multiplexer.
    Channel 0-7, we use 0-4 for 5 sensors.
    
    Channel selection table:
    Channel | C (GP12) | B (GP11) | A (GP10)
    --------|----------|----------|----------
       0    |    0     |    0     |    0
       1    |    0     |    0     |    1
       2    |    0     |    1     |    0
       3    |    0     |    1     |    1
       4    |    1     |    0     |    0
    """
    select_a.value(channel & 0x01)        # Bit 0
    select_b.value((channel >> 1) & 0x01) # Bit 1
    select_c.value((channel >> 2) & 0x01) # Bit 2
    
    # Small delay for multiplexer to settle
    time.sleep_us(100)


def read_voltage(channel):
    """Read voltage from a specific channel."""
    select_channel(channel)
    
    # Take multiple readings and average for stability
    readings = []
    for _ in range(10):
        readings.append(adc.read_u16())
        time.sleep_us(50)
    
    avg_raw = sum(readings) / len(readings)
    voltage = (avg_raw / 65535) * 3.3
    return voltage


def read_all_sensors():
    """Read all 5 flex sensors and return voltages."""
    voltages = {}
    for channel in range(5):
        voltages[channel] = read_voltage(channel)
    return voltages


def voltage_to_percent(channel, voltage):
    """Convert voltage to bend percentage (0% = flat, 100% = fully bent)."""
    cal = CALIBRATION[channel]
    flat_v = cal["flat"]
    bent_v = cal["bent"]
    
    # Clamp and calculate percentage
    if bent_v > flat_v:
        percent = ((voltage - flat_v) / (bent_v - flat_v)) * 100
    else:
        percent = ((flat_v - voltage) / (flat_v - bent_v)) * 100
    
    return max(0, min(100, percent))


def read_all_percentages():
    """Read all sensors and return bend percentages."""
    percentages = {}
    for channel in range(5):
        voltage = read_voltage(channel)
        percentages[channel] = voltage_to_percent(channel, voltage)
    return percentages


def calibrate_sensor(channel):
    """Interactive calibration for a single sensor."""
    name = CALIBRATION[channel]["name"]
    
    print(f"\n=== Calibrating {name} (Channel {channel}) ===")
    
    # Flat position
    input(f"Hold {name} FLAT and press Enter...")
    flat_readings = []
    for _ in range(50):
        flat_readings.append(read_voltage(channel))
        time.sleep_ms(20)
    flat_v = sum(flat_readings) / len(flat_readings)
    print(f"  Flat voltage: {flat_v:.3f}V")
    
    # Bent position
    input(f"BEND {name} fully and press Enter...")
    bent_readings = []
    for _ in range(50):
        bent_readings.append(read_voltage(channel))
        time.sleep_ms(20)
    bent_v = sum(bent_readings) / len(bent_readings)
    print(f"  Bent voltage: {bent_v:.3f}V")
    
    CALIBRATION[channel]["flat"] = flat_v
    CALIBRATION[channel]["bent"] = bent_v
    
    print(f"  Calibration saved!")
    return flat_v, bent_v


def calibrate_all():
    """Calibrate all 5 sensors interactively."""
    print("\n" + "="*50)
    print("    5-FINGER FLEX SENSOR CALIBRATION")
    print("="*50)
    
    for channel in range(5):
        calibrate_sensor(channel)
    
    print("\n=== CALIBRATION COMPLETE ===")
    print("\nCalibration values (copy to CALIBRATION dict):")
    print("CALIBRATION = {")
    for ch, cal in CALIBRATION.items():
        print(f'    {ch}: {{"flat": {cal["flat"]:.3f}, "bent": {cal["bent"]:.3f}, "name": "{cal["name"]}"}},')
    print("}")


def print_sensor_bar(name, percent, width=30):
    """Print a visual bar for sensor reading."""
    filled = int((percent / 100) * width)
    bar = "█" * filled + "░" * (width - filled)
    print(f"{name:8s} [{bar}] {percent:5.1f}%")


def main_live_display():
    """Main loop with live sensor display."""
    print("\n" + "="*50)
    print("    5 FLEX SENSORS - LIVE READINGS")
    print("="*50)
    print("Press Ctrl+C to stop\n")
    
    try:
        while True:
            # Clear screen (works in some terminals)
            print("\033[H\033[J", end="")
            
            print("╔════════════════════════════════════════════════╗")
            print("║         5-FINGER FLEX SENSOR MONITOR           ║")
            print("╠════════════════════════════════════════════════╣")
            
            for channel in range(5):
                voltage = read_voltage(channel)
                percent = voltage_to_percent(channel, voltage)
                name = CALIBRATION[channel]["name"]
                
                # Create visual bar
                bar_width = 25
                filled = int((percent / 100) * bar_width)
                bar = "█" * filled + "░" * (bar_width - filled)
                
                print(f"║ {name:7s} │{bar}│ {percent:5.1f}% │ {voltage:.2f}V ║")
            
            print("╚════════════════════════════════════════════════╝")
            print("\n[Press Ctrl+C to exit]")
            
            time.sleep_ms(100)
            
    except KeyboardInterrupt:
        print("\n\nStopped by user.")


def main_simple():
    """Simple main loop - prints raw values."""
    print("\n5 Flex Sensors - Simple Reading Mode")
    print("Press Ctrl+C to stop\n")
    
    try:
        while True:
            values = []
            for channel in range(5):
                voltage = read_voltage(channel)
                percent = voltage_to_percent(channel, voltage)
                name = CALIBRATION[channel]["name"]
                values.append(f"{name}:{percent:.0f}%")
            
            print(" | ".join(values))
            time.sleep_ms(200)
            
    except KeyboardInterrupt:
        print("\nStopped.")


def main_json():
    """Output readings as JSON for serial communication."""
    print("JSON output mode - Ctrl+C to stop")
    
    try:
        while True:
            data = {}
            for channel in range(5):
                voltage = read_voltage(channel)
                percent = voltage_to_percent(channel, voltage)
                name = CALIBRATION[channel]["name"].lower()
                data[name] = {
                    "voltage": round(voltage, 3),
                    "percent": round(percent, 1)
                }
            
            import json
            print(json.dumps(data))
            time.sleep_ms(100)
            
    except KeyboardInterrupt:
        print("\nStopped.")


# ============== RUN ==============

if __name__ == "__main__":
    print("\n" + "="*50)
    print("  5 FLEX SENSORS WITH CD4051 MULTIPLEXER")
    print("="*50)
    print("\nOptions:")
    print("  1. Run main_live_display() - Visual monitor")
    print("  2. Run main_simple() - Simple text output")
    print("  3. Run main_json() - JSON output for Unity/apps")
    print("  4. Run calibrate_all() - Calibrate all sensors")
    print("\nStarting live display in 3 seconds...")
    time.sleep(3)
    
    main_live_display()
