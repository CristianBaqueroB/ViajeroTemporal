using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "<b>OBJETIVO:</b> " + newObjective;
        }
    }
}