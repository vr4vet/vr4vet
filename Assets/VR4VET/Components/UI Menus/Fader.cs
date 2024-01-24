using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float fadeDuration = 2.0f; // Duration of the fade in seconds
    [SerializeField]
    private bool isFading = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Set the canvas group alpha to fully opaque
        canvasGroup.alpha = 1.0f;
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float startAlpha = canvasGroup.alpha;
        float targetAlpha = 0.0f;
        float currentTime = 0.0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);

            canvasGroup.alpha = alpha;
            yield return null;
        }

        // Ensure the final alpha value is set correctly
        canvasGroup.alpha = targetAlpha;

        isFading = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
