using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRadius = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InteractorSource == null) return;

            Collider[] hitColliders = Physics.OverlapSphere(InteractorSource.position, InteractRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (InteractorSource != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(InteractorSource.position, InteractRadius);
        }
    }
}