using UnityEngine;
using UnityEngine.AI;

public class CindyFollower : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    [SerializeField] private float stoppingDistance = 2.0f;
    [SerializeField] private float followSpeed = 3.5f;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private Animator _animator;
    private bool _isFollowing = false;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (_agent != null)
        {
            _agent.stoppingDistance = stoppingDistance;
            _agent.speed = followSpeed;
        }
    }

    public void EmpezarASeguir()
    {
        if (PlayerMovement.InstanceTransform != null)
        {
            _playerTransform = PlayerMovement.InstanceTransform;
            _isFollowing = true;
        }
    }

    private void Update()
    {
        if (!_isFollowing || _playerTransform == null || _agent == null) return;

        _agent.SetDestination(_playerTransform.position);

        if (_animator != null)
        {
            _animator.SetFloat(SpeedHash, _agent.velocity.magnitude);
        }
    }
}