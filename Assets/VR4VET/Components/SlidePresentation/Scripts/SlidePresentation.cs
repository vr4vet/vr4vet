using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class SlideChanged : UnityEvent<int>
{
}


public class SlidePresentation : MonoBehaviour
{
    [Tooltip("Whether or not the slide presentation is powered on.\n" +
             "Method PowerOn() will need to be called when presentation should start if this is set false.")]
    [SerializeField] private bool PoweredOn = true;

    [Tooltip("Will change slides every specified second.")]
    [SerializeField] private bool AutomaticSlideshow = false;

    [Tooltip("Amount of seconds that will pass before slides are changed when Automatic Slideshow is enabled.")]
    [SerializeField] private float AutomaticSlideshowInterval = 10f;

    [Tooltip("Enable to prevent video clips from playing audio.")]
    [SerializeField] private bool MuteAudio = false;

    [Tooltip("OPTIONAL: Place an image component with Image Type set to 'Filled', Fill Method to 'Vertical', and Fill Origin to 'Bottom' here. The component will function as a volume bar and display the current audio volume.")]
    [SerializeField] private Image VolumeBar;

    [Tooltip("Place instances of the Slide prefab here. These are the slides that will be displayed.")]
    [SerializeField] private List<GameObject> Slides = new();

    private Image _targetImage;
    private int _imageIndex = 0;
    private RawImage _videoTexture;
    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;

    public SlideChanged m_SlideChanged;
    public UnityEvent OnPowerOff;
    public UnityEvent OnPowerOn;

    // Start is called before the first frame update
    void Start()
    {
        if (m_SlideChanged != null)
            m_SlideChanged = new SlideChanged();

        if (OnPowerOff != null)
            OnPowerOff = new UnityEvent();

        if (OnPowerOn != null)
            OnPowerOn = new UnityEvent();

        _videoTexture = GetComponentInChildren<RawImage>();
        _videoPlayer = _videoTexture.GetComponentInChildren<VideoPlayer>();
        _videoPlayer.waitForFirstFrame = true;

        _audioSource = _videoPlayer.GetComponent<AudioSource>();
        _targetImage = GetComponentInChildren<Image>();

        SetSlide(0);
        SetAutomaticSlideShow(AutomaticSlideshow);
        Mute(MuteAudio);
        AdjustVolumeBar();
    }

    public void NextSlide()
    {
        _imageIndex = (_imageIndex + 1) % Slides.Count;
        SetSlide(_imageIndex);
        m_SlideChanged.Invoke(_imageIndex);
    }

    public void PrevSlide()
    {
        _imageIndex = (_imageIndex - 1) < 0 ? Slides.Count - 1 : _imageIndex - 1;
        SetSlide(_imageIndex);
        m_SlideChanged.Invoke(_imageIndex);
    }

    public void PowerOff()
    {
        if (!PoweredOn)
            return;

        if (AutomaticSlideshow && IsInvoking(nameof(NextSlide)))
            CancelInvoke(nameof(NextSlide));


        _targetImage.color = Color.clear;
        if (_videoTexture.enabled)
        {
            _videoPlayer.Pause();
            _videoTexture.color = Color.clear;
        }

        PoweredOn = false;
        OnPowerOff.Invoke();
    }

    public void PowerOn()
    {
        if (PoweredOn)
            return;

        if (AutomaticSlideshow && !IsInvoking(nameof(NextSlide)))
            InvokeRepeating(nameof(NextSlide), 10, 10);

        _targetImage.color = Color.white;
        if (_videoTexture.enabled)
        {
            _videoPlayer.Play();
            _videoTexture.color = Color.white;
        }

        PoweredOn = true;
        OnPowerOn.Invoke();
    }

    public void TogglePower()
    {
        if (PoweredOn)
            PowerOff();
        else
            PowerOn();
    }

    public void SetAutomaticSlideShow(bool enable)
    {
        AutomaticSlideshow = enable;
        if (enable)
        {
            if (!IsInvoking(nameof(NextSlide)))
                InvokeRepeating(nameof(NextSlide), 10, 10);
        }
        else
        {
            if (IsInvoking(nameof(NextSlide)))
                CancelInvoke(nameof(NextSlide));
        }
    }

    public void ToggleAutomaticSlideShow()
    {
        AutomaticSlideshow = !AutomaticSlideshow;
        SetAutomaticSlideShow(AutomaticSlideshow);
    }

    public void Mute(bool mute)
    {
        MuteAudio = mute;
        _audioSource.mute = mute;
    }

    public void ToggleMute()
    {
        MuteAudio = !MuteAudio;
        _audioSource.mute = MuteAudio;
    }

    public void VolumeUp()
    {
        _audioSource.volume += .125f;
        AdjustVolumeBar();
    }

    public void VolumeDown()
    {
        _audioSource.volume -= .125f;
        AdjustVolumeBar();
    }

    private void AdjustVolumeBar()
    {
        if (VolumeBar != null)
            VolumeBar.fillAmount = _audioSource.volume;
    }

    private void SetSlide(int index)
    {
        Slide slide = Slides[index].GetComponent<Slide>();
        _targetImage.sprite = slide.SlideTexture;

        if (slide.VideoClip != null)
        {
            _videoPlayer.clip = slide.VideoClip;
            _videoPlayer.frame = 0;
            _videoTexture.transform.localPosition = slide.VideoClipPosition;
            _videoTexture.transform.localScale = slide.VideoClipScale * new Vector3(1, 1, 0);
            _videoTexture.enabled = true;
            _videoPlayer.Play();
        }
        else
        {
            _videoPlayer.Stop();
            _videoTexture.enabled = false;
        }
    }
}
