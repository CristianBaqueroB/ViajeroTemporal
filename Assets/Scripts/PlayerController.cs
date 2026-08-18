using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private void Start()
    {
        // Obtenemos automáticamente el componente CharacterController adherido al jugador
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Comprobamos si el jugador está tocando el suelo
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Capturamos la entrada del teclado (WASD / Flechas)
        float moveX = Input.GetAxis("Horizontal"); // A/D o Flecha Izquierda/Derecha
        float moveZ = Input.GetAxis("Vertical");   // W/S o Flecha Arriba/Abajo

        // Calculamos la dirección en la que nos queremos mover en base a hacia dónde mira el jugador
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Desplazamos al jugador horizontalmente
        controller.Move(move * playerSpeed * Time.deltaTime);

        // Aplicamos la gravedad verticalmente
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}