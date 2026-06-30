using UnityEngine;

public class LavaHefesto : MonoBehaviour
{
   public float lifeTime = 3f;

   void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
