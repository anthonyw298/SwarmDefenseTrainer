# Blueprint Tutorial 3: Mock Input Mode (Development Without Hardware)

This tutorial shows you how to set up a mouse-and-keyboard input system so you can develop and test the full game without needing the VN-100 sensor or Arduino trigger hardware.

## Why Mock Input?

- **Not everyone has hardware access** — Mock mode lets the entire team develop and test
- **Faster iteration** — No need to plug in devices during rapid prototyping
- **Required for Tier 1/2 laptops** — Only Tier 3 laptops have the USB ports and drivers needed for hardware

## Understanding the Input Mode Setting

In `Config/DefaultGame.ini`:

```ini
[/Script/SwarmDefenseTrainer.SDTSettings]
InputMode=MockInput          ; Change to "HardwareInput" for physical hardware
```

Your Blueprint logic should check this setting to decide which input source to use.

## Step 1: Create an Input Mode Check

### Option A: Simple Boolean Variable

1. In your pawn Blueprint, create a variable: `bUseHardware` (Boolean, default: False)
2. On BeginPlay, read the config setting (or just use the variable default for now)
3. All input logic branches on this variable

### Option B: Read from Config (Advanced)

1. Use the **Get Game Instance** node to access settings
2. Or simply check if `Start VN100` returns true — if hardware isn't connected, fall back to mock

```
Event BeginPlay
    → Start VN100 ("COM3", 115200) → Branch
        True → Set bUseHardware = True
        False → Set bUseHardware = False (mock mode)
```

## Step 2: Mouse Look (Replacing VN-100)

When in mock mode, use mouse movement to control the camera:

### 2a: Enable Mouse Input

1. In your **Player Controller** Blueprint (or create one: `BP_PlayerController`):
   - Set **Show Mouse Cursor** = False
   - Set **Input Mode** = Game Only (use **Set Input Mode Game Only** on BeginPlay)

### 2b: Read Mouse Delta

1. In your pawn's **Event Tick**:
2. Add: **Get Input Mouse Delta** node (returns Delta X and Delta Y)
3. Multiply by sensitivity variables:
   - `MouseSensitivity` (Float, default: 0.3)
4. Add delta to current rotation:

```
Event Tick (Mock Mode branch):
    → Get Input Mouse Delta (DeltaX, DeltaY)
    → CurrentYaw += DeltaX × MouseSensitivity
    → CurrentPitch = Clamp(CurrentPitch - DeltaY × MouseSensitivity, -89, 89)
    → Set World Rotation (PlayerCamera, Pitch: CurrentPitch, Yaw: CurrentYaw, Roll: 0)
```

### 2c: Variables Needed

| Variable | Type | Default | Purpose |
|----------|------|---------|---------|
| `CurrentYaw` | Float | 0.0 | Accumulated yaw rotation |
| `CurrentPitch` | Float | 0.0 | Accumulated pitch rotation |
| `MouseSensitivity` | Float | 0.3 | Mouse look speed |

## Step 3: Mouse Click (Replacing Trigger)

1. In **Project Settings → Input → Action Mappings**:
   - Add: `Fire` → **Left Mouse Button**
2. In the pawn Blueprint:
   - Add: **Input Action Fire (Pressed)** event
   - Route to the same fire logic from Tutorial 02

```
Input Action Fire (Pressed)
    → [Fire weapon logic — same as Tutorial 02]
```

## Step 4: Combined Input Graph

Here's how to structure your Event Tick to support both modes:

```
Event Tick
    → Branch (bUseHardware)

    [True — Hardware Mode]:
        → Is VN100 Connected → Branch
            True → Get VN100 Orientation → Apply to camera
        → Consume Trigger Press → Branch
            True → Fire weapon

    [False — Mock Mode]:
        → Get Input Mouse Delta → Apply to camera
        → (Fire is handled by Input Action, not tick)
```

## Step 5: Keyboard Bindings

Add these input bindings in **Project Settings → Input → Action Mappings**:

| Action | Key | Purpose |
|--------|-----|---------|
| `Fire` | Left Mouse Button | Fire weapon |
| `Calibrate` | C | Reset VN-100 forward direction |
| `Restart` | R | Restart the current game |
| `StartGame` | Space Bar | Start/begin a round |
| `ToggleInput` | T | Switch between Mock and Hardware mode at runtime |

### Toggle Input Mode at Runtime (Optional)

```
Input Action ToggleInput (Pressed)
    → Flip Flop
        A: Set bUseHardware = True, Start VN100, Start Trigger, Print "Hardware Mode"
        B: Set bUseHardware = False, Stop VN100, Stop Trigger, Print "Mock Mode"
```

## Step 6: Testing Mock Mode

1. Ensure `InputMode=MockInput` in `Config/DefaultGame.ini`
2. Press **Play** in the UE5 Editor
3. Move the mouse — camera should rotate
4. Click left mouse button — weapon should fire
5. The game should be fully playable without any hardware connected

## Architecture Recommendation

For clean code, consider creating two Actor Components:

1. **`BP_MockInputComponent`** — handles mouse look + mouse click
2. **`BP_HardwareInputComponent`** — handles VN-100 + Arduino trigger

Your pawn activates one or the other based on the input mode setting. This keeps the logic separated and easy to maintain.

```
BP_SensorPawn
    ├── BP_MockInputComponent (active when bUseHardware == false)
    └── BP_HardwareInputComponent (active when bUseHardware == true)
```

## Next Steps

- Your team can now develop the full game using only mouse and keyboard
- Test with hardware periodically to verify the integration still works
- Focus on game design: drones, waves, scoring, HUD, sound effects
