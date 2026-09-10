using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    private int pistasRecolectadas = 0;
    private const int TOTAL_PISTAS = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RecolectarPista(TipoPista tipo)
    {
        pistasRecolectadas++;

        if (pistasRecolectadas < TOTAL_PISTAS)
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.ActualizarObjetivo($"Investiga el bosque: Junta 3 pistas del asesino ({pistasRecolectadas}/{TOTAL_PISTAS})");
            }
        }
        else
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.ActualizarObjetivo("¡Pistas suficientes! Infíltrate en la cabaña y busca el Cuaderno de Coordenadas.");
            }
        }
    }
}