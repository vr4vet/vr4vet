using System;
using UnityEngine;

[CreateAssetMenu(menuName ="NPCScriptableObjects/DialogueTree")]
[Serializable]
public class DialogueTree : ScriptableObject
{
	public bool speakButtonOnExit = true;
	public DialogueSection[] sections;
}
