using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Following,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField] private float patrolWaitTime = 4f;
    [SerializeField] private float stopAtDistance = 0.5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float viewAngle = 120f;
    [SerializeField] private float losePlayerTime = 3f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Physics Mask")]
    [Tooltip("Capas que BLOQUEAN la visión (ej: Default, Obstacles). NO incluyas la capa del Player ni del Enemy.")]
    [SerializeField] private LayerMask obstacleLayers;

    [Header("Speed Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyState _state = EnemyState.Patrolling;
    private int _currentPatrolIndex;
    private bool _isWaiting;
    private float _timeSinceLostPlayer;
    private Coroutine _waitCoroutine;
    private bool _isHitting;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsFollowingHash = Animator.StringToHash("IsFollowing");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Start()
    {
        if (patrolPoints.Length > 0)
        {
            MoveToCurrentPatrolPoint();
        }
    }

    public void Update()
    {
        // Obtiene directamente el Transform de PlayerCapsule vía Singleton
        if (player == null && PlayerMovement.InstanceTransform != null)
        {
            player = PlayerMovement.InstanceTransform;
        }

        if (player == null)
        {
            Patrol();
            UpdateAnimations();
            return;
        }

        var distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (_state)
        {
            case EnemyState.Patrolling:
                Patrol();

                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    StopWaitCoroutine();
                    _state = EnemyState.Following;
                }
                break;

            case EnemyState.Following:
                FollowPlayer();
                if (distanceToPlayer <= attackRange)
                {
                    _state = EnemyState.Attacking;
                    StartAttack();
                }
                if (!CanSeePlayer())
                {
                    _timeSinceLostPlayer += Time.deltaTime;
                    if (_timeSinceLostPlayer >= losePlayerTime)
                    {
                        _state = EnemyState.Patrolling;
                        GoToClosestPatrolPoint();
                    }
                }
                else
                {
                    _timeSinceLostPlayer = 0f;
                }
                break;
            case EnemyState.Attacking:
                Attack();

                // Si terminó la animación del golpe y el jugador se alejó, vuelve a perseguir
                if (!_isHitting && distanceToPlayer > attackRange)
                {
                    _state = EnemyState.Following;
                    _agent.isStopped = false;
                }
                // Si sigue en rango pero terminó el golpe previo, encadena otro ataque
                else if (!_isHitting && distanceToPlayer <= attackRange)
                {
                    StartAttack();
                }
                break;
        }

        UpdateAnimations();
    }

    private void StartAttack()
    {
        _agent.isStopped = true;
        _isHitting = true;
        _animator.SetTrigger("Hit");

    }

    private void Attack()
    {
        _agent.isStopped = true;

        // Orientar suavemente hacia el jugador durante el ataque
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void OnBiteAnimationEnd()
    {
        _isHitting = false;
    }

    private void FollowPlayer()
    {
        _agent.speed = chaseSpeed;
        if (_agent.isStopped) _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }

    private void Patrol()
    {
        if (_isWaiting || patrolPoints.Length == 0) return;

        if (!_agent.pathPending && _agent.hasPath)
        {
            float effectiveStopDistance = Mathf.Max(_agent.stoppingDistance, stopAtDistance);

            if (_agent.remainingDistance <= effectiveStopDistance)
            {
                _waitCoroutine = StartCoroutine(WaitAtPatrolPoint());
            }
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        _isWaiting = true;
        _agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
        MoveToCurrentPatrolPoint();
        _isWaiting = false;
    }

    private void MoveToCurrentPatrolPoint()
    {
        if (patrolPoints.Length == 0 || patrolPoints[_currentPatrolIndex] == null) return;

        _agent.speed = patrolSpeed;
        _agent.isStopped = false;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
    }

    private void StopWaitCoroutine()
    {
        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
        }
        _isWaiting = false;
        _agent.isStopped = false;
    }

    private void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        var closestIndex = 0;
        var closestDistance = float.MaxValue;

        for (var i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;
            var distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        _currentPatrolIndex = closestIndex;
        MoveToCurrentPatrolPoint();
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        float speed = _agent.velocity.magnitude;
        _animator.SetFloat(SpeedHash, speed);

        // Envía true cuando está persiguiendo, false cuando está en patrulla
        _animator.SetBool(IsFollowingHash, _state == EnemyState.Following);
    }

    private bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    private bool IsFacingPlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle <= viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
        Vector3 targetPosition = player.position + Vector3.up * 1.0f;
        Vector3 dirToPlayer = targetPosition - eyePosition;

        if (Physics.Raycast(eyePosition, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude))
        {
            // Detecta si impactó a PlayerCapsule, a su padre __app, o a cualquier objeto etiquetado como Player
            bool isPlayer = hit.transform == player ||
                             hit.transform.IsChildOf(player) ||
                             player.IsChildOf(hit.transform) ||
                             hit.transform.CompareTag("Player");

            Debug.DrawLine(eyePosition, hit.point, isPlayer ? Color.green : Color.red, 0.1f);
            return isPlayer;
        }

        return true;
    }
}