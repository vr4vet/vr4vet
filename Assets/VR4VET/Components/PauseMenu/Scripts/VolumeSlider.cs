using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] private Slider _sliderMaster;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderEffects;
    [SerializeField] private Slider _sliderVoice;
    // Start is called before the first frame update
    void Start()
    {
        //set the slider to the actual volume value
        _sliderMaster.value = SoundManager.Instance.GetMasterVolume();
        _sliderMusic.value = SoundManager.Instance.GetMusicVolume();
        _sliderEffects.value = SoundManager.Instance.GetEffectsVolume();
        _sliderVoice.value = SoundManager.Instance.GetVoiceVolume();

        //adding change volume functions to the sliders 
        _sliderMaster.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
        _sliderMusic.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        _sliderEffects.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        _sliderVoice.onValueChanged.AddListener(val => SoundManager.Instance.ChangeVoiceVolume(val));

    }


}
