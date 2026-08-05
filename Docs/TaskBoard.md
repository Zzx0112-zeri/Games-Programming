# Task Board — Power Cell Escape

Status legend: ✅ done · 🔶 in progress · ⬜ todo

## Part 1 — Concept & Design (20%)
- ✅ Game concept and scope defined
- ✅ `ConceptAndDesign.md` (EN) and `ConceptAndDesign.zh.md` (ZH)
- ✅ `ConceptAndDesign.docx` generated
- ✅ Architecture & accessibility plan written

## Implementation
- ✅ Project scaffolding (Packages, ProjectSettings, .gitignore, scene)
- ✅ `GameManager` — state, timer, lives, win/lose
- ✅ `GameBootstrap` + `LevelBuilder` — runtime level construction
- ✅ `GameArt` / `SpriteAssets` — procedural + external PNG sprites
- ✅ `PlayerController` — movement, invulnerability, triggers
- ✅ `Battery` / `ExitDoor` — collectables & locked/open exit
- ✅ `PatrollingEnemy` — patrol + chase
- ✅ `HUDManager` / `InstructionsPanel` — OnGUI HUD & help
- ✅ `WorldLabel` — screen-space text, no font assets
- ✅ `AudioFeedback` — runtime synth SFX
- ✅ `SettingsManager` — C / M / `[` / `]` / R, high contrast

## Part 2 — Implementation, testing, report
- 🔶 Standalone build verified (Build/ folder)
- ⬜ Demo MP4 (≤5 min) recorded
- ⬜ `FinalReport.md` finalised (includes AI statement & third-party resources)
- ⬜ Tag `resit-submission` pushed; ≥5 days of commits confirmed

## Quality / accessibility
- ✅ High-contrast mode
- ✅ Mute + volume control
- ✅ Status not colour-only (labels, LOCKED/OPEN, `!`)
- ✅ Large readable HUD text
