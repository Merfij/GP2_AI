using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Attacking,
    Electrocuted
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

    [Header("Health")]
    public float health;

    [Header("Enemy Type")]
    public bool canBeStunned;
    private bool isStunned;
    public bool isEthereal;

    [Header("Materials")]
    public List<Material> materials;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Transform[] checkpoints;

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
        skinnedMeshRenderer = GameObject.FindGameObjectWithTag("EnemySkin").GetComponent<SkinnedMeshRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Patrolling;
        if (checkpoints.Length > 0)
        {
            agent.SetDestination(checkpoints[currentIndex].position);
        }

        skinnedMeshRenderer.material = materials[1];
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
        if (isEthereal == true)
        {
            skinnedMeshRenderer.material = materials[1];
        }
        else
        {
            skinnedMeshRenderer.material = materials[0];
        }

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
            case EnemyState.Electrocuted:
                animator.SetInteger("AIState", 4);
                UpdateElectrocuted(); 
                break;
        }

        FieldOfViewCheck();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (player.useLightBeam == false)
        {
            isStunned = false;
        }
    }

    private void UpdateElectrocuted()
    {
        agent.isStopped = true;
        isStunned = true;
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
        isStunned = false;

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

        if (player.health <= 0)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    private void UpdateChase()
    {
        isStunned = false;
        if (canSeePlayer && player.health > 0)
        {
            agent.isStopped = false;
            float currentSpeed = Vector3.Distance(transform.position,target.transform.position);
            if (currentSpeed > chaseSpeed)
            {
                currentSpeed = chaseSpeed;
            }
            agent.speed = currentSpeed;
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
        isStunned = false;

        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
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

    public void ActivateEnemyCollider()
    {
        handCollider.enabled = true;
    }

    public void DisaleEnemyCollider()
    {
        handCollider.enabled = false;
    }

    public void StunEnemy()
    {
        if (canBeStunned == true)
        {
            Debug.Log("Enemy Stunned");
            currentState = EnemyState.Electrocuted;
        } 
    }

    public void ResumeEnemy()
    {
        if (canBeStunned == true)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    public void EtherealEnemy()
    {
        if (isEthereal == true)
        {
            skinnedMeshRenderer.material = materials[0];
        }
    }

    public void TakeDamage(int damage)
    {
        if (canBeStunned == true)
        {
            if (isStunned == true)
            {
                health -= damage;
            }
            else return;
        }
        else
        {
            health -= damage;
        }
    }
}