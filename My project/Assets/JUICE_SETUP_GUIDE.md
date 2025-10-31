# Game Juice Setup Guide

This guide will help you set up all the awesome visual effects and polish for your game!

## Overview

I've created a comprehensive "juice" system with:
- **Camera Shake** - Dynamic screen shake for impacts and damage
- **Time Effects** - Hit stop and slow motion on kills
- **Particle Effects** - Death explosions, hit impacts, muzzle flashes
- **Floating Damage Numbers** - Visual damage feedback
- **Screen Effects** - Damage flash and vignette
- **UI System** - Health bars, score, combo system
- **Enemy Health Bars** - Individual health bars above enemies

---

## Step 1: Scene Setup

### Create Manager GameObjects

Create empty GameObjects in your scene hierarchy:

1. **GameManagers** (Empty parent object to organize)
   - **CameraShake**
   - **JuiceManager**
   - **ParticleEffects**
   - **DamageNumberSpawner**

---

## Step 2: Camera Setup

### Main Camera
1. Select your Main Camera in the scene
2. Add Component → **CameraShake** script
3. This will automatically set up the singleton instance

---

## Step 3: UI Canvas Setup

### Create UI Canvas
1. Right-click in Hierarchy → UI → Canvas
2. Set Canvas Scaler to "Scale With Screen Size"
3. Reference Resolution: 1920 x 1080

### Screen Effects (Damage Flash & Vignette)
1. Create: Canvas → **ScreenEffects** (Empty GameObject)
2. Add Component → **ScreenEffects** script
3. Create two child UI Images:
   - **DamageFlash**: Set to stretch (anchor to all corners), set color to Red with 0 alpha
   - **Vignette**: Set to stretch (anchor to all corners), add vignette texture or solid black, 0 alpha

### Game UI
1. Create: Canvas → **GameUI** (Empty GameObject)
2. Add Component → **GameUI** script
3. Create child UI elements:

#### Health Bar
```
GameUI
└── HealthBarContainer (Panel)
    ├── HealthBarBackground (Image - dark color)
    ├── HealthBar (Slider)
    │   └── Fill (Image - red to green gradient)
    └── HealthText (TextMeshPro)
```

#### Score Display
```
GameUI
└── ScoreContainer (Panel - top right)
    ├── ScoreText (TextMeshPro)
    └── KillCountText (TextMeshPro)
```

#### Wave Info
```
GameUI
└── WaveContainer (Panel - top center)
    ├── WaveText (TextMeshPro)
    └── EnemyCountText (TextMeshPro)
```

#### Combo Display
```
GameUI
└── ComboContainer (Panel - center)
    └── ComboText (TextMeshPro - large, bold)
```

4. Drag all these UI elements to the corresponding fields in GameUI component
5. Create a health color gradient (green → yellow → red)

---

## Step 4: Particle Systems

### Create Particle Effect Prefabs

You'll need to create 3 particle system prefabs:

#### 1. Enemy Death Explosion
1. GameObject → Effects → Particle System
2. Configure:
   - Duration: 0.5-1.0
   - Start Lifetime: 0.5-1.5
   - Start Speed: 5-15
   - Start Size: 0.5-2.0
   - Start Color: Orange/Red/Yellow gradient
   - Emission: Burst of 30-50 particles
   - Shape: Sphere
   - Color over Lifetime: Fade to transparent
   - Size over Lifetime: Shrink to 0
3. Save as prefab: **EnemyDeathExplosion**

#### 2. Hit Impact
1. GameObject → Effects → Particle System
2. Configure:
   - Duration: 0.2-0.3
   - Start Lifetime: 0.2-0.5
   - Start Speed: 2-8
   - Start Size: 0.2-0.8
   - Start Color: White/Yellow
   - Emission: Burst of 10-20 particles
   - Shape: Cone (small angle)
3. Save as prefab: **HitImpact**

#### 3. Muzzle Flash
1. GameObject → Effects → Particle System
2. Configure:
   - Duration: 0.1
   - Start Lifetime: 0.1-0.2
   - Start Size: 0.5-1.5
   - Start Color: Yellow/White
   - Emission: Burst of 5-10 particles
   - Shape: Cone
   - Add Light component (optional) for flash
3. Save as prefab: **MuzzleFlash**

### Setup ParticleEffects Manager
1. Select the **ParticleEffects** GameObject
2. Add Component → **ParticleEffects** script
3. Drag your prefabs to the corresponding fields:
   - Enemy Death Explosion Prefab
   - Hit Impact Prefab
   - Muzzle Flash Prefab

---

## Step 5: Damage Numbers

### Create Damage Number Prefab
1. Create: 3D Object → 3D Text (TextMeshPro) or World Space Canvas with TextMeshPro
2. Configure TextMeshPro:
   - Font Size: 4
   - Alignment: Center
   - Color: White
   - Auto Size: Off
3. Add Component → **DamageNumber** script
4. Assign the TextMeshPro component to the script's damageText field
5. Save as prefab: **DamageNumber**

### Setup DamageNumberSpawner
1. Select **DamageNumberSpawner** GameObject
2. Add Component → **DamageNumberSpawner** script
3. Drag the DamageNumber prefab to the Damage Number Prefab field

---

## Step 6: Enemy Setup

### Update Enemy Prefabs
For each enemy prefab:

1. Enemy should already have the updated **Enemy.cs** script
2. Ensure proper tags:
   - Enemy GameObject: Tag = "Enemy"
   - Enemy should have Collider and Rigidbody

### Optional: Add Enemy Health Bar
1. Create World Space Canvas as child of enemy
2. Add Slider (UI → Slider)
3. Style the slider as a health bar
4. Add **EnemyHealthBar** component to canvas
5. Assign references in inspector
6. Update Enemy.cs to call UpdateHealthBar() in TakeDamage()

---

## Step 7: Player Setup

### Player Health
1. Select your Player GameObject
2. Add Component → **PlayerHealth** script
3. Remove or update the IDamageable implementation from your existing Player script to avoid conflicts
4. Set Max Health (default: 100)

### Player GameObject should have:
- **Player** script (movement)
- **PlayerWeapon** script (already updated with juice effects)
- **PlayerHealth** script (new)
- Tag: "Player"
- Collider (for collision detection)

---

## Step 8: Manager Setup Summary

Add these scripts to empty GameObjects:
- **CameraShake** → Main Camera
- **JuiceManager** → Empty GameObject
- **ParticleEffects** → Empty GameObject
- **DamageNumberSpawner** → Empty GameObject
- **ScreenEffects** → UI Canvas child
- **GameUI** → UI Canvas child

All these use singleton pattern and will auto-register on Start().

---

## Step 9: Testing

### Test Checklist:
- [ ] Projectiles damage enemies
- [ ] Enemies spawn death explosion particles
- [ ] Camera shakes on hit and death
- [ ] Floating damage numbers appear
- [ ] UI shows health, score, kills
- [ ] Combo counter appears after multiple kills
- [ ] Screen flashes red when player takes damage
- [ ] Vignette increases at low health
- [ ] Time briefly stops on enemy death (hit stop)
- [ ] Muzzle flash appears when shooting

---

## Customization Tips

### Camera Shake Intensity
In **CameraShake.cs**, adjust:
- `ShakeLight()`: Small impacts
- `ShakeMedium()`: Enemy deaths
- `ShakeHeavy()`: Player damage

### Time Effects
In **JuiceManager.cs**, adjust:
- `hitStopDuration`: Freeze duration
- `slowMotionScale`: Time scale (0-1)
- `slowMotionDuration`: How long slow-mo lasts

### Particle Colors
Edit the particle system prefabs to match your game's aesthetic:
- Enemy explosions: Red/orange for fire, blue for ice enemies, etc.
- Hit impacts: White for normal, yellow for critical hits

### Damage Numbers
In **DamageNumber.cs**:
- `lifetime`: How long numbers stay visible
- `moveSpeed`: Upward movement speed
- `fadeSpeed`: Fade out speed

### UI Colors
In **GameUI.cs**:
- Create custom gradients for health bar
- Adjust combo multiplier formula
- Change score calculation

---

## Advanced Features

### Audio Integration
Add AudioSource components to managers:
- CameraShake: Impact sounds
- ParticleEffects: Explosion sounds
- PlayerWeapon: Already has audio support

### Post-Processing
For even more juice, add Unity Post-Processing:
- Bloom: Makes particles glow
- Chromatic Aberration: On heavy hits
- Color Grading: Health-based color shifts
- Motion Blur: During slow-motion

### Additional Polish Ideas
- Screen shake on shooting (already light shake implemented)
- Recoil animation on weapon
- Enemy flash white when hit
- Player invulnerability flash effect
- Kill streak rewards
- Weapon trail effects
- Hit markers (crosshair confirmation)
- Impact freeze frames (already implemented)

---

## Troubleshooting

### Camera shake not working?
- Ensure CameraShake is on the Main Camera
- Check that Camera is tagged as "MainCamera"

### Particles not spawning?
- Check that prefabs are assigned in ParticleEffects inspector
- Verify particle systems have "Play On Awake" enabled

### Damage numbers not appearing?
- Ensure DamageNumber prefab has TextMeshPro component assigned
- Check that DamageNumberSpawner has prefab assigned
- Verify camera can see world space canvas

### UI not updating?
- Check that all UI elements are assigned in GameUI inspector
- Ensure GameUI is active in scene
- Verify singleton instances are created (check console for errors)

### Time effects causing issues?
- Make sure Time.timeScale resets to 1.0 after effects
- Adjust hitStopDuration to be very short (0.03-0.08)
- Use Time.unscaledDeltaTime for UI updates if needed

---

## Performance Optimization

### For Mobile/Lower-End Devices:
1. Reduce particle counts in all effects
2. Disable enemy health bars (set showOnlyWhenDamaged = true)
3. Lower damage number spawn rate
4. Reduce camera shake magnitude
5. Disable or reduce hit stop duration

### Pooling
- Damage numbers should be pooled if spawning frequently
- Consider object pooling for particle effects
- Already using projectile pooling

---

## Final Notes

All systems are designed to work together and create a cohesive, satisfying game feel. The effects are layered:

**When enemy takes damage:**
1. Light camera shake
2. Hit impact particles spawn
3. Damage number floats up
4. (If critical) Stronger shake + yellow visuals

**When enemy dies:**
1. Death explosion particles
2. Medium camera shake
3. Brief time stop (hit stop)
4. Score and kill count update
5. Combo system activates

**When player takes damage:**
1. Heavy camera shake
2. Red screen flash
3. Brief time stop
4. Health UI updates
5. Vignette intensifies at low health

Adjust values to fit your game's feel and pace. Have fun!
