using UnityEngine;
using TMPro;

public class CindyInteraction : MonoBehaviour
{
    [Header("UI de Diálogo")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Mensaje de Salvación")]
    [TextArea(3, 5)]
    [SerializeField] private string mensajeDialogo = "<b>Jugador:</b> ¡Tranquila, Cindy! Te encontré. Prometo que no voy a dejar que ese cazador te asesine.\n\n<i>[Presiona E o ESPACIO para continuar]</i>";

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

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ActualizarObjetivo("Escolta a Cindy a través del bosque.");
        }

        CindyFollower follower = GetComponent<CindyFollower>();
        if (follower != null)
        {
            follower.EmpezarASeguir();
        }
    }
}