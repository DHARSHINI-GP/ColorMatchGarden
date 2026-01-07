"""
3 Flex Sensor Reader for Raspberry Pi Pico
==========================================
Reads 3 flex sensors on GP26, GP27, GP28 with smoothing and calibration.
Outputs percentage values: 0% = flat, 100% = fully bent.

Wiring: Each sensor uses voltage divider with 10kΩ resistor and
        100nF + 1000nF capacitors for noise filtering.
"""

from machine import ADC, Pin
import time


# ============================================================================
# CONFIGURATION
# ============================================================================

# ADC Pin assignments
FLEX1_PIN = 26  # GP26 = ADC0
FLEX2_PIN = 27  # GP27 = ADC1
FLEX3_PIN = 28  # GP28 = ADC2

# Calibration values (ADC readings 0-65535)
# Adjust these after running calibration!
CALIBRATION = {
    "flex1": {"flat": 30000, "bent": 50000},
    "flex2": {"flat": 30000, "bent": 50000},
    "flex3": {"flat": 30000, "bent": 50000},
}

# Smoothing settings
SMOOTHING_SAMPLES = 10  # Number of samples for moving average
READ_DELAY_MS = 50      # Delay between readings in milliseconds


# ============================================================================
# SENSOR CLASS
# ============================================================================

class FlexSensor:
    """Flex sensor with smoothing and percentage calculation."""
    
    def __init__(self, pin_number, name, flat_value, bent_value):
        """
        Initialize a flex sensor.
        
        Args:
            pin_number: GPIO pin number (26, 27, or 28)
            name: Sensor name for display
            flat_value: ADC reading when sensor is flat
            bent_value: ADC reading when sensor is fully bent
        """
        self.adc = ADC(Pin(pin_number))
        self.name = name
        self.flat_value = flat_value
        self.bent_value = bent_value
        
        # Buffer for smoothing
        self.samples = [0] * SMOOTHING_SAMPLES
        self.sample_index = 0
        
    def read_raw(self):
        """Read raw ADC value (0-65535)."""
        return self.adc.read_u16()
    
    def add_sample(self, value):
        """Add a sample to the smoothing buffer."""
        self.samples[self.sample_index] = value
        self.sample_index = (self.sample_index + 1) % SMOOTHING_SAMPLES
        
    def get_smoothed(self):
        """Get the smoothed (averaged) value."""
        return sum(self.samples) // SMOOTHING_SAMPLES
    
    def read_smoothed(self):
        """Read a new sample and return smoothed value."""
        raw = self.read_raw()
        self.add_sample(raw)
        return self.get_smoothed()
    
    def get_percentage(self):
        """
        Calculate bend percentage.
        
        Returns:
            Percentage from 0 (flat) to 100 (fully bent), clamped.
        """
        smoothed = self.read_smoothed()
        
        # Avoid division by zero
        range_value = self.bent_value - self.flat_value
        if range_value == 0:
            return 0
        
        # Calculate percentage: (value - flat) / (bent - flat) * 100
        percentage = ((smoothed - self.flat_value) / range_value) * 100
        
        # Clamp to 0-100 range
        percentage = max(0, min(100, percentage))
        
        return int(percentage)


# ============================================================================
# CALIBRATION MODE
# ============================================================================

