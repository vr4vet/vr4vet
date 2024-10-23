using System;
using UnityEngine;

[CreateAssetMenu(menuName ="NPCScriptableObjects/DialogueTree")]
[Serializable]
public class DialogueTree : ScriptableObject
{
	public bool shouldTriggerOnProximity = true;
	public bool speakButtonOnExit = true;
	public DialogueSection[] sections;


	// This method is called when something changes in the inspector
    private void OnValidate()
    {
        InitializeInterruptableElements();
    }

    // Method to initialize the interruptableElements array based on the dialogue size in each section
    public void InitializeInterruptableElements()
    {
        if (sections == null) return;

        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].dialogue != null)
            {
                int dialogueCount = sections[i].dialogue.Length;

                // Initialize or resize the interruptableElements array to match the size of the dialogue array
                if (sections[i].interruptableElements == null || sections[i].interruptableElements.Length != dialogueCount)
                {
                    sections[i].interruptableElements = new bool[dialogueCount];
                }
            }
        }
    }
}
