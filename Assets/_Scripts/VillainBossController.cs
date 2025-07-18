using UnityEngine;

public class VillainBossController : EnemyController
{
    [Header("Boss Specifics")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;


    void Start()
    {
        health = 300;
        attackDamage = 25;
        moveSpeed = 1.5f;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss took " + damage + " damage! Current health: " + health);

        // Knockback logic
        if (player != null)
        {
            Debug.Log("knock back triggerd");
            // Stop current movement before applying knockback
            rb.linearVelocity = Vector2.zero;
            // Calculate direction away from player
            Vector2 knockbackDirection = (transform.position - player.position).normalized;
            // Apply the force
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }


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