def run_calibration(sensors):
    """
    Interactive calibration routine.
    
    Instructions:
    1. Keep all sensors FLAT, press Enter
    2. Bend all sensors FULLY, press Enter
    3. New calibration values are printed
    """
    print("\n" + "=" * 50)
    print("        FLEX SENSOR CALIBRATION")
    print("=" * 50)
    
    # Prime the smoothing buffers
    print("\nWarming up sensors...")
    for _ in range(SMOOTHING_SAMPLES * 2):
        for sensor in sensors:
            sensor.read_smoothed()
        time.sleep_ms(20)
    
    # Step 1: Read FLAT values
    print("\n[STEP 1] Keep all sensors FLAT (straight)")
    print("         Press Enter when ready...")
    input()
    
    flat_values = []
    print("Reading flat values...")
    for _ in range(20):  # Take multiple readings
        for sensor in sensors:
            sensor.read_smoothed()
        time.sleep_ms(50)
    
    for sensor in sensors:
        flat_val = sensor.get_smoothed()
        flat_values.append(flat_val)
        print(f"  {sensor.name} FLAT: {flat_val}")
    
    # Step 2: Read BENT values
    print("\n[STEP 2] BEND all sensors FULLY")
    print("         Press Enter when ready...")
    input()
    
    bent_values = []
    print("Reading bent values...")
    for _ in range(20):
        for sensor in sensors:
            sensor.read_smoothed()
        time.sleep_ms(50)
    
    for sensor in sensors:
        bent_val = sensor.get_smoothed()
        bent_values.append(bent_val)
        print(f"  {sensor.name} BENT: {bent_val}")
    
    # Print new calibration values
    print("\n" + "=" * 50)
    print("        NEW CALIBRATION VALUES")
    print("=" * 50)
    print("\nCopy this into the CALIBRATION section:\n")
    print("CALIBRATION = {")
    for i, sensor in enumerate(sensors):
        name_key = sensor.name.lower().replace(" ", "")
        print(f'    "{name_key}": {{"flat": {flat_values[i]}, "bent": {bent_values[i]}}},')
    print("}")
    print("\n" + "=" * 50)


# ============================================================================
# MAIN PROGRAM
# ============================================================================

def main():
    """Main program loop."""
    
    print("\n" + "=" * 50)
    print("   3 FLEX SENSOR READER - Raspberry Pi Pico")
    print("=" * 50)
    
    # Initialize sensors
    sensors = [
        FlexSensor(
            FLEX1_PIN, "Flex1",
            CALIBRATION["flex1"]["flat"],
            CALIBRATION["flex1"]["bent"]
        ),
        FlexSensor(
            FLEX2_PIN, "Flex2",
            CALIBRATION["flex2"]["flat"],
            CALIBRATION["flex2"]["bent"]
        ),
        FlexSensor(
            FLEX3_PIN, "Flex3",
            CALIBRATION["flex3"]["flat"],
            CALIBRATION["flex3"]["bent"]
        ),
    ]
    
    print("\nSensors initialized:")
    print(f"  - Flex1 on GP{FLEX1_PIN} (ADC0)")
    print(f"  - Flex2 on GP{FLEX2_PIN} (ADC1)")
    print(f"  - Flex3 on GP{FLEX3_PIN} (ADC2)")
    
    # Ask for calibration
    print("\nOptions:")
    print("  [C] Run calibration")
    print("  [R] Start reading (use existing calibration)")
    print("\nChoice (C/R): ", end="")
    
    try:
        choice = input().strip().upper()
    except:
        choice = "R"  # Default to reading if no input
    
    if choice == "C":
        run_calibration(sensors)
        print("\nNow starting normal reading mode...\n")
        time.sleep(1)
    
    # Prime smoothing buffers before reading
    print("\nPriming sensors...")
    for _ in range(SMOOTHING_SAMPLES):
        for sensor in sensors:
            sensor.read_smoothed()
        time.sleep_ms(10)
    
    print("\nReading sensors (Ctrl+C to stop):\n")
    print("-" * 50)
    
    # Main reading loop
    try:
        while True:
            # Get percentages from all sensors
            p1 = sensors[0].get_percentage()
            p2 = sensors[1].get_percentage()
            p3 = sensors[2].get_percentage()
            
            # Format output
            output = f"Flex1: {p1:3d}% | Flex2: {p2:3d}% | Flex3: {p3:3d}%"
            print(output)
            
            time.sleep_ms(READ_DELAY_MS)
            
    except KeyboardInterrupt:
        print("\n\nStopped by user.")


# Run the program
if __name__ == "__main__":
    main()
