using System.Collections;
using UnityEngine;

public class ControlTecho : MonoBehaviour
{
    // Cambiamos a un Array [] para poder arrastrar múltiples piezas del techo
    [SerializeField] private Renderer[] techosRenderers;
    [SerializeField] private float velocidadFade = 2f;

    private Coroutine corrutinaActual;
    private MaterialPropertyBlock propiedadBloque;
    private int idColorBase;

    private void Awake()
    {
        propiedadBloque = new MaterialPropertyBlock();
        idColorBase = Shader.PropertyToID("_BaseColor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) IniciarFade(0f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) IniciarFade(1f);
    }

    private void IniciarFade(float alfaObjetivo)
    {
        if (corrutinaActual != null) StopCoroutine(corrutinaActual);
        corrutinaActual = StartCoroutine(FadeTecho(alfaObjetivo));
    }

    private IEnumerator FadeTecho(float alfaObjetivo)
    {
        // Si vamos a mostrar el techo, activamos todos primero
        if (alfaObjetivo > 0f)
        {
            foreach (Renderer r in techosRenderers) r.enabled = true;
        }

        // Tomamos el color del primer elemento como referencia para la transición
        techosRenderers[0].GetPropertyBlock(propiedadBloque);
        Color colorActual = techosRenderers[0].sharedMaterial.GetColor(idColorBase);
        if (propiedadBloque.GetColor(idColorBase) != Color.clear)
        {
            colorActual = propiedadBloque.GetColor(idColorBase);
        }

        while (!Mathf.Approximately(colorActual.a, alfaObjetivo))
        {
            colorActual.a = Mathf.MoveTowards(colorActual.a, alfaObjetivo, velocidadFade * Time.deltaTime);
            propiedadBloque.SetColor(idColorBase, colorActual);

            // ¡Clave! Aplicamos el cambio de color a cada pieza de la lista
            foreach (Renderer r in techosRenderers)
            {
                r.SetPropertyBlock(propiedadBloque);
            }

            yield return null;
        }

        // Si el objetivo era ser invisible, apagamos todos los renderizadores
        if (alfaObjetivo == 0f)
        {
            foreach (Renderer r in techosRenderers) r.enabled = false;
        }
    }
}
