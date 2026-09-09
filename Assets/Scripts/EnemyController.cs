using System.Collections; // Necesario para IEnumerator
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField] private float patrolWaitTime = 4f;
    [SerializeField] private float stopAtDistance = 0.5f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _currentPatrolIndex;
    private bool _isWaiting;

    // Hash para optimizar la llamada al parámetro del Animator
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Start()
    {
        GoToNextPatrolPoint();
    }

    public void Update()
    {
        Patrol();
        UpdateAnimations();
    }

    private void Patrol()
    {
        if (_isWaiting || patrolPoints.Length == 0) return;

        // Comprueba si el agente llegó a su destino
        if (!_agent.pathPending && _agent.remainingDistance <= stopAtDistance)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        _isWaiting = true;
        _agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        _agent.isStopped = false;
        GoToNextPatrolPoint();
        _isWaiting = false;
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);

        // Corregido: Lenght -> Length
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        // Obtiene la velocidad actual del NavMeshAgent (0 cuando está detenido)
        float speed = _agent.velocity.magnitude;

        // Actualiza el parámetro Float en lugar de Bool
        _animator.SetFloat("Speed", speed);
    }
}