using UnityEngine;
using UnityEngine.UI;

public class UIAudioBinder : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image muteImage;
    [SerializeField] private Button muteButton;

    private void Start()
    {
        // Registra os elementos dessa cena no controlador
        if (ControladorSom.instancia != null)
        {
            ControladorSom.instancia.RegistrarUI(volumeSlider, muteImage);
        }

        // Configura o botão mute
        if (muteButton != null && ControladorSom.instancia != null)
        {
            muteButton.onClick.RemoveAllListeners();
            muteButton.onClick.AddListener(ControladorSom.instancia.LigarDesligarSom);
        }
    }
}
