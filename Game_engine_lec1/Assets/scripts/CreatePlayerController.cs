using UnityEngine;

public class CreatePlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 기본 이동 속도
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null) Debug.Log("Animator 컴포넌트를 찾았습니다!");
        else Debug.LogError("Animator 컴포넌트가 없습니다!");
    }

    void Update()
    {
        // --- 이동 ---
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 달리기(Shift) : 속도 2배
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

        if (movement != Vector3.zero)
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime);

        // 이동 속도를 Animator에 전달
        float currentSpeed = movement != Vector3.zero ? currentMoveSpeed : 0f;
        if (animator != null) animator.SetFloat("Speed", currentSpeed);

        // --- 점프: GetKeyDown & Bool(isJumping) ---
        if (animator != null)
        {
            // Space를 누르면 한 번만 isJumping = true
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping"))
            {
                animator.SetBool("isJumping", true);
                Debug.Log("점프!");
            }

            // 점프 애니메이션(Player_Jump)이 끝났는지 감지해서 자동 복귀
            AnimatorStateInfo st = animator.GetCurrentAnimatorStateInfo(0);
            bool isInJump = st.IsName("Player_Jump");    
            bool clipEnded = st.normalizedTime >= 1f;
            bool notTransition = !animator.IsInTransition(0);

            if (animator.GetBool("isJumping") && isInJump && clipEnded && notTransition)
            {
                animator.SetBool("isJumping", false);    // Idle/Walk로 복귀
            }
        }
    }
}
