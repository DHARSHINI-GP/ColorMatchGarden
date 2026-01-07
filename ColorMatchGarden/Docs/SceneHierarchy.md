# Unity Scene Hierarchy - Color Match Garden

This document describes how to set up the Unity scene for Color Match Garden.

## Scene Hierarchy

```
GardenScene
│
├── 🎮 [GameManager]
│   ├── GameManager.cs
│   ├── FlexSensorInput.cs
│   ├── GestureRecognizer.cs
│   └── AccessibilityManager.cs
│
├── 📷 [Main Camera]
│   ├── Camera (clear flags: Skybox)
│   ├── AudioListener
│   └── Post Processing (optional)
│
├── 💡 [Lighting]
│   ├── Directional Light (Main Sun)
│   │   └── Soft shadows, warm color
│   ├── Point Light (Garden Ambient 1)
│   ├── Point Light (Garden Ambient 2)
│   └── Reflection Probe
│
├── 🌳 [Environment]
│   ├── GardenEnvironment.cs
│   ├── Ground
│   │   └── Grass material with gentle texture
│   ├── Trees (background)
│   │   ├── Tree_01
│   │   ├── Tree_02
│   │   └── Tree_03
│   ├── Bushes
│   │   ├── Bush_01
│   │   └── Bush_02
│   ├── Decorations
│   │   ├── Rocks
│   │   ├── Mushrooms
│   │   └── Butterflies
│   └── Skybox (gradient blue/pink)
│
├── 🧚 [Guide Character]
│   ├── GuideCharacter.cs
│   ├── Animator Controller
│   ├── Model (friendly fairy/sprite)
│   ├── ColorOrb
│   │   ├── Mesh (sphere)
│   │   ├── Material (emissive)
│   │   └── Point Light
│   └── Effects
│       ├── Float Particles
│       └── Sparkle Trail
│
├── 🌸 [Interactive Flower]
│   ├── InteractiveFlower.cs
│   ├── ColorController.cs
│   ├── Flower Model
│   │   ├── Stem
│   │   ├── Petals (colored material)
│   │   └── Center
│   ├── Flower Glow Light
│   └── Particle Effects
│       ├── Pollen Particles
│       └── Sparkle Particles
│
├── ✨ [Particle Systems]
│   ├── ParticleController.cs
│   ├── Celebration Particles
│   │   └── Burst of colorful particles
│   ├── Ambient Particles
│   │   └── Floating dust/sparkles
│   ├── Color Change Particles
│   └── Confirm Glow Particles
│
├── 🔊 [Audio]
│   ├── SoundManager.cs
│   ├── Ambient Source
│   │   └── Garden ambience (birds, wind)
│   └── Feedback Source
│       └── For chimes and sounds
│
├── 🖥️ [UI Canvas]
│   ├── Canvas (Screen Space - Overlay)
│   ├── Webcam Display (corner preview)
│   │   ├── WebcamHandler.cs
│   │   ├── Raw Image
│   │   └── Gesture Frame Overlay
│   ├── Gesture Indicators
│   │   ├── Confirm Ring (progress)
│   │   ├── Open Hand Icon
│   │   └── Closed Fist Icon
│   └── Accessibility Panel (hidden by default)
│       └── AccessibilityPanel.cs
│
└── 📁 [Managers]
    └── Event System
```

## Quick Setup Steps

### 1. Create Empty Scene
- File → New Scene → Save as "GardenScene"

### 2. Set Up GameManager
```
1. Create empty GameObject named "[GameManager]"
2. Add components:
   - GameManager.cs
   - FlexSensorInput.cs
   - GestureRecognizer.cs
   - AccessibilityManager.cs
3. Set simulation mode to true for testing
```

### 3. Create Environment
```
1. Create plane for ground (scale 10, 1, 10)
2. Apply grass material
3. Add 3D trees as background (can use Unity primitives initially)
4. Add soft directional light
5. Set skybox to gradient
```

### 4. Create Guide Character
```
1. Create capsule as placeholder
2. Add GuideCharacter.cs
3. Create child sphere as ColorOrb
4. Add animator controller with states:
   - Idle (default, looping)
   - Wave (trigger)
   - Present (trigger)
   - Celebrate (trigger)
   - Nod (trigger)
```

### 5. Create Interactive Flower
```
1. Create flower from primitives:
   - Cylinder (stem) - green
   - 6 spheres flattened (petals) - white initially
   - Sphere (center) - yellow
2. Add InteractiveFlower.cs
3. Add ColorController.cs
4. Add point light as child (color glow)
5. Add particle systems as children
```

### 6. Set Up Audio
```
1. Create [Audio] empty object
2. Add SoundManager.cs
3. Add two AudioSource components:
   - Ambient (loop enabled)
   - Feedback (one-shot)
4. Import calming audio clips
```

### 7. Create UI
```
1. Create Canvas
2. Add webcam display (RawImage in corner)
3. Add gesture progress indicators
4. Create accessibility panel (initially hidden)
```

## Testing Without Hardware

The game includes simulation mode:
- **Arrow Up/Down**: Adjust flex sensor value
- **1, 2, 3 keys**: Quick set to Light/Medium/Bright
- **Space**: Simulate open hand gesture
- **R key**: Simulate closed fist gesture
- **Escape**: Toggle accessibility panel

## Recommended Unity Settings

### Quality Settings
- Anti-aliasing: 4x
- Soft particles: Enabled
- Texture quality: Full

### Lighting Settings
- Ambient mode: Gradient
- Sky color: Soft blue
- Ground color: Soft green
- Realtime GI: Enabled

### Audio Settings
- DSP Buffer: Good Latency
- Sample Rate: 48000
