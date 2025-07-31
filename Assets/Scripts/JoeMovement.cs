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

        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);

        // Set directional animation parameters
        animator.SetBool("GoingLeft", isMoving && moveInput.x < 0);
        animator.SetBool("GoingRight", isMoving && moveInput.x > 0);
        animator.SetBool("GoingUp", isMoving && moveInput.y > 0);
        animator.SetBool("GoingDown", isMoving && moveInput.y < 0);
    }

    void FixedUpdate()
    {
        // Apply movement directly from input
        rb.velocity = moveInput * moveSpeed;
    }
}
