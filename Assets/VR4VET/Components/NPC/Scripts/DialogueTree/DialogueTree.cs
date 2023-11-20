using System;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class DialogueTree : ScriptableObject
{
	public DialogueSection[] sections;
}
