using UnityEngine;

public class JogadorSpawnController : MonoBehaviour
{
    public GameObject jogadorPrefab;

    private void Awake()
    {
        if (JogadorScript.instance == null)
        {
            Instantiate(jogadorPrefab, transform.position, Quaternion.identity);
        }
    }
}
