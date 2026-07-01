using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    
    public void Jogar()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
