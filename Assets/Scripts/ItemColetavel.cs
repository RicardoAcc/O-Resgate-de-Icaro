using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    [Header("Configurações do Poder")]
    public bool liberaDash = false;
    public bool liberaPuloDuplo = false;
    public bool liberaEscalada = false;

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        // 1. Verifica se quem encostou tem a etiqueta (Tag) de "Player"
        if (colisao.CompareTag("Player"))
        {
            // 2. Toca o seu efeito sonoro
            AudioSource meuAudio = GetComponent<AudioSource>();
            if (meuAudio != null)
            {
                meuAudio.Play();
            }

            // 3. Comunica com o Script do Dédalo!
            JogadorScript scriptDoJogador = colisao.GetComponent<JogadorScript>();

            //if (scriptDoJogador != null)
            //{
            //    if (liberaDash) scriptDoJogador.temDash = true;
            //    if (liberaPuloDuplo) scriptDoJogador.temPuloDuplo = true;
            //    if (liberaEscalada) scriptDoJogador.temEscalada = true;
            //} 

            // 4. Esconde a imagem e desativa o gatilho para não pegar duas vezes
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // 5. Destrói o objeto após 1 segundo (tempo para o seu áudio terminar de tocar)
            Destroy(gameObject, 1f);
        }
    }
}