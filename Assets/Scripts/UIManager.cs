using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Derrota")]
    [SerializeField] private GameObject panelDerrota;
    [SerializeField] private TextMeshProUGUI textoDerrota;
    [SerializeField] private Button botonReintentar;

    [Header("Barra de Vida (Opcional)")]
    [SerializeField] private Slider healthBar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (panelDerrota != null)
            panelDerrota.SetActive(false);

        if (botonReintentar != null)
        {
            botonReintentar.onClick.RemoveAllListeners();
            botonReintentar.onClick.AddListener(ReiniciarNivel);
        }

        // 1. Reubica al jugador en la posición del SpawnPoint de la escena
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null && PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.transform.position = spawnPoint.transform.position;
            PlayerMovement.Instance.transform.rotation = spawnPoint.transform.rotation;
        }

        // 2. Busca al jugador persistente y restablece su vida al 100%
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetearVida();
        }
    }

    public void MostrarPantallaDerrota()
    {
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);

            if (textoDerrota != null)
            {
                textoDerrota.text = "<b>¡HAS FALLADO!</b>\n\nEl enemigo te ha derrotado. Inténtalo de nuevo.";
            }
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ActualizarBarraVida(float vidaActual, float vidaMaxima)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = vidaMaxima;
            healthBar.value = vidaActual;
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}