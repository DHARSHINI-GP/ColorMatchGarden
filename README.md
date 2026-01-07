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
- [🎮 How to Play](#-how-to-play)
- [⌨️ Keyboard Controls](#️-keyboard-controls-test-mode)
- [�️ 5-Sensor System](#️-5-sensor-system)
- [🚀 Quick Start](#-quick-start)
- [📁 Project Structure](#-project-structure)
- [🎨 Color Mixing](#-color-mixing)
- [♿ Accessibility](#-accessibility)

---

## ✨ About the Game

**Color Match Garden** is a peaceful, stress-free experience set in a beautiful garden environment. Children mix colors using **5 flex sensors** (one for each finger) connected to a Raspberry Pi Pico, with absolutely **NO text, timers, scores, or failure states**.

<div align="center">

| 🚫 No Timers | 🚫 No Scores | 🚫 No Failures | ✅ Only Joy! |
|:---:|:---:|:---:|:---:|
| Work at your own pace | Every attempt is wonderful | No pressure, no stress | Endless celebration |

</div>

---

## 🎮 How to Play

1. **👀 See the Target Color** - A target color appears on screen
2. **🤲 Bend Your Fingers** - Each finger controls a different color
3. **🎨 Mix Colors** - Combine colors by bending multiple fingers
4. **✋ Press SPACEBAR** - Verify your color match
5. **🎉 Celebrate!** - Enjoy the magical celebration!

---

## ⌨️ Keyboard Controls (Test Mode)

<div align="center">

### 🖥️ Play Without Hardware! 🖥️

*Test the game using your keyboard - each key simulates a flex sensor!*

</div>

### 🎨 Color Keys (Hold to Activate)

| Key | Color | Finger |
|:---:|:---:|:---:|
| **Q** | 🔴 Red | Thumb |
| **W** | 💛 Yellow | Index |
| **E** | 🟢 Green | Middle |
| **R** | 🩵 Cyan | Ring |
| **T** | 🟣 Purple/Violet | Pinky |

### ✅ Confirm

| Key | Action |
|:---:|:---|
| **SPACEBAR** | Verify your color match |

### � How to Mix Colors

| Want This Color? | Hold These Keys |
|:---:|:---|
| 🔴 Red | Q only |
| 💛 Yellow | W only |
| 🟢 Green | E only |
| 🩵 Cyan | R only |
| 🟣 Purple | T only |
| 🟠 Orange | Q + W (Red + Yellow) |
| 🩷 Pink | Q + T (Red + Purple) |
| 💙 Blue-Green | E + R (Green + Cyan) |
| � Any Mix! | Combine any keys! |

---

## 🎛️ 5-Sensor System

### Hardware Setup

<div align="center">

| Sensor | Finger | Color | Pico Pin |
|:---:|:---:|:---:|:---:|
| Sensor 1 | Thumb | 🔴 Red | GP26 (ADC0) |
| Sensor 2 | Index | 💛 Yellow | GP27 (ADC1) |
| Sensor 3 | Middle | 🟢 Green | GP28 (ADC2) |
| Sensor 4 | Ring | 🩵 Cyan | ADC via MUX |
| Sensor 5 | Pinky | 🟣 Purple | ADC via MUX |

</div>

### How It Works

- **Bend a finger** → That color activates
- **Bend harder** → Color gets stronger
- **Relax finger** → Color fades smoothly
- **Mix fingers** → Colors blend together!

---

## 🚀 Quick Start

### Prerequisites

| Requirement | Description |
|:---:|:---|
| 🎮 Unity | Version 2022.3 LTS or later |
| 🐍 Python | Version 3.8+ |
| 🤲 Flex Sensors | 5x connected to Raspberry Pi Pico |

### Setup Steps

#### 1️⃣ Unity
```bash
1. Open Unity Hub
2. Open ColorMatchGarden project
3. Open scene: Assets/game.unity
4. Press Play! ▶️
```

#### 2️⃣ Python Bridge (for hardware)
```bash
cd ColorMatchGarden/Python
pip install -r requirements.txt
python five_sensor_bridge.py
```

---

## 📁 Project Structure

```
🌸 ColorMatchGarden/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/        # GameManager, ColorController
│   │   ├── Input/       # FiveSensorInput, ThreeSensorInput
│   │   ├── Effects/     # ParticleController, SoundManager
│   │   ├── Flowers/     # InteractiveFlower, FiveSensorFlower
│   │   └── UI/          # FiveSensorVisualUI
│   └── game.unity       # Main scene
│
├── Python/
│   ├── five_sensor_bridge.py   # 5-sensor → Unity
│   ├── three_sensor_bridge.py  # 3-sensor → Unity
│   └── pico_3_sensors.py       # Pico firmware
│
└── Docs/                # Documentation
```

---

## 🎨 Color Mixing

The game uses **weighted average color mixing** - just like real paint!

<div align="center">

| Base Colors | Mix Result |
|:---:|:---:|
| 🔴 Red + 💛 Yellow | 🟠 Orange |
| � Yellow + �🟢 Green | 🍀 Lime |
| � Green + 🩵 Cyan | 🌊 Teal |
| 🩵 Cyan + 🟣 Purple | 💙 Blue |
| 🟣 Purple + 🔴 Red | � Magenta |

</div>

*Bend multiple fingers at different amounts for infinite color combinations!*

---

## ♿ Accessibility Features

<div align="center">

| Feature | Description |
|:---:|:---|
| 📝 **No Text** | Fully visual gameplay |
| ⏰ **No Timers** | Work at your own pace |
| ❌ **No Failures** | Every attempt is celebrated |
| � **High Tolerance** | Forgiving color matching |
| � **Smooth Animations** | Calming visual feedback |
| 🌿 **Garden Theme** | Peaceful, relaxing environment |

</div>

---

<div align="center">

## 🌻 Ready to Play? 🌻

**Press Q, W, E, R, T to mix colors - then SPACEBAR to confirm!**

*Every color you create is beautiful!*

---

Made with 💖 for **neurodiverse children** everywhere

🌸 🌼 🌺 🌷 🌻 🌹 💐 🪻 🪷

</div>
