using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TutorialEntry))]
public class TimeoutTrigger : MonoBehaviour
{
    /// <summary>
    /// Gets an event that is fired when the player enters the box collider.
    /// </summary>
    public UnityEvent OnTriggered;
    public int TimeoutSeconds = 5;
    private TutorialEntry tutorialEntry;
    private DateTime triggeredAt;

    // Start is called before the first frame update
    void Start()
    {
        tutorialEntry = GetComponent<TutorialEntry>();
    }

    private void Update()
    {
        if (tutorialEntry.IsActive && triggeredAt == default)
        {
            triggeredAt = DateTime.Now;
        }

        if (tutorialEntry.IsActive && (DateTime.Now - triggeredAt) > TimeSpan.FromSeconds(TimeoutSeconds))
        {
            OnTriggered.Invoke();
            triggeredAt = DateTime.Now;
        }
    }
}