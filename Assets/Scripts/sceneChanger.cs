using UnityEngine;
using UnityEngine.SceneManagement;

public class randomNumber : MonoBehaviour, IInteractable
{
    public string loadScene;

    // Firma exacta de la interfaz IInteractable (sin parámetros)
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));

        if (!string.IsNullOrEmpty(loadScene))
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}