using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [Header("Configuración del Objetivo")]
    [SerializeField] private string newObjectiveText = "Encuentra a Cindy y escapa del asesino";
    [SerializeField] private bool updateOnStart = true;

    void Start()
    {
        if (updateOnStart && ObjectiveUI.Instance != null)
        {
            ObjectiveUI.Instance.SetObjective(newObjectiveText);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!updateOnStart && other.CompareTag("Player"))
        {
            if (ObjectiveUI.Instance != null)
            {
                ObjectiveUI.Instance.SetObjective(newObjectiveText);
            }
            gameObject.SetActive(false);
        }
    }
}