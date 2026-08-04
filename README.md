# Power Cell Escape

A small, complete 2D game made with **Unity 2022.3 LTS**. You play a little robot trapped in a small room. You must collect **3 power cells** to unlock the exit and escape, while avoiding one enemy that patrols and chases you. A round lasts about 2–4 minutes.



---

## What's in the repository

- `Assets/` — all C# scripts, organised by role, plus the single scene file.
- `Packages/` and `ProjectSettings/` — Unity project files (kept so the project opens correctly).
- `Docs/` — concept & design document (`ConceptAndDesign.docx` / `.md`, Chinese original `ConceptAndDesign.zh.md`), development log, task board, and final report template.
- `README.md` — this file.
- `.gitignore` — ignores generated folders (`Library/`, `Temp/`, `Logs/`, `Obj/`, `Build/`).

> Per the assessment rules, generated folders such as `Library/`, `Temp/`, `Logs/`, and `Obj/` are **not** committed.

---

## How to play

- **Goal**: within 180 seconds, collect the 3 cells (yellow dots labelled `B1`, `B2`, `B3`), then reach the exit. The exit stays **LOCKED** (red) until all three cells are collected, then it turns green and opens.
- **Threat**: a red square enemy walks a set path. If you get close, it chases you. Touching it costs 1 life (you start with 3). After a hit you blink for ~1.5 seconds and cannot be hurt again right away.
- **Lose**: run out of lives, or run out of time.
- **Win**: collect all 3 cells and reach the open exit.
- **Restart**: press `R` at any time to reload the level.

## Controls

| Key | Action |
|-----|--------|
| `W A S D` / Arrow keys | Move |
| `R` | Restart |
| `C` | Toggle high-contrast mode |
| `M` | Mute / unmute |
| `[` / `]` | Decrease / increase volume |
| `I` | Show / hide help |

## Accessibility

- High-contrast mode (`C`): black background with bright text, for low-vision players.
- Volume control and mute (`M`, `[`, `]`).
- Status is not colour-only: cells have text labels, the exit shows `LOCKED`/`OPEN`, and the enemy carries a `!` marker.
- HUD and instructions use large, readable text.

---

## Build and run

### Option 1 — Run in the Unity Editor

1. Install **Unity 2022.3 LTS** (use Unity Hub and include the **2D** module).
2. Open this folder (`PowerCellEscape/`) as a project in Unity Hub.
3. Open `Assets/Scenes/MainScene.unity`.
4. Press **Play**.

### Option 2 — Build a playable version (for the ZIP submission)

1. Open the project as above.
2. Go to **File → Build Settings**.
3. Click **Add Open Scenes** so `Assets/Scenes/MainScene.unity` appears in *Scenes In Build*. **This step is required** — without it, the level will not load after the build.
4. Choose your platform (Windows / Mac / Linux) and click **Build**. Output to a `Build/` folder.
5. Zip the `Build/` folder as the "playable build" submission.

> The level is built in code at runtime by `GameBootstrap` + `LevelBuilder`, so the scene only needs a camera — there are no hand-placed objects to set up.

---

## Project structure

```
PowerCellEscape/
├─ Assets/
│  ├─ Scripts/
│  │  ├─ Core/        GameManager, LevelBuilder, GameBootstrap
│  │  ├─ Player/      PlayerController
│  │  ├─ Items/       Battery, ExitDoor
│  │  ├─ Enemy/       PatrollingEnemy
│  │  ├─ UI/          HUDManager, InstructionsPanel
│  │  ├─ Audio/       AudioFeedback
│  │  ├─ Settings/    SettingsManager
│  │  └─ Utils/       GameArt
│  └─ Scenes/         MainScene.unity
├─ Packages/          manifest.json (built-in modules only)
├─ ProjectSettings/   ProjectVersion.txt and others
├─ Docs/              ConceptAndDesign.docx/.md, ConceptAndDesign.zh.md,
│                     DevelopmentLog.md, TaskBoard.md, FinalReport.md
├─ README.md
├─ README.zh.md
└─ .gitignore
```

---

## External resources and credits

- **Engine**: Unity® 2022.3 LTS (© Unity Technologies, under its licence terms).
- **Font**: Unity's built-in **Arial** (loaded with `Resources.GetBuiltinResource<Font>("Arial.ttf")`, shipped with Unity).
- **Graphics, audio, and third-party libraries**: **none**. All sprites are generated at runtime by `GameArt`; all sounds are synthesised at runtime by `AudioFeedback`. No external assets or libraries are used.
- Any tutorial or example I referred to is credited in the final report's "Third-party resources" section.

## Credits

- Game design, programming, and documentation: done by me (the student) during the assessment period.
- Generative AI was used as a helper for writing and explaining code. How it was used and verified is explained in the final report's **AI statement**.

---

## Submission notes

- The final release is tagged **`retit-submission`**.
- At least five different days have substantive commits, and the development log is updated daily (not written at the end).
- This project is coursework. Code is provided for educational use; Unity-related assets follow Unity Technologies' licence.
