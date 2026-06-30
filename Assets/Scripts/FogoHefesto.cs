using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FogoHefesto : MonoBehaviour
{
    private bool canShoot = true;
    public float shootCooldown = 3f;
    public float shootSpeed = 2f;
    public GameObject fireballPrefab;

    void FixedUpdate()
    {
        shoot();
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

    private void shoot()
    {
        if (canShoot)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(0f, shootSpeed);
            canShoot = false;
            Invoke("ResetShoot", shootCooldown);
        }
    }


}
