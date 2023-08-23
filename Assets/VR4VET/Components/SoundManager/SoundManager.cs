/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives functions to interact with the 3 different audio (music, effects, and voices)
/// all public methods are self-explanatory
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _musicSource, _effectsSource, _voiceSource; 
    //Singleton function (only an instance of this class will run )
     void Awake()
    {
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


    //Play Functions
    public void PlayEffect(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayVoice(AudioClip clip)
    {
        _voiceSource.PlayOneShot(clip);
    }


    public void PlayMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }


    //Change Volume Functions

    public void ChangeMasterVolume (float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        _musicSource.volume = value;
    }

    public void ChangeEffectVolume(float value)
    {
        _effectsSource.volume = value;
    }
    public void ChangeVoiceVolume(float value)
    {
        _voiceSource.volume = value;
    }

//getter for volume
//

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public float GetMusicVolume()
    {
        return AudioListener.volume;
    }

    public float GetEffectsVolume()
    {
        return AudioListener.volume;
    }
    public float GetVoiceVolume()
    {
        return AudioListener.volume;
    }


}
