using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    [Header("Efecto Visual")]
    [SerializeField] private float velocidadRotacion = 50f;
    [SerializeField] private Light luzBrillo; // Opcional: luz para resaltarlo

    private void Start()
    {
        // Si no asignaste una luz manualmente, intenta buscar una en los hijos
        if (luzBrillo == null) luzBrillo = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        // Hace rotar el objeto suavemente
        transform.Rotate(Vector3.up * (velocidadRotacion * Time.deltaTime), Space.World);

        // Hace que la intensidad de la luz oscile para simular un brillo/destello
        if (luzBrillo != null)
        {
            luzBrillo.intensity = 1.0f + Mathf.PingPong(Time.time * 3f, 2.0f);
        }
    }
}