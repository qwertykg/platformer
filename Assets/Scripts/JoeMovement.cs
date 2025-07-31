using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class JoeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0; // Disable gravity for top-down games
    }

    void Update()
    {
        // Get raw input and normalize it
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Animate based on input, not velocity
        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        // Flip sprite based on horizontal input
        if (moveInput.x < 0)
            spriteRenderer.flipX = true;
        else if (moveInput.x > 0)
            spriteRenderer.flipX = false;
    }

    void FixedUpdate()
    {
        // Apply movement directly from input
        rb.velocity = moveInput * moveSpeed;
    }
}
