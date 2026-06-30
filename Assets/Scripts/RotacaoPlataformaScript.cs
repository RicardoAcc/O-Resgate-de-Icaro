using System.Collections;
using UnityEngine;

public class RotacaoPlataformaScript : MonoBehaviour
{
    public float tempoRotacao = 3f;
    private float tempoInicio = 0f;
    private float duracaoRotacao = 0.5f;
    public int angulo = 90;
    private float anguloInicial;
    private bool jogadorNaArea = false;
    private Coroutine coroutineRotacao;

    private void Start()
    {
        tempoInicio = Time.time;
        anguloInicial = transform.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        if(tempoInicio + tempoRotacao <= Time.time)
        {
            Rotate();
            tempoInicio = Time.time;
        }
            
    }

    private void Rotate()
    {
        if (jogadorNaArea)
            return;

        if (coroutineRotacao != null)
            StopCoroutine(coroutineRotacao);

        coroutineRotacao = StartCoroutine(Rotacionar());
    }

    private IEnumerator Rotacionar()
    {
        float anguloInicial = transform.eulerAngles.z;
        float anguloFinal = anguloInicial + angulo;

        float tempo = 0f;

        while (tempo < duracaoRotacao)
        {
            tempo += Time.deltaTime;

            float t = tempo / duracaoRotacao;

            t = Mathf.SmoothStep(0f, 1f, t);

            float novoAngulo = Mathf.LerpAngle(anguloInicial, anguloFinal, t);

            transform.rotation = Quaternion.Euler(0f, 0f, novoAngulo);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, anguloFinal);

        coroutineRotacao = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Jogador"))
        {
            jogadorNaArea = true;
            tempoInicio = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Jogador"))
        {
            jogadorNaArea = false;
            tempoInicio = Time.time;
        }
    }
}