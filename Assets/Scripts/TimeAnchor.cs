using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeAnchor : MonoBehaviour
{
    [Header("Elementos de Interfaz")]
    [SerializeField] private GameObject interactPrompt;   // Mensaje "[E] Presiona E"
    [SerializeField] private GameObject storyPanel;       // Fondo negro
    [SerializeField] private TextMeshProUGUI storyText;   // Texto narrativo

    [Header("Configuración Narrativa y Escena")]
    [SerializeField] private string alaskaSceneName = "lvl1";
    [TextArea(3, 5)]
    [SerializeField] private string narrativeStory = "1983: Varias víctimas desaparecieron en los bosques de Alaska...\n\nEl frío de esa noche aún permanece atrapado en estas esposas.";
    [SerializeField] private float displayDuration = 5f;  // Duración del texto en segundos

    private bool isPlayerNearby = false;
    private bool isTransitioning = false;

    private void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (storyPanel != null) storyPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isPlayerNearby = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNearby && !isTransitioning && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartNarrativeSequence());
        }
    }

    private IEnumerator StartNarrativeSequence()
    {
        isTransitioning = true;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        // Muestra la pantalla negra con el relato
        if (storyPanel != null) storyPanel.SetActive(true);
        if (storyText != null) storyText.text = narrativeStory;

        // Espera los segundos indicados para permitir la lectura
        yield return new WaitForSeconds(displayDuration);

        // Actualiza el objetivo global del juego
        if (ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective("Encuentra a Cindy y escapa del asesino");
        }

        // Carga la escena de Alaska
        if (Application.CanStreamedLevelBeLoaded(alaskaSceneName))
        {
            SceneManager.LoadScene(alaskaSceneName);
        }
        else
        {
            Debug.Log("Escena de Alaska activada: " + alaskaSceneName);
        }
    }
}   