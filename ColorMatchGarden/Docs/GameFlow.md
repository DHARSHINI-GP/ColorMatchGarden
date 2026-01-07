# Game Flow - Color Match Garden

A detailed breakdown of the complete game experience.

## 🌅 Game Start Sequence

```
Time 0s:    Scene loads
            ↓
Time 0.5s:  Ambient garden sounds begin (birds, gentle wind)
            ↓
Time 2s:    Guide character fades in with sparkles
            ↓
Time 3s:    Guide waves hello (friendly greeting)
            ↓
Time 4s:    Soft chime plays
            ↓
Time 5s:    Guide floats toward ColorOrb
            ↓
Time 6s:    First color appears in orb (with gentle glow)
            ↓
Time 7s:    Guide gestures toward flower
            ↓
Time 8s:    Matching phase begins
```

## 🎨 Color Matching Phase

```
┌─────────────────────────────────────────────────────────────┐
│                    MATCHING LOOP                            │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. GUIDE SHOWS COLOR                                       │
│     • ColorOrb pulses with target color                     │
│     • Soft note plays                                       │
│     • Particles float up from orb                           │
│                                                             │
│  2. CHILD ADJUSTS FLOWER                                    │
│     • Flex sensor controls brightness                       │
│     • Flower color transitions smoothly                     │
│     • Gentle glow increases with brightness                 │
│     • No time pressure - infinite time allowed              │
│                                                             │
│  3. CHILD CONFIRMS (OPEN HAND ✋)                           │
│     • Progress ring fills as hand stays open                │
│     • At 100%: confirmation complete                        │
│     • ANY color match is "correct"                          │
│                                                             │
│  4. CELEBRATION                                             │
│     • Guide does happy dance                                │
│     • Colorful particles burst                              │
│     • Happy chime plays                                     │
│     • Flower pulses beautifully                             │
│                                                             │
│  5. TRANSITION                                              │
│     • Gentle fade                                           │
│     • New color appears                                     │
│     • Loop continues forever                                │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## 🔄 Reset Behavior (Closed Fist ✊)

When child shows closed fist:
1. Flower gently resets to neutral white
2. Guide gives encouraging nod
3. Soft whoosh sound plays
4. Child can try again
5. **No negative feedback** - just a fresh start

## 🎛️ Flex Sensor Mapping

```
Raw Sensor    Normalized    Brightness    Visual Effect
─────────────────────────────────────────────────────────
0-30%         0.0-0.3       Light         Soft pastel
31-70%        0.31-0.7      Medium        Balanced color
71-100%       0.71-1.0      Bright        Rich saturated
```

### Visual Transition Curve

```
Brightness
    │
1.0 │                           ╭────────
    │                      ╭────╯
0.7 │                 ╭────╯
    │            ╭────╯
0.5 │       ╭────╯
    │  ╭────╯
0.3 │──╯
    │
0.0 └────────────────────────────────────
    0%        50%        100%    Flex Sensor
         (Smooth eased curve)
```

## ✨ Feedback Systems

### Positive Reinforcement Only

| Action              | Visual Feedback           | Audio Feedback     |
|---------------------|---------------------------|-------------------|
| Color change        | Smooth glow transition    | Subtle shimmer    |
| Near target color   | Extra sparkles            | Gentle note       |
| Confirm gesture     | Progress ring fills       | Soft tick         |
| Match complete      | Celebration burst         | Happy chime       |
| Reset               | Gentle fade               | Soft whoosh       |

### Never Shown
- ❌ No "wrong" indicators
- ❌ No red colors for errors
- ❌ No buzzer sounds
- ❌ No countdown timers
- ❌ No score decreasing
- ❌ No game over screen

## 🧚 Guide Character Behaviors

### Idle
- Gentle floating motion
- Soft breathing animation
- Occasional blink
- Sparkle particles

### During Matching
- Looks at child encouragingly
- Occasional small nods
- Points to flower gently
- Color orb pulses softly

### Celebration
- Happy jump
- Spin
- Arms raised
- Extra sparkles
- Joyful expression

## 🌈 Color Palette

Target colors cycle through these therapeutic hues:

| Color       | RGB              | Meaning           |
|-------------|-----------------|-------------------|
| Soft Pink   | (255, 153, 204) | Warmth, comfort   |
| Gentle Blue | (153, 204, 255) | Calm, peace       |
| Warm Yellow | (255, 242, 153) | Joy, energy       |
| Calm Green  | (153, 255, 179) | Nature, safety    |
| Lavender    | (229, 179, 255) | Creativity        |
| Peach       | (255, 204, 153) | Friendliness      |

## ⏰ Timing Guidelines

All timings are adjustable via Accessibility Settings:

| Phase           | Default  | Min    | Max    |
|-----------------|----------|--------|--------|
| Color transition| 0.5s     | 0.2s   | 2.0s   |
| Gesture hold    | 1.5s     | 0.5s   | 3.0s   |
| Celebration     | 3.0s     | 2.0s   | 5.0s   |
| Between colors  | 1.5s     | 0.5s   | 3.0s   |

## 🔇 Audio Landscape

### Ambient Layer (Continuous)
- Gentle wind rustling leaves
- Distant bird songs
- Soft water trickling
- Very low volume (~20%)

### Feedback Layer (Triggered)
- All sounds are:
  - Soft and non-startling
  - Melodic (pentatonic scale)
  - Short duration (< 1 second)
  - Spatialized toward flower/guide

## 📱 Screen Layout

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│     ┌─────────┐                          ┌─────────┐        │
│     │ Webcam  │                          │ Gesture │        │
│     │ Preview │                          │ Status  │        │
│     └─────────┘                          └─────────┘        │
│                                                             │
│                                                             │
│                      🌸                                     │
│            🧚        ⬆️                                     │
│           Guide   Flower                                    │
│                                                             │
│                                                             │
│                                                             │
│                                                             │
│   ┌────────────────────────────────────────────┐            │
│   │               ⚙️ Accessibility             │ (hidden)   │
│   └────────────────────────────────────────────┘            │
└─────────────────────────────────────────────────────────────┘
```

## 🔧 Testing Checklist

- [ ] Colors transition smoothly
- [ ] No jarring movements
- [ ] All sounds are soft
- [ ] Guide animations are friendly
- [ ] Reset works without negative feedback
- [ ] Celebration feels rewarding
- [ ] Webcam shows mirror image
- [ ] Gestures are detected reliably
- [ ] Flex sensor response is smooth
- [ ] Accessibility settings persist
