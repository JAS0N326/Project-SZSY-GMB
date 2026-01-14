using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("移动")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 9f;
    [SerializeField] private float accelTime = 0.05f; // 速度平滑时间

    [Header("跳跃")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxExtraJumps = 1; // 二段跳：额外跳跃次数
    [SerializeField] private float coyoteTime = 0.12f; // 允许短暂在离地后跳跃
    [SerializeField] private float jumpBufferTime = 0.12f; // 按键缓冲

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool facingRight = true;

    // 状态
    private Vector2 velocitySmooth;
    private int remainingJumps;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        remainingJumps = maxExtraJumps;
    }

    private void Update()
    {
        // 输入
        float h = Input.GetAxisRaw("Horizontal");
        bool runHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        // 跳跃按键缓冲
        if (Input.GetButtonDown("Jump")) jumpBufferTimer = jumpBufferTime;
        else jumpBufferTimer -= Time.deltaTime;

        // 地面检测
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            remainingJumps = maxExtraJumps;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // 尝试跳跃（考虑缓冲与迟滞）
        if (jumpBufferTimer > 0f)
        {
            if (coyoteTimer > 0f)
            {
                DoJump();
                jumpBufferTimer = 0f;
            }
            else if (remainingJumps > 0)
            {
                DoJump();
                remainingJumps--;
                jumpBufferTimer = 0f;
            }
        }

        // 可控短跳（放开跳键减少上升高度）
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // 移动（平滑）
        float targetSpeed = h * (runHeld ? runSpeed : walkSpeed);
        Vector2 targetVelocity = new Vector2(targetSpeed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocitySmooth, accelTime);

        // 翻转角色朝向（如果需要）
        if (h > 0.01f && !facingRight) Flip();
        else if (h < -0.01f && facingRight) Flip();
    }

    private void DoJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // 重置垂直速度以获得稳定跳跃高度
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteTimer = 0f;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}