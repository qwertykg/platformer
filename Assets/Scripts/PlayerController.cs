using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 100f;   // how fast to reach target speed
    public float deceleration = 100f;   // how fast to stop

    [Header("Jumping")]
    public float jumpForce = 14f;
    public int extraAirJumps = 0;       // set >0 if you want double-jump later
    public float coyoteTime = 0.1f;     // jump grace after leaving ground
    public float jumpBuffer = 0.1f;     // queue jump slightly before landing

    [Header("Ground Check")]
    public Transform groundCheck;       // empty child at feet
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    SpriteRenderer sr;                  // optional: to flip visual
    int jumpsLeft;
    float coyoteTimer;
    float jumpBufferTimer;
    float inputX;
    bool facingRight = true;            // starts true: rightwards facing

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // okay if missing
        jumpsLeft = extraAirJumps;
    }

    void Update()
    {
        // --- Read input ---
        inputX = Input.GetAxisRaw("Horizontal"); // -1,0,1
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBuffer;

        // --- Grounding / timers ---
        bool grounded = IsGrounded();
        if (grounded)
        {
            coyoteTimer = coyoteTime;
            jumpsLeft = extraAirJumps;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (jumpBufferTimer > 0) jumpBufferTimer -= Time.deltaTime;

        // --- Jump logic (buffer + coyote) ---
        if (jumpBufferTimer > 0 && (coyoteTimer > 0 || jumpsLeft > 0))
        {
            DoJump();
            jumpBufferTimer = 0;
        }

        // --- Variable jump height (release early to cut jump) ---
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        // --- Flip sprite to match movement direction ---
        if (inputX > 0 && !facingRight) Flip(true);
        else if (inputX < 0 && facingRight) Flip(false);
    }

    void FixedUpdate()
    {
        // Smooth horizontal movement
        float target = inputX * moveSpeed;
        float speedDiff = target - rb.velocity.x;
        float accel = Mathf.Abs(target) > 0.01f ? acceleration : deceleration;
        float movement = Mathf.Clamp(speedDiff * accel * Time.fixedDeltaTime, -Mathf.Abs(speedDiff), Mathf.Abs(speedDiff));
        rb.velocity = new Vector2(rb.velocity.x + movement, rb.velocity.y);
    }

    void DoJump()
    {
        // reset Y velocity before jump for consistent height
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (coyoteTimer <= 0)            // spent an air jump
            jumpsLeft--;

        coyoteTimer = 0;                 // prevent double coyote jump
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Flip(bool toRight)
    {
        facingRight = toRight;
        // If you have a SpriteRenderer, flip it. Otherwise flip transform scale.
        if (sr != null)
        {
            sr.flipX = !toRight; // assuming original art faces right
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (toRight ? 1 : -1);
            transform.localScale = scale;
        }
    }

    // For visualizing ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
