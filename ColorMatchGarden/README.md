<div align="center">

# 🌸 Color Match Garden 🌸

### *A calm, therapeutic Unity game designed for neurodiverse children*

![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-blue?style=for-the-badge&logo=unity)
![Made for Kids](https://img.shields.io/badge/Made_For-Neurodiverse_Children-ff69b4?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

---

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
- [♿ Accessibility](#-accessibility)
- [🌟 Design Philosophy](#-design-philosophy)

---

## ✨ About the Game

**Color Match Garden** is a peaceful, stress-free experience set in a beautiful garden environment. Children match flower colors using physical inputs (flex sensors connected to Raspberry Pi Pico) and keyboard controls, with absolutely **NO text, timers, scores, or failure states**.

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

### 🎨 Color Controls (RGB)

| Key | Action | Color Channel |
|:---:|:---|:---:|
| **R** / **E** | Increase / Decrease | 🔴 Red |
| **G** / **D** | Increase / Decrease | 🟢 Green |
| **B** / **N** | Increase / Decrease | 🔵 Blue |

### ✨ Additional Controls

| Key | Action |
|:---:|:---|
| **F** / **T** | Increase / Decrease Brightness |
| **Y** / **H** | Increase / Decrease Magic Effect |
| **SPACEBAR** | Verify color match |

---

## 🎮 How to Play

<div align="center">

### 🌻 It's Simple & Fun! 🌻

</div>

1. **👀 Watch the Target** - A target color appears on screen
2. **🎨 Mix Your Color** - Use keyboard or flex sensors to adjust RGB values
3. **✋ Verify Match** - Press SPACEBAR when you think your color matches
4. **🎉 Celebrate!** - Enjoy the magical celebration when you match correctly!

<div align="center">

*There's no wrong answer - every color you create is beautiful!*

</div>

---

## 🌈 Gameplay Flow

<div align="center">

```
🌟 YOUR MAGICAL JOURNEY 🌟

    🎨 Target color appears
            ↓
    🌸 You mix colors with sensors/keyboard
            ↓
    ✋ Press SPACEBAR to verify
            ↓
    🎉 CELEBRATION TIME!
            ↓
    🔄 New color appears (endless fun!)
```

</div>

---

## 🎛️ Controls

### 🖐️ Flex Sensors (3-Sensor Setup with Raspberry Pi Pico)

<div align="center">

| Sensor | Controls | ADC Pin |
|:---:|:---:|:---:|
| Sensor 1 | 🔴 Red Channel | GP26 |
| Sensor 2 | 🟢 Green Channel | GP27 |
| Sensor 3 | 🔵 Blue Channel | GP28 |

</div>

### 📷 Additional Inputs

| Input | What It Does |
|:---:|:---|
| **Keyboard** | RGB control + Brightness + Magic effects |
| **Webcam** | Optional gesture detection |

---

## 🚀 Quick Start

### Prerequisites

<div align="center">

| Requirement | Description |
|:---:|:---|
| 🎮 Unity | Version 2022.3 LTS or later |
| 🐍 Python | Version 3.8+ |
| 🤲 Flex Sensors | 3x connected to Raspberry Pi Pico |

</div>

### Step-by-Step Setup

#### 1️⃣ Unity Setup
```bash
1. Open Unity Hub
2. Open the ColorMatchGarden project
3. Open the scene: Assets/game.unity
4. Press Play! ▶️
```

#### 2️⃣ Python Bridge Setup
```bash
# Navigate to Python folder
cd ColorMatchGarden/Python

# Install required packages
pip install -r requirements.txt

# Run the sensor bridge
python three_sensor_bridge.py
```

#### 3️⃣ Hardware Connections

| Component | Connection |
|:---:|:---|
| 🤲 Flex Sensors | Connect to Raspberry Pi Pico ADC pins (GP26, GP27, GP28) |
| 🔌 Pico | Connect USB to computer |
| ⚡ Power | 3.3V from Pico with 10kΩ pull-down resistors |

---

## 📁 Project Structure

```
🌸 ColorMatchGarden/
│
├── 📜 Assets/
│   ├── Scripts/
│   │   ├── Core/           # GameManager, ColorController, CameraController
│   │   ├── Input/          # ThreeSensorInput, FiveSensorInput, GestureRecognizer
│   │   ├── Effects/        # ParticleController, SoundManager
│   │   ├── Environment/    # BackgroundManager, GardenEnvironment
│   │   ├── Flowers/        # InteractiveFlower, FiveSensorFlower
│   │   └── UI/             # FiveSensorVisualUI, GameGuideUI
│   │
│   ├── Materials/          # Flower & environment materials
│   ├── Textures/           # Garden textures and backgrounds
│   ├── Prefabs/            # Reusable game objects
│   └── game.unity          # Main game scene
│
├── 🐍 Python/
│   ├── three_sensor_bridge.py    # 3-sensor → Unity bridge
│   ├── five_sensor_bridge.py     # 5-sensor → Unity bridge
│   ├── pico_3_sensors.py         # Pico firmware for 3 sensors
│   └── requirements.txt          # Python dependencies
│
├── 📚 Docs/
│   ├── QuickStart.md
│   ├── GameFlow.md
│   ├── SceneHierarchy.md
│   └── AnimatorSetup.md
│
└── GamePlayGuide.md              # Detailed gameplay instructions
```

---

## 🎨 Color System

<div align="center">

The game uses simple **primary and secondary colors** for easy matching:

| Color | RGB Values | How to Create |
|:---:|:---:|:---|
| 🔴 Red | `(255, 0, 0)` | Red sensor only |
| 🟢 Green | `(0, 255, 0)` | Green sensor only |
| 🔵 Blue | `(0, 0, 255)` | Blue sensor only |
| 💛 Yellow | `(255, 255, 0)` | Red + Green |
| 🟣 Magenta | `(255, 0, 255)` | Red + Blue |
| 🩵 Cyan | `(0, 255, 255)` | Green + Blue |

</div>

### ✨ Visual Effects

- **Smooth Transitions**: All color changes use eased interpolation
- **Auto-Confirmation**: Colors match automatically within tolerance
- **Celebration Particles**: Sparkles and effects on successful match
- **Persistent Colors**: Colors "stick" when sensors are released

---

## ♿ Accessibility Features

<div align="center">

| Feature | Description |
|:---:|:---|
| 🎯 **High Tolerance** | Very forgiving color matching (40% tolerance) |
| 🔄 **Auto-Confirm** | No need to press buttons - matches automatically |
| 📝 **No Text** | Fully visual gameplay |
| ⏰ **No Timers** | Work at your own pace |
| 🚫 **No Failures** | Every attempt is celebrated |

</div>

---

## 🌟 Design Philosophy

<div align="center">

| Principle | Description |
|:---:|:---|
| 💖 **No Failure States** | Every attempt is celebrated with joy! |
| ⏰ **No Timers** | Children work at their own peaceful pace |
| 📝 **No Text** | All communication is visual |
| 🌈 **Positive Reinforcement** | Happy animations and gentle sounds only |
| 🌿 **Safe Environment** | Calming colors and peaceful garden setting |

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
