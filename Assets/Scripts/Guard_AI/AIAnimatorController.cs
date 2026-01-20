using UnityEngine;

public class AIAnimatorController : MonoBehaviour
{
    private Animator animator;
    private int aiStateHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aiStateHash = Animator.StringToHash("AIState");
    }

    public void SetState(AIState state)
    {
        animator.SetInteger(aiStateHash, (int)state);
    }
}
