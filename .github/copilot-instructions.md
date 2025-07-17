### Unity Project Conventions

This is a 2D game developed in Unity with C#. The core logic for enemies is structured around an inheritance model.

#### Enemy Architecture

- **Base Class:** `Assets/_Scripts/EnemyController.cs` is the base class for all enemy types. It provides common functionality including:
    - Basic stats (health, attack damage)
    - Patrol behavior (moving left/right, flipping at edges/walls)
    - A virtual `TakeDamage(int damage)` method.
    - A virtual `Die()` method.

- **Derived Classes:** Specific enemy types, like `Assets/_Scripts/VillainBossController.cs`, inherit from `EnemyController`.
    - They **override** virtual methods like `TakeDamage` and `Die` to implement unique behavior (e.g., knockback for the boss).
    - They use the `Start()` method to set their own specific stats (e.g., higher health for a boss).

**Example: Creating a new Enemy**
To create a new enemy, create a new script that inherits from `EnemyController` and attach it to a GameObject.

```csharp
public class NewEnemyController : EnemyController
{
    void Start()
    {
        // Set stats for this new enemy
        health = 50;
        moveSpeed = 3f;
    }

    // Optionally override methods for unique behavior
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        // Add custom logic, e.g., play a sound
    }
}
```

#### Initialization (`Awake` vs. `Start`)

- `Awake()`: Used in the base `EnemyController` to get essential components like `Rigidbody2D` and find the `player` GameObject. This ensures references are set before any other script tries to access them.
- `Start()`: Used in derived classes (`VillainBossController`) to initialize enemy-specific stats. This happens after `Awake`, so components from the base class are guaranteed to be available.

#### Physics and Movement

- All physics-related logic is in `FixedUpdate()`.
- Movement is handled by directly manipulating the `Rigidbody2D.linearVelocity`.
- Ground and wall checks use `Physics2D.OverlapCircle` with a specific `whatIsGround` layer mask.

#### Player Reference

- The player GameObject is consistently found using the tag `"Player"`. Ensure the player GameObject in the scene has this tag.
- The `player` transform is stored in the base `EnemyController` and is accessible to all derived enemy classes.
