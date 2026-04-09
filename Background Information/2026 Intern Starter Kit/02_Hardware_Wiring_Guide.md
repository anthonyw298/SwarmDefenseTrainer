# Hardware Wiring Guide — Trigger Button Assembly

This guide covers the physical wiring and assembly of the Arduino-based trigger button used in the Swarm Defense Trainer.

## Components Required

| Component | Quantity | Notes |
|-----------|----------|-------|
| Arduino Nano (or Uno) | 1 | Any Arduino board with a digital pin and USB |
| Momentary Push Button | 1 | Standard tactile switch or arcade-style button |
| Jumper Wires | 2 | Male-to-male or as needed for your button type |
| USB Cable | 1 | USB Mini-B (Nano) or USB-B (Uno) to USB-A |

**No external resistors are needed.** The Arduino's internal pull-up resistor is used.

## Wiring Diagram

```
                    Arduino Nano
                 ┌──────────────────┐
                 │                  │
                 │    D2 ──────────┤──── Button Pin 1
                 │                  │
                 │   GND ──────────┤──── Button Pin 2
                 │                  │
                 │   USB ──────────┤──── To PC
                 │                  │
                 └──────────────────┘

     Push Button (Momentary)
     ┌─────────┐
     │  ┌───┐  │
     │  │   │  │  Pin 1 ─── Arduino D2
     │  │ O │  │
     │  │   │  │  Pin 2 ─── Arduino GND
     │  └───┘  │
     └─────────┘
```

### Pin Connections

| Arduino Pin | Connects To | Purpose |
|-------------|-------------|---------|
| **D2** | Button terminal 1 | Digital input (active LOW with internal pull-up) |
| **GND** | Button terminal 2 | Ground reference |
| **USB** | PC USB port | Serial communication (115200 baud) |

### How It Works

1. Pin D2 is configured with an **internal pull-up resistor** (`INPUT_PULLUP`)
2. When the button is **not pressed**: D2 reads HIGH (pulled up to 5V internally)
3. When the button is **pressed**: D2 reads LOW (connected to GND through button)
4. The Arduino sends the character `'F'` over serial when it detects a LOW→press transition
5. A 100ms debounce prevents multiple fires from a single press
6. The built-in LED (pin 13) lights up as visual feedback when the button is pressed

## Flashing the Arduino

1. Open **Arduino IDE**
2. Open the sketch: `Arduino/TriggerButton/TriggerButton.ino`
3. Select your board:
   - **Tools → Board → Arduino Nano** (or your board type)
   - For Nano clones: **Tools → Processor → ATmega328P (Old Bootloader)**
4. Select the COM port: **Tools → Port → COMx** (the one that appeared when you plugged in)
5. Click **Upload** (→ arrow button)
6. Wait for "Done uploading"

### Verifying the Upload

1. Open **Tools → Serial Monitor**
2. Set baud rate to **115200** (bottom-right dropdown)
3. Press the button — you should see `F` characters appear in the monitor
4. **Close the Serial Monitor before running UE5** — only one application can use the COM port

## Physical Enclosure Considerations

Teams are expected to design and build a physical handheld device. Consider:

### Ergonomics
- The device should be comfortable to hold and aim with one or two hands
- Button placement should allow natural trigger-finger activation
- Consider the weight distribution — the VN-100 sensor adds weight

### Mounting the VN-100 IMU
- The VN-100 must be rigidly mounted to the device (it senses orientation)
- Mount it with the sensor's X-axis pointing forward (direction of aim)
- Avoid mounting near motors, magnets, or large metal objects (interferes with magnetometer)
- Secure with screws or strong adhesive — vibration will cause noisy readings

### Cable Management
- Two USB cables will exit the device: one for Arduino, one for VN-100
- Route cables so they don't interfere with aiming or trigger operation
- Consider a strain relief at the cable exit points

### Materials
- 3D printing (PLA/PETG) is recommended for the enclosure
- Alternatively: project boxes, PVC pipe, or laser-cut acrylic
- Keep total weight reasonable (target: under 1 kg / 2.2 lbs)

## Wiring Diagram — Complete System

```
┌─────────────────────────────────────────────────────┐
│                 HANDHELD DEVICE                      │
│                                                      │
│    ┌──────────┐        ┌──────────────┐             │
│    │  Arduino │        │   VN-100     │             │
│    │   Nano   │        │    IMU       │             │
│    │          │        │              │             │
│    │  D2──Button       │  TX/RX ──────┤── USB ──┐  │
│    │  GND─Button       │              │         │  │
│    │          │        └──────────────┘         │  │
│    │  USB ────┤──────────────────────────── USB ─┤  │
│    └──────────┘                                  │  │
│                                                  │  │
└──────────────────────────────────────────────────┤──┘
                                                   │
                              To PC USB ports ─────┘
                              (2 USB cables)

PC Side:
  COM3 ← VN-100 (orientation data)
  COM4 ← Arduino (trigger signals)
```

## Safety Notes

- Always use low-voltage components (5V USB-powered) — no high-voltage elements
- Ensure no exposed wiring on the exterior of the device
- The device is for indoor use only
- Handle USB cables carefully to avoid disconnection during use
