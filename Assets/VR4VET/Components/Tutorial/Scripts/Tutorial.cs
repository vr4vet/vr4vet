using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour, ITutorial
{
    public TooltipScript[] Items = Array.Empty<TooltipScript>();
    public GameObject PopupHint;

    /// <summary>
    /// Gets an event which is fired when all the tutorial entires have been completed.
    /// </summary>
    public UnityEvent OnCompleted;

    /// <summary>
    /// Gets an event which is fired when the tutorial is triggered.
    /// </summary>
    public UnityEvent OnTriggered;

    private int indexOfCurrentItem = -1;
    private bool triggered;
    private bool dismissed;

    public GameObject Current
        => IndexOfCurrentItem >= 0 && IndexOfCurrentItem < Items.Length
        ? Items[IndexOfCurrentItem].gameObject
        : null;

    private int IndexOfCurrentItem
    {
        get => indexOfCurrentItem;
        set
        {
            if (indexOfCurrentItem == value)
                return;

            if (Current != null)
            {
                Current.SetActive(false);
            }

            indexOfCurrentItem = value;

            if (Current != null)
            {
                Current.SetActive(false);
            }
        }
    }

    public bool Triggered
    {
        get => triggered;
        internal set
        {
            if (triggered == value)
                return;

            triggered = value;
            if (value && !dismissed)
            {
                IndexOfCurrentItem = 0;
            }
        }
    }

    /// <summary>
    /// Starts the tutorial if it has not already been started.
    /// </summary>
    public void Trigger()
    {
        Triggered = true;
    }

    /// <summary>
    /// Dismisses the tutorial, removing all UI elements from the scene.
    /// </summary>
    public void Dismiss()
    {
        IndexOfCurrentItem = -1;
    }

    /// <summary>
    /// Advances to the next tutorial entry, or completes the tutorial if all entries are enumerated.
    /// </summary>
    /// <returns><see langword="true"/> if there are more entires in the tutorial.</returns>
    public void MoveNext()
    {
        IndexOfCurrentItem = Math.Min(IndexOfCurrentItem, Items.Length) + 1;
        foreach (var entry in Items)
        {
            if(entry != Current) entry.gameObject.SetActive(false);
        }

        if (IndexOfCurrentItem == Items.Length && Items.Length > 0)
        {
            OnCompleted.Invoke();
        }
    }

    /// <summary>
    /// Restores the previous tutorial item.
    /// </summary>
    /// <returns><see langword="true"/> if the tutorial is still active.</returns>
    public bool MoveBack()
    {
        IndexOfCurrentItem = Math.Max(IndexOfCurrentItem, 0) - 1;

        return IndexOfCurrentItem >= 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var entry in Items)
        {
            if(entry != Current) entry.gameObject.SetActive(false);
        }
    }
}