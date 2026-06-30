using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    public static GlobalLightController instance;

    [SerializeField] private Light2D globalLight;

    private void Awake()
    {
        instance = this;

        if (globalLight == null)
            globalLight = GetComponent<Light2D>();
    }

    public void ApplyGodLight(string god)
    {
        if (globalLight == null)
            return;

        switch (god)
        {
            case "Hefesto":
                globalLight.color = new Color(1f, 0.8846129f, 0.6933962f);
                globalLight.intensity = 1f;
                break;

            case "Hecate":
                globalLight.color = new Color(0f, 0f, 0f);
                globalLight.intensity = 0f;
                break;

            case "Hipnos":
                globalLight.color = new Color(0.5887772f, 0.7353016f, 0.9528302f);
                globalLight.intensity = 1f;
                break;

            default:
                globalLight.color = Color.white;
                globalLight.intensity = 0.6f;
                break;
        }
    }
}