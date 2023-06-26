using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Tutorial : MonoBehaviour
{
    public TutorialEntry[] Items = Array.Empty<TutorialEntry>();
    /// <summary>
    /// Gets an event which is fired when all the tutorial entires have been completed.
    /// </summary>
    public UnityEvent OnCompleted;

    /// <summary>
    /// Gets an event which is fired when the tutorial is triggered.
    /// </summary>
    public UnityEvent OnTriggered;

    private int indexOfCurrentItem = 0;
    private bool triggered;
    private bool dismissed;

    //Setting the necessary values and variables needed
    public TutorialEntry Current
        => IndexOfCurrentItem >= 0 && IndexOfCurrentItem < Items.Length
        ? Items[IndexOfCurrentItem]
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
                Current.gameObject.SetActive(true);
            }

            indexOfCurrentItem = value;

            if (Current == null)
            {
                Dismiss();
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
    /// Dismisses the tutorial, removing deactivates all tutorial elements in the scene
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
        Debug.Log(IndexOfCurrentItem);
        foreach (var entry in Items)
        {
            if(entry != Current) entry.gameObject.SetActive(false);
            if(entry == Current) entry.gameObject.SetActive(true);
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

    //Deactivates all but the starting entry
    private void Start()
    {
        foreach (var entry in Items)
        {
            if(entry != Current) entry.gameObject.SetActive(false);
            if(entry == Current) entry.gameObject.SetActive(true);
            if(entry.GetComponents<TutorialEntry>().Length<=0) ArrayUtility.Remove(ref Items, entry);
            entry.Tutorial = this;
        }

    }   //For debugging purposes, proceeds to the next tutorial step when the spacebar is pressed
    private void Update(){
        if (Application.isEditor && Input.GetKeyDown("space"))
        {
            MoveNext();
        }
    }
}