# 2026 TLS Intern Starter Kit — Swarm Defense Trainer

## Overview

This starter kit contains everything your team needs to begin developing the Counter-UAS Swarm Defense Training System. The project involves building an Unreal Engine 5 simulation where a trainee defends a High-Value Asset (HVA) against incoming drone swarms, using either mouse/keyboard (mock mode) or physical hardware (VN-100 IMU + Arduino trigger button).

## What's Included

### Documentation

| Document | Description |
|----------|-------------|
| `01_Quick_Start_Guide.md` | Step-by-step instructions to get the starter project running |
| `02_Hardware_Wiring_Guide.md` | Wiring diagrams and assembly instructions for the Arduino trigger device |
| `03_Blueprint_Tutorials/` | Step-by-step tutorials for using the provided plugins in UE5 Blueprints |
| `04_Plugin_API_Reference.md` | Complete API reference for the VN100Input and HardwareTrigger plugins |
| `05_Trade_Study_Template.md` | Template for conducting trade study analyses |
| `06_Collaboration_Guide.md` | **Read first.** Git + LFS setup, Blueprint ownership rules, weekly workflow, and conflict resolution |

### Starter Project (`Starter_Project/`)

A minimal Unreal Engine 5 project pre-configured with both hardware plugins. This is your starting point — open it in UE5, and you'll have a compiling project with hardware integration ready to go.

**Contents:**
- `SwarmDefenseStarter.uproject` — UE5 project file
- `Config/` — Pre-configured input bindings, game settings, and mock/hardware mode toggle
- `Source/SwarmDefenseStarter/` — Minimal C++ game module (compile target only)
- `Plugins/VN100Input/` — VN-100 orientation sensor plugin (provided, do not modify)
- `Plugins/HardwareTrigger/` — Arduino trigger button plugin (provided, do not modify)
- `Arduino/TriggerButton/` — Arduino sketch to flash onto trigger hardware

## Getting Started (Summary)

1. Install **Unreal Engine 5.4+** via the Epic Games Launcher
2. Install **Visual Studio 2022** with the "Game development with C++" workload
3. Open `Starter_Project/SwarmDefenseStarter.uproject` — UE5 will generate project files and compile
4. The project starts in **Mock Input** mode (mouse + keyboard) — no hardware needed
5. Follow the Blueprint tutorials to build your game systems
6. When hardware is available, switch to **Hardware Input** mode in `Config/DefaultGame.ini`

## Provided Plugins (Do Not Modify)

These plugins handle all serial communication with the hardware. They are provided as pre-built libraries. You do **not** need to understand the C++ source code — just use the Blueprint nodes they expose.

### VN100Input Plugin
Reads orientation data (yaw, pitch, roll) from a VectorNav VN-100 IMU sensor connected via USB serial.

**Blueprint Nodes:**
- `Start VN100` — Connect to the sensor
- `Stop VN100` — Disconnect
- `Get VN100 Orientation` — Returns current yaw/pitch/roll as a Rotator
- `Is VN100 Connected` — Check connection status

### HardwareTrigger Plugin
Reads fire button signals from an Arduino connected via USB serial.

**Blueprint Nodes:**
- `Start Trigger` — Connect to the Arduino
- `Stop Trigger` — Disconnect
- `Consume Trigger Press` — Returns true if trigger was pressed (clears the flag)
- `Is Trigger Pressed` — Non-consuming check
- `Is Trigger Connected` — Check connection status

## Team Structure Recommendation

| Role | Count | Responsibilities |
|------|-------|-----------------|
| Program Manager | 1 | Schedule, milestones, stakeholder communication |
| Software Lead | 1-2 | UE5 Blueprint/C++ architecture decisions |
| Game Developers | 3-5 | Level design, drone behavior, HUD, scoring |
| Hardware Engineers | 2-3 | Trigger device design, Arduino, wiring, 3D printing |
| Documentation / QA | 1-2 | Trade studies, testing, presentation materials |

## Important Notes

- **Mock Input Mode** allows full development and testing without hardware. Use this for the majority of development.
- **Only team members with Tier 3 laptops can run Unreal Engine.** Plan work accordingly — assign non-UE5 tasks (hardware design, trade studies, documentation, presentation) to team members without Tier 3 access.
- **Use Blueprint visual scripting** as your primary development approach. C++ is optional and only recommended for advanced features.
- All events and SME sessions will be offered both in-person and on Microsoft Teams.
