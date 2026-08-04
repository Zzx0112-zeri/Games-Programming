# Development Log — Power Cell Escape

Updated daily during the assessment period. Each entry records what was actually done
that day.

---

## 2026-07-31 (Day 1) — Concept & scaffolding
- Decided on scope: one room, one enemy, three cells, 180s timer, 3 lives.
- Wrote `Docs/ConceptAndDesign.md` (EN) and `ConceptAndDesign.zh.md` (ZH).
- Created the Unity 2022.3 project skeleton: `Packages/manifest.json`,
  `ProjectSettings/ProjectVersion.txt`, `.gitignore`.
- Chose a "build everything in code" approach so the scene needs no hand-placed objects.

## 2026-08-01 (Day 2) — Core state & bootstrap
- Implemented `GameManager` (state, timer, lives, events) and the `GameBootstrap`
  entry point using `[RuntimeInitializeOnLoadMethod]`.
- Implemented `GameArt` for procedural sprites (robot, enemy, cell, wall).
- Implemented `LevelBuilder` to construct the room and entities.

## 2026-08-02 (Day 3) — Players, items, enemy
- Implemented `PlayerController` (WASD/arrows, invulnerability blink, trigger logic).
- Implemented `Battery` and `ExitDoor` (locked/open behaviour).
- Implemented `PatrollingEnemy` (waypoint patrol + chase within detection radius).

## 2026-08-03 (Day 4) — UI, audio, accessibility
- Implemented `HUDManager` and `InstructionsPanel` using `OnGUI`.
- Implemented `WorldLabel` for screen-space text (cell labels, `!`, `LOCKED`/`OPEN`)
  with no font assets.
- Implemented `AudioFeedback` (runtime-synthesised SFX) and `SettingsManager`
  (C / M / `[` / `]` / R input, high-contrast background).
- Manual playtest: verified win path, both lose paths, wall collision, invuln window.

## 2026-08-04 (Day 5) — Docs, packaging, review
- Wrote `README.md` / `README.zh.md`, `TaskBoard.md`, `FinalReport.md`.
- Generated `ConceptAndDesign.docx` from the markdown source.
- Re-read all scripts for namespace/using correctness and Unity 2022.3 API usage.
- Tagged the release `resit-submission`; prepared the standalone build steps.

---

## Open items / follow-ups
- Produce the ≤5 minute demo MP4 (record a full win run + an accessibility toggle).
- Final proof-read of `FinalReport.md` before submission (deadline 2026-08-05 23:59 UTC+8).
