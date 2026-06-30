using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public string spawnPointID;

    private bool isLoadingScene;

    private string[] playableScenes = { "Sala Direita", "Sala Esquerda", "Sala Superior" };
    private string[] gods = { "Hefesto", "Hecate", "Hipnos" };

    public Dictionary<string, string> sceneToGod = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, string targetSpawnPointID)
    {
        if (isLoadingScene)
            return;

        StartCoroutine(LoadSceneRoutine(sceneName, targetSpawnPointID));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, string targetSpawnPointID)
    {
        isLoadingScene = true;
        spawnPointID = targetSpawnPointID;

        if (ScreenFader.instance != null)
            yield return ScreenFader.instance.FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return null;

        InitializeSceneAfterLoad();

        yield return new WaitForFixedUpdate();

        Physics2D.SyncTransforms();

        InitializeSceneAfterLoad();

        yield return null;

        if (ScreenFader.instance != null)
            yield return ScreenFader.instance.FadeIn();

        isLoadingScene = false;
    }

    private void InitializeSceneAfterLoad()
    {
        Transform player = JogadorScript.instance != null
            ? JogadorScript.instance.transform
            : null;

        if (player == null)
            return;

        if (!string.IsNullOrEmpty(spawnPointID))
        {
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude);

            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.spawnID == spawnPointID)
                {
                    player.position = spawnPoint.transform.position;

                    if (JogadorScript.instance.jogadorMoveScript != null)
                        JogadorScript.instance.jogadorMoveScript.ResetPlayerState();

                    break;
                }
            }

            spawnPointID = null;
        }

        Physics2D.SyncTransforms();

        Collider2D bounds = null;

        CameraBoundsMarker boundsMarker = FindAnyObjectByType<CameraBoundsMarker>();

        if (boundsMarker != null)
            bounds = boundsMarker.bounds;

        if (CameraController.instance != null)
        {
            CameraController.instance.InitializeForScene(player, bounds);
        }
    }

    private void chooseGodForScene(string sceneName)
    {
        if (sceneToGod.ContainsKey(sceneName))
            return;

        int randomIndex = Random.Range(0, gods.Length);
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