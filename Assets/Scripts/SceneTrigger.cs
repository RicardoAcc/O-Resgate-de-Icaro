using UnityEngine;

public class SceneTriggerController : MonoBehaviour
{
    public string sceneName;
    public string targetSpawnPointID;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Jogador"))
        {
            GameController.instance.LoadScene(sceneName, targetSpawnPointID);
        }
    }
}
