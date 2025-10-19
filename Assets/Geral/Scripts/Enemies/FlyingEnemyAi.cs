using UnityEngine;

[RequireComponent(typeof(FlyingEnemyMovement))]
public class FlyingEnemyAi : MonoBehaviour
{
    private enum State { Patrolling, Chasing }
    private State currentState;
    private FlyingEnemyMovement enemyMovement;
    private Transform player;

    [Header("Patrulha")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private bool patrolVertically = false; 
    [SerializeField] private float arrivalThreshold = 0.5f; 

    [Header("Perseguição")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float detectionRange = 7f; 
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float chaseStopDistance = 1f;

    private Vector3 patrolPointA;
    private Vector3 patrolPointB;
    private Vector3 currentPatrolTargetPosition;
    private bool movingTowardsB;

    private void Awake()
    {
        enemyMovement = GetComponent<FlyingEnemyMovement>();
        currentState = State.Patrolling;

        Vector3 homePosition = transform.position;
        Vector3 patrolDirection = (patrolVertically) ? Vector3.up : Vector3.right;

        patrolPointA = homePosition - (patrolDirection * patrolRange);
        patrolPointB = homePosition + (patrolDirection * patrolRange);

        currentPatrolTargetPosition = patrolPointA;
        movingTowardsB = false;
    }

    private void Update()
    {
        HandleStateTransitions();
        HandleCurrentStateLogic();
    }

    private void HandleStateTransitions()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, whatIsPlayer);
        if (playerCollider != null)
        {
            currentState = State.Chasing;
            player = playerCollider.transform;
        }
        else
        {
            if (currentState == State.Chasing)
            {
                currentState = State.Patrolling;
                float distA = Vector3.Distance(transform.position, patrolPointA);
                float distB = Vector3.Distance(transform.position, patrolPointB);
                if (distA < distB)
                {
                    currentPatrolTargetPosition = patrolPointA;
                    movingTowardsB = false;
                }
                else
                {
                    currentPatrolTargetPosition = patrolPointB;
                    movingTowardsB = true;
                }
            }
            player = null;
        }
    }

    private void HandleCurrentStateLogic()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        Vector2 directionToTarget = (currentPatrolTargetPosition - transform.position).normalized;
        
        enemyMovement.SetVelocity(directionToTarget * patrolSpeed);

        HandleFlip(directionToTarget.x);

        if (Vector2.Distance(transform.position, currentPatrolTargetPosition) < arrivalThreshold)
        {
            if (movingTowardsB)
            {
                currentPatrolTargetPosition = patrolPointA;
                movingTowardsB = false;
            }
            else
            {
                currentPatrolTargetPosition = patrolPointB;
                movingTowardsB = true;
            }
        }
    }

    private void Chase()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < chaseStopDistance)
        {
            enemyMovement.SetVelocity(Vector2.zero); 
            return;
        }
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        enemyMovement.SetVelocity(directionToPlayer * chaseSpeed);
        HandleFlip(directionToPlayer.x);
    }
    private void HandleFlip(float horizontalDirection)
    {
        if (horizontalDirection > 0.01f && !enemyMovement.IsFacingRight())
            enemyMovement.Flip();
        else if (horizontalDirection < -0.01f && enemyMovement.IsFacingRight())
            enemyMovement.Flip();
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) 
        {
            Gizmos.color = Color.blue;
            Vector3 home = transform.position;
            Vector3 dir = (patrolVertically) ? Vector3.up : Vector3.right;
            Vector3 pA = home - (dir * patrolRange);
            Vector3 pB = home + (dir * patrolRange);
            Gizmos.DrawWireSphere(pA, arrivalThreshold);
            Gizmos.DrawWireSphere(pB, arrivalThreshold);
            Gizmos.DrawLine(pA, pB);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseStopDistance);
    }
}
