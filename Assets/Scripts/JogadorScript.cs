using UnityEngine;

public class JogadorScript : MonoBehaviour
{
    public static JogadorScript instance;
    public JogadorMoveScript jogadorMoveScript;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        jogadorMoveScript = GetComponent<JogadorMoveScript>();
    }
}
