using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer myAudioMixer;

    public void SetVolume(float value)
    {
        // Evita erro de logaritmo com zero
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        myAudioMixer.SetFloat("MasterVolume", volume);
    }
}
