using UnityEngine;
using UnityEngine.UI;

public class CameraNightEffect : MonoBehaviour
{
    public float nightStartTime = 120f;  // Tempo (segundos) até começar a escurecer
    public float fadeDuration = 30f;     // Tempo da transição para a noite
    public float nightDuration = 60f;    // Quanto tempo a noite dura antes de voltar ao dia
    public Image nightOverlay;           // UI Image preta para escurecer a tela
    public Text nightText;               // Texto que aparece quando anoitece
    public float maxAlpha = 0.5f;        // Define a opacidade máxima do escurecimento
    public Light postLight;              // Luz do poste
    public float lightFadeDuration = 5f; // Tempo para acender/apagar a luz
    public float maxLightIntensity = 2f; // Intensidade máxima da luz

    private float timer = 0f;
    private bool isNight = false;
    private bool isDayTransitioning = false;

    void Start()
    {
        if (nightOverlay != null)
        {
            nightOverlay.gameObject.SetActive(false);
        }

        if (nightText != null)
        {
            nightText.gameObject.SetActive(false);
        }

        if (postLight != null)
        {
            postLight.intensity = 0f;
            postLight.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nightStartTime && !isNight)
        {
            nightOverlay.gameObject.SetActive(true);
            StartCoroutine(FadeToNight());
            isNight = true;
        }

        // Após a noite durar 'nightDuration', começa a transição para o dia
        if (isNight && timer >= nightStartTime + fadeDuration + nightDuration && !isDayTransitioning)
        {
            StartCoroutine(FadeToDay());
            isDayTransitioning = true;
        }
    }

    private System.Collections.IEnumerator FadeToNight()
    {
        float elapsedTime = 0f;
        Color overlayColor = nightOverlay.color;
        overlayColor.a = 0f;
        nightOverlay.color = overlayColor;

        while (elapsedTime < fadeDuration)
        {
            overlayColor.a = Mathf.Lerp(0f, maxAlpha, elapsedTime / fadeDuration);
            nightOverlay.color = overlayColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayColor.a = maxAlpha;
        nightOverlay.color = overlayColor;

        if (nightText != null)
        {
            nightText.gameObject.SetActive(true);
            nightText.text = "";
        }

        if (postLight != null)
        {
            postLight.gameObject.SetActive(true);
            StartCoroutine(FadeInLight());
        }
    }

    private System.Collections.IEnumerator FadeToDay()
    {
        float elapsedTime = 0f;
        Color overlayColor = nightOverlay.color;

        if (nightText != null)
        {
            nightText.gameObject.SetActive(false);
        }

        while (elapsedTime < fadeDuration)
        {
            overlayColor.a = Mathf.Lerp(maxAlpha, 0f, elapsedTime / fadeDuration);
            nightOverlay.color = overlayColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayColor.a = 0f;
        nightOverlay.color = overlayColor;
        nightOverlay.gameObject.SetActive(false);

        if (postLight != null)
        {
            StartCoroutine(FadeOutLight());
        }

        // Reseta o ciclo para começar tudo de novo
        yield return new WaitForSeconds(2f); // Pequena pausa para suavizar a transição
        timer = 0f;
        isNight = false;
        isDayTransitioning = false;
    }

    private System.Collections.IEnumerator FadeInLight()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lightFadeDuration)
        {
            postLight.intensity = Mathf.Lerp(0f, maxLightIntensity, elapsedTime / lightFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        postLight.intensity = maxLightIntensity;
    }

    private System.Collections.IEnumerator FadeOutLight()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lightFadeDuration)
        {
            postLight.intensity = Mathf.Lerp(maxLightIntensity, 0f, elapsedTime / lightFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        postLight.intensity = 0f;
        postLight.gameObject.SetActive(false);
    }
}
