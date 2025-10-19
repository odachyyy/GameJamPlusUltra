using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        Chasing
    }

    private State currentState;
    private EnemyMovement enemyMovement;
    private Transform player;

    [Header("Configuração da Patrulha")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Transform ledgeCheckPoint;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Configuração da Perseguição")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private LayerMask whatIsPlayer;

    private float moveDirection = 1f;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        currentState = State.Patrolling;
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
                
                moveDirection = enemyMovement.IsFacingRight() ? 1f : -1f;
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
        bool isNearWall = Physics2D.OverlapCircle(wallCheckPoint.position, checkRadius, whatIsGround);

        bool isNearLedge = !Physics2D.OverlapCircle(ledgeCheckPoint.position, checkRadius, whatIsGround); 

        if (isNearWall || isNearLedge)
        {
            moveDirection *= -1;
            enemyMovement.Flip();
        }

        enemyMovement.SetHorizontalMovement(patrolSpeed * moveDirection);
    }

    private void Chase()
    {
        if (player == null) return;

        float directionToPlayer = (player.position.x - transform.position.x);

        if (directionToPlayer > 0.1f && !enemyMovement.IsFacingRight())
        {
            enemyMovement.Flip();
        }
        else if (directionToPlayer < -0.1f && enemyMovement.IsFacingRight())
        {
            enemyMovement.Flip();
        }

        bool isNearLedge = !Physics2D.OverlapCircle(ledgeCheckPoint.position, checkRadius, whatIsGround);

        if (isNearLedge)
        {
            enemyMovement.SetHorizontalMovement(0);
        }
        else
        {
            float chaseDirection = Mathf.Sign(directionToPlayer);
            enemyMovement.SetHorizontalMovement(chaseSpeed * chaseDirection);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if(wallCheckPoint) Gizmos.DrawWireSphere(wallCheckPoint.position, checkRadius);
        if(ledgeCheckPoint) Gizmos.DrawWireSphere(ledgeCheckPoint.position, checkRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
