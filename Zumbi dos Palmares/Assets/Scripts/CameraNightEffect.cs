using UnityEngine;
using UnityEngine.UI;

public class CameraNightEffect : MonoBehaviour
{
    public float nightStartTime = 120f;  
    public float fadeDuration = 30f;     
    public float nightDuration = 60f;    
    public Image nightOverlay;           
    public Text nightText;               
    public float maxAlpha = 0.5f;        
    public Light postLight;              
    public float lightFadeDuration = 5f; 
    public float maxLightIntensity = 2f; 

   
    public AudioClip dayMusic;           
    public AudioClip nightMusic;         
    private AudioSource audioSource;     

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

        
        audioSource = GetComponent<AudioSource>();

        
        if (dayMusic != null && audioSource != null)
        {
            audioSource.clip = dayMusic;  
            audioSource.loop = true;      
            audioSource.Play();           
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

        
        if (nightMusic != null && audioSource != null)
        {
            audioSource.Stop();  
            audioSource.clip = nightMusic;  
            audioSource.Play();  
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

        
        if (dayMusic != null && audioSource != null)
        {
            audioSource.Stop();  
            audioSource.clip = dayMusic;  
            audioSource.Play();  
        }

        
        yield return new WaitForSeconds(2f); 
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
