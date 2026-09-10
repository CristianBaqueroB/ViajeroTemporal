using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Transform InstanceTransform { get; private set; }

    [Header("Movimiento")]
    public float speed = 4f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float jumpForce = 8f;
    public float groundCheckDistance = 1.2f; // Distancia para alcanzar el suelo desde el pivote
    public LayerMask groundLayer = ~0; // Detecta todas las capas por defecto

    [Header("Referencias")]
    public Animator animator;

    private Vector3 forward, right;
    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        InstanceTransform = transform;
        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Start()
    {
        if (Camera.main != null)
        {
            forward = Camera.main.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);

            right = Camera.main.transform.right;
            right.y = 0;
            right = Vector3.Normalize(right);
        }
    }

    void Update()
    {
        // Detecta el suelo lanzando un rayo hacia abajo desde el centro del personaje
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer);

        // Dibuja una línea roja/verde en la pestaña Scene para visualizar la detección de suelo
        Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * right + verticalInput * forward;
        float moveMagnitude = direction.magnitude;

        if (moveMagnitude > 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Salto al presionar Espacio si está tocando el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveMagnitude);
        }
    }
}