using System;
using UnityEngine;

public enum TTSProvider
{
    Wit,
    OpenAI
}

[CreateAssetMenu(menuName = "NPCScriptableObjects/NPC")]
public class NPC : ScriptableObject
{
    public string NameOfNPC;
    public GameObject NpcPrefab;
    public GameObject CharacterModel;
    public Avatar CharacterAvatar;
    [HideInInspector]
    public TTSProvider selectedTTSProvider; // Dropdown for TTS provider
    [HideInInspector]
    public int WitVoiceId; // Voice ID for Wit
    [HideInInspector]

    public string OpenAiVoiceId; // Voice ID for OpenAI
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;
    public bool ShouldFollow;
    public DialogueTree[] DialogueTreesSO;
    public TextAsset[] DialogueTreeJSON;

    public RuntimeAnimatorController runtimeAnimatorController;
    [Range(0, 1)]
    public float SpatialBlend;
    [Range(1, 100)]
    public float MinDistance;

    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 1000;

}
