# Rythle 🎮🎵
[![Status](https://img.shields.io/badge/status-active--development-28a745?style=for-the-badge)](https://github.com/RtoNox/Rythle)
[![Stars](https://img.shields.io/github/stars/RtoNox/Rythle?style=for-the-badge)](https://github.com/RtoNox/Rythle/stargazers)
[![Issues](https://img.shields.io/github/issues/RtoNox/Rythle?style=for-the-badge)](https://github.com/RtoNox/Rythle/issues)
[![License](https://img.shields.io/github/license/RtoNox/Rythle?style=for-the-badge)](LICENSE)
![C#](https://img.shields.io/badge/C%23-100%25-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

A Metroidvania with rhythm-based battles—explore, unlock, and time your moves to the beat. Built with C# and currently in active development. 🚧

---

## Table of Contents
- About the Project
- Features
- Gameplay Overview
- Roadmap
- Getting Started
- Contributing
- Community & Support
- License
- Acknowledgements

---

## About the Project 🧭

Rythle blends classic Metroidvania exploration with rhythmic combat. Navigate interconnected zones, discover abilities that reshape the map, and face enemies and bosses where timing to the soundtrack matters.

This repository contains the game’s source code (C#). The project is evolving rapidly—expect frequent updates and occasional breaking changes while we iterate.

---

## Features ✨

- Metroidvania progression with ability-gated exploration
- Rhythm-based combat: attack, dodge, and parry on-beat for bonuses
- Boss battles that remix mechanics and music
- Collectibles that expand lore and unlockables
- Accessibility-friendly timing windows and assist options (planned)
- Controller and keyboard support (planned)
- Save system with multiple slots (planned)

---

## Gameplay Overview 🕹️

- Explore: Find shortcuts, secrets, and new biomes as you gain abilities.
- Fight on the beat: Land actions in sync with the soundtrack for crits, shields, and combos.
- Grow stronger: Unlock movement and combat upgrades that open new paths and strategies.

Screenshots and clips coming soon!
---

## Roadmap 🗺️

- [ ] Core combat loop polish (hitstop, VFX, SFX sync)
- [ ] First boss prototype
- [ ] Ability system (dash, wall-jump, parry)
- [ ] Save/Load and checkpoints
- [ ] Input rebinding UI
- [ ] Accessibility toggles (timing window, visual metronome)
- [ ] Level art pass and ambient audio
- [ ] Demo build and feedback round
- [ ] Steam/Itch page setup

Track progress and suggest ideas in Issues: https://github.com/RtoNox/Rythle/issues

---

## Getting Started 🔧

Note: This project is written in C#. If you’re setting up locally, choose the path that matches your engine/framework. If you’re unsure, open an issue and we’ll help you get running quickly.

### Prerequisites
- Git
- C# toolchain matching the engine/framework used (Unity / .NET / Godot C# / MonoGame)

### Clone
```bash
git clone https://github.com/RtoNox/Rythle.git
cd Rythle
```

### Open & Run

Pick the option that applies:

- Unity (recommended if this is a Unity project)
  1) Install Unity Editor matching the project version (see ProjectSettings/ProjectVersion.txt).
  2) Open the folder in Unity Hub.
  3) Press Play to run, or File → Build to export.

- Godot C# (Mono)
  1) Install Godot Mono matching your .NET SDK.
  2) Open the project.godot in the repo.
  3) Run the main scene from the editor.

- .NET/MonoGame
  1) Install .NET SDK (see global.json if present).
  2) Restore and run:
     ```bash
     dotnet restore
     dotnet run
     ```
  3) For MonoGame templates, ensure MGCB content is built.

If you run into setup issues, please open an issue with your OS, engine, and logs.

---

## Contributing 🤝

We’d love your help—bug fixes, features, docs, or playtest feedback!

1) Check existing issues: https://github.com/RtoNox/Rythle/issues
   - Good first tasks: “good first issue”
   - Open-ended help: “help wanted”
2) Fork and create a feature branch:
   ```bash
   git checkout -b feat/short-description
   ```
3) Commit clearly (Conventional Commits appreciated):
   - feat: add metronome visualizer
   - fix: correct parry timing window
4) Open a Pull Request describing:
   - What changed and why
   - How to test it
   - Any follow-ups or known gaps

Tips:
- Small, focused PRs are easier to review.
- Include before/after clips or screenshots when UI/FX change.
- Add or update documentation when behavior changes.

Code of Conduct: Please be respectful and inclusive. We’re building a welcoming community. 💜

---

## Community & Support 🙌

- Issues: Report bugs, request features, ask questions
- Discussions (if enabled): Share ideas, feedback, and concepts
- Want to contribute but not sure where to start? Comment on any “help wanted” issue and we’ll guide you.

---

## License 📄

See the [LICENSE](LICENSE) file for details.

If no license is present yet, usage is limited—please open an issue to discuss contributions or distribution.

---

## Acknowledgements 💡

- Inspiration from rhythm and Metroidvania classics
- Thanks to early testers, contributors, and friends supporting development

Made with ❤️ by [RtoNox](https://github.com/RtoNox) and contributors.