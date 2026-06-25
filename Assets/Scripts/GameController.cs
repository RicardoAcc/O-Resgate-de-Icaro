using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public string spawnPointID;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, string targetSpawnPointID)
    {
        spawnPointID = targetSpawnPointID;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(spawnPointID))
            return;
            
        
        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>();

        Debug.Log("Spawn Point ID: " + spawnPointID);
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            Debug.Log("Checking Spawn Point: " + spawnPoint.spawnID);
            if (spawnPoint.spawnID == spawnPointID)
            {
                JogadorScript.instance.transform.position = spawnPoint.transform.position;
                break;
            }
        }

        spawnPointID = null;
    }
}
