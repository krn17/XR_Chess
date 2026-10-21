# ♟️ XR_Chess

[![Unity](https://img.shields.io/badge/Unity-2019.4%20LTS-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Mixed%20Reality%20%2F%20AR-orange)]()

An immersive **3D Mixed Reality (MR) Chess experience** built in Unity. Play interactive spatial chess against a custom AI engine directly in your physical environment.

---

## 🌟 Key Features

* **🧠 Minimax AI Opponent**: Custom chess engine implementation featuring Minimax tree search with heuristic evaluation algorithms (`MiniMax.cs`, `MoveHeuristic.cs`).
* **🖐️ 3D Spatial Interactions**:
  * **Freeform Board Placement**: Move, rotate, and scale the 3D chessboard in physical space (`ChessboardTransformer.cs`).
  * **Gaze & Hover Highlights**: Visual piece highlighting with dynamic outlines upon hover/pointing (`OutlineOnHover.cs`, `QuickOutline`).
  * **Piece Spawner & Transitions**: Smooth piece instantiation and move animations (`PieceSpriteSpawner.cs`).
* **💡 Move Hint System**: Built-in move recommendations for players (`HintManager.cs`).
* **⚔️ Full Game Rules Engine**: Turn tracking, legal move validation, capture mechanics, and check/checkmate overlay alerts (`BoardManager.cs`, `GameManager.cs`, `OverlayCheck.cs`).
* **🎨 Complete UI & Audio Pipeline**: Interactive spatial menus, splash screen, move sound effects, and persistent leaderboard/game saving.

---

## 🏗️ Project Architecture

```text
Assets/
├── Scripts/
│   ├── BoardManager.cs       # Board state, tile mappings, and turn orchestrator
│   ├── ChessPiece.cs         # Base class and rules for individual chess pieces
│   ├── GameManager.cs        # Game loop, check/checkmate logic, and game state
│   ├── MiniMax.cs            # Minimax chess AI engine with decision heuristics
│   ├── MoveHeuristic.cs      # Position and material evaluation scoring
│   ├── HintManager.cs        # Move assistance engine
│   ├── UIManager.cs          # Spatial UI and heads-up displays
│   └── Leaderboard/          # Score recording and local persistence
├── ChessboardTransformer.cs  # Spatial manipulation (move, scale, rotate board)
├── OutlineOnHover.cs         # Visual shaders and interaction feedback
├── Scenes/
│   ├── SplashScreen.unity    # App launch screen
│   ├── MenuScreen.unity      # Spatial menu interface
│   ├── Main.unity            # Core gameplay scene
│   └── Sandbox.unity         # Testing & prototyping sandbox
```

---

## 🚀 Getting Started

### Prerequisites
* **Unity**: `2019.4.3f1 LTS` (or compatible Unity 2019.4+ / 2020.3 LTS)
* **Target Platforms**: Android (XR Headsets / AR Glasses / Mobile AR) or Unity Editor with XR simulation.

### Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/krn17/XR_Chess.git
   ```
2. Open **Unity Hub** and click **Add**.
3. Select the `XR_Chess` folder.
4. Set the project version to `2019.4.3f1` (or click *Confirm* to upgrade if using a newer LTS).
5. In Unity, navigate to `Assets/Scenes/` and open `MenuScreen.unity` or `Main.unity`.
6. Press **Play** in the Unity Editor to test.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
