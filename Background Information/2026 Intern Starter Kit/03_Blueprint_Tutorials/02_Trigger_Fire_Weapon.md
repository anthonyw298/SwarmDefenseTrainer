# Blueprint Tutorial 2: Trigger Button — Fire a Weapon

This tutorial shows you how to use the HardwareTrigger plugin to detect when the physical trigger button is pressed and fire a weapon in-game.

## What You'll Build

A weapon system that fires a hitscan ray (line trace) when the Arduino trigger button is pressed, with visual effects and damage to hit actors.

## Prerequisites

- Starter project open in UE5
- HardwareTrigger plugin enabled
- Completed Tutorial 01 (or have a pawn with a camera)

## Step 1: Initialize the Trigger on Begin Play

In your pawn's Blueprint (e.g., `BP_SensorPawn`):

1. After your VN-100 initialization, add: **Start Trigger** node
2. Set **Port Name** to: `COM4` (or your Arduino's COM port)
3. Set **Baud Rate** to: `115200`
4. Add a Branch to check success

```
Event BeginPlay
    → Start VN100 ("COM3", 115200)
    → Start Trigger ("COM4", 115200) → Branch → Print success/failure
```

## Step 2: Check for Trigger Press Every Tick

1. In your **Event Tick** graph, after the VN-100 camera update:
2. Add: **Consume Trigger Press** node
   - This returns `true` if the trigger was pressed since last check, and clears the flag
   - Use this instead of `Is Trigger Pressed` to prevent continuous firing
3. Add: **Branch** (condition: Consume Trigger Press return value)
4. On True: call your fire logic

```
Event Tick
    → [camera update logic from Tutorial 01]
    → Consume Trigger Press → Branch
        True → [Fire weapon]
        False → (nothing)
```

## Step 3: Implement Hitscan Fire

When the trigger is pressed, cast a ray from the camera forward:

### 3a: Line Trace

1. Get the camera's **World Location** and **Forward Vector**
2. Calculate end point: `Start + (Forward * 50000)` (50,000 units = ~500 meters)
3. Add: **Line Trace By Channel** node
   - Start: Camera World Location
   - End: Calculated end point
   - Trace Channel: Visibility (or custom)
   - Check "Ignore Self"

### 3b: Handle Hit Result

1. From the Line Trace **Out Hit** result:
   - **Break Hit Result** to get: Hit Actor, Hit Location, Hit Normal
   - If **Hit Actor** is valid, apply damage:
     - **Apply Damage** node (Damage Amount, e.g., 25.0)
     - Or call a custom function on the hit actor

```
Fire Weapon:
    → Get World Location (PlayerCamera)
    → Get Forward Vector (PlayerCamera) × 50000 → Add to Location
    → Line Trace By Channel (Start, End)
        → Break Hit Result
            → Is Valid (Hit Actor) → Branch
                True → Apply Damage (Hit Actor, 25.0)
                False → (missed)
```

### 3c: Visual and Audio Feedback

1. **Muzzle Flash**: Spawn a particle emitter at the weapon mesh location
2. **Tracer Line**: Use **Draw Debug Line** (for prototyping) or spawn a niagara beam effect
3. **Impact Effect**: At Hit Location, spawn a particle/decal
4. **Sound**: Play a fire sound at the weapon location

```
On Fire:
    → Play Sound at Location (FireSound, Weapon Location)
    → Spawn Emitter at Location (MuzzleFlash, Weapon Muzzle)
    → [If hit]: Spawn Emitter at Location (ImpactEffect, Hit Location)
```

## Step 4: Add Fire Rate Cooldown

Prevent firing faster than intended:

1. Create a variable: `bCanFire` (Boolean, default: True)
2. Create a variable: `FireCooldown` (Float, default: 0.15 seconds)
3. Before firing, check `bCanFire`:
   - If True: fire, set `bCanFire = False`, then use **Delay** node (FireCooldown) → set `bCanFire = True`
   - If False: ignore the trigger press

```
Consume Trigger Press → Branch (True) →
    Branch (bCanFire == True) →
        True:
            → Set bCanFire = False
            → [Fire weapon logic]
            → Delay (FireCooldown)
            → Set bCanFire = True
```

## Step 5: Mock Input Fallback (Mouse Click)

For development without hardware, also bind the left mouse button:

1. Go to **Project Settings → Input → Action Mappings**
2. Add action "Fire" bound to **Left Mouse Button**
3. In the event graph, add **Input Action Fire (Pressed)**
4. Route it to the same fire logic

```
Input Action Fire (Pressed)
    → [Same fire weapon logic]
```

This way, mouse click and hardware trigger both fire the weapon.

## Step 6: Clean Up on End Play

```
Event EndPlay
    → Stop VN100
    → Stop Trigger
```

## Complete Event Graph Summary

```
BeginPlay:
    Start VN100 → Start Trigger → Print status

Tick:
    Update camera from VN100 orientation
    Consume Trigger Press → if true + bCanFire → Fire weapon → Cooldown

Fire Weapon:
    Line Trace from camera
    Apply damage on hit
    Spawn VFX/SFX
    Set bCanFire = false → Delay → Set bCanFire = true

EndPlay:
    Stop VN100
    Stop Trigger
```

## Debugging Tips

- Use **Print String** nodes to verify trigger presses are being detected
- Use **Draw Debug Line** to visualize your line trace during development
- Check the **Output Log** for "Trigger: Started on COMx" messages
- If trigger never fires: verify the Arduino is flashed and the correct COM port is set
- If trigger fires multiple times: the 100ms debounce is handled on the Arduino side, but ensure you're using `Consume Trigger Press` (not `Is Trigger Pressed` which doesn't clear the flag)

## Next Steps

- Proceed to **Tutorial 03** to set up a complete mock input system for hardware-free development
- Build your drone targets (actors that can receive damage)
- Design your scoring system
