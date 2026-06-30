using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TochaHecateController : MonoBehaviour
{

    void Start()
    {
        disableFire();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jogador"))
        {
            enableFire();
        }
    }

    public void disableFire()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.GetComponent<ParticleSystem>() != null)
            {
                child.gameObject.GetComponent<ParticleSystem>().Stop();
            }
            if (child.gameObject.GetComponent<Light2D>() != null)
            {
                child.gameObject.GetComponent<Light2D>().enabled = false;
            }
        }
    }

    public void enableFire()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.GetComponent<ParticleSystem>() != null)
            {
                child.gameObject.GetComponent<ParticleSystem>().Play();
            }
            if (child.gameObject.GetComponent<Light2D>() != null)
            {
                child.gameObject.GetComponent<Light2D>().enabled = true;
            }
        }
    }
}
