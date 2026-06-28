using System.Collections;
using UnityEngine;

public class RotacaoPlataformaScript : MonoBehaviour
{
    public float tempoMinEspera = 3f;
    public float tempoMaxEspera = 4f;
    public float duracaoGiro = 0.3f;

    private bool jogadorNaArea = false;

    private void Start()
    {
        StartCoroutine(RotinaPlataforma());
    }

    private IEnumerator RotinaPlataforma()
    {
        while (true)
        {
            float tempoEspera = Random.Range(tempoMinEspera, tempoMaxEspera);
            float cronometro = 0f;

            while (cronometro < tempoEspera)
            {
                if (!jogadorNaArea)
                {
                    cronometro += Time.deltaTime;
                }
                yield return null;
            }

            Quaternion rotInicial = transform.rotation;
            Quaternion rotAlvo = rotInicial * Quaternion.Euler(0, 0, 180f);
            float tempoGiro = 0f;

            while (tempoGiro < duracaoGiro)
            {
                if (jogadorNaArea)
                {
                    yield return new WaitWhile(() => jogadorNaArea);
                }

                tempoGiro += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(rotInicial, rotAlvo, tempoGiro / duracaoGiro);
                yield return null;
            }

            transform.rotation = rotAlvo;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jogador"))
        {
            jogadorNaArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jogador"))
        {
            jogadorNaArea = false;
        }
    }
}