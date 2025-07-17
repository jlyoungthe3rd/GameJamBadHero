using UnityEngine;

public class VillainBossController : EnemyController
{
    void Start()
    {
        // Initialize the boss with specific stats
        health = 300; 
        attackDamage = 20; 
        moveSpeed = 3f;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss took " + damage + " damage! Current health: " + health);

        // TODO: Add visual feedback for taking damage (like a brief flash)

        if (health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log("BOSS DEFEATED! Transformation should happen now.");
        // We will trigger the player transformation from here later.

        // For now, just destroy the boss object.
        Destroy(gameObject);
    }
}