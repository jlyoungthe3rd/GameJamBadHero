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

    private Rigidbody2D rb;
    private bool isGrounded;

    // Reference to your generated Input Actions class
    private PlayerInputActions playerInputActions;

    // Variable to store the current movement input
    private Vector2 currentMovementInput;

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
    }

    public void TakeDamage(int damage)
    {
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
}