using UnityEngine;
using UnityEngine.UI;

public class CameraNightEffect : MonoBehaviour
{
    public float nightStartTime = 120f; // Tempo (segundos) até começar a escurecer
    public float fadeDuration = 30f;    // Tempo da transição
    public Image nightOverlay;          // UI Image preta para escurecer a tela
    public Text nightText;              // Texto que aparece quando anoitece
    public float maxAlpha = 0.5f;       // Define a opacidade máxima do escurecimento
    public Light postLight;             // Luz do poste
    public float lightFadeDuration = 5f;// Tempo para acender a luz
    public float maxLightIntensity = 2f;// Intensidade máxima da luz
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        if (nightOverlay != null)
        {
            nightOverlay.gameObject.SetActive(false); // Começa desativado
        }
        
        if (nightText != null)
        {
            nightText.gameObject.SetActive(false); // Começa desativado
        }
        
        if (postLight != null)
        {
            postLight.intensity = 0f; // Luz começa apagada
            postLight.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nightStartTime && !isFading)
        {
            nightOverlay.gameObject.SetActive(true); // Ativa a imagem
            StartCoroutine(FadeToNight());
            isFading = true;
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
        overlayColor.a = maxAlpha; // Define a opacidade máxima configurável
        nightOverlay.color = overlayColor;
        
        if (nightText != null)
        {
            nightText.gameObject.SetActive(true); // Exibe o texto de noite
            nightText.text = "";
        }
        
        if (postLight != null)
        {
            postLight.gameObject.SetActive(true);
            StartCoroutine(FadeInLight());
        }
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
}
