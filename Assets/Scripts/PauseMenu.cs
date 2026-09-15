using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    [SerializeField] private GameObject menuPausaUI;
    [SerializeField] private GameObject panelOpciones;

    [Header("Configuración de Escena")]
    [Tooltip("Nombre de la escena del menú principal")]
    [SerializeField] private string nombreMenuPrincipal = "MainMenuScen";

    public static bool JuegoPausado = false;

    private void Start()
    {
        // Aseguramos que el menú de pausa inicie oculto al cargar la escena
        if (menuPausaUI != null) menuPausaUI.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    private void Update()
    {
        // Detectar la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    // --- 1. CONTINUAR EL JUEGO ---
    public void Reanudar()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
        menuPausaUI.SetActive(false);

        Time.timeScale = 1f; // Reanuda el tiempo del juego
        JuegoPausado = false;

        // Oculta el ratón para volver al control del personaje
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- 2. PAUSAR EL JUEGO ---
    public void Pausar()
    {
        menuPausaUI.SetActive(true);

        Time.timeScale = 0f; // Congela el tiempo del juego (físicas, animaciones)
        JuegoPausado = true;

        // Libera el ratón para interactuar con los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- 3. ABRIR OPCIONES ---
    public void AbrirOpciones()
    {
        menuPausaUI.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
        menuPausaUI.SetActive(true);
    }

    // --- 4. VOLVER AL MENÚ PRINCIPAL ---
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f; // Importante reanudar el tiempo antes de cambiar de escena
        JuegoPausado = false;
        SceneManager.LoadScene(nombreMenuPrincipal);
    }

    // --- 5. SALIR DEL JUEGO ---
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}