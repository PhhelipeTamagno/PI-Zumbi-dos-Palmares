using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    public RectTransform panel;          // Painel desativado no início
    public float duration = 0.5f;        // Duração da animação
    public Vector2 targetPos = Vector2.zero; // Posição central da tela
    public Vector2 hiddenPos = new Vector2(1920, 0); // Posição fora da tela (ajuste conforme a resolução)

    private bool isAnimating = false;

    // Abre o painel
    public void OpenPanel()
    {
        panel.gameObject.SetActive(true);
        if (!isAnimating)
            StartCoroutine(Slide(panel.anchoredPosition, targetPos));
    }

    // Fecha o painel
    public void ClosePanel()
    {
        if (!isAnimating)
            StartCoroutine(Slide(panel.anchoredPosition, hiddenPos, true));
    }

    // Coroutine que faz a animação de slide
    private System.Collections.IEnumerator Slide(Vector2 startPos, Vector2 endPos, bool deactivateAfter = false)
    {
        isAnimating = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }

        panel.anchoredPosition = endPos;

        if (deactivateAfter)
            panel.gameObject.SetActive(false);

        isAnimating = false;
    }
}
