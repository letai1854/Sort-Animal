using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Header("Music Tracks")]
    public List<AudioClip> homeMusicTracks;
    public List<AudioClip> gameplayMusicTracks;
    public List<AudioClip> winMusicTracks;

    [Header("Sound Effects (SFX)")]
    public List<AudioClip> buttonClickSFX;

    public AudioSource musicSource; 
    public AudioSource sfxSource;

    protected override void Awake()
    {
        base.Awake();
        //musicSource = gameObject.AddComponent<AudioSource>();
        //sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        sfxSource.loop = false;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }
    private void Start()
    {
        musicSource.clip = homeMusicTracks[homeMusicTracks.Count-1];
        musicSource.Play();
    }


    public void PlayHome()
    {
               PlayMusic(homeMusicTracks);
    }

    public void PlayGameplay()
    {
        PlayMusic(gameplayMusicTracks);
    }
    public void PlayWin()
    {
        PlaySFX(winMusicTracks);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSFX);
    }
    public void PlayMusic(List<AudioClip> musicTracks)
    {
        if (musicTracks == null || musicTracks.Count == 0) return;
        musicSource.clip = musicTracks[Random.Range(0, musicTracks.Count)];

        musicSource.Play();
    }
    public void PlaySFX(List<AudioClip> sfxTracks)
    {
        if (sfxTracks == null || sfxTracks.Count == 0) return;
        sfxSource.clip = sfxTracks[Random.Range(0, sfxTracks.Count)];
        sfxSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void StopAllSounds()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }
}
