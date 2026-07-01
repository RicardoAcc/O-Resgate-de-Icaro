using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    [Header("Configurações do Poder")]
    public bool liberaDash = false;
    public bool liberaPuloDuplo = false;
    public bool liberaEscalada = false;

    public void Start()
    {
        JogadorScript scriptDoJogador = JogadorScript.instance;
        if (scriptDoJogador != null)
        {
            if (liberaDash && scriptDoJogador.GetTemDash()) Destroy(this.gameObject);
            if (liberaPuloDuplo && scriptDoJogador.GetTemPuloDuplo()) Destroy(this.gameObject);
            if (liberaEscalada && scriptDoJogador.GetTemEscalada()) Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Jogador"))
        {
            AudioSource meuAudio = GetComponent<AudioSource>();
            if (meuAudio != null)
            {
                meuAudio.Play();
            }

            JogadorScript scriptDoJogador = colisao.GetComponent<JogadorScript>();

            if (scriptDoJogador != null)
            {
               if (liberaDash) scriptDoJogador.temDash = true;
               if (liberaPuloDuplo) scriptDoJogador.temPuloDuplo = true;
               if (liberaEscalada) scriptDoJogador.temEscalada = true;
            } 

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}