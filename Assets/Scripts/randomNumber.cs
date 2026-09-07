using UnityEngine;

public class randomNumber : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
    }
}
