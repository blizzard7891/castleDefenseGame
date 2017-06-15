using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingleTon<SoundManager>
{
    public AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach(AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
            Debug.Log(clip.name);
        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void ChangeBGM()
    {
        musicSource.mute = false;
        musicSource.clip = audioClips["gameOverBgm"];
        musicSource.loop = false;
        musicSource.Play();
    }

    public void InGameBGM(bool inGame)
    {
        if (inGame)
        {
            musicSource.volume = 0.13f;
        }
        else
        {
            musicSource.volume = PlayerPrefs.GetFloat("Music") ;
        }
    }

    private void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;

        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    private void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.4f);
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }

    public void SoundMute(bool flag)
    {
        musicSource.mute = flag;
        sfxSource.mute = flag;
    }

    //public void BgmMute(bool flag)
    //{
    //    musicSource.mute = flag;
    //}

    //public void SfxMute(bool flag)
    //{
    //    sfxSource.mute = flag;
    //}
}
