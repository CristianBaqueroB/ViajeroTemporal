using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour, IInteractable
{
    [Header("Configuración de Escena")]
    public string loadScene; // Nombre de la escena destino (ej: Mapa_2)

    [Header("UI de Interacción e Historia")]
    [SerializeField] private GameObject promptUI;  // El objeto "Text (TMP)" de Presiona E
    [SerializeField] private GameObject storyPanel; // El objeto "CanvasHistoria"

    private bool isPlayerNearby = false;

    private void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
        if (storyPanel != null) storyPanel.SetActive(false);
    }

    private void Update()
    {
        // Solo procesa la tecla E si el panel NO está abierto aún
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (storyPanel != null && !storyPanel.activeSelf)
            {
                Interact();
            }
        }
    }

    // Este método es llamado por el Trigger O por tu script Interactor del jugador
    public void Interact()
    {
        if (storyPanel != null && !storyPanel.activeSelf)
        {
            storyPanel.SetActive(true);
            if (promptUI != null) promptUI.SetActive(false);

            Time.timeScale = 0f; // Pausa el juego
            Cursor.lockState = CursorLockMode.None; // Libera el cursor
            Cursor.visible = true;
        }
    }

    // Este método se asigna únicamente en el evento OnClick() del Botón "Continuar"
    public void CargarSiguienteEscena()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!string.IsNullOrEmpty(loadScene))
        {
            SceneManager.LoadScene(loadScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}