using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGod : MonoBehaviour
{
    [SerializeField] private string God;

    void Start()
    {
        EnableGod();
    }

    public void EnableGod()
    {
        if (GameController.instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            string selectedGod = GameController.instance.GetGodForScene(sceneName);

            if (GlobalLightController.instance != null)
            {
                GlobalLightController.instance.ApplyGodLight(selectedGod);
            }

            gameObject.SetActive(selectedGod == God);
        }
    }

}
