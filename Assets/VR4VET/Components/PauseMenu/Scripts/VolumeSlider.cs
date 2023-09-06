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