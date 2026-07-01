using UnityEngine;
using UnityEngine.SceneManagement;

public class JogadorScript : MonoBehaviour
{
    public static JogadorScript instance;
    public bool temDash = false;
    public bool temPuloDuplo = false;
    public bool temEscalada = false;
    public JogadorMoveScript jogadorMoveScript;
    public int vidas = 5;
    public string ultimoSpawnPoint = "";
    private float inicioInvulnerabilidade = 0;
    public float tempoInvulnerabilidade = 1f;
    
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

    public bool GetTemDash()
    {
        return temDash;
    }
    
    public bool GetTemPuloDuplo()
    {
        return temPuloDuplo;
    }

    public bool GetTemEscalada()
    {
        return temEscalada;
    }

    public int GetVidas()
    {
        return vidas;
    }

    private void ResetaJogador()
    {
        temDash = false;
        temEscalada = false;
        temPuloDuplo = false;
        jogadorMoveScript.isSleeping = false;
        vidas = 5;
    }

    public void RecebeDano()
    {
        if (inicioInvulnerabilidade + tempoInvulnerabilidade >= Time.time)
            return;
        
        vidas -= 1;

        if (vidas <= 0)
        {
            GameController.instance.ResetaTodasCenas();
            ResetaJogador();
            inicioInvulnerabilidade = Time.time;
        }
        else
        {
            jogadorMoveScript.isSleeping = false;
            GameController.instance.ResetaCena(ultimoSpawnPoint);
            inicioInvulnerabilidade = Time.time;
        }
    }

}
