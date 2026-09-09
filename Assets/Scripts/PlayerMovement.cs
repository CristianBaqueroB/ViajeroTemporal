using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Propiedad Singleton accesible desde cualquier script (EnemyController)
    public static Transform InstanceTransform { get; private set; }

    public float speed = 4;
    public float rotationSpeed = 10;

    private Vector3 forward, right;

    private void Awake()
    {
        // Se registra a sí mismo inmediatamente al cargarse en la escena
        InstanceTransform = transform;
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * right + verticalInput * forward;
        if (direction.magnitude > 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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