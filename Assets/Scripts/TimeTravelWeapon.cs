using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTravelWeapon : MonoBehaviour, IInteractable
{
    [Header("Configuración del Viaje Temporal")]
    [SerializeField] private string pastSceneName = "lvl1";

    public void Interact()
    {
        if (!string.IsNullOrEmpty(pastSceneName))
        {
            SceneManager.LoadScene(pastSceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena del pasado.");
        }
    }
}