using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClueMissionManager : MonoBehaviour
{
    public static ClueMissionManager Instance { get; private set; }

    [Header("UI de Evidencia")]
    [SerializeField] private GameObject panelEvidencia;
    [SerializeField] private TextMeshProUGUI textoEvidencia;

    [Header("Objeto Final de la Misión")]
    [Tooltip("Arrastra aquí el objeto de la pista final que está dentro de la cabaña")]
    [SerializeField] private GameObject objetoFinal;

    private bool tieneCartera = false;
    private bool tieneFotos = false;
    private bool tieneHerramientas = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (panelEvidencia != null) panelEvidencia.SetActive(false);

        // Oculta la pista final desde el inicio del juego
        if (objetoFinal != null)
        {
            objetoFinal.SetActive(false);
        }
    }

    public void RecolectarPista(TipoPista tipo)
    {
        switch (tipo)
        {
            case TipoPista.CarteraVictima: tieneCartera = true; break;
            case TipoPista.FotosTrofeo: tieneFotos = true; break;
            case TipoPista.HerramientasCaza: tieneHerramientas = true; break;
        }

        ActualizarTexto();
        VerificarCompletado();
    }

    private void ActualizarTexto()
    {
        int total = (tieneCartera ? 1 : 0) + (tieneFotos ? 1 : 0) + (tieneHerramientas ? 1 : 0);

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ActualizarObjetivo(
                $"Pistas de Cindy ({total}/3): " +
                $"Cartera {(tieneCartera ? "[✓]" : "[ ]")} | " +
                $"Fotos {(tieneFotos ? "[✓]" : "[ ]")} | " +
                $"Herramientas {(tieneHerramientas ? "[✓]" : "[ ]")}"
            );
        }
    }

    private void VerificarCompletado()
    {
        if (tieneCartera && tieneFotos && tieneHerramientas)
        {
            if (panelEvidencia != null)
            {
                panelEvidencia.SetActive(true);

                if (textoEvidencia != null)
                {
                    textoEvidencia.text = "<b>¡EVIDENCIA CONFIRMADA!</b>\n\nLas pistas confirman que el cazador es un asesino en serie. Busca dentro de la cabaña para encontrar la pista definitiva.";
                }

                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // Vincula esta función al botón "Continuar" del Panel de Evidencia
    public void ContinuarACabana()
    {
        Time.timeScale = 1f;
        if (panelEvidencia != null) panelEvidencia.SetActive(false);

        // Activa la pista final dentro de la cabaña
        if (objetoFinal != null)
        {
            objetoFinal.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ActualizarObjetivo("Se ha ubicado una nueva pista dentro de la cabaña. Si encuentras la pista sin que el asesino te vea podrás regresar a tu vida normal y Cindy podrá escapar.");
        }
    }
}