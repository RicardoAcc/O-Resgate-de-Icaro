using UnityEngine;

public class ParedeQuebravelController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Jogador") && collision.gameObject.GetComponent<JogadorMoveScript>().IsDashing())
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
