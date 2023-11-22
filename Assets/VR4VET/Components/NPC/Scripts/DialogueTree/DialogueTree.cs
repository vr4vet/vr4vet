using System;
using UnityEngine;

[CreateAssetMenu(menuName ="NPCScriptableObjects/DialogueTree")]
[Serializable]
public class DialogueTree : ScriptableObject
{
	public DialogueSection[] sections;
}
