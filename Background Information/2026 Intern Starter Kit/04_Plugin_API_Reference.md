# Plugin API Reference

This document provides the complete API reference for the two provided UE5 plugins. These plugins are pre-built and should not be modified — use the Blueprint nodes they expose to integrate hardware into your game.

---

## VN100Input Plugin

**Purpose:** Reads orientation data (yaw, pitch, roll) from a VectorNav VN-100 Inertial Measurement Unit (IMU) connected via USB serial port.

**How it works:** The plugin spawns a background thread that continuously reads serial data from the VN-100. The sensor outputs ASCII sentences in NMEA-like format (`$VNYPR,yaw,pitch,roll*checksum`). The plugin parses these sentences, validates the checksum, and stores the latest orientation in a thread-safe variable accessible from the game thread.

### Blueprint Nodes

All nodes are found under the **"VN100"** category in the Blueprint node search.

---

#### Start VN100

```
Category:    VN100
Node Type:   BlueprintCallable (has execution pins)
```

**Description:** Opens the serial port and starts reading orientation data from the VN-100 sensor.

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Port Name | String | (required) | Serial port name. Windows: `"COM3"`, `"COM4"`, etc. Mac: `"/dev/tty.usbserial-XXXX"` |
| Baud Rate | Integer | 115200 | Communication speed. The VN-100 defaults to 115200. |

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if the port opened successfully, `false` if the port doesn't exist or is in use |

**Notes:**
- Only one VN-100 connection can be active at a time
- Calling Start when already connected will stop the previous connection first
- If the port is used by another application (e.g., VectorNav SensorExplorer), this will fail

---

#### Stop VN100

```
Category:    VN100
Node Type:   BlueprintCallable
```

**Description:** Stops the reader thread and closes the serial port. Safe to call even if not connected.

| Parameters | None |
|------------|------|

| Return | None |
|--------|------|

**Notes:**
- Always call this on EndPlay to properly release the serial port
- The plugin module also calls this automatically on shutdown, but explicit cleanup is best practice

---

#### Get VN100 Orientation

```
Category:    VN100
Node Type:   BlueprintPure (no execution pins — can be called inline)
```

**Description:** Returns the latest orientation reading from the VN-100 as a Rotator.

| Parameters | None |
|------------|------|

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Rotator | `Pitch` (X), `Yaw` (Y), `Roll` (Z) in degrees. Returns `(0,0,0)` if not connected. |

**Notes:**
- This is a non-blocking read of the latest cached value — it does not perform I/O
- The value updates at the sensor's output rate (typically 40-800 Hz depending on VN-100 configuration)
- Values are in degrees: Yaw (0-360), Pitch (-90 to +90), Roll (-180 to +180)
- The rotator maps VN-100 axes as: Yaw → Rotator.Yaw, Pitch → Rotator.Pitch, Roll → Rotator.Roll

---

#### Is VN100 Connected

```
Category:    VN100
Node Type:   BlueprintPure
```

**Description:** Returns whether the VN-100 is currently connected and actively reading data.

| Parameters | None |
|------------|------|

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if serial port is open and reader thread is running |

**Notes:**
- Use this to implement fallback logic (switch to mock input if hardware is disconnected)
- Does not detect if the sensor is physically removed — it detects if the serial port is open

---

## HardwareTrigger Plugin

**Purpose:** Reads fire button signals from an Arduino connected via USB serial port. The Arduino sends the byte `'F'` (0x46) when the trigger button is pressed.

**How it works:** The plugin spawns a background thread that continuously reads bytes from the serial port. When it detects the `'F'` character, it sets a thread-safe flag. The game thread can then check and consume this flag.

### Blueprint Nodes

All nodes are found under the **"Trigger"** category in the Blueprint node search.

---

#### Start Trigger

```
Category:    Trigger
Node Type:   BlueprintCallable
```

**Description:** Opens the serial port and starts listening for trigger button signals.

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Port Name | String | (required) | Serial port name. Same format as VN100. |
| Baud Rate | Integer | 115200 | Must match the Arduino sketch baud rate (115200). |

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if the port opened successfully |

---

#### Stop Trigger

```
Category:    Trigger
Node Type:   BlueprintCallable
```

**Description:** Stops the reader thread and closes the serial port.

---

#### Consume Trigger Press

```
Category:    Trigger
Node Type:   BlueprintCallable
```

**Description:** Checks if the trigger was pressed since the last call, then **clears the flag**. This is the recommended way to check for trigger presses.

| Parameters | None |
|------------|------|

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if trigger was pressed since last call to ConsumeTriggerPress |

**Notes:**
- **Use this, not IsTriggerPressed**, for firing logic — it prevents duplicate fires
- The flag is set by the background thread when `'F'` is received
- After returning `true`, the flag is cleared — subsequent calls return `false` until the next press
- Call this once per frame (in Event Tick) for responsive input

---

#### Is Trigger Pressed

```
Category:    Trigger
Node Type:   BlueprintCallable
```

**Description:** Checks if the trigger flag is set **without clearing it**. Useful for UI indicators.

| Parameters | None |
|------------|------|

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if trigger has been pressed and flag hasn't been consumed |

**Notes:**
- This does NOT clear the flag — repeated calls return `true` until `ConsumeTriggerPress` is called
- Use for visual indicators (e.g., "trigger held" UI element), not for firing logic

---

#### Is Trigger Connected

```
Category:    Trigger
Node Type:   BlueprintPure
```

**Description:** Returns whether the Arduino trigger is connected and reader is active.

| Parameters | None |
|------------|------|

| Return | Type | Description |
|--------|------|-------------|
| Return Value | Boolean | `true` if serial port is open and reader thread is running |

---

## Configuration Reference

Settings are stored in `Config/DefaultGame.ini`:

```ini
[/Script/SwarmDefenseTrainer.SDTSettings]
InputMode=MockInput              ; "MockInput" or "HardwareInput"
OrientationSerialPort=COM3       ; VN-100 COM port
TriggerSerialPort=COM4           ; Arduino COM port
BaudRate=115200                  ; Serial baud rate (both devices)
YawSensitivity=1.0               ; Yaw rotation multiplier
PitchSensitivity=1.0             ; Pitch rotation multiplier
SmoothingAlpha=0.3               ; Exponential smoothing (0.0-1.0, lower = smoother)
DeadZoneDegrees=1.0              ; Ignore orientation changes smaller than this
FireCooldown=0.15                ; Minimum seconds between shots
```

## Finding Your COM Port Numbers

1. Open **Device Manager** on Windows (Win+X → Device Manager)
2. Expand **"Ports (COM & LPT)"**
3. Plug in the VN-100 USB cable — note which new COM port appears (e.g., COM3)
4. Plug in the Arduino USB cable — note which new COM port appears (e.g., COM4)
5. Update `DefaultGame.ini` with the correct port numbers

## Common Issues

| Issue | Cause | Solution |
|-------|-------|----------|
| `Start VN100` returns false | Wrong COM port or port in use | Check Device Manager, close other serial apps |
| Orientation is jittery | Sensor vibration or low smoothing | Increase `SmoothingAlpha` or mount sensor firmly |
| Trigger fires twice per press | Debounce issue | Should not happen — Arduino has 100ms debounce. Check for multiple `ConsumeTriggerPress` calls per frame |
| Nodes don't appear in Blueprint | Plugin not enabled | Edit → Plugins → enable both plugins, restart editor |
| Game freezes when starting hardware | Blocking serial call | Should not happen — serial runs on background thread. Check Output Log for errors |
