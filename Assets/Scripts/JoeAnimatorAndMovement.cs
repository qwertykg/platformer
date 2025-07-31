using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class JoeAnimatorAndMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private string lastDirection = "down"; // Default idle direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0; // Disable gravity for top-down games
    }

    void Update()
    {
        // Get raw input and normalize it
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Determine direction and play appropriate animation
        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Horizontal movement takes priority
                if (moveInput.x > 0)
                {
                    animator.Play("walk_right");
                    lastDirection = "right";
                }
                else
                {
                    animator.Play("walk_left");
                    lastDirection = "left";
                }
            }
            else
            {
                // Vertical movement
                if (moveInput.y > 0)
                {
                    animator.Play("walk_up");
                    lastDirection = "up";
                }
                else
                {
                    animator.Play("walk_down");
                    lastDirection = "down";
                }
            }
        }
        else
        {
            // Idle animation based on last direction
            animator.Play("idle_" + lastDirection);
        }
    }

    void FixedUpdate()
    {
        // Move the character
        rb.velocity = moveInput * moveSpeed;
    }
}
