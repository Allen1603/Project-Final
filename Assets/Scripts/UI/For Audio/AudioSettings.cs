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
        // load saved values from PlayerPrefs directly
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bgmToggle.isOn = PlayerPrefs.GetInt("BGMMuted", 0) == 0; // isOn = NOT muted
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 0;

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
        AudioManager.Instance.ToggleBGM(isOn);
    }

    public void ToggleSFX(bool isOn)
    {
        AudioManager.Instance.ToggleSFX(isOn);
    }
}