using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips (BGM)")]
    public List<AudioClip> bgmClips;

    [Header("Audio Clips (SFX)")]
    public List<AudioClip> sfxClips;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitAudioDictionaries();
            LoadSettings(); // load saved settings
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // load settings from PlayerPrefs
    private void LoadSettings()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bool bgmMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        bool sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
            bgmSource.mute = bgmMuted;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            sfxSource.mute = sfxMuted;
        }
    }

    // save settings to PlayerPrefs
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("BGMMuted", bgmSource != null && bgmSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", sfxSource != null && sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void InitAudioDictionaries()
    {
        foreach (var clip in bgmClips)
        {
            if (clip != null && !bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }

        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }

        if (bgmSource != null) bgmSource.volume = bgmVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSettings(); // re-apply settings on every scene load
        StopBGM();

        switch (scene.name)
        {
            case "Scene1":
                PlayBGM("BGM 2");
                break;
            case "Scene2":
            case "Scene3":
                PlayBGM("BGM 3");
                break;
            default:
                break;
        }
    }

    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmSource == null) return;

        AudioClip clip;
        if (!bgmDict.TryGetValue(clipName, out clip) || clip == null)
        {
            clip = Resources.Load<AudioClip>("Audio/" + clipName);
            if (clip != null) bgmDict[clipName] = clip;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void PlaySFX(string clipName)
    {
        if (sfxSource == null) return;

        AudioClip clip;
        if (!sfxDict.TryGetValue(clipName, out clip) || clip == null)
        {
            clip = Resources.Load<AudioClip>("Audio/" + clipName);
            if (clip != null) sfxDict[clipName] = clip;
            else return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null) bgmSource.volume = bgmVolume;
        SaveSettings(); // save immediately
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null) sfxSource.volume = sfxVolume;
        SaveSettings(); // save immediately
    }

    public void ToggleBGM(bool isOn)
    {
        if (bgmSource != null) bgmSource.mute = !isOn;
        SaveSettings(); // save immediately
    }

    public void ToggleSFX(bool isOn)
    {
        if (sfxSource != null) sfxSource.mute = !isOn;
        SaveSettings(); // save immediately
    }
}