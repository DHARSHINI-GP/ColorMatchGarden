# Quick Start Guide - Color Match Garden

Get the game running in 5 minutes!

## Prerequisites

- Unity 2021.3 LTS or newer
- Python 3.8+ (for input bridges)
- Webcam (for gesture detection)
- Flex sensor + microcontroller (optional)

## Step 1: Open in Unity

1. Open Unity Hub
2. Click "Add" and select the `ColorMatchGarden` folder
3. Open the project

## Step 2: Setup Scene

**Option A: Automatic Setup**
1. In Unity, go to menu: `Color Match Garden → Setup Scene`
2. Click "Yes" to create all objects

**Option B: Manual Setup**
1. Open `Assets/Scenes/GardenScene.unity`
2. Follow `Docs/SceneHierarchy.md`

## Step 3: Configure Input

### For Testing (No Hardware)
1. Select `[GameManager]` in Hierarchy
2. In `FlexSensorInput` component, check `Use Simulation`
3. In `GestureRecognizer` component, check `Use Simulation`

### With Real Hardware
1. Uncheck `Use Simulation` on both components
2. Run Python bridges (see Step 4)

## Step 4: Start Python Bridges

Open two terminals:

**Terminal 1 - Flex Sensor:**
```bash
cd ColorMatchGarden/Python
pip install -r requirements.txt
python flex_sensor_bridge.py
```

**Terminal 2 - Gesture Detection:**
```bash
cd ColorMatchGarden/Python
python gesture_detection.py
```

## Step 5: Play!

1. Press Play in Unity
2. Use keyboard to test:
   - **↑/↓ Arrow**: Adjust brightness
   - **1, 2, 3**: Quick brightness presets
   - **Space**: Confirm (open hand)
   - **R**: Reset (closed fist)
   - **Escape**: Open settings

## Quick Troubleshooting

| Issue | Solution |
|-------|----------|
| No color change | Check FlexSensorInput is enabled |
| Gestures not working | Check GestureRecognizer is enabled |
| No audio | Add audio clips to SoundManager |
| Dark scene | Add directional light |
| Script errors | Ensure all scripts are in correct folders |

## Project Structure

```
ColorMatchGarden/
├── Assets/
│   ├── Scripts/      ← All C# scripts
│   ├── Scenes/       ← Unity scenes
│   └── ...
├── Python/           ← Input bridges
├── Docs/             ← Documentation
└── README.md         ← Overview
```

## Next Steps

1. Import 3D models for guide and flower
2. Add audio clips (ambient + feedback)
3. Customize colors in GameManager
4. Adjust accessibility settings
5. Connect real hardware

## Support

Check `Docs/` folder for detailed guides:
- `SceneHierarchy.md` - Full object setup
- `AnimatorSetup.md` - Animation guide
- `GameFlow.md` - Complete game flow

Enjoy the calm! 🌸
