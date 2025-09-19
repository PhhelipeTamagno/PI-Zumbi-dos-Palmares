using UnityEngine;
using UnityEngine.UI;

public class ControladorSom : MonoBehaviour
{
    public static ControladorSom instancia;

    [SerializeField] private AudioSource fundoMusical;

    [Header("UI")]
    private Slider volumeSlider;
    private Image muteImage;
    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;

    private bool somLigado = true;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Carrega configs salvas
        float volumeSalvo = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        fundoMusical.volume = volumeSalvo;

        somLigado = PlayerPrefs.GetInt("SomLigado", 1) == 1;
        if (!somLigado) fundoMusical.volume = 0f;
    }

    // Essa função você chama em cada cena que tiver UI de áudio
    public void RegistrarUI(Slider slider, Image muteImg)
    {
        volumeSlider = slider;
        muteImage = muteImg;

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
            volumeSlider.onValueChanged.AddListener(AlterarVolume);
        }

        AtualizarIcone();
    }

    public void AlterarVolume(float valor)
    {
        if (somLigado)
        {
            fundoMusical.volume = valor;
        }
        PlayerPrefs.SetFloat("VolumeMusica", valor);
    }

    public void LigarDesligarSom()
    {
        somLigado = !somLigado;

        if (somLigado)
        {
            fundoMusical.volume = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        }
        else
        {
            fundoMusical.volume = 0f;
        }

        PlayerPrefs.SetInt("SomLigado", somLigado ? 1 : 0);

        AtualizarIcone();
    }

    private void AtualizarIcone()
    {
        if (muteImage != null)
        {
            muteImage.sprite = somLigado ? somLigadoSprite : somDesligadoSprite;
        }
    }
}
