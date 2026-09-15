using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("main")]
    [SerializeField] private string nombreEscenaJuego = "main";

    [Header("Paneles de Interfaz")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelOpciones;

    private void Start()
    {
        // Aseguramos que el tiempo corra a velocidad normal y el ratón sea visible
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(true);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    // --- 1. BOTÓN JUGAR ---
    public void Jugar()
    {
        Debug.Log("Cargando escena de juego: " + nombreEscenaJuego);
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // --- 2. BOTÓN OPCIONES ---
    public void AbrirOpciones()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(true);
    }

    // Control general de volumen de audio
    public void CambiarVolumenGeneral(float volumen)
    {
        AudioListener.volume = volumen;
    }

    // --- 3. BOTÓN SALIR ---
    public void Salir()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();

        // Si estás dentro del editor de Unity, detiene la ejecución
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}