using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRadius = 2f; // Radio del área de interacción

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InteractorSource == null) return;

            // Detecta todos los colliders dentro del radio de la esfera
            Collider[] hitColliders = Physics.OverlapSphere(InteractorSource.position, InteractRadius);

            foreach (var hitCollider in hitColliders)
            {
                // Busca si alguno de los objetos encontrados tiene la interfaz IInteractable
                if (hitCollider.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    break; // Ejecuta la interacción con el primero que encuentre y sale del bucle
                }
            }
        }
    }

    // Dibuja la esfera en la pestaña Scene para que veas el área de alcance
    private void OnDrawGizmosSelected()
    {
        if (InteractorSource != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(InteractorSource.position, InteractRadius);
        }
    }
}