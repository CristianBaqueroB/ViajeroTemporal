using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnpointFinder : MonoBehaviour
{
    [SerializeField] private Transform player;
    private GameObject spawnPoint;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "lvl1")
        {
            spawnPoint = GameObject.Find("spawnpoint");
            player.SetPositionAndRotation(spawnPoint.transform.position, player.transform.rotation);
            Physics.SyncTransforms();
        }
    }
}
