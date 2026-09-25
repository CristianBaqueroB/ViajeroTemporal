using UnityEngine;

public class ControlTecho : MonoBehaviour
{
    // Array para arrastrar múltiples piezas del techo desde el Inspector
    [SerializeField] private Renderer[] techosRenderers;

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra al trigger, ocultamos el techo de inmediato
        if (other.CompareTag("Player"))
        {
            MostrarTecho(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del trigger, volvemos a mostrar el techo de inmediato
        if (other.CompareTag("Player"))
        {
            MostrarTecho(true);
        }
    }

    private void MostrarTecho(bool visible)
    {
        if (techosRenderers == null) return;

        foreach (Renderer r in techosRenderers)
        {
            if (r != null)
            {
                r.enabled = visible;
            }
        }
    }
}