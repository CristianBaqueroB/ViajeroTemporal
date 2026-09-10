using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Propiedad Singleton accesible desde cualquier script (EnemyController)
    public static Transform InstanceTransform { get; private set; }

    [Header("Configuración de Movimiento")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float runSpeed = 5.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Animator _animator;
    private Vector3 _forward, _right;
    private float _currentSpeed;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        // Se registra a sí mismo inmediatamente al cargarse en la escena
        InstanceTransform = transform;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            _forward = Camera.main.transform.forward;
            _forward.y = 0;
            _forward = Vector3.Normalize(_forward);

            _right = Camera.main.transform.right;
            _right.y = 0;
            _right = Vector3.Normalize(_right);
        }
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * _right + verticalInput * _forward;

        if (direction.magnitude > 0.1f)
        {
            // Detecta si se presiona la tecla de Sprint (Shift Izquierdo o Derecho)
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // Define la velocidad deseada
            float targetSpeed = isSprinting ? runSpeed : walkSpeed;

            // Transición suave de velocidad
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * 10f);

            // Desplazamiento y rotación
            transform.position += direction.normalized * (_currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Si no se presiona ninguna tecla, la velocidad desacelera a 0
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime * 10f);
        }

        // Envia el valor de velocidad al Animator
        if (_animator != null)
        {
            _animator.SetFloat(SpeedHash, _currentSpeed);
        }
    }

    private void OnDestroy()
    {
        // Limpia la referencia global al destruir o cambiar de escena
        if (InstanceTransform == transform)
        {
            InstanceTransform = null;
        }
    }
}