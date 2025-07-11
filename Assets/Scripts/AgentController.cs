using UnityEngine;
using UnityEngine.InputSystem;

public class AgentController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    public float moveSpeed = 5f;
    public float jumpForce = 4.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded = true;
    private Vector3 originalScale;

    public InputActionReference attackAction;
    public InputActionReference skillAction;
    public Animator animator;
    private float lastAttackTime;
    public float attackCooldown = 0.3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        moveAction.action.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();
        jumpAction.action.performed += ctx =>
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        };
        moveAction.action.Enable();
        jumpAction.action.Enable();

        attackAction.action.performed += ctx => TryAttack();
        attackAction.action.Enable();
        // skillAction.action.performed += ctx => TrySkill();
        // skillAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.performed -= null;
        moveAction.action.canceled -= null;
        jumpAction.action.performed -= null;
        moveAction.action.Disable();
        jumpAction.action.Disable();

        attackAction.action.performed -= ctx => TryAttack();
        attackAction.action.Disable();
        // skillAction.action.performed -= ctx => TrySkill();
        // skillAction.action.Disable();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
            isGrounded = true;
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        Debug.Log("공격!");
        // 공격 애니메이션, 히트박스 활성화 등
    }

    void TrySkill()
    {
        Debug.Log("스킬!");
        // 스킬 애니메이션, 이펙트 등
    }
}