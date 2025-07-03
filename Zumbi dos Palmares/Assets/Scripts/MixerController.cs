using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolumeValue", 0.75f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        myAudioMixer.SetFloat("MasterVolume", volumeInDb);

        // Salva o valor para outras cenas
        PlayerPrefs.SetFloat("MasterVolumeValue", value);
    }
}
