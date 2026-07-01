using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Jogador"))
        {
            JogadorScript.instance.RecebeDano();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jogador"))
        {
            JogadorScript.instance.RecebeDano();
        }
    }
}
