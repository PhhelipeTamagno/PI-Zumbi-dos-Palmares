using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.5f;
    public Vector2 targetPos = Vector2.zero;
    public Vector2 hiddenPos = new Vector2(1920, 0);

    private bool isAnimating = false;

    public void OpenPanel()
    {
        panel.gameObject.SetActive(true);
        if (!isAnimating)
            StartCoroutine(Slide(panel.anchoredPosition, targetPos));
    }

    public void ClosePanel()
    {
        if (!isAnimating)
            StartCoroutine(Slide(panel.anchoredPosition, hiddenPos, true));
    }

    private System.Collections.IEnumerator Slide(Vector2 startPos, Vector2 endPos, bool deactivateAfter = false)
    {
        isAnimating = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // usa tempo não afetado pelo pause
            elapsed += Time.unscaledDeltaTime;

            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);

            yield return null;
        }

        panel.anchoredPosition = endPos;

        if (deactivateAfter)
            panel.gameObject.SetActive(false);

        isAnimating = false;
    }
}
