using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Header("UI del Objetivo")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ActualizarObjetivo("Sigue los sollozos y encuentra a Cindy.");
    }

    public void ActualizarObjetivo(string nuevoTexto)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "<b>OBJETIVO:</b> " + nuevoTexto;
        }
    }
}