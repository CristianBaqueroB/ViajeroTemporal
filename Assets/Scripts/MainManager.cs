using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("Punto de Aparición tras Ganar")]
    [SerializeField] private Transform spawnPointGanador;

    private void Start()
    {
        // Reubica al jugador persistente si existe en la escena
        if (PlayerMovement.Instance != null)
        {
            if (spawnPointGanador != null)
            {
                PlayerMovement.Instance.transform.position = spawnPointGanador.position;
                PlayerMovement.Instance.transform.rotation = spawnPointGanador.rotation;
            }
            else
            {
                // Alternativa por búsqueda si no se asignó en el Inspector
                GameObject spawn = GameObject.Find("SpawnPointMain");
                if (spawn != null)
                {
                    PlayerMovement.Instance.transform.position = spawn.transform.position;
                    PlayerMovement.Instance.transform.rotation = spawn.transform.rotation;
                }
            }
        }
    }
}