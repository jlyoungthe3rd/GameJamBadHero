using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider; // A reference to the UI Slider

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public LayerMask enemyLayers;

    private Rigidbody2D rb;
    private bool isGrounded;

    // Reference to your generated Input Actions class
    private PlayerInputActions playerInputActions;

    // Variable to store the current movement input
    private Vector2 currentMovementInput;

    private bool isBlocking = false;
    public float blockDuration = 1f; // How long the block lasts

    public float blockCooldown = 3f; // Time before you can block again
    private float timeSinceLastBlock = 0f;

    private SpriteRenderer spriteRenderer;
    void Awake() // Use Awake for initial setup like input actions
    {

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        rb = GetComponent<Rigidbody2D>();

        // Instantiate the generated Input Actions class
        playerInputActions = new PlayerInputActions();

        // Subscribe to the Move action's performed and canceled events
        playerInputActions.Player.Move.performed += ctx => currentMovementInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => currentMovementInput = Vector2.zero; // Stop movement when input is released

        // Subscribe to the Jump action's performed event
        playerInputActions.Player.Jump.performed += ctx => Jump();

        // Subscribe to the Attack Action
        playerInputActions.Player.Attack.performed += ctx => Attack();

        // Subscribe to the Block action
        playerInputActions.Player.Block.performed += ctx => TryBlock();

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void OnEnable() // Enable input actions when the GameObject is active
    {
        playerInputActions.Enable();
    }

    void OnDisable() // Disable input actions when the GameObject is inactive
    {
        playerInputActions.Disable();
    }

    void Update()
    {

        // Apply horizontal movement based on the stored input
        rb.linearVelocity = new Vector2(currentMovementInput.x * moveSpeed, rb.linearVelocity.y);

        // Optional: Flip the sprite based on movement direction
        if (currentMovementInput.x > 0) // Moving right
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (currentMovementInput.x < 0) // Moving left
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Increment block cooldown timer
        timeSinceLastBlock += Time.deltaTime;
    }

    public void TakeDamage(int damage)

    {

        if (isBlocking)
        {
            Debug.Log("Player blocked the attack!");
            StartCoroutine(SuccessfulBlockFeedback()); // Start the flash feedback
            return; // If blocking, don't take damage
        }

        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            // Handle player death here
            Debug.Log("Player has died!");
        }
    }

    // Jump method (called by the Jump action)
    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Attack()
    {
        if (isBlocking == false) // Only allow attack if not blocking
        {
            Debug.Log("Player Attacked!");

            // 1. Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            // 2. Damage all detected enemies
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                // First, check if the object we hit is a regular enemy
                EnemyController enemy = enemyCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // If it is, deal damage and move to the next hit object
                    enemy.TakeDamage(attackDamage);
                    continue;
                }

                // If it wasn't a regular enemy, check if it's the boss
                VillainBossController boss = enemyCollider.GetComponent<VillainBossController>();
                if (boss != null)
                {
                    // If it is the boss, deal damage to it
                    boss.TakeDamage(attackDamage);
                }
            }
        }
        else
        {
            Debug.Log("Cannot attack while blocking!");
            return; // Exit if trying to attack while blocking
        }


    }

    private System.Collections.IEnumerator BlockCoroutine()
    {
        Debug.Log("Player started blocking!");
        isBlocking = true;

        // TODO: Add visual feedback for blocking here (e.g., change color, play animation)
        spriteRenderer.color = Color.cyan; // Change color to cyan when block starts

        // Wait for the duration of the block
        yield return new WaitForSeconds(blockDuration);

        // After the wait, the block is over
        isBlocking = false;
        spriteRenderer.color = Color.blue; // Change color back to normal
        Debug.Log("Player stopped blocking!");

        // TODO: Revert visual feedback here
    }

    private System.Collections.IEnumerator SuccessfulBlockFeedback()
    {
        spriteRenderer.color = Color.yellow; // Flash a bright color
        yield return new WaitForSeconds(0.15f); // For a very short time

        // If we are still in the main block duration, return to the cyan color.
        if (isBlocking)
        {
            spriteRenderer.color = Color.cyan;
        }

        isBlocking = false; // Reset blocking state
        Debug.Log("Player successfully blocked an attack!");
    }

    private void TryBlock()
    {
        // Check if the block is off cooldown
        if (timeSinceLastBlock >= blockCooldown)
        {
            // Reset the timer and start the block coroutine
            timeSinceLastBlock = 0f;
            StartCoroutine(BlockCoroutine());
        }
        else
        {
            Debug.Log("Block is on cooldown!");
        }
    }
    // Basic ground check (keep this as is for now)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Hazard>() != null)
        {
            Hazard hazard = other.GetComponent<Hazard>();
            TakeDamage(hazard.damage);
        }
    }
}