using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private float destinationUpdateInterval = 0.25f;
    [SerializeField] private float attackStartDistance = 1.8f;
    [SerializeField] private float attackInterval = 1.2f;
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string attackTriggerParameter = "Attack";

    private NavMeshAgent agent;
    private int currentHealth;
    private float nextDestinationUpdateTime;
    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        UpdateDestination();
    }

    private void Update()
    {
        if (targetPoint == null)
        {
            UpdateAnimatorSpeed(0f);
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);
        bool isInAttackRange = distanceToTarget <= attackStartDistance;

        agent.isStopped = isInAttackRange;

        if (!isInAttackRange && Time.time >= nextDestinationUpdateTime)
        {
            UpdateDestination();
        }

        if (isInAttackRange)
        {
            TryAttack();
        }

        UpdateAnimatorSpeed(agent.velocity.magnitude);
    }

    public void SetTarget(Transform newTarget)
    {
        targetPoint = newTarget;
        UpdateDestination();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateDestination()
    {
        nextDestinationUpdateTime = Time.time + destinationUpdateInterval;

        if (targetPoint == null || agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        agent.SetDestination(targetPoint.position);
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackInterval;

        if (animator != null)
        {
            animator.SetTrigger(attackTriggerParameter);
        }
    }

    private void UpdateAnimatorSpeed(float speed)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat(speedParameter, speed);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
