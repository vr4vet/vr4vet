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
    [Tooltip("Will change slides every 10 seconds.")]
    [SerializeField] private bool AutomaticSlideShow = false;

    [Tooltip("Place instances of the Slide prefab here. These are the slides that will be displayed.")]
    [SerializeField] private List<GameObject> Slides = new();

    private Image _targetImage;
    private int _imageIndex = 0;
    private RawImage _videoTexture;
    private VideoPlayer _videoPlayer;

    public SlideChanged m_SlideChanged;
    public UnityEvent ToggledOff;
    public UnityEvent ToggledOn;

    // Start is called before the first frame update
    void Start()
    {
        if (m_SlideChanged != null)
            m_SlideChanged = new SlideChanged();

        if (ToggledOff != null)
            ToggledOff = new UnityEvent();

        if (ToggledOn != null)
            ToggledOn = new UnityEvent();

        _videoTexture = GetComponentInChildren<RawImage>();
        _videoPlayer = _videoTexture.GetComponentInChildren<VideoPlayer>();
        _targetImage = GetComponentInChildren<Image>();

        SetSlide(0);
        if (AutomaticSlideShow)
            InvokeRepeating(nameof(NextSlide), 10, 10);

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

    public void ToggleOff()
    {
        _targetImage.color = Color.clear;
        ToggledOff.Invoke();
    }

    public void ToggleOn()
    {
        _targetImage.color = Color.white;
        ToggledOn.Invoke();
    }

    private void SetSlide(int index)
    {
        Slide slide = Slides[index].GetComponent<Slide>();
        _targetImage.sprite = slide.SlideTexture;

        if (slide.VideoClip != null)
        {
            _videoTexture.enabled = true;
            _videoPlayer.clip = slide.VideoClip;
            _videoTexture.transform.localPosition = slide.VideoClipPosition;
            _videoTexture.transform.localScale = slide.VideoClipScale * new Vector3(1, 1, 0);
            _videoPlayer.frame = 0;
            _videoPlayer.Play();
        }
        else
        {
            _videoPlayer.Stop();
            _videoTexture.enabled = false;
        }
    }
}
