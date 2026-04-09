# Quick Start Guide — Swarm Defense Trainer

This guide walks you through getting the starter project running on your machine.

## Prerequisites

### Required Software

1. **Unreal Engine 5.4 or later**
   - Install via the [Epic Games Launcher](https://www.unrealengine.com/download)
   - During installation, ensure the "Starter Content" pack is included

2. **Visual Studio 2022** (Community Edition is free)
   - Install with the following workloads:
     - "Game development with C++"
     - ".NET desktop development" (required for UE5 build tools)
   - Under "Individual Components", ensure these are checked:
     - Windows 10/11 SDK (latest)
     - MSVC v143 build tools

3. **Arduino IDE** (for hardware trigger — only needed when working with physical hardware)
   - Download from [arduino.cc](https://www.arduino.cc/en/software)

### Hardware (Optional — Not needed for initial development)

- VectorNav VN-100 IMU sensor + USB cable
- Arduino Nano (or Uno) + push button + USB cable
- 2x jumper wires (for button wiring)

## Step 1: Open the Project

1. Navigate to `Starter_Project/`
2. Double-click `SwarmDefenseStarter.uproject`
3. If prompted, select your installed UE5 version
4. UE5 will generate Visual Studio project files — this takes 1-2 minutes on first launch
5. The project will compile the C++ modules — wait for the "Compiling Shaders" progress bar to complete

**If compilation fails:**
- Ensure Visual Studio 2022 is installed with the C++ game development workload
- Right-click the `.uproject` file → "Generate Visual Studio project files"
- Open the `.sln` file in Visual Studio and build from there

## Step 2: Verify Plugins Are Loaded

1. In the UE5 Editor, go to **Edit → Plugins**
2. Search for "VN100" — verify "VN-100 Input Plugin" is enabled
3. Search for "Trigger" — verify "Hardware Trigger Plugin" is enabled
4. If either is disabled, enable it and restart the editor

## Step 3: Create Your First Level

1. **File → New Level → Empty Level**
2. Save it as `Maps/TestLevel`
3. Add a **Player Start** actor to the level
4. Press **Play** (Alt+P) to test — you should see an empty world with a default camera

## Step 4: Verify Mock Input Mode

The project is pre-configured for **Mock Input** (mouse + keyboard). You do not need hardware to develop.

To verify: Open `Config/DefaultGame.ini` and confirm:
```ini
InputMode=MockInput
```

## Step 5: Test the Plugin Blueprint Nodes

1. Create a new Blueprint class (Actor or GameMode)
2. In the Event Graph, right-click and search for "VN100" or "Trigger"
3. You should see all the plugin nodes available:
   - Under **VN100** category: Start VN100, Stop VN100, Get VN100 Orientation, Is VN100 Connected
   - Under **Trigger** category: Start Trigger, Stop Trigger, Consume Trigger Press, Is Trigger Pressed, Is Trigger Connected

If these nodes don't appear, the plugins may not be compiled. Check the Output Log for errors.

## Step 6: Switching to Hardware Input Mode

When you're ready to test with physical hardware:

1. Open `Config/DefaultGame.ini`
2. Change:
   ```ini
   InputMode=HardwareInput
   ```
3. Set the correct COM ports:
   ```ini
   OrientationSerialPort=COM3
   TriggerSerialPort=COM4
   ```
   To find your COM port numbers:
   - Open **Device Manager** on Windows
   - Expand "Ports (COM & LPT)"
   - Plug/unplug the USB device to see which port appears

4. Flash the Arduino with the provided sketch (`Arduino/TriggerButton/TriggerButton.ino`)

## Troubleshooting

### "Module could not be compiled" on startup
- Ensure Visual Studio 2022 is installed with C++ game dev workload
- Delete the `Intermediate/` and `Binaries/` folders, then reopen the project

### Plugin nodes don't appear in Blueprints
- Check **Edit → Plugins** to ensure both plugins are enabled
- Check the **Output Log** (Window → Developer Tools → Output Log) for errors

### Serial port "Access Denied"
- Close any other application using the COM port (Arduino IDE Serial Monitor, PuTTY, etc.)
- Only one application can use a COM port at a time

### Game runs but no input response
- Verify `InputMode` in `DefaultGame.ini` matches your setup (MockInput or HardwareInput)
- For hardware mode, verify COM port numbers match your connected devices

## Next Steps

- Follow the **Blueprint Tutorials** in `03_Blueprint_Tutorials/` to learn how to:
  - Use the VN-100 to control a camera
  - Use the trigger button to fire a weapon
  - Set up mock input for development without hardware
- Begin designing your game systems (drones, waves, scoring, HUD)
- Refer to `04_Plugin_API_Reference.md` for detailed function documentation
