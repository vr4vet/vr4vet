using System;
using UnityEngine;

[CreateAssetMenu(menuName ="NPCScriptableObjects/NPC")]
public class NPC : ScriptableObject
{
    public String NameOfNPC;
    public GameObject NpcPrefab;
    public GameObject CharacterModel;
    public Avatar CharacterAvatar;
    public int VoicePresetId;
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;
    public bool ShouldFollow;
    public DialogueTree[] DialogueTreesSO;
    public TextAsset[] DialogueTreeJSON;
    public RuntimeAnimatorController runtimeAnimatorController; 
}
