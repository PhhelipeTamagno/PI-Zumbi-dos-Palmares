using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("UI")]
    [SerializeField] private Slider volumeSlider;

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
        }
    }

    private void Start()
    {
        // Carrega volume salvo
        float volumeSalvo = PlayerPrefs.GetFloat("MasterVolume", 1f);

        SetVolume(volumeSalvo);

        if (volumeSlider != null)
        {
            volumeSlider.value = volumeSalvo;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float valor)
    {
        // Converte 0-1 em dB (-80 = mudo, 0 = máximo)
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", valor);
    }
}
