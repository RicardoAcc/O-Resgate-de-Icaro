using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public string spawnPointID;
    private string[] playableScenes = { "Sala Direita", "Sala Esquerda", "Sala Superior" };
    private string[] gods = { "Hefesto", "Hécate", "Hipnos", null };
    public Dictionary<string, string> sceneToGod = new Dictionary<string, string>();

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

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnID == spawnPointID)
            {
                JogadorScript.instance.transform.position = spawnPoint.transform.position;
                JogadorScript.instance.jogadorMoveScript.ResetPlayerState();
                break;
            }
        }

        spawnPointID = null;
    }

    private void chooseGodForScene(string sceneName)
    {
        if (sceneToGod.ContainsKey(sceneName))
            return;

        int randomIndex = Random.Range(0, gods.Length - 2);
        sceneToGod[sceneName] = gods[randomIndex];
    }

    public string GetGodForScene(string sceneName)
    {
        if (!sceneToGod.ContainsKey(sceneName))
        {
            chooseGodForScene(sceneName);
        }

        return sceneToGod[sceneName];
    }
}
