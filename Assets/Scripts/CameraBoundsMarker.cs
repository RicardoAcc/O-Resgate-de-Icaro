using UnityEngine;

public class CameraBoundsMarker : MonoBehaviour
{
    public Collider2D bounds;

    private void Reset()
    {
        bounds = GetComponent<Collider2D>();
    }
}