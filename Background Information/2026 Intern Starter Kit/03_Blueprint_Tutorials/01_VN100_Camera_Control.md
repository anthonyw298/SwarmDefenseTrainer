# Blueprint Tutorial 1: VN-100 Camera Control

This tutorial shows you how to create a Blueprint that reads orientation data from the VN-100 IMU sensor and uses it to control a camera (player's aim direction).

## What You'll Build

A first-person pawn that aims based on the VN-100 sensor orientation. When you physically rotate the handheld device, the in-game camera rotates to match.

## Prerequisites

- Starter project open in UE5
- VN100Input plugin enabled (Edit → Plugins → search "VN100")

## Step 1: Create the Player Pawn Blueprint

1. In the Content Browser, right-click → **Blueprint Class**
2. Select **Pawn** as the parent class
3. Name it `BP_SensorPawn`
4. Double-click to open the Blueprint Editor

## Step 2: Add Components

In the Components panel, add:

1. **Scene** (root — rename to `Root`)
2. **Camera** (child of Root — rename to `PlayerCamera`)
   - Set Location to (0, 0, 170) for approximate eye height
3. **Static Mesh** (optional, child of Root — for a weapon model)

## Step 3: Initialize the Sensor on Begin Play

In the **Event Graph**:

1. Drag from **Event BeginPlay**
2. Search and add: **Start VN100** node (under the VN100 category)
3. Set **Port Name** input to: `COM3` (or your VN-100's COM port)
4. Set **Baud Rate** to: `115200`
5. The **Return Value** (bool) tells you if connection succeeded
6. Connect a **Branch** node to handle success/failure
7. On failure, add a **Print String** node: `"VN-100 connection failed — check COM port"`

```
Event BeginPlay
    → Start VN100 (Port Name: "COM3", Baud Rate: 115200)
        → Branch (Condition: Return Value)
            True → Print String ("VN-100 connected")
            False → Print String ("VN-100 connection failed")
```

## Step 4: Read Orientation Every Tick

1. Drag from **Event Tick**
2. Add: **Is VN100 Connected** node
3. Add: **Branch** node (condition: Is VN100 Connected return value)
4. On True:
   - Add: **Get VN100 Orientation** node — this returns an **FRotator** (Pitch, Yaw, Roll)
   - Add: **Set Actor Rotation** or **Set World Rotation** on the Camera component
   - Connect the Rotator output from Get VN100 Orientation to the New Rotation input

```
Event Tick
    → Is VN100 Connected
        → Branch
            True → Get VN100 Orientation
                     → Set World Rotation (Target: PlayerCamera, New Rotation: [VN100 output])
            False → (do nothing, or use mock input fallback)
```

## Step 5: Add Sensitivity and Smoothing (Optional)

For a smoother experience, add exponential smoothing:

1. Create a variable: `SmoothedRotation` (type: Rotator)
2. Create a variable: `SmoothingAlpha` (type: Float, default: 0.3)
3. On each tick:
   - Get VN100 Orientation → store in `RawRotation`
   - Use **RInterp To** (or manual lerp):
     ```
     SmoothedRotation = RInterp To(SmoothedRotation, RawRotation, DeltaTime, InterpSpeed)
     ```
   - Apply `SmoothedRotation` to the camera instead of raw values

For sensitivity scaling:
1. Create variables: `YawSensitivity` (Float, default 1.0) and `PitchSensitivity` (Float, default 1.0)
2. Break the rotator → multiply Yaw by YawSensitivity, Pitch by PitchSensitivity → make rotator

## Step 6: Add Calibration

The VN-100 reports absolute orientation. You'll want to calibrate a "forward" direction:

1. Create a variable: `CalibrationOffset` (type: Rotator)
2. Create a custom event: `Calibrate`
3. On Calibrate: set `CalibrationOffset = Get VN100 Orientation` (capture current orientation)
4. On each tick: subtract `CalibrationOffset` from the raw orientation before applying
5. Bind Calibrate to a key (e.g., C key) using an **Input Action**

```
Custom Event: Calibrate
    → Get VN100 Orientation → Set CalibrationOffset

Event Tick (modified):
    → Get VN100 Orientation → Subtract CalibrationOffset
        → Apply to camera rotation
```

## Step 7: Clean Up on End Play

1. Drag from **Event EndPlay**
2. Add: **Stop VN100** node
3. This properly closes the serial port when the game ends

```
Event EndPlay
    → Stop VN100
```

## Step 8: Test It

1. Place `BP_SensorPawn` in your level
2. Set it as the **Default Pawn** in your GameMode
3. If using Mock Input: temporarily replace the VN-100 reading with mouse input for testing
4. If hardware is connected: press Play and move the sensor to see the camera respond

## Full Node Graph Summary

```
Event BeginPlay → Start VN100 ("COM3", 115200) → Branch → Print success/failure

Event Tick → Is VN100 Connected → Branch (True) →
    Get VN100 Orientation → Subtract CalibrationOffset →
    RInterp To (SmoothedRotation, target, DeltaTime, 10.0) →
    Set World Rotation (PlayerCamera)

Input Action (C key) → Get VN100 Orientation → Set CalibrationOffset

Event EndPlay → Stop VN100
```

## Next Steps

- Proceed to **Tutorial 02** to add trigger button firing
- Proceed to **Tutorial 03** to set up mock input fallback for development without hardware
