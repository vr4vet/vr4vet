using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(NPC))]
public class NPCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target;

        // Draw default inspector properties
        DrawDefaultInspector();

        // Populate TTS Providers
        TTSProvider[] providers = (TTSProvider[])System.Enum.GetValues(typeof(TTSProvider));
        string[] providerNames = System.Enum.GetNames(typeof(TTSProvider));

        // Create a dropdown for selecting TTS Provider
        npc.selectedTTSProvider = (TTSProvider)EditorGUILayout.EnumPopup("TTS Provider", npc.selectedTTSProvider);

        // Depending on the selected TTS Provider, show the corresponding voice presets
        switch (npc.selectedTTSProvider)
        {
            case TTSProvider.Wit:
                // Populate voice presets for Wit (0 to 22)
                string[] WitVoicePresets = new string[23]; // Array size for 23 general presets
                for (int i = 0; i <= 22; i++)
                {
                    WitVoicePresets[i] = "Preset " + i; // Presets from 1 to 22
                }

                // Clamp the selected preset index for witVoiceId
                int selectedWitPresetIndex = Mathf.Clamp(npc.WitVoiceId, 0, WitVoicePresets.Length - 1);
                selectedWitPresetIndex = EditorGUILayout.Popup("Wit Voice Preset", selectedWitPresetIndex, WitVoicePresets);
                npc.WitVoiceId = selectedWitPresetIndex; // Update witVoiceId based on selection
                break;

            case TTSProvider.OpenAI:
                // Populate voice presets for OpenAI
                string[] openAiVoicePresets = { "Alloy", "Echo", "Fable", "Onyx", "Nova", "Shimmer" };

                int selectedOpenAiPresetIndex = Array.IndexOf(openAiVoicePresets, npc.OpenAiVoiceId);
                selectedOpenAiPresetIndex = EditorGUILayout.Popup("OpenAI Voice Preset", selectedOpenAiPresetIndex, openAiVoicePresets);
                npc.OpenAiVoiceId = openAiVoicePresets[selectedOpenAiPresetIndex]; // Update openAiVoiceId based on selection
                break;
        }

        // Mark the object as dirty to ensure changes are saved
        if (GUI.changed)
        {
            EditorUtility.SetDirty(npc);
        }
    }
}
