using UnityEngine;

public class AgentAnimationEvents : MonoBehaviour
{
    private AgentController agentController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        agentController = GetComponentInParent<AgentController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 애니메이션 이벤트에서 호출될 함수 예시
    public void OnAttackHit()
    {
        Debug.Log("공격 판정 발생!");
        // 실제 공격 판정, 히트박스 활성화 등 구현
    }

    public void DisableMovementAndJump()
    {
        if (agentController != null)
            agentController.EnableJumpAndMovement(false);
    }

    public void EnableMovementAndJump()
    {
        if (agentController != null)
            agentController.EnableJumpAndMovement(true);
    }

    // 공격 애니메이션이 끝날 때 호출할 함수
    public void EndAttack()
    {
        EnableMovementAndJump(); // 공격 후 이동/점프 다시 허용
    }
}
