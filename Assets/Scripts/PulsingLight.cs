using UnityEngine;

public class PulsingLight : MonoBehaviour
{
    private Light myLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 3.5f;
    public float pulseSpeed = 2.5f;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        if (myLight != null)
        {
            // Crea un efecto de respiración/pulso suave en la intensidad
            float lerp = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp);
        }
    }
}