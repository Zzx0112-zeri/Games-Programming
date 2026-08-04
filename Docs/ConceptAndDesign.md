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

1. The round starts: timer counts down from 180s.
2. The player explores the room and collects cells (`B1`, `B2`, `B3`).
3. The enemy wanders randomly; the player must avoid touching it.
4. Touching the enemy ends the round immediately (Game Over).
5. Once all three cells are collected, the exit unlocks (red → green, `LOCKED` → `OPEN`).
6. Reaching the open exit wins. Touching the enemy or running out of time loses.

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
| R | Restart level |
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
        ├─ creates GameManager (state, timer, win/lose)
        ├─ creates SettingsManager (C / M / [ ] / R input)
        ├─ creates AudioFeedback (runtime-synthesised SFX)
        ├─ creates HUDManager + InstructionsPanel (OnGUI)
        └─ LevelBuilder.Build()
              ├─ camera
              ├─ 4 walls
              ├─ Player (+ PlayerController)
              ├─ 3 × Battery (+ label)
              ├─ ExitDoor (locked/open)
              └─ PatrollingEnemy (random wander, "!" marker)
```

- **GameManager** is the single source of truth for state and exposes events
  (`OnCellCollected`, `OnWin`, `OnLose`).
- **LevelBuilder** constructs the room and entities using procedural sprites from
  `GameArt`.
- **WorldLabel** draws text (cell labels, `!`, `LOCKED`/`OPEN`) in screen space with
  `OnGUI`, so no font or text assets are shipped.
- **Restart** reloads the active scene, which destroys all objects and re-runs the
  bootstrapper, giving a clean fresh round.

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

- Sprites (robot, enemy square, cell circle, walls) are drawn into `Texture2D` objects
  at runtime by `GameArt` and turned into `Sprite`s.
- Sounds (collect, hit, win, lose) are short sine tones generated with
  `AudioClip.Create` and played through a single `AudioSource`.
- The only font used is Unity's built-in Arial, loaded with
  `Resources.GetBuiltinResource<Font>("Arial.ttf")`.

## 10. Testing plan

- **Manual playtest:** confirm win path (collect 3 → exit opens → win) and both lose
  paths (enemy contact, timer = 0).
- **Collision checks:** player blocked by walls; cells/enemy/exit trigger correctly.
- **Enemy contact:** any touch ends the round (Game Over); there is no life buffer.
- **Accessibility:** toggle high contrast, mute, and volume; verify HUD updates.
- **Restart:** press `R` mid-game and after win/lose; confirm a clean reset.
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
