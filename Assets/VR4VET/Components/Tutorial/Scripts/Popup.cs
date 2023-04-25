// Created by Trym Lund Flogard <trym@flogard.no>
using BNG;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshPro))]
public class Popup : MonoBehaviour, ITutorial
{
    private const int AnimationDurationMs = 500;
    private TextMeshPro textMesh;
    private Renderer currentRenderer;
    private Renderer backgroundRenderer;

    [field: SerializeField]
    public GameObject Background { get; set; }

    /// <summary>
    /// Fades the popup into view
    /// </summary>
    public void Show()
    {
        StopAllCoroutines();
        enabled = currentRenderer.enabled = backgroundRenderer.enabled = true;
        var material = backgroundRenderer.material;
        Color color = material.color;
        StartCoroutine(Fade(static progress => Easing.InCubic(progress)).GetEnumerator());
    }

    /// <summary>
    /// Hides the popup from view
    /// </summary>
    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Out());

        IEnumerator Out()
        {
            foreach (var _ in Fade(static progress => Easing.OutCubic(1 - progress)))
            {
                yield return null;
            }

            SetEnabled(value: false);
        }
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        currentRenderer = GetComponent<Renderer>();
        if (Background)
        {
            backgroundRenderer = Background.GetComponent<Renderer>();
        }
        SetEnabled(false);
    }

    private void OnDrawGizmosSelected()
    {
        SetEnabled(true);
    }

    // Update is called once per frame
    private void Update()
    {
        // Make the popup face the player
        Vector3 playerPosition = Camera.main.transform.position;
        var playerToSelf = transform.position - playerPosition;
        transform.rotation = Quaternion.LookRotation(playerToSelf);
        Background.transform.rotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(90, 0, 0));

        // Update the rounded rect background
        var textSize = textMesh.textBounds.size;
        backgroundRenderer.material.SetVector("_Size", textSize);
        var backgroundSize = Background.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        Background.transform.localScale = new Vector3(textSize.x / backgroundSize.x, 1, textSize.y / backgroundSize.z) * 1.25f;
    }

    private IEnumerable Fade(Func<float, float> interpolate)
    {
        var backgroundMaterial = backgroundRenderer.material;
        float start = Time.time;
        Color color = backgroundMaterial.color;
        float targetAlpha = color.a;
        color.a = interpolate(0);
        backgroundMaterial.color = color;
        float progress = 0;
        while (progress < 1)
        {
            progress = (Time.time - start) / (AnimationDurationMs / 1000f);
            color.a = targetAlpha * interpolate(progress);
            backgroundMaterial.color = color;
            yield return null;
        }
        color.a = targetAlpha;
    }

    private void SetEnabled(bool value)
    {
        enabled = currentRenderer.enabled = backgroundRenderer.enabled = value;
    }
}
