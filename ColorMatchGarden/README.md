<div align="center">

# 🌸 Color Match Garden 🌸

### *A calm, therapeutic Unity game designed for neurodiverse children*

![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-blue?style=for-the-badge&logo=unity)
![Made for Kids](https://img.shields.io/badge/Made_For-Neurodiverse_Children-ff69b4?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

---

<img src="https://i.imgur.com/placeholder.png" alt="Game Preview" width="600"/>

*A peaceful garden experience where every moment is a celebration! 🎉*

</div>

---

## 📖 Table of Contents

- [✨ About the Game](#-about-the-game)
- [⌨️ Keyboard Controls](#️-keyboard-controls-testing-mode)
- [🎮 How to Play](#-how-to-play)
- [🌈 Gameplay Flow](#-gameplay-flow)
- [🎛️ Controls](#️-controls)
- [🚀 Quick Start](#-quick-start)
- [📁 Project Structure](#-project-structure)
- [🎨 Color System](#-color-system)
- [🧚 Guide Character](#-guide-character)
- [♿ Accessibility](#-accessibility)
- [🌟 Design Philosophy](#-design-philosophy)

---

## ✨ About the Game

**Color Match Garden** is a peaceful, stress-free experience set in a beautiful garden environment. Children match flower colors using physical inputs (flex sensor) and hand gestures (webcam), with absolutely **NO text, timers, scores, or failure states**.

<div align="center">

| 🚫 No Timers | 🚫 No Scores | 🚫 No Failures | ✅ Only Joy! |
|:---:|:---:|:---:|:---:|
| Work at your own pace | Every attempt is wonderful | No pressure, no stress | Endless celebration |

</div>

---

## ⌨️ Keyboard Controls (Testing Mode)

<div align="center">

### 🖥️ Play Without Hardware! 🖥️

*The game includes a **simulation mode** so you can test everything using just your keyboard!*

</div>

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                     ⌨️  K E Y B O A R D   C O N T R O L S  ⌨️               ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║   🎨 BRIGHTNESS CONTROL (Simulates Flex Sensor)                              ║
║   ══════════════════════════════════════════════════════════════════════     ║
║                                                                              ║
║       ⬆️  UP ARROW      │  Increase brightness (hold to increase)           ║
║       ⬇️  DOWN ARROW    │  Decrease brightness (hold to decrease)           ║
║                                                                              ║
║       1️⃣  KEY 1         │  Quick set to LIGHT (15% - soft pastels)          ║
║       2️⃣  KEY 2         │  Quick set to MEDIUM (50% - balanced)             ║
║       3️⃣  KEY 3         │  Quick set to BRIGHT (85% - vivid colors)         ║
║                                                                              ║
║   ✋ GESTURE CONTROL (Simulates Hand Gestures)                                ║
║   ══════════════════════════════════════════════════════════════════════     ║
║                                                                              ║
║       SPACEBAR          │  ✋ Open Hand - CONFIRM your color choice          ║
║                         │     (Hold for 1.5 seconds to confirm)             ║
║                                                                              ║
║       R KEY             │  ✊ Closed Fist - RESET and try again              ║
║                         │     (Hold for 1 second to reset)                  ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
```

<div align="center">

### 📊 On-Screen Debug Display

When playing, you'll see two debug panels in the top-left corner:

| Panel | Shows |
|:---:|:---|
| **Flex Sensor Debug** | Raw value, Normalized value (0-1), Brightness Level, Mode (Simulation) |
| **Gesture Debug** | Current gesture, Hold time, Confirmed status |

</div>

### 🎯 Quick Test Flow

```
1️⃣  Press PLAY in Unity Editor
2️⃣  Use UP/DOWN arrows to adjust flower brightness
3️⃣  Press 1, 2, or 3 for quick brightness presets
4️⃣  Hold SPACEBAR to confirm your color (watch the hold timer!)
5️⃣  Enjoy the celebration! 🎉
6️⃣  Wait for next color, or press R to reset
```

---

## 🎮 How to Play

<div align="center">

### 🌻 It's Simple & Fun! 🌻

</div>

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                           🎮  H O W   T O   P L A Y  🎮                      ║
╠══════════════════════════════════════════════════════════════════════════════╣
║                                                                              ║
║   STEP 1  │  👀 WATCH THE GUIDE                                              ║
║   ────────┼─────────────────────────────────────────────────────────────     ║
║           │  A friendly guide character will appear and wave hello!          ║
║           │  The guide will show you a beautiful color to match.             ║
║                                                                              ║
║   STEP 2  │  🎛️ BEND THE FLEX SENSOR                                         ║
║   ────────┼─────────────────────────────────────────────────────────────     ║
║           │  Gently bend the flex sensor on your hand.                       ║
║           │  Watch the flower change brightness as you bend!                 ║
║           │                                                                  ║
║           │    🤲 Flat hand    → Soft, light colors                          ║
║           │    ✊ Bent fingers  → Bright, vivid colors                        ║
║                                                                              ║
║   STEP 3  │  ✋ CONFIRM WITH HAND GESTURE                                     ║
║   ────────┼─────────────────────────────────────────────────────────────     ║
║           │  When you're happy with your color, show an open hand ✋         ║
║           │  to the camera to confirm your choice!                           ║
║                                                                              ║
║   STEP 4  │  🎉 CELEBRATE!                                                   ║
║   ────────┼─────────────────────────────────────────────────────────────     ║
║           │  Watch the magical celebration with sparkles and sounds!         ║
║           │  Then a new color will appear - play as long as you want!        ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
```

<div align="center">

### 🔄 Want to Start Over?

**Make a fist** ✊ to gently reset and try again!

*There's no wrong answer - every color you create is beautiful!*

</div>

---

## 🌈 Gameplay Flow

<div align="center">

```
                    🌟 YOUR MAGICAL JOURNEY 🌟

          ┌─────────────────────────────────────────┐
          │                                         │
          │   🧚  Guide waves hello!                │
          │        ↓                                │
          │   🎨  Shows you a target color          │
          │        ↓                                │
          │   🌸  You adjust the flower color       │
          │        ↓                                │
          │   ✋  Open hand = Confirm!               │
          │        ↓                                │
          │   🎉  CELEBRATION TIME!                 │
          │        ↓                                │
          │   🔄  New color appears (endless fun!)  │
          │                                         │
          └─────────────────────────────────────────┘
```

</div>

---

## 🎛️ Controls

### 🖐️ Flex Sensor (Wear on Hand)

<div align="center">

| Bend Level | Range | Brightness | Visual Effect |
|:---:|:---:|:---:|:---:|
| 🤲 Flat | 0-30% | **Light** | Soft pastel tones |
| 🤏 Slight Bend | 31-70% | **Medium** | Balanced vibrant colors |
| ✊ Full Bend | 71-100% | **Bright** | Rich saturated colors |

</div>

### 📷 Webcam Hand Gestures

<div align="center">

| Gesture | What It Does | Visual Feedback |
|:---:|:---:|:---:|
| ✋ **Open Hand** | Confirm your color choice! | ✨ Sparkle particles appear |
| ✊ **Closed Fist** | Gently reset and try again | 🌊 Soft fade animation |

</div>

---

## 🚀 Quick Start

### Prerequisites

<div align="center">

| Requirement | Description |
|:---:|:---|
| 🎮 Unity | Version 2022.3 LTS or later |
| 🐍 Python | Version 3.8+ |
| 📷 Webcam | For gesture detection |
| 🤲 Flex Sensor | Connected via serial port |

</div>

### Step-by-Step Setup

#### 1️⃣ Unity Setup
```bash
1. Open Unity Hub
2. Create new 3D project OR open ColorMatchGarden
3. Open the scene: Assets/Scenes/GardenScene.unity
4. Press Play! ▶️
```

#### 2️⃣ Python Bridge Setup
```bash
# Navigate to Python folder
cd ColorMatchGarden/Python

# Install required packages
pip install opencv-python mediapipe websocket-client pyserial

# Run the bridges (in separate terminals)
python flex_sensor_bridge.py    # Terminal 1 - Flex sensor
python gesture_detection.py     # Terminal 2 - Hand gestures
```

#### 3️⃣ Hardware Connections
```
┌──────────────────────────────────────────────────────────┐
│                  📋 HARDWARE SETUP                       │
├──────────────────────────────────────────────────────────┤
│                                                          │
│   🤲 FLEX SENSOR:                                        │
│      • Connect to Raspberry Pi Pico or Arduino           │
│      • Connect USB to computer                           │
│      • Note the COM port (e.g., COM3 on Windows)         │
│                                                          │
│   📷 WEBCAM:                                             │
│      • Built-in or external USB webcam                   │
│      • Position at face/hand level                       │
│      • Ensure good lighting                              │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## 📁 Project Structure

```
🌸 ColorMatchGarden/
│
├── 🎬 Assets/
│   ├── 🎭 Scenes/
│   │   └── GardenScene.unity          # Main game scene
│   │
│   ├── 📜 Scripts/
│   │   ├── 🎮 Core/
│   │   │   ├── GameManager.cs         # Main game orchestration
│   │   │   ├── ColorController.cs     # Color transitions & effects
│   │   │   ├── AccessibilityManager   # Accessibility settings
│   │   │   └── CameraController.cs    # Camera control
│   │   │
│   │   ├── 🎛️ Input/
│   │   │   ├── FlexSensorInput.cs     # Flex sensor handling
│   │   │   ├── GestureRecognizer.cs   # Hand gesture detection
│   │   │   └── WebcamHandler.cs       # Webcam processing
│   │   │
│   │   ├── 🧚 Characters/
│   │   │   └── GuideCharacter.cs      # Friendly guide animations
│   │   │
│   │   ├── ✨ Effects/
│   │   │   ├── ParticleController.cs  # Celebration particles
│   │   │   └── SoundManager.cs        # Calming sounds & music
│   │   │
│   │   └── 🌍 Environment/
│   │       ├── BackgroundManager.cs   # Sky & background
│   │       └── GardenEnvironment.cs   # Garden decorations
│   │
│   ├── 🎨 Materials/                   # Flower & environment materials
│   ├── 🔊 Audio/                       # Ambient sounds & feedback
│   └── 🎬 Animations/                  # Character & flower animations
│
├── 🐍 Python/
│   ├── flex_sensor_bridge.py          # Hardware → Unity bridge
│   └── gesture_detection.py           # MediaPipe hand detection
│
└── 📚 Docs/
    └── AnimatorSetup.md               # Guide character setup
```

---

## 🎨 Color System

<div align="center">

The game uses a therapeutic **pastel color palette** designed to be calming and inviting:

| Color | Name | RGB Values | Purpose |
|:---:|:---:|:---:|:---:|
| 🩷 | Soft Pink | `(255, 153, 204)` | Warm & loving |
| 💙 | Gentle Blue | `(153, 204, 255)` | Calm & peaceful |
| 💛 | Warm Yellow | `(255, 242, 153)` | Happy & bright |
| 💚 | Calming Green | `(153, 255, 178)` | Natural & fresh |
| 💜 | Lavender | `(229, 178, 255)` | Dreamy & soft |
| 🧡 | Peach | `(255, 204, 153)` | Cozy & comfortable |

</div>

### ✨ Visual Effects

- **Smooth Transitions**: All color changes use eased interpolation
- **Gentle Glow**: Flowers emit a warm, magical glow
- **Particle Magic**: Sparkles accompany every color change
- **Pulsing Celebration**: Colors pulse joyfully when confirmed

---

## 🧚 Guide Character

<div align="center">

*Meet your friendly garden guide!*

```
     ╔══════════════════════════════════════════════════════════╗
     ║              🧚 GUIDE ANIMATION STATES 🧚                ║
     ╠══════════════════════════════════════════════════════════╣
     ║                                                          ║
     ║                     ┌──────────────┐                     ║
     ║                     │    IDLE      │                     ║
     ║                     │ (breathing)  │                     ║
     ║                     └──────┬───────┘                     ║
     ║                            │                             ║
     ║           ┌────────────────┼────────────────┐            ║
     ║           ▼                ▼                ▼            ║
     ║    ┌───────────┐    ┌───────────┐    ┌───────────┐       ║
     ║    │   WAVE    │    │  PRESENT  │    │   POINT   │       ║
     ║    │  👋 hello │    │ 🎨 show   │    │ 🌸 flower │       ║
     ║    └───────────┘    └───────────┘    └───────────┘       ║
     ║           │                │                │            ║
     ║           └────────────────┼────────────────┘            ║
     ║                            ▼                             ║
     ║                     ┌───────────┐                        ║
     ║                     │ CELEBRATE │                        ║
     ║                     │  🎉 yay!  │                        ║
     ║                     └───────────┘                        ║
     ║                                                          ║
     ╚══════════════════════════════════════════════════════════╝
```

</div>

---

## ♿ Accessibility Settings

*Fully customizable to meet every child's needs!*

<div align="center">

| Setting | Range | Default | What It Does |
|:---:|:---:|:---:|:---|
| 🕐 **Transition Speed** | 0.5-3.0s | 1.0s | How fast colors change |
| 🔆 **Brightness Boost** | 0-50% | 0% | Extra brightness for visibility |
| 🎯 **Color Tolerance** | 10-50% | 30% | How close matches need to be |
| ⏱️ **Gesture Timeout** | 1-5s | 2s | Time to hold gesture |
| ✨ **Particle Density** | 0-100% | 75% | Visual feedback intensity |

</div>

---

## 🌟 Design Philosophy

<div align="center">

```
╔════════════════════════════════════════════════════════════════════════╗
║                                                                        ║
║      💖  N O   F A I L U R E   S T A T E S                            ║
║          Every attempt is celebrated with joy!                         ║
║                                                                        ║
║      ⏰  N O   T I M E R S                                             ║
║          Children work at their own peaceful pace                      ║
║                                                                        ║
║      📝  N O   T E X T                                                 ║
║          All communication is visual and auditory                      ║
║                                                                        ║
║      🌈  P O S I T I V E   R E I N F O R C E M E N T                  ║
║          Happy animations and gentle sounds only                       ║
║                                                                        ║
║      🌿  S A F E   E N V I R O N M E N T                              ║
║          Calming colors and peaceful ambient sounds                    ║
║                                                                        ║
╚════════════════════════════════════════════════════════════════════════╝
```

</div>

---

<div align="center">

## 🌻 Ready to Play? 🌻

**Start the game and let the magic begin!**

*Remember: There's no wrong way to play - every color you create is beautiful!*

---

Made with 💖 for **neurodiverse children** everywhere

*Because every child deserves joy in learning!*

🌸 🌼 🌺 🌷 🌻 🌹 💐 🪻 🪷

</div>
