using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // 背景音乐和音效音频源
    public AudioSource musicSource;
    public AudioSource effectSource;

    // 音效字典（用于存储音效和对应的音频片段）
    public AudioClip[] soundEffects;  // 直接在Inspector中拖动音效
    public AudioClip backgroundMusic;  // 背景音乐      // 背景音乐

    public Slider SFXSlider;
    public Slider MusicSlider;

    private void Awake()
    {
        if (PlayerPrefs.GetFloat("SFXvolume") == 0)
        {
            SFXSlider.value = 0.5f;
        }
        else
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXvolume");
        }
        if (PlayerPrefs.GetFloat("Musicvolume") == 0)
        {
            MusicSlider.value = 0.5f;
        }
        else
        {
            MusicSlider.value = PlayerPrefs.GetFloat("Musicvolume");
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        PlayBackgroundMusic();
    }
    private void Update()
    {
        effectSource.volume = SFXSlider.value;
        musicSource.volume = MusicSlider.value;
        PlayerPrefs.SetFloat("SFXvolume", SFXSlider.value);
        PlayerPrefs.SetFloat("Musicvolume", MusicSlider.value);
    }
    // 音效ID枚举
    public void PlayEffect(string effectName)
    {
        // 在音效数组中查找对应的音效并播放
        AudioClip clip = GetAudioClipByName(effectName);
        if (clip != null)
        {
            effectSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("音效未找到: " + effectName);
        }
    }

    // 音效映射表
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
    private AudioClip GetAudioClipByName(string name)
    {
        foreach (var clip in soundEffects)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }


    // 播放音效

}
