using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Toggles")]
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    void Start()
    {

        // Set default UI values from AudioManager
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        bgmToggle.isOn = AudioManager.Instance.bgmSource.mute;
        sfxToggle.isOn = AudioManager.Instance.sfxSource.mute;

        // Add listeners
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmToggle.onValueChanged.AddListener(ToggleBGM);
        sfxToggle.onValueChanged.AddListener(ToggleSFX);
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.Instance.SetBGMVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void ToggleBGM(bool isOn)
    {
        AudioManager.Instance.ToggleBGM(!isOn);
    }

    public void ToggleSFX(bool isOn)
    {
        AudioManager.Instance.ToggleSFX(!isOn);
    }
}