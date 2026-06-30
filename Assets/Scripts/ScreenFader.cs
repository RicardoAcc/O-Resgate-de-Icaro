using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.25f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            SetAlpha(0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeOut()
    {
        yield return FadeTo(1f);
    }

    public IEnumerator FadeIn()
    {
        yield return FadeTo(0f);
    }

    public void SetBlack()
    {
        SetAlpha(1f);
    }

    public void SetClear()
    {
        SetAlpha(0f);
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        if (canvasGroup == null)
            yield break;

        canvasGroup.blocksRaycasts = true;

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        SetAlpha(targetAlpha);

        canvasGroup.blocksRaycasts = targetAlpha > 0f;
    }

    private void SetAlpha(float alpha)
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = alpha;
        canvasGroup.blocksRaycasts = alpha > 0f;
    }
}