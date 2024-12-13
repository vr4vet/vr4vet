using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class SlideChanged : UnityEvent<int>
{
}


public class SlidePresentation : MonoBehaviour
{
    [SerializeField] private bool AutomaticSlideShow = false;
    [SerializeField] private List<Sprite> Slides = new();
    [SerializeField] private List<GameObject> SlidesNew = new();

    private Image _targetImage;
    private int _imageIndex = 0;

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

        _targetImage = GetComponentInChildren<Image>();
        _targetImage.sprite = Slides[0];
        //_targetImage.sprite = SlidesNew[0].GetComponent<Slide>().SlideTexture;
        if (AutomaticSlideShow)
            InvokeRepeating(nameof(NextSlide), 10, 10);
    }

    public void NextSlide()
    {
        _imageIndex = (_imageIndex + 1) % Slides.Count;
        _targetImage.sprite = Slides[_imageIndex];
        m_SlideChanged.Invoke(_imageIndex);
    }

    public void PrevSlide()
    {
        _imageIndex = (_imageIndex - 1) < 0 ? Slides.Count - 1 : _imageIndex - 1;
        _targetImage.sprite = Slides[_imageIndex];
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
}
