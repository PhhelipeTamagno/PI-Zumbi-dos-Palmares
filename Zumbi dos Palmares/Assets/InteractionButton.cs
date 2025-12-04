using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    public static InteractionButton Instance;
    public Button interactButton;

    public System.Action onInteractionPressed;

    void Awake()
    {
        Instance = this;
        interactButton.onClick.AddListener(() => {
            onInteractionPressed?.Invoke();
        });
    }
}
