using UnityEngine;
using UnityEditor;
using System;

// Custom inspector for TTS provider selection for the NPC scriptable object

[CustomEditor(typeof(NPC))]
public class NPCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target;

        DrawDefaultInspector();

        TTSProvider[] providers = (TTSProvider[])Enum.GetValues(typeof(TTSProvider));
        string[] providerNames = Enum.GetNames(typeof(TTSProvider));

        npc.selectedTTSProvider = (TTSProvider)EditorGUILayout.EnumPopup("TTS Provider", npc.selectedTTSProvider);

        // Depending on the selected TTS Provider, show and set the corresponding voice presets
        switch (npc.selectedTTSProvider)
        {
            case TTSProvider.Wit:
                string[] WitVoicePresets = new string[22];
                for (int i = 0; i <= 21; i++)
                {
                    WitVoicePresets[i] = "Preset " + i; 
                }

                int selectedWitPresetIndex = Mathf.Clamp(npc.WitVoiceId, 0, WitVoicePresets.Length - 1);
                selectedWitPresetIndex = EditorGUILayout.Popup("Wit Voice Preset", selectedWitPresetIndex, WitVoicePresets);
                npc.WitVoiceId = selectedWitPresetIndex;
                break;

            case TTSProvider.OpenAI:
                string[] openAiVoicePresets = { "alloy", "echo", "fable", "onyx", "nova", "shimmer" };

                int selectedOpenAiPresetIndex = Array.IndexOf(openAiVoicePresets, npc.OpenAiVoiceId);
                if (selectedOpenAiPresetIndex == -1)
                {
                    selectedOpenAiPresetIndex = 0;
                }

                selectedOpenAiPresetIndex = EditorGUILayout.Popup("OpenAI Voice Preset", selectedOpenAiPresetIndex, openAiVoicePresets);
                npc.OpenAiVoiceId = openAiVoicePresets[selectedOpenAiPresetIndex];
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(npc);
        }
    }
}
