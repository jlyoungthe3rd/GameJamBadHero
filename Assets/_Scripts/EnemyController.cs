using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int attackDamage = 10;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public bool isFacingRight = true;

    [Header("Checks")]
    public Transform groundCheck;   // A point to check for the ground
    public Transform wallCheck;     // A point to check for a wall
    public LayerMask whatIsGround;  // Defines what layer is considered ground
    public float checkRadius = 0.1f;
    private bool isTouchingWall;
    private bool isAtEdge;

    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 2f; // Time in seconds between attacks
    private float timeSinceLastAttack = 0f;
    public LayerMask playerLayer;

    void Start()
    {
        // Get the Rigidbody2D component attached to this enemy
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is used for physics calculations
    void FixedUpdate()
    {
        // Use Physics2D.OverlapCircle to check the sensor points
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, whatIsGround);
        isAtEdge = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // If the enemy is at a wall or the edge of a platform, flip it
        if (isTouchingWall || isAtEdge)
        {
            Flip();
        }

        // Set the enemy's velocity (this code is the same as before)
        if (isFacingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        // Increment the attack timer
        timeSinceLastAttack += Time.fixedDeltaTime;

        // Check if the player is in range
        bool playerInRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        // If player is in range AND our attack is off cooldown
        if (playerInRange && timeSinceLastAttack >= attackCooldown)
        {
            Attack();
        }
    }

    private void Flip()
    {
        // Switch the direction the enemy is facing
        isFacingRight = !isFacingRight;

        // Flip the enemy's local scale on the x-axis
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy health: " + health);

        if (health <= 0)
        {
            Die();
        }

    }

    protected virtual void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    void Attack()
    {
        // Reset the attack timer
        timeSinceLastAttack = 0f;

        Debug.Log("Enemy is attacking the player!");

        // Detect the player in front of us
        Collider2D playerToDamage = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        // Get the PlayerController script and deal damage
        if (playerToDamage != null)
        {
            PlayerController playerController = playerToDamage.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
            }
        }
    }
}

