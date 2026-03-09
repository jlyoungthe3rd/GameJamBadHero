# GameJamBadHero **WIP**

A 2D Action Platformer built with Unity, featuring a classic hero vs. villain dynamic with modular enemy systems. This project demonstrates clean game architecture using inheritance and the new Unity Input System.

## 🎮 Gameplay Features
- **Player Controller**: Smooth platforming movement with jump mechanics.
- **Combat System**: Includes melee attacks and a timed blocking mechanic (with cooldowns!) to parry incoming damage.
- **Dynamic Enemies**: Modular enemy AI that patrols platforms, detects edges/walls, and flips direction automatically.
- **Boss Mechanics**: Specific boss behaviors like the `VillainBossController`, featuring custom knockback physics and rage states using polymorphism.

## 🛠 Tech Stack
- **Engine**: Unity 6 (Utilizing Physics 2D v3.0+)
- **Pipeline**: Universal Render Pipeline (URP)
- **Input**: New Unity Input System (Event-driven architecture)
- **Physics**: Rigidbody2D-based movement (`linearVelocity`) with custom layers for ground/wall detection.

## 🏗 Architecture & Patterns
This project follows clean OOP principles:
- **Inheritance**: `EnemyController` serves as the base class for all enemies, handling movement and basic stats. Derived classes like `VillainBossController` override specific behaviors (e.g., `TakeDamage`) for unique reactions.
- **Separation of Concerns**: Input logic is decoupled from game logic using C# Events (`PlayerInputActions` -> `PlayerController`).

## 🚀 Getting Started
1. Clone the repository.
2. Open the project in Unity Hub (Ensure you have the latest Unity 6 version installed).
3. Open the scene at `Assets/Scenes/SampleScene.unity`.
4. Press Play!

## 🕹 Controls
- **Move**: [A/D] or [Left/Right Arrow]
- **Jump**: [Space]
- **Attack**: [Left Mouse Button]
- **Block**: [Right Mouse Button]
