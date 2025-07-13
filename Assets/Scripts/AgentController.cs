using UnityEngine;
using UnityEngine.InputSystem;

public class AgentController : MonoBehaviour
{
    // 입력 액션 참조 (Input System에서 Inspector에서 할당)
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference attackAction;

    // 이동/공격 속도
    [Header("속도 설정")]
    public float moveSpeed = 5f;         // 이동 속도
    public float attackSpeed = 1.0f;     // 공격 애니메이션 속도 (1.0 = 기본)
    public float jumpForce = 4.5f;       // 점프 힘
    public float attackCooldown = 0.3f;  // 공격 쿨타임

    // 상태 변수
    private Rigidbody2D rb;              // 2D 물리 엔진 컴포넌트
    private Vector2 moveInput;           // 입력값 저장
    private bool isGrounded = true;      // 바닥에 닿아있는지 여부
    private Vector3 originalScale;       // 원래 스케일(좌우 반전용)
    private float lastAttackTime;        // 마지막 공격 시각

    // 이동/점프 가능 여부
    public bool canMove = true;
    public bool canJump = true;

    // 애니메이터
    public Animator animator;

    // InputAction 이벤트 등록/해제용 델리게이트
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> movePerformed;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> moveCanceled;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> jumpPerformed;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> attackPerformed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        if (moveAction == null || moveAction.action == null) { Debug.LogError("moveAction이 할당되지 않았습니다!"); return; }
        if (jumpAction == null || jumpAction.action == null) { Debug.LogError("jumpAction이 할당되지 않았습니다!"); return; }
        if (attackAction == null || attackAction.action == null) { Debug.LogError("attackAction이 할당되지 않았습니다!"); return; }

        movePerformed = HandleMove;
        moveCanceled = HandleMoveCancel;
        jumpPerformed = HandleJump;
        attackPerformed = HandleAttack;

        moveAction.action.performed += movePerformed;
        moveAction.action.canceled += moveCanceled;
        jumpAction.action.performed += jumpPerformed;
        attackAction.action.performed += attackPerformed;

        moveAction.action.Enable();
        jumpAction.action.Enable();
        attackAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed -= movePerformed;
            moveAction.action.canceled -= moveCanceled;
            moveAction.action.Disable();
        }
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.performed -= jumpPerformed;
            jumpAction.action.Disable();
        }
        if (attackAction != null && attackAction.action != null)
        {
            attackAction.action.performed -= attackPerformed;
            attackAction.action.Disable();
        }
    }

    void Update()
    {
        UpdateAnimatorParameters();
    }

    // 애니메이터 파라미터 갱신
    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("AttackSpeed", attackSpeed); // 공격 속도 파라미터 전달
    }

    // 물리 연산(이동 처리)
    void FixedUpdate()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 이동 방향에 따라 좌우 반전 (공격 중에는 방향 전환 불가)
        if (canMove && moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    // 바닥에 닿았는지 판정
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
            isGrounded = true;
    }

    // 입력 핸들러 함수들
    private void HandleMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void HandleMoveCancel(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void HandleJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    private void HandleAttack(InputAction.CallbackContext ctx)
    {
        TryAttack();
    }

    // 공격 시도 함수
    void TryAttack()
    {
        // 이미 공격 중이면(이동 불가 상태) 추가 공격 불가
        if (!canMove) return;

        if (isGrounded)
        {
            if (Time.time - lastAttackTime < attackCooldown) return; // 쿨타임 체크
            lastAttackTime = Time.time;
            canMove = false; // 공격 중 이동 불가
            canJump = false; // 공격 중 점프 불가
            animator.SetTrigger("attack"); // 공격 애니메이션 트리거
            // 실제 공격 판정은 애니메이션 이벤트에서 처리됨
        }
    }

    // 스킬 시도 함수(예시)
    void TrySkill()
    {
        Debug.Log("스킬!");
        // 스킬 애니메이션, 이펙트 등
    }

    // 이동/점프 활성화 제어 함수
    public void EnableJumpAndMovement(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    // 외부에서 이동/공격 속도 조정 함수
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    public void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }
}