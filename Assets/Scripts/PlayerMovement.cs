using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public static Transform InstanceTransform { get; private set; }

    [Header("Movimiento")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 7.5f;
    public float rotationSpeed = 10f;
    public float acceleration = 8f;

    [Header("Salto")]
    public float jumpForce = 8f;
    public float groundCheckDistance = 1.2f;
    public LayerMask groundLayer = ~0;

    [Header("Referencias")]
    public Animator animator;

    private Rigidbody rb;
    private bool isGrounded;
    private float currentSpeedSmooth;

    private void Awake()
    {
        // Obtiene el objeto raíz (Player) para no destruir solo el script o un objeto hijo
        GameObject rootPlayer = transform.root.gameObject;

        if (Instance != null && Instance != this)
        {
            Destroy(rootPlayer);
            return;
        }

        Instance = this;
        InstanceTransform = transform;
        DontDestroyOnLoad(rootPlayer);

        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        // Detección de suelo
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer);

        // Recalcula la orientación según la cámara activa en cada frame
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (Camera.main != null)
        {
            forward = Camera.main.transform.forward;
            forward.y = 0;
            forward.Normalize();

            right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * right + verticalInput * forward;
        float moveMagnitude = direction.magnitude;

        // Detección de carrera
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && moveMagnitude > 0.1f;
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        if (moveMagnitude > 0.1f)
        {
            currentSpeedSmooth = Mathf.Lerp(currentSpeedSmooth, targetSpeed, acceleration * Time.deltaTime);
            transform.position += direction.normalized * currentSpeedSmooth * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeedSmooth = Mathf.Lerp(currentSpeedSmooth, 0f, acceleration * Time.deltaTime);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Enviar parámetro al Blend Tree (0 = Idle, 1 = Walking, 2 = Running)
        if (animator != null)
        {
            float targetAnimValue = moveMagnitude > 0.1f ? (isRunning ? 2f : 1f) : 0f;
            animator.SetFloat("Speed", targetAnimValue, 0.15f, Time.deltaTime);
        }
    }
}