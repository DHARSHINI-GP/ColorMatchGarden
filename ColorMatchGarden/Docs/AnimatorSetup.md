# Guide Character Animator Setup

## Animator Controller: GuideAnimator

### States Overview

```
┌─────────────────────────────────────────────────────────┐
│                 GUIDE ANIMATOR FLOW                     │
├─────────────────────────────────────────────────────────┤
│                                                         │
│                    ┌──────────┐                         │
│         ┌─────────►│   IDLE   │◄─────────┐              │
│         │          │(default) │          │              │
│         │          └────┬─────┘          │              │
│         │               │                │              │
│   Exit ─┤    ┌──────────┼──────────┐     ├── Exit       │
│         │    │          │          │     │              │
│         │    ▼          ▼          ▼     │              │
│      ┌──┴────┐    ┌─────────┐   ┌────────┴─┐            │
│      │ WAVE  │    │ PRESENT │   │  POINT   │            │
│      │(hello)│    │ (show)  │   │ (flower) │            │
│      └───────┘    └────┬────┘   └──────────┘            │
│                        │                                │
│                        ▼                                │
│               ┌────────────────┐                        │
│               │   CELEBRATE    │                        │
│               │   (success)    │                        │
│               └────────────────┘                        │
│                        │                                │
│                        ▼                                │
│                  ┌───────────┐                          │
│                  │   NOD     │                          │
│                  │  (reset)  │                          │
│                  └───────────┘                          │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## Creating the Animator Controller

### Step 1: Create Animator Controller
1. Right-click in Project → Create → Animator Controller
2. Name it "GuideAnimator"
3. Double-click to open Animator window

### Step 2: Create Animation States

| State     | Type    | Motion           | Speed |
|-----------|---------|------------------|-------|
| Idle      | Default | Idle_Breathing   | 1.0   |
| Wave      | Trigger | Wave_Friendly    | 1.0   |
| Present   | Trigger | Present_Color    | 0.8   |
| Celebrate | Trigger | Celebrate_Happy  | 1.2   |
| Nod       | Trigger | Nod_Gentle       | 0.8   |
| Point     | Trigger | Point_Flower     | 1.0   |

### Step 3: Create Parameters

| Parameter | Type    | Description              |
|-----------|---------|--------------------------|
| Wave      | Trigger | Triggers wave animation  |
| Present   | Trigger | Triggers present color   |
| Celebrate | Trigger | Triggers celebration     |
| Nod       | Trigger | Triggers gentle nod      |
| Point     | Trigger | Triggers pointing        |

### Step 4: Create Transitions

From IDLE to each state:
- Condition: Respective trigger
- Has Exit Time: false
- Transition Duration: 0.2s

From each state back to IDLE:
- Has Exit Time: true
- Exit Time: 0.95
- Transition Duration: 0.3s

## Animation Clip Details

### Idle_Breathing (Loop)
```
Duration: 3 seconds, Loop
- Scale.y: 1.0 → 1.02 → 1.0 (breathing effect)
- Position.y: 0 → 0.05 → 0 (gentle float)
- Rotation.y: -5° → 5° → -5° (gentle sway)
```

### Wave_Friendly
```
Duration: 2 seconds
- Arm rotation: 0° → 45° → 0° (waving motion)
- Hand rotation: 0° → 30° → -30° → 0° (wave gesture)
- Body lean: slight tilt toward child
- Expression: eyes sparkle (if animated sprite)
```

### Present_Color
```
Duration: 1.5 seconds
- Arms extend forward (presenting orb)
- Orb scale: 0.5 → 1.2 → 1.0 (pop effect)
- Body leans forward slightly
- Head tilts thoughtfully
```

### Celebrate_Happy
```
Duration: 2.5 seconds
- Jump: Position.y: 0 → 0.5 → 0 (happy hop)
- Arms raise: rotation up
- Spin: Rotation.y: 0° → 360°
- Particles burst on peak
```

### Nod_Gentle
```
Duration: 1 second
- Head rotation.x: 0° → 15° → 0° (understanding nod)
- Slight smile (if animated)
- Gentle body sway
```

### Point_Flower
```
Duration: 1.5 seconds
- Arm extends toward flower
- Body turns slightly
- Head looks at flower
- Encouraging gesture
```

## Simple Animation Creation (No 3D Model)

If creating with primitives:

### Setup for Capsule Character
```csharp
// Simple animator script for primitive guide
public class SimpleGuideAnimator : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform colorOrb;
    
    private Vector3 originalPosition;
    
    void Start()
    {
        originalPosition = transform.position;
    }
    
    void Update()
    {
        // Idle breathing
        float breathe = Mathf.Sin(Time.time * 2f) * 0.03f;
        transform.position = originalPosition + Vector3.up * breathe;
        
        // Gentle sway
        float sway = Mathf.Sin(Time.time * 0.5f) * 3f;
        transform.rotation = Quaternion.Euler(0, sway, 0);
    }
    
    public void PlayWave()
    {
        StartCoroutine(WaveRoutine());
    }
    
    IEnumerator WaveRoutine()
    {
        // Animate arm waving
        // ...
    }
}
```

## Key Design Principles

1. **Smooth Transitions**: All animations blend smoothly
2. **Non-Threatening**: Guide is small, cute, and friendly
3. **Clear Communication**: Gestures are obvious and exaggerated
4. **Positive Energy**: All animations feel encouraging
5. **Calming Pace**: Nothing moves too fast

## Particle Effects for Guide

### FloatingSparkles (attached to guide)
- Shape: Sphere around guide
- Rate: 3/second
- Size: 0.05 → 0.02 (shrink)
- Color: Soft white/gold
- Lifetime: 2 seconds

### CelebrationBurst (triggered on celebrate)
- Emission: Burst of 50
- Shape: Hemisphere upward
- Colors: Rainbow
- Gravity: -0.5 (float up)
