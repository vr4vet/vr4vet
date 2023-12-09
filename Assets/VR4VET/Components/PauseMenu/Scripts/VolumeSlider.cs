using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assigns the respective listeners to the slider on start
/// this is so the menu sliders change the different volume channels
/// </summary>
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _sliderMaster;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderEffects;
    [SerializeField] private Slider _sliderVoice;

    // Start is called before the first frame update
    private void Start()
    {
        //set the slider to the actual volume value
        //adding change volume functions to the sliders
        if (_sliderMaster != null)
        {
            _sliderMaster.value = SoundManager.Instance.GetMasterVolume();
            _sliderMaster.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
        }
        if (_sliderMusic != null)
        {
            _sliderMusic.value = SoundManager.Instance.GetMusicVolume();
            _sliderMusic.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        }
        if (_sliderEffects != null)
        {
            _sliderEffects.value = SoundManager.Instance.GetEffectsVolume();
            _sliderEffects.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        }
        if (_sliderVoice != null)
        {
            _sliderVoice.value = SoundManager.Instance.GetVoiceVolume();
            _sliderVoice.onValueChanged.AddListener(val => SoundManager.Instance.ChangeVoiceVolume(val));
        }   
    }
}