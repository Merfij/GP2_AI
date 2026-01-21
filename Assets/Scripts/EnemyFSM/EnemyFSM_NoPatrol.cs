using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM_NoPatrol : MonoBehaviour
{
    [Header("FOV Settings")]
    public float radius;
    [UnityEngine.Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    //public GameObject playerRef;

    [Header("Enemy Target")]
    public Transform target;

    [Header("Chase Range")]
    public float chaseRange;
    public float chaseSpeed;
    public float attackRange;
    public float waypointTolerance;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    public NPCTarget player;

    public SphereCollider handCollider;

    int currentIndex = 0;
    NavMeshAgent agent;

    public EnemyState currentState;

    private void Awake()
    {
        handCollider.enabled = false;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.autoBraking = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NPCTarget>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Chasing;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Vector3 fovLine1 = Quaternion.AngleAxis(angle / 2, transform.up) * transform.forward * radius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-angle / 2, transform.up) * transform.forward * radius;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
        if (canSeePlayer && player.health > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * radius);
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chasing:
                animator.SetInteger("AIState", 2);
                UpdateChase();
                break;
            case EnemyState.Attacking:
                animator.SetInteger("AIState", 3);
                UpdateAttack();
                break;
        }

        FieldOfViewCheck();
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void UpdateAttack()
    {
        agent.isStopped = true;
        agent.ResetPath();

        // Face the player

        if (player.health > 0)
        {
            Vector3 lookDir = target.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * 5f
            );

            if (Vector3.Distance(transform.position, target.position) > attackRange + 0.5f)
            {
                currentState = EnemyState.Chasing;
                return;
            }
        }
    }

    private void UpdateChase()
    {
        if (canSeePlayer && player.health > 0)
        {
            agent.isStopped = false;
            chaseSpeed = Vector3.Distance(transform.position,target.transform.position);
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = EnemyState.Attacking;
            return;
        }
    }

    public void ActivateEnemyCollider()
    {
        handCollider.enabled = true;
    }

    public void DisaleEnemyCollider()
    {
        handCollider.enabled = false;
    }
}