# Power Cell Escape — Concept & Design

**Document type:** Part 1 — Concept & Design (assessment deliverable)
**Engine:** Unity 2022.3 LTS (2D)
**Author:** Zhixin Zhu (student)
**Status:** Ready for implementation

---

## 1. Overview

*Power Cell Escape* is a tiny, self-contained 2D arcade game. The player controls a
small robot trapped in a single enclosed room. To escape, the robot must collect three
power cells scattered around the room and then reach the exit. A single enemy wanders
randomly around the room. The round is timed at 180
seconds; a single touch from the enemy ends the round.

The whole point of the brief was to keep the scope small but complete: one room, one
enemy, three collectables, clear win/lose states, and built-in accessibility. A round
lasts roughly two to four minutes, which feels right for a pick-up-and-play assessment
demo.

## 2. Target audience & platform

- **Audience:** casual players and assessors; anyone who can use a keyboard.
- **Platform:** desktop (Windows / Mac / Linux) via the Unity player. Keyboard only,
  no gamepad or mouse required.
- **Session length:** 2–4 minutes per attempt.

## 3. Core gameplay loop

The game has three screens driven by a single `GameManager` state
(`Menu` → `Playing` → `Won`/`Lost`):

- **Start page** (`Menu`): shows the title with **Start** (enter the game) and
  **Exit** (close the game) buttons.
- **Gameplay** (`Playing`): the round is active.
- **End screen** (`Won`/`Lost`): on a win or Game Over, shows **Restart**
  (back to the start page) and **Exit Game** (close the game).

1. The game opens on the **start page**. Pressing **Start** begins a round and the timer counts down from 180s.
2. The player explores the room and collects cells (`B1`, `B2`, `B3`).
3. The enemy wanders randomly; the player must avoid touching it.
4. Touching the enemy ends the round immediately (Game Over).
5. Once all three cells are collected, the exit unlocks (red → green, `LOCKED` → `OPEN`).
6. Reaching the open exit wins. Touching the enemy or running out of time shows the **end screen**.

## 4. Mechanics in detail

| Element | Behaviour |
|---------|-----------|
| Player | Top-down movement with WASD / arrows. Dynamic body, blocked by walls. Cannot leave the room. |
| Power cell | Trigger pickup. Hides on collect, increments the counter, plays a sound. |
| Exit door | Trigger zone. Locked (red) until 3 cells collected, then open (green). Entering while open wins. |
| Enemy | Wanders randomly around the room at 3 u/s (picks a new random target every 1–3 s and never chases). **Touching it ends the game immediately (Game Over).** |
| Lives | One hit ends the round, so there is no life buffer — survival means avoiding the enemy entirely. |
| Timer | 180s count-down. Reaching 0 loses. |

## 5. Controls

| Key | Action |
|-----|--------|
| W A S D / Arrows | Move |
| R | Return to the start page (from play or end screen) |
| C | Toggle high-contrast mode |
| M | Mute / unmute |
| `[` / `]` | Volume down / up |
| I | Show / hide help |

## 6. Level layout

The room is a 20×12 world-unit rectangle bounded by four solid walls. Objects are
placed so the player must move around the room, crossing the enemy's usual area at least once:

- Player spawn: bottom-centre `(0, -3.5)`.
- `B1` left `(-6, 0)`, `B2` right `(6, 0)`, `B3` top-centre `(0, 3)`.
- Exit: top-centre, embedded in the top wall.
- Enemy loop waypoints: `(-5,-2) → (5,-2) → (5,2) → (-5,2)`.

The orthographic camera (size 6.5) frames the whole room, so nothing is off-screen.

## 7. Software architecture

The project deliberately avoids hand-placed scene objects. A single bootstrapper builds
everything in code, which keeps the repository free of fragile serialized references and
makes the game easy to rebuild on restart.

```
RuntimeInitializeOnLoadMethod
        │
        ▼
   GameBootstrap.Init()
        ├─ creates GameManager (state: Menu/Playing/Won/Lost, timer, win/lose)
        ├─ creates SettingsManager (C / M / [ ] / R input)
        ├─ creates AudioFeedback (runtime-synthesised SFX)
        ├─ creates HUDManager + InstructionsPanel + MenuManager (OnGUI)
        └─ LevelBuilder.Build()
              ├─ camera
              ├─ 4 walls
              ├─ Player (+ PlayerController)
              ├─ 3 × Battery (+ label)
              ├─ ExitDoor (locked/open)
              └─ PatrollingEnemy (random wander, "!" marker)
```

- **GameManager** is the single source of truth for state and exposes events
  (`OnCellCollected`, `OnWin`, `OnLose`). It owns the screen flow: `StartGame()`
  rebuilds a fresh level and enters `Playing`; `ReturnToMenu()` rebuilds and returns
  to `Menu`.
- **MenuManager** draws the start page (`Menu`) and end screen (`Won`/`Lost`) as
  OnGUI overlays with `Start`/`Exit` and `Restart`/`Exit Game` buttons. `Exit`/
  `Exit Game` call `Application.Quit()` (editor play is stopped via
  `EditorApplication.isPlaying = false`).
- **LevelBuilder** constructs the room and entities under a `LevelRoot` GameObject so
  it can be destroyed and rebuilt cleanly on every start / return-to-menu.
- **WorldLabel** draws text (cell labels, `!`, `LOCKED`/`OPEN`) in screen space with
  `OnGUI`, so no font or text assets are shipped.

## 8. Accessibility

- **High-contrast mode (C):** switches the camera background to black and HUD text to
  bright yellow.
- **Audio:** global mute (`M`) and volume steps (`[` / `]`); applied through
  `AudioListener.volume`.
- **Not colour-only:** cells carry text labels, the exit shows `LOCKED`/`OPEN`, the
  enemy shows `!`.
- **Readable text:** HUD and help use large fonts.

## 9. Art & audio approach

There are **no external assets**:

- Player, enemy and battery sprites are stored as PNGs in `Resources/Sprites/`
  and loaded at runtime via `SpriteAssets`. Walls and the exit still use
  procedurally generated sprites as a fallback.
- Sounds (collect, hit, win, lose) are short sine tones generated with
  `AudioClip.Create` and played through a single `AudioSource`.
- The only font used is Unity's built-in font, resolved via `GuiFonts`
  (`LegacyRuntime.ttf` on Unity 2022.3, `Arial.ttf` on older versions, with a
  safe fallback to the skin default).

## 10. Testing plan

- **Manual playtest:** confirm win path (collect 3 → exit opens → win) and both lose
  paths (enemy contact, timer = 0).
- **Collision checks:** player blocked by walls; cells/enemy/exit trigger correctly.
- **Enemy contact:** any touch ends the round (Game Over); there is no life buffer.
- **Accessibility:** toggle high contrast, mute, and volume; verify HUD updates.
- **Screens:** start page shows Start/Exit; end screen shows Restart/Exit Game; Exit closes the game.
- **Restart / return to menu:** press `R` mid-game and on the end screen, or click the buttons; confirm a clean reset and a fresh level.
- **Build:** produce a standalone player build and verify the level loads.

## 11. Risks & mitigations

| Risk | Mitigation |
|------|------------|
| Scene references break on different machines | No hand-placed objects; everything built in code. |
| Enemy too hard / too easy | Tunable speeds and detection radius as serialized fields. |
| Text unreadable | Large OnGUI fonts + high-contrast mode. |
| Build missing the scene | Document "Add Open Scenes" step in README. |

## 12. Scope boundaries (explicitly out of scope)

Multiple rooms, score tables, save games, mobile/touch controls, and networked play are
not part of this assessment build. The design leaves clear extension points (e.g. more
waypoints, more cells) if desired later.
