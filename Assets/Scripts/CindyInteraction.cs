using UnityEngine;
using TMPro;

public class CindyInteraction : MonoBehaviour
{
    [Header("UI de Diálogo")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Mensaje de Salvación")]
    [TextArea(4, 8)]
    [SerializeField] private string mensajeDialogo = "<b>Jugador:</b> ¡Tranquila, Cindy! Te encontré. Prometo que no voy a dejar que ese cazador te asesine.\n\n<b>Cindy:</b> ¡Gracias! Pero ten cuidado, hay unas pistas detrás de la cabaña. Encuéntralas para demostrar que el asesino es quien vive ahí.\n\n<i>[Presiona E, ESPACIO o Clic para continuar]</i>";

    private bool yaFueEncontrada = false;
    private bool esperandoTeclas = false;

    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaFueEncontrada)
        {
            yaFueEncontrada = true;
            MostrarDialogo();
        }
    }

    private void MostrarDialogo()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            if (dialogueText != null) dialogueText.text = mensajeDialogo;

            Time.timeScale = 0f; // Pausa el tiempo
            esperandoTeclas = true;
        }
    }

    private void Update()
    {
        // Detecta la tecla aun con el tiempo en 0
        if (esperandoTeclas && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            CerrarDialogoYContinuar();
        }
    }

    public void CerrarDialogoYContinuar()
    {
        esperandoTeclas = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        Time.timeScale = 1f; // Reanuda el juego

        // Actualiza el objetivo en pantalla para dirigir al jugador a las pistas
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ActualizarObjetivo("Hay unas pistas detrás de la cabaña, encuéntralas para demostrar que el asesino es quien vive en esta cabaña.");
        }

        CindyFollower follower = GetComponent<CindyFollower>();
        if (follower != null)
        {
            follower.EmpezarASeguir();
        }
    }
}