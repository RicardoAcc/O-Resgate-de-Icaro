using UnityEngine;
using System.Collections;

public class TravesseiroHipnos : MonoBehaviour
{
    public float Velocidade = 1f;
    public float TempoDeDuracao = 5f;
    public int Angulacao = 15;

    private void MoveTravesseiro()
    {
        transform.Translate(Vector3.right * Velocidade);
        transform.localRotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z + Angulacao);
    }

    private void FixedUpdate()
    {
        MoveTravesseiro();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jogador"))
        {
            collision.gameObject.GetComponent<JogadorMoveScript>().isSleeping = true;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.2f;
            StartCoroutine(DespertarJogador(collision.gameObject));
            SetAlive(false);
        }
    }

    private IEnumerator DespertarJogador(GameObject jogador)
    {
        yield return new WaitForSeconds(TempoDeDuracao);
        jogador.GetComponent<JogadorMoveScript>().isSleeping = false;
        SetAlive(true);
    }

    private void SetAlive(bool isAlive)
    {
        this.gameObject.GetComponent<Collider2D>().enabled = isAlive;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = isAlive;
    }
}
