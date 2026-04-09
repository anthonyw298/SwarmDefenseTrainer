# 06 — Team Collaboration Guide
### Swarm Defense Trainer | 2026 TLS Intern Program

---

## Table of Contents

1. [Why This Guide Exists](#1-why-this-guide-exists)
2. [Version Control Overview](#2-version-control-overview)
3. [One-Time Setup (Every Team Member)](#3-one-time-setup-every-team-member)
4. [Blueprint Architecture and File Ownership](#4-blueprint-architecture-and-file-ownership)
5. [Blueprint Interfaces — How Systems Talk to Each Other](#5-blueprint-interfaces--how-systems-talk-to-each-other)
6. [Weekly Workflow — Step by Step](#6-weekly-workflow--step-by-step)
7. [Commit and Push Guidelines](#7-commit-and-push-guidelines)
8. [Handling Conflicts](#8-handling-conflicts)
9. [Non-UE5 Contributors](#9-non-ue5-contributors)
10. [Team Lead Responsibilities](#10-team-lead-responsibilities)
11. [Quick Reference Cheat Sheet](#11-quick-reference-cheat-sheet)

---

## 1. Why This Guide Exists

Unreal Engine 5 (UE5) uses **binary asset files** (`.uasset`, `.umap`) for Blueprints, levels, and materials. Unlike code files, binary files cannot be automatically merged by Git. If two people edit the same Blueprint and both try to push their changes, one person's work will overwrite the other's — with no warning.

This guide establishes the architecture, ownership rules, and workflows that prevent this from happening. **Read this guide before writing a single line of Blueprint logic.**

Following these rules is not optional. A single ownership violation can cost the team hours of lost work.

---

## 2. Version Control Overview

### What We Use

**Git + Git LFS (Large File Storage)** hosted on **GitHub**.

- **Git** handles all text files: code, config files, documentation, `.gitignore`, Arduino sketches.
- **Git LFS** handles all binary files: UE5 Blueprints (`.uasset`), maps (`.umap`), textures, and other large assets. LFS stores these files on GitHub's servers and replaces them in the repo with small pointer files.

### Why Not Perforce or Plastic SCM?

Perforce and Plastic SCM (Unreal Version Control) are purpose-built for game development and handle binary files better. However, they require a server to be set up and maintained, and they have user limits on free tiers that don't accommodate teams of 10–15. Git + Git LFS is well-understood, free, and sufficient for a project of this scope.

### The Golden Rule

> **One person edits one Blueprint at a time. Ownership is assigned in advance. Nobody edits a file they do not own without explicit coordination with the owner.**

---

## 3. One-Time Setup (Every Team Member)

Complete these steps exactly once before your first working session.

---

### Step 1 — Install Git

1. Go to [https://git-scm.com/download/win](https://git-scm.com/download/win) and download the Windows installer.
2. Run the installer. On the "Adjusting your PATH environment" screen, select **"Git from the command line and also from 3rd-party software"**.
3. Leave all other options at their defaults and complete the installation.
4. Open a new Command Prompt or PowerShell and verify:
   ```
   git --version
   ```
   You should see something like `git version 2.44.0.windows.1`.

---

### Step 2 — Install Git LFS

Git LFS is a Git extension and must be installed separately.

1. Go to [https://git-lfs.com](https://git-lfs.com) and download the Windows installer.
2. Run the installer.
3. Open a Command Prompt and run:
   ```
   git lfs install
   ```
   You should see: `Git LFS initialized.`

> **Important:** Git LFS must be installed before you clone the repository. If you clone first, LFS-tracked files will appear as small pointer text files instead of real assets.

---

### Step 3 — Install GitHub Desktop (Recommended for Beginners)

GitHub Desktop provides a graphical interface for Git that is much easier to use than the command line for most intern tasks.

1. Download from [https://desktop.github.com](https://desktop.github.com).
2. Sign in with your GitHub account.
3. You can use GitHub Desktop for all day-to-day operations (pull, commit, push, view history). The command line instructions in this guide are provided as reference — GitHub Desktop can perform all of the same actions through its UI.

---

### Step 4 — Clone the Repository

Your team lead will provide the GitHub repository URL. Clone it to your local machine.

**Via GitHub Desktop:**
1. Open GitHub Desktop → File → Clone Repository
2. Click the URL tab and paste the repository URL
3. Choose a local path (e.g., `C:\Projects\SwarmDefenseTrainer`)
4. Click **Clone**

**Via Command Line:**
```bash
git clone <repository-url> SwarmDefenseTrainer
cd SwarmDefenseTrainer
```

After cloning, verify LFS files downloaded correctly — `.uasset` files should be several kilobytes or more, not a few hundred bytes. If they look like small text files, run:
```bash
git lfs pull
```

---

### Step 5 — Configure Your Git Identity

This tags your commits with your name and email. Run once:

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

---

### Step 6 — Connect UE5 to GitHub (Optional but Recommended)

UE5 has a built-in source control panel that lets you see file status, check out files, and submit changes without leaving the editor.

1. Open the project in UE5.
2. In the bottom-right corner of the editor, click the **Source Control** icon (looks like a branching diagram).
3. Select **Git (beta)** as the provider.
4. Set the repository path to your cloned folder.
5. Click **Accept Settings**.

Once connected, assets in the Content Browser will show colored indicators:
- **Green checkmark** — Up to date, no local changes
- **Red exclamation** — Modified locally, not yet committed
- **Blue arrow** — Added, not yet committed
- **Lock icon** — Locked by a team member (only visible if using LFS locking)

---

### Step 7 — Verify Your Setup

Before your first working session, confirm the following with your team lead:

- [ ] Git and Git LFS are installed
- [ ] Repository is cloned and `.uasset` files are real size (not pointer files)
- [ ] Your name is on the file ownership table (see Section 4)
- [ ] You know which Blueprint(s) you own
- [ ] You can open the project in UE5 and it compiles successfully

---

## 4. Blueprint Architecture and File Ownership

### Project Blueprint Structure

The following table defines every major Blueprint in the project, who owns it, and what it does. This is the **authoritative source** for ownership. When in doubt, check here before editing anything.

| Blueprint File | Owner Role | Description |
|---|---|---|
| `BP_GameMode` | Software Lead | Core game rules, wave spawning logic, game state |
| `BP_DroneEnemy_Base` | Software Lead | Parent drone class — shared movement and health logic |
| `BP_DroneEnemy_Basic` | Game Developer 1 | Basic drone variant — inherits from Base |
| `BP_DroneEnemy_Advanced` | Game Developer 1 | Advanced/fast drone variant — inherits from Base |
| `BP_WeaponComponent` | Game Developer 2 | Shooting logic — fire rate, projectile spawn, cooldown |
| `BP_ProjectileBase` | Game Developer 2 | Projectile movement, collision, damage |
| `BP_PlayerController` | Game Developer 3 | Input handling, connects hardware inputs to player actions |
| `BP_HUD` | Game Developer 3 | On-screen display — ammo, health, score, wave counter |
| `BP_ScoreManager` | Game Developer 4 | Score tracking, kill counting, end-of-game results |
| `BP_WaveManager` | Game Developer 4 | Wave configuration, spawn timing, difficulty progression |
| `L_BattleArena` | Software Lead | Main game level/map |
| `L_TestDev_[Name]` | Each Developer | Personal test level — safe to edit freely |

> **How to read this table:** "Owner Role" refers to the role assigned to a team member at the start of the project. Your team lead will assign specific names to each role during your project kickoff meeting. Once assigned, that person is the sole editor of that file for the duration of the project.

### Personal Test Levels

Every developer on UE5 should create their own personal test level (`L_TestDev_YourName`) immediately after setup. This is a sandbox — you can place any actors, test any Blueprint, and break things freely. Changes to your personal test level never affect other team members.

**Never develop or test in `L_BattleArena` directly.** The main level is only modified by the Software Lead during integration sessions.

### Requesting a File Change from Another Owner

If you need logic changed in a Blueprint you don't own:

1. Open a GitHub Issue describing the change you need and why.
2. Tag the file owner in the issue.
3. The owner reviews the request and either implements it or discusses an alternative.
4. Never modify another person's Blueprint directly, even to "fix a quick thing."

---

## 5. Blueprint Interfaces — How Systems Talk to Each Other

### The Problem

If `BP_WaveManager` needs to tell `BP_DroneEnemy` to start moving, it needs a reference to `BP_DroneEnemy`. But what if the drone developer refactors `BP_DroneEnemy` and renames a function? Now `BP_WaveManager` is broken — and the developer who owns it had no idea anything changed.

This kind of hidden dependency between files is the primary cause of integration failures in team projects.

### The Solution — Blueprint Interfaces (BPIs)

A Blueprint Interface is a contract. It defines function signatures (name, inputs, outputs) without any implementation. Any Blueprint can implement the interface and any other Blueprint can call it — without either one needing to know anything about the other's internals.

Think of it like a wall outlet: the appliance (caller) doesn't know anything about how your house's wiring works, and your house wiring doesn't know what appliance is plugged in. The outlet shape is the interface.

### Project Interface Definitions

The Software Lead creates these interfaces on Day 1. They are locked — no other team member modifies them without a team discussion.

---

#### `BPI_Enemy` — Implemented by all drone Blueprints

| Function | Inputs | Outputs | Description |
|---|---|---|---|
| `InitializeDrone` | SpawnConfig (struct) | — | Called when drone spawns — sets speed, health, type |
| `TakeDamage` | DamageAmount (float) | — | Apply damage to the drone |
| `GetCurrentHealth` | — | Health (float) | Returns current health value |
| `OnDroneDestroyed` | — | — | Event — called when drone is killed |

*Implemented by:* `BP_DroneEnemy_Base` (and all child drones inherit it automatically)
*Called by:* `BP_WaveManager`, `BP_ProjectileBase`

---

#### `BPI_Weapon` — Implemented by the weapon component

| Function | Inputs | Outputs | Description |
|---|---|---|---|
| `FireWeapon` | — | — | Attempt to fire — checks cooldown internally |
| `IsReadyToFire` | — | Ready (bool) | Returns whether weapon is off cooldown |
| `GetAmmoCount` | — | Ammo (int) | Returns current ammo |
| `ReloadWeapon` | — | — | Trigger a reload sequence |

*Implemented by:* `BP_WeaponComponent`
*Called by:* `BP_PlayerController`

---

#### `BPI_Scoreable` — Implemented by anything that contributes to score

| Function | Inputs | Outputs | Description |
|---|---|---|---|
| `GetPointValue` | — | Points (int) | Returns how many points this actor is worth |
| `OnScored` | — | — | Event — called when this actor's score is collected |

*Implemented by:* `BP_DroneEnemy_Base`
*Called by:* `BP_ScoreManager`

---

#### `BPI_HUDProvider` — Implemented by game systems that feed the HUD

| Function | Inputs | Outputs | Description |
|---|---|---|---|
| `GetCurrentScore` | — | Score (int) | Current total score |
| `GetCurrentWave` | — | Wave (int) | Current wave number |
| `GetDronesRemaining` | — | Count (int) | Drones still alive in current wave |
| `GetAmmoStatus` | — | Ammo (int), MaxAmmo (int) | Current and max ammo |

*Implemented by:* `BP_ScoreManager` (score/wave), `BP_WeaponComponent` (ammo)
*Called by:* `BP_HUD`

---

### How to Use an Interface in Blueprint

**Implementing an interface (receiving calls):**
1. Open your Blueprint.
2. Go to Class Settings → Interfaces → Add.
3. Select the interface (e.g., `BPI_Enemy`).
4. In your Event Graph, right-click → search for the function name. You'll see an "Event" version — this is where you implement the logic.

**Calling an interface function (making calls):**
1. In your Event Graph, drag from any Actor reference.
2. Search for the function name (e.g., `Take Damage`).
3. If the target implements the interface, the node will appear. If not, the call is silently ignored — no crash.

---

## 6. Weekly Workflow — Step by Step

Follow this sequence every time you sit down to work on the project.

### Before You Start

**Step 1 — Pull the latest changes**

Always pull before starting work. This ensures you have everyone else's latest commits before you begin.

Via GitHub Desktop:
1. Click **Fetch origin** (top bar)
2. If there are new commits, click **Pull origin**

Via command line:
```bash
git pull
```

If the pull fails with a conflict message, stop and contact your team lead immediately. Do not attempt to resolve merge conflicts in UE5 binary files yourself.

**Step 2 — Open your personal test level**

Open `L_TestDev_YourName` in UE5. Never start a session in `L_BattleArena`.

**Step 3 — Check the GitHub Issues board**

Spend 2 minutes reviewing open issues. Check for:
- Any requests directed at you
- Any known bugs that affect your Blueprint
- Updates from the team lead

---

### During Your Session

**Step 4 — Work only in files you own**

Refer to the ownership table in Section 4. If the work you need to do requires touching a file you don't own, open a GitHub Issue before making any changes (see Section 4 — Requesting a File Change).

**Step 5 — Save frequently inside UE5**

UE5 does not auto-save assets. Use `Ctrl+Shift+S` (Save All) regularly during your session. Unsaved changes will be lost if UE5 crashes.

**Step 6 — Use the Blueprint Interfaces**

When your Blueprint needs to communicate with another system, use the defined interfaces in Section 5. Do not create direct references to other team members' Blueprints. If you find yourself needing to open another developer's Blueprint just to call a function, that is a sign you should be using an interface instead — flag it to the team lead.

---

### When You Are Done

**Step 7 — Save all assets in UE5**

Press `Ctrl+Shift+S` one final time before closing the editor.

**Step 8 — Stage your changes**

Via GitHub Desktop:
1. In the left panel, you will see a list of changed files.
2. Review the list. You should only see files you own.
3. If you see files you did not intentionally modify (e.g., `L_BattleArena.umap`), uncheck them — these are often auto-modified by UE5's thumbnail or lighting systems and should not be committed unless you intentionally changed them.

Via command line:
```bash
git status          # See all changed files
git add Content/Blueprints/BP_DroneEnemy_Basic.uasset   # Stage specific files
```

**Step 9 — Commit with a descriptive message**

Via GitHub Desktop:
1. In the bottom-left, type a short summary (required) and optionally a description.
2. Click **Commit to main**.

Via command line:
```bash
git commit -m "BP_DroneEnemy_Basic: add patrol movement between waypoints"
```

**Good commit messages:**
- `BP_WaveManager: spawn 5 drones per wave with 3-second delay`
- `BP_HUD: add wave counter and ammo display`
- `Hardware wiring guide: add Arduino Uno pinout diagram`

**Bad commit messages:**
- `updates`
- `fixed stuff`
- `wip`

**Step 10 — Push your commit**

Via GitHub Desktop: Click **Push origin** (top bar).

Via command line:
```bash
git push
```

If push is rejected because the remote has new commits:
```bash
git pull        # Get the new commits first
git push        # Then push yours
```

> If pull produces a merge conflict message on a `.uasset` file, **stop and contact your team lead.** Do not attempt to resolve it yourself. Binary file conflicts require a manual decision about whose version to keep.

---

## 7. Commit and Push Guidelines

### Commit Frequency

Commit at the end of every working session, even if your work is incomplete. A commit with in-progress work is far safer than losing unsaved work.

Aim for commits that are **small and focused** — one logical change per commit. Avoid committing 10 Blueprint changes across 5 different files in one commit. It makes it very hard to understand what changed and why.

### What to Commit

**Always commit:**
- Your owned Blueprint files after any working session
- Documentation you have written or updated
- Config file changes you intentionally made
- Arduino sketch changes

**Never commit:**
- `Binaries/` folder (compiled output, regenerated automatically)
- `Intermediate/` folder (build artifacts)
- `Saved/` folder (logs, autosaves, screenshots)
- Files you do not own unless explicitly coordinated
- UE5 auto-modified thumbnails or level lighting builds

The `.gitignore` in the starter project already excludes `Binaries/`, `Intermediate/`, and `Saved/`. If you see these folders listed as untracked in Git, something went wrong with your `.gitignore` — contact the team lead.

### Branch Strategy

For a project of this size and timeline, **everyone works on the `main` branch directly.** Feature branches add coordination overhead that is not worth it given the 2-hour-per-week time constraint. The file ownership model prevents the conflicts that feature branches are normally designed to solve.

The exception: if a developer wants to experiment with something risky or exploratory, they may create a personal branch (`dev/yourname-experiment`) and merge it into main only after the team lead reviews it.

---

## 8. Handling Conflicts

### Text File Conflicts (Usually Safe to Resolve)

If two people edit the same `.md`, `.ini`, `.cs`, or `.ino` file, Git can usually auto-merge them. If it cannot, GitHub Desktop will highlight the conflicting lines. Edit the file to keep the correct content, then stage and commit the resolved file.

### Binary File Conflicts (UE5 Assets — Requires Lead)

If Git reports a conflict on a `.uasset` or `.umap` file, **you cannot merge it.** You must choose one version to keep. This should never happen if the ownership rules are followed — but if it does:

1. Stop. Do not push.
2. Contact the team lead immediately.
3. The lead will decide whose version is authoritative.
4. To keep one version over another:
   ```bash
   git checkout --ours   Content/Blueprints/BP_Example.uasset   # Keep your version
   git checkout --theirs Content/Blueprints/BP_Example.uasset   # Keep their version
   git add Content/Blueprints/BP_Example.uasset
   git commit -m "Resolve conflict in BP_Example — keeping [ours/theirs]"
   ```
5. The person whose version was discarded will need to re-apply their changes on top of the kept version.

### Preventing Conflicts with LFS Locking (Optional)

Git LFS supports file locking, which prevents other team members from pushing changes to a file while you hold the lock. This is the most reliable way to prevent binary conflicts.

To lock a file before editing:
```bash
git lfs lock Content/Blueprints/BP_DroneEnemy_Basic.uasset
```

To unlock when done:
```bash
git lfs unlock Content/Blueprints/BP_DroneEnemy_Basic.uasset
```

To see all current locks:
```bash
git lfs locks
```

LFS locking requires write access to the GitHub repository. Your team lead will configure this during setup.

---

## 9. Non-UE5 Contributors

Team members without Tier 3 laptops cannot run UE5 but have significant, meaningful work to contribute. Their files are all text-based (markdown, `.ino`, images) and work with Git in the normal way — no special rules required.

### Hardware Sub-Team

Owns and commits to:
- `Arduino/TriggerButton/TriggerButton.ino` — Arduino firmware
- `Hardware/WiringDiagrams/` — Wiring diagrams (images or vector files)
- `Hardware/EnclosureDesign/` — 3D model files for the handheld enclosure
- `Hardware/BOM.md` — Bill of materials

Workflow is identical to the standard Git workflow. Because these are text or non-UE5 binary files, conflicts are rare and resolvable.

### Documentation / QA Sub-Team

Owns and commits to:
- `Docs/TradeStudy_[Topic].md` — Trade study documents
- `Docs/TestPlan.md` — Test procedures and results
- `Docs/FinalPresentation/` — Slide assets and speaker notes

QA team members work with the UE5 developers to define test cases. They do not need to open UE5 themselves — they can run the packaged game build (a standalone `.exe` produced by the Software Lead) to perform testing.

### Requesting Builds for Testing

At the end of each integration session (approximately weekly), the Software Lead packages a standalone Windows executable and commits it to a `Builds/` folder (or shares it via a link in GitHub Releases). QA uses this build for testing — they do not need UE5 installed.

---

## 10. Team Lead Responsibilities

The Software Lead carries additional Git responsibilities beyond normal development.

### Week 1 — Repository Setup

- [ ] Create the GitHub repository and invite all team members as collaborators
- [ ] Verify `.gitignore` is correct and LFS tracking is configured for `.uasset` and `.umap`
- [ ] Create the `L_BattleArena` main level and commit initial layout
- [ ] Create all Blueprint Interface stubs (`BPI_Enemy`, `BPI_Weapon`, `BPI_Scoreable`, `BPI_HUDProvider`) and commit them
- [ ] Create skeleton (empty) Blueprint files for each ownership entry in Section 4 and commit them
- [ ] Assign file ownership to each team member and update the ownership table
- [ ] Enable LFS locking on the repository (GitHub → Settings → Git LFS → Enable locking)

**Why create skeleton files first?** If the lead commits empty Blueprint skeletons for all files upfront, each developer can immediately pull them and begin working in their own file without waiting for others. It also means the ownership table maps to real files from day one.

### Weekly Integration Sessions

Once per week (ideally mid-week), the Software Lead runs an integration session:
1. Pull all team members' latest commits.
2. Open `L_BattleArena` and place/update actor instances from each developer's Blueprint.
3. Do a quick smoke test — does the game compile and run without crashing?
4. Note any interface mismatches or broken references in GitHub Issues.
5. Package a standalone build and share it with the QA team.
6. Commit and push the updated `L_BattleArena`.

### Conflict Resolution

The Software Lead is the final decision-maker on binary merge conflicts. When a conflict is reported, the lead decides which version to keep and communicates the decision to the affected developer.

---

## 11. Quick Reference Cheat Sheet

### Start of Every Session
```bash
git pull                          # Get everyone's latest work
```
Then open your personal test level in UE5.

### End of Every Session
```bash
# In UE5: Ctrl+Shift+S (Save All)

git status                        # See what changed
git add Content/Blueprints/BP_YourFile.uasset
git commit -m "Brief description of what you did"
git push
```

### Check Who Owns a File
See the ownership table in Section 4 of this document.

### Lock a File Before Editing
```bash
git lfs lock Content/Blueprints/BP_YourFile.uasset
# ... do your work ...
git lfs unlock Content/Blueprints/BP_YourFile.uasset
```

### I Need Logic Changed in Someone Else's Blueprint
Open a GitHub Issue. Do not touch the file.

### There's a Merge Conflict on a .uasset File
Stop. Contact the Software Lead.

### UE5 Won't Open After a Pull
Run `git lfs pull` to ensure all asset files downloaded correctly. If the issue persists, contact the Software Lead.

---

*This guide is a living document. If a workflow in this guide does not match how the team is actually operating, update this document and commit the change. Accurate documentation is part of the deliverable.*
