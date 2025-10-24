using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class MusicManager : SingletonMonobehaviour<MusicManager>
{
    
    private AudioSource musicAudioSource = null;
    private AudioClip currentAudioClip = null;
    private Coroutine fadeOutMusicCoroutine;
    private Coroutine fadeInMusicCoroutine;
    public int musicVolume = 10;

    protected override void Awake() //override the code while still using the singleton method
    {

        base.Awake();

        //load components
        musicAudioSource = GetComponent<AudioSource>();

        //start with music off
        GameResources.Instance.musicOffSnapshot.TransitionTo(0f);

    }


    private void Start() 
    {

        //basically saves the musics volume
        //check if volume levels have been saved in playerprefs - if so retrieve and set them to it
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolume = PlayerPrefs.GetInt("musicVolume");
        }
        
        SetMusicVolume(musicVolume);

    }


    private void OnDisable() 
    {

        //save volume settings in playerprefs
        PlayerPrefs.SetInt("musicVolume", musicVolume);

    }


    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = Settings.musicFadeOutTime, float fadeInTime = Settings.musicFadeInTime)
    {

        //play music track 
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeOutTime, fadeInTime));

    }


    //play music for room routine 
    private IEnumerator PlayMusicRoutine(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {

        //if fade out routine is already running then it will stop
        if(fadeOutMusicCoroutine != null)
        {
            StopCoroutine(fadeOutMusicCoroutine);
        }

        //if fade in routine is already running then stop it
        if(fadeInMusicCoroutine != null)
        {
            StopCoroutine(fadeInMusicCoroutine);
        }

        //if the music track has changed then play new music track
        if(musicTrack.musicClip != currentAudioClip)
        {
            currentAudioClip = musicTrack.musicClip;

            yield return fadeOutMusicCoroutine = StartCoroutine(FadeOutMusic(fadeOutTime));

            yield return fadeInMusicCoroutine = StartCoroutine(FadeInMusic(musicTrack, fadeInTime));
        }

        yield return null;

    }


    //fade out music routine
    private IEnumerator FadeOutMusic(float fadeOutTime)
    {

        GameResources.Instance.musicLowSnapshot.TransitionTo(fadeOutTime);

        yield return new WaitForSeconds(fadeOutTime);

    }


    //fade in music routine 
    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {

        //set clip and play
        musicAudioSource.clip = musicTrack.musicClip;
        musicAudioSource.volume = musicTrack.musicVolume;
        musicAudioSource.Play(); 

        GameResources.Instance.musicOnFullSnapshot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);

    }


    //increase music volume 
    public void IncreaseMusicVolume() 
    {

        int maxMusicVolume = 20;

        if(musicVolume >= maxMusicVolume) return;

        musicVolume += 1;

        SetMusicVolume(musicVolume);

    }


    //decrease music volume 
    public void DecreaseMusicVolume() 
    {

        if(musicVolume == 0) return;

        musicVolume -= 1;

        SetMusicVolume(musicVolume);

    }


    //set music volume 
    public void SetMusicVolume(int musicVolume)
    {

        float muteDecibels = -80f;

        if(musicVolume == 0)
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", muteDecibels);
        }
        else 
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", HelperUtilities.LinearToDecibels(musicVolume)); //lower volume
        }

    }


}
