using System.Collections;
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

    private Image _targetImage; // image component which displays slides
    private int _imageIndex = 0;
    
    private RawImage _videoTexture; // this will hold the Render Texture (below) which is used to display video clips on the canvas
    private RenderTexture _renderTexture;
    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;

    public SlideChanged m_SlideChanged;
    public UnityEvent OnPowerOff;
    public UnityEvent OnPowerOn;

    // Start is called before the first frame update
    void Start()
    {
        // setting up events
        if (m_SlideChanged != null)
            m_SlideChanged = new SlideChanged();

        if (OnPowerOff != null)
            OnPowerOff = new UnityEvent();

        if (OnPowerOn != null)
            OnPowerOn = new UnityEvent();

        // setting up video player. new render texture is created because it has methods Release() and Create(). used to prevent last frames of previous video clip from playing.
        _videoTexture = GetComponentInChildren<RawImage>();
        _videoPlayer = _videoTexture.GetComponentInChildren<VideoPlayer>();
        _renderTexture = new RenderTexture(1024, 1024, 16, RenderTextureFormat.ARGB32);

        // placing newly created RenderTexture object where it's needed
        _videoTexture.texture = _renderTexture;
        _videoPlayer.targetTexture = _renderTexture;

        _audioSource = _videoPlayer.GetComponent<AudioSource>();
        _targetImage = GetComponentInChildren<Image>();

        // setting up presentation using provided attribute values
        DisplaySlide(0);
        SetAutomaticSlideShow(AutomaticSlideshow);
        Mute(MuteAudio);

        AdjustVolumeBar();
    }

    /// <summary>
    /// Change to the next slide.
    /// </summary>
    public void NextSlide()
    {
        if (!PoweredOn)
            return;

        _imageIndex = (_imageIndex + 1) % Slides.Count;
        DisplaySlide(_imageIndex);
        m_SlideChanged.Invoke(_imageIndex);
    }

    /// <summary>
    /// Change to the previous slide.
    /// </summary>
    public void PrevSlide()
    {
        if (!PoweredOn)
            return;

        _imageIndex = (_imageIndex - 1) < 0 ? Slides.Count - 1 : _imageIndex - 1;
        DisplaySlide(_imageIndex);
        m_SlideChanged.Invoke(_imageIndex);
    }

    /// <summary>
    /// Hide all visual elements to simulate turning a projector off.
    /// The (possibly) repeatedly invoking NextSlide() (suggesting automatic slideshow is activated) is also stopped to prevent slideshow from continuing while invisible.
    /// </summary>
    public void PowerOff()
    {
        if (!PoweredOn)
            return;

        if (IsInvoking(nameof(NextSlide)))
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

    /// <summary>
    /// Show all visual elements to simulate turning a projector on.
    /// NextSlide() is again (if automatic slideshow is activated) invoked repeatedly.
    /// </summary>
    public void PowerOn()
    {
        if (PoweredOn)
            return;

        SetAutomaticSlideShow(AutomaticSlideshow);

        _targetImage.color = Color.white;
        if (_videoTexture.enabled)
        {
            _videoPlayer.Play();
            _videoTexture.color = Color.white;
        }

        PoweredOn = true;
        OnPowerOn.Invoke();
    }

    /// <summary>
    /// Toggles the power (of the simulated projector) on or off. Simply changes the state back and forth.
    /// </summary>
    public void TogglePower()
    {
        if (PoweredOn)
            PowerOff();
        else
            PowerOn();
    }

    /// <summary>
    /// Either activates or deactivates automatic slideshow depending on the parameter value.
    /// </summary>
    /// <param name="enable"></param>
    public void SetAutomaticSlideShow(bool enable)
    {
        AutomaticSlideshow = enable;
        if (enable)
        {
            if (!IsInvoking(nameof(NextSlide)))
                InvokeRepeating(nameof(NextSlide), AutomaticSlideshowInterval, AutomaticSlideshowInterval);
        }
        else
        {
            if (IsInvoking(nameof(NextSlide)))
                CancelInvoke(nameof(NextSlide));
        }
    }

    /// <summary>
    /// Toggles automatic slideshow on or off. Simply changes the state back and forth.
    /// </summary>
    public void ToggleAutomaticSlideShow()
    {
        AutomaticSlideshow = !AutomaticSlideshow;
        SetAutomaticSlideShow(AutomaticSlideshow);
    }

    /// <summary>
    /// Mutes video clip audio.
    /// </summary>
    /// <param name="mute"></param>
    public void Mute(bool mute)
    {
        MuteAudio = mute;
        _audioSource.mute = mute;
    }

    /// <summary>
    /// Toggles video clip mute on or off. Simply changes the state back and forth.
    /// </summary>
    public void ToggleMute()
    {
        MuteAudio = !MuteAudio;
        _audioSource.mute = MuteAudio;
    }

    /// <summary>
    /// Increases video clip volume. Adjusts VolumeBar accordingly if provided. 
    /// </summary>
    public void VolumeUp()
    {
        _audioSource.volume += .125f;
        AdjustVolumeBar();
    }

    /// <summary>
    /// Lowers video clip volume. Adjusts VolumeBar accordingly if provided. 
    /// </summary>
    public void VolumeDown()
    {
        _audioSource.volume -= .125f;
        AdjustVolumeBar();
    }

    /// <summary>
    /// The (optionally) provided image VolumeBar will be filled using the Audio Source's current volume
    /// </summary>
    private void AdjustVolumeBar()
    {
        if (VolumeBar != null)
            VolumeBar.fillAmount = _audioSource.volume;
    }


    /// <summary>
    /// Displays the given slide at the given index. 
    /// If it has a video clip in its VideoClip attribute, a Render Texture will be created, enabled, scaled and positioned, and then play the video clip.
    /// Otherwise will simply display the slide's provided image without any video clip on top.
    /// </summary>
    /// <param name="index"></param>
    private void DisplaySlide(int index)
    {
        Slide slide = Slides[index].GetComponent<Slide>();
        _targetImage.sprite = slide.SlideTexture;
        _renderTexture.Release();

        if (slide.VideoClip != null)
        {
            _videoPlayer.clip = slide.VideoClip;
            _videoPlayer.frame = 0;
            _videoTexture.transform.localPosition = slide.VideoClipPosition;
            _videoTexture.transform.localScale = slide.VideoClipScale * new Vector3(1, 1, 0);
            StartCoroutine(nameof(PlayVideo));
        }
        else
        {
            _videoPlayer.Stop();
            _videoTexture.enabled = false;
        }
    }

    /// <summary>
    /// The actual setting up of render texture and video file involves waiting for resources to load properly.
    /// Otherwise, visual artifacts appear on the render texture.
    /// Therefore, this is called as a coroutine, which waits until both the video player and the render texture are ready
    /// before displaying the render texture and playing the video.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayVideo()
    {
        // hiding and preparing render texture, and preparing video player
        _videoTexture.color = Color.clear;
        _videoTexture.enabled = true;
        _renderTexture.Create();
        _videoPlayer.Prepare();

        // waiting for video player and render texture to get ready
        while (!_videoPlayer.isPrepared || !_renderTexture.IsCreated())
            yield return null;

        // showing render texture when ready
        _videoTexture.color = Color.white;
        _videoPlayer.Play();
    }
}
