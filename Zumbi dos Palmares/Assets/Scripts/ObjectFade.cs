using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float fadeAmount = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color fadedColor = originalColor;
            fadedColor.a = fadeAmount;
            spriteRenderer.color = fadedColor;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = originalColor;
        }
    }
}
