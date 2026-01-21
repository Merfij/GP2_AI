using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Attacking,
}

public class EnemyFSM : MonoBehaviour
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
    public float patrolSpeed;
    public float chaseRange;
    public float chaseSpeed;
    public float attackRange;
    public float waypointTolerance;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    public Transform[] checkpoints;

    public NPCTarget player;

    int currentIndex = 0;
    NavMeshAgent agent;

    public EnemyState currentState;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.autoBraking = true;
        //playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Patrolling;
        if (checkpoints.Length > 0)
        {
            agent.SetDestination(checkpoints[currentIndex].position);
        }
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
            case EnemyState.Patrolling:
                animator.SetInteger("AIState", 1);
                UpdatePatrol();
                break;
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

    private void UpdateAttack()
    {
        Debug.Log("Player was attacked");

        agent.isStopped = true;
        agent.ResetPath();

        // Face the player
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(lookDir),
            Time.deltaTime * 5f
        );

        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        if (player.health <= 0)
        {
            currentState = EnemyState.Patrolling;
        }
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

    private void UpdateChase()
    {
        //if (canSeePlayer)
        //{
        //    agent.SetDestination(target.position);
        //    agent.speed = chaseSpeed;
        //}

        //if(Vector3.Distance(transform.position, target.position) < attackRange)
        //{
        //    currentState = EnemyState.Attacking;
        //    return;
        //}

        if (canSeePlayer)
        {
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = EnemyState.Attacking;
            return;
        }
    }

    private void UpdatePatrol()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            currentIndex = (currentIndex + 1) % checkpoints.Length;
            agent.SetDestination(checkpoints[currentIndex].position);
        }

        if (player.health > 0)
        {
            if (Vector3.Distance(transform.position, target.position) < chaseRange || canSeePlayer == true)
            {
                currentState = EnemyState.Chasing;
                return;
            }
        }
    }
}