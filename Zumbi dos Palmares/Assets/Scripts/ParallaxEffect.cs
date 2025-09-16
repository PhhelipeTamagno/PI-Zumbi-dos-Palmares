using UnityEngine;
using System.Collections;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform background;
        public Transform bandeirante;
        public float backgroundMoveAmount = 0.1f;
        public float bandeiranteMoveAmount = 0.3f;
        public AudioClip narrationClip;
    }

    public ParallaxLayer[] layers;
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;

    private Vector3[] bgStartPos;
    private Vector3[] bandeiranteStartPos;
    private AudioSource audioSource;
    private int currentLayerIndex = 0;
    private float moveTimer = 0f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        bgStartPos = new Vector3[layers.Length];
        bandeiranteStartPos = new Vector3[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].background != null)
            {
                bgStartPos[i] = layers[i].background.localPosition;
                SetAlpha(layers[i].background, 0f);
            }

            if (layers[i].bandeirante != null)
            {
                bandeiranteStartPos[i] = layers[i].bandeirante.localPosition;
                SetAlpha(layers[i].bandeirante, 0f);
            }
        }

        StartCoroutine(PlayLayer(currentLayerIndex));
    }

    void Update()
    {
        if (layers.Length == 0) return;

        moveTimer += Time.deltaTime * moveSpeed;

        // Move apenas o layer atual
        ParallaxLayer layer = layers[currentLayerIndex];
        float offset = Mathf.Sin(moveTimer);

        if (layer.background != null)
            layer.background.localPosition = bgStartPos[currentLayerIndex] + new Vector3(offset * layer.backgroundMoveAmount, 0f, 0f);
        if (layer.bandeirante != null)
            layer.bandeirante.localPosition = bandeiranteStartPos[currentLayerIndex] + new Vector3(offset * layer.bandeiranteMoveAmount, 0f, 0f);
    }

    IEnumerator PlayLayer(int index)
    {
        ParallaxLayer layer = layers[index];

        // Fade In
        yield return StartCoroutine(FadeLayer(layer, 0f, 1f));

        // Toca o áudio
        if (layer.narrationClip != null)
        {
            audioSource.clip = layer.narrationClip;
            audioSource.Play();
            yield return new WaitForSeconds(layer.narrationClip.length);
        }

        // Fade Out
        yield return StartCoroutine(FadeLayer(layer, 1f, 0f));

        // Próximo layer
        currentLayerIndex++;
        if (currentLayerIndex < layers.Length)
            StartCoroutine(PlayLayer(currentLayerIndex));
    }

    IEnumerator FadeLayer(ParallaxLayer layer, float start, float end)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            if (layer.background != null) SetAlpha(layer.background, alpha);
            if (layer.bandeirante != null) SetAlpha(layer.bandeirante, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        if (layer.background != null) SetAlpha(layer.background, end);
        if (layer.bandeirante != null) SetAlpha(layer.bandeirante, end);
    }

    void SetAlpha(Transform obj, float alpha)
    {
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
