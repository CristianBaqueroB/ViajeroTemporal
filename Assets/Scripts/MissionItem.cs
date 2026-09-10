using UnityEngine;

public enum TipoPista { CarteraVictima, FotosTrofeo, HerramientasCaza }

public class MissionItem : MonoBehaviour
{
    [Header("Tipo de Pista")]
    public TipoPista tipoPista;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ClueMissionManager.Instance != null)
            {
                ClueMissionManager.Instance.RecolectarPista(tipoPista);
            }
            Destroy(gameObject);
        }
    }
}