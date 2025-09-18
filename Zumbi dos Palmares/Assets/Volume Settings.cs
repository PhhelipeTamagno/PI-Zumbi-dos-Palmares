using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    public void SetMusicVolume(float value)
    {
        // Evita log(0)
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        mixer.SetFloat("MusicVolume", volume);
    }

}
