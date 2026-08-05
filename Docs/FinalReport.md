# Final Report — Power Cell Escape

> Fill in the bracketed sections before submission. Keep it in your own words; this
> template is meant to guide, not to be submitted verbatim.

---

## 1. Introduction
[One or two paragraphs: what the game is, the platform, and the assessment goal. Write
it as if explaining the project to a marker who has not seen the brief.]

## 2. Design summary
- Core loop: collect 3 cells → unlock exit → escape within 180s; avoid the enemy; 3 lives.
- Key design decisions:
  - Everything is built in code at runtime (`GameBootstrap` + `LevelBuilder`) so the
    scene has no fragile hand-placed references.
  - External art assets: player, enemy and battery PNGs in `Resources/Sprites/`,
    loaded at runtime via `SpriteAssets`. Walls and exit remain procedural
    fallbacks. Sounds are synthesised with `AudioFeedback`.
  - Status is conveyed by text as well as colour (accessibility).

## 3. Implementation
[Describe the main classes and how they collaborate. Reference `GameManager`,
`LevelBuilder`, `PlayerController`, `PatrollingEnemy`, `ExitDoor`, `HUDManager`,
`AudioFeedback`, `SettingsManager`. Mention the Unity version and that the 2D module is
used.]

Bullet points are fine here, for example:
- `GameManager` holds all state and raises events on cell collect / hit / win / lose.
- `PatrollingEnemy` switches between a waypoint loop and chase based on distance.
- `WorldLabel` draws labels in screen space with `OnGUI`.

## 4. Accessibility
[Explain the high-contrast mode, mute/volume, text labels, and large fonts, and why they
matter.]

## 5. Testing
[Describe what you tested and how. Include at least: win path, both lose paths, wall
collision, the 1.5s invulnerability window, restart, and a standalone build check.
State any bugs found and fixed.]

| Test | Result |
|------|--------|
| Collect 3 cells → exit opens → win | [Pass / Fail] |
| Lives reach 0 → lose | [Pass / Fail] |
| Timer reaches 0 → lose | [Pass / Fail] |
| Wall blocks player | [Pass / Fail] |
| Invulnerability after hit | [Pass / Fail] |
| Restart with R | [Pass / Fail] |
| Standalone build loads level | [Pass / Fail] |

## 6. Reflection
[What went well, what was difficult, what you would change. Be honest — markers value
critical reflection over boilerplate.]

## 7. AI statement
[State clearly how generative AI was used. Example: "I used a generative AI assistant to
help draft and explain C# code and to tidy the documentation. I reviewed every script,
checked it against the Unity 2022.3 API, and tested the game manually before submitting.
No AI-generated asset (art/audio) is included; all are created at runtime by my code."]

## 8. Third-party resources
- Unity® 2022.3 LTS — engine (© Unity Technologies, under its licence).
- Unity built-in font (resolved via `GuiFonts`, using `LegacyRuntime.ttf` on Unity 2022.3 with a safe fallback).
- [List any tutorials, forum threads, or docs you consulted, with URLs.]

## 9. Submission checklist
- [ ] Source committed; generated folders (`Library/`, `Temp/`, `Build/`, ...) ignored.
- [ ] At least five different days with substantive commits.
- [ ] Tag `resit-submission` created.
- [ ] `README.md` explains how to run and build.
- [ ] Demo MP4 (≤5 min) recorded.
- [ ] This report completed.
