using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Slide : MonoBehaviour
{
    [Tooltip("Place the slide texture here after setting texture type to 'Sprite (2D and UI)'.")]
    public Sprite SlideTexture;

    [Tooltip("Place a video clip here if it should play when this slide is displayed.")]
    public VideoClip VideoClip;

    [Tooltip("The position of the video clip offset from the center of slide.")]
    public Vector2 VideoClipPosition = Vector2.zero;

    [Tooltip("The scale of the video clip-")]
    public float VideoClipScale = 1.0f;
}
