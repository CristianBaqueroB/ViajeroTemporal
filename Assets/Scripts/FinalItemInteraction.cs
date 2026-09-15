using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalItemInteraction : MonoBehaviour, IInteractable
{
    [Header("UI de Victoria")]
    [SerializeField] private GameObject panelFinal;
    [SerializeField] private TextMeshProUGUI textoFinal;

    [Header("Configuración de Escena")]
    [SerializeField] private string nombreEscenaMenu = "main";

    [Header("Mensaje Final")]
    [TextArea(4, 6)]
    [SerializeField] private string mensajeVictoria = "<b>¡MISIÓN COMPLETADA!</b>\n\nHas recuperado la pista final dentro de la cabaña. El misterio ha sido resuelto, las evidencias están aseguradas y Cindy está a salvo. Puedes regresar a tu presente.";

    private bool yaInteractuado = false;

    public void Interact()
    {
        if (yaInteractuado) return;
        yaInteractuado = true;

        CompletarMision();
    }

    private void CompletarMision()
    {
        if (panelFinal != null)
        {
            panelFinal.SetActive(true);

            if (textoFinal != null)
            {
                textoFinal.text = mensajeVictoria;
            }

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ActualizarObjetivo("¡Misión cumplida! Has completado el objetivo.");
        }
    }

    // Función vinculada al botón "Regresar al Presente"
    public void RegresarAlPresente()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}