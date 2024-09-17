using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using DotNetEnv;

namespace UnityDotenv
{
    public class DotenvConfigWindow : EditorWindow
    {
        private Vector2 scrollPos = Vector2.zero;
        private bool foldoutEnvConfigSettings = true;
        private bool foldoutEnvDataSettings = true;
        private int showLoadDebugDotenv;
        private Dictionary<string, string> envValuPair = new Dictionary<string, string>();
        private string dotenvFilePath = ".env";

        void OnEnable()
        {
            envValuPair = Env.Load(DotenvFile.FileFullPath).ToDictionary();
            showLoadDebugDotenv = PlayerPrefs.GetInt(Const.ShowLoadDebugDotenvKey, 1);
        }

        [MenuItem("Tools/Dotenv Config")]
        static void Open()
        {
            EditorWindow.GetWindow<DotenvConfigWindow>("Dotenv Config");
        }

        // Windowのクライアント領域のGUI処理を記述
        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            WithInFoldoutBlock(".env config", ref foldoutEnvConfigSettings, () =>
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("show log when load .env file.");
                int newShowLoadDebugDotenv = EditorGUILayout.Toggle("LocalStorage", PlayerPrefs.GetInt(Const.ShowLoadDebugDotenvKey, showLoadDebugDotenv) == 1) ? 1 : 0;
                if (showLoadDebugDotenv != newShowLoadDebugDotenv)
                {
                    PlayerPrefs.SetInt(Const.ShowLoadDebugDotenvKey, showLoadDebugDotenv = newShowLoadDebugDotenv);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(".env file path");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(Application.streamingAssetsPath);
                EditorGUILayout.LabelField(dotenvFilePath);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Open"))
                {
                    EditorUtility.RevealInFinder(DotenvFile.FileFullPath);
                }
                EditorGUILayout.EndHorizontal();
            });

            WithInFoldoutBlock(".env data params", ref foldoutEnvDataSettings, () =>
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Key Name");
                EditorGUILayout.LabelField("Value");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                Dictionary<string, string> tmpKV = envValuPair.ToDictionary();
                foreach (var kv in tmpKV)
                {
                    EditorGUILayout.BeginHorizontal();
                    string newKey = EditorGUILayout.TextField(kv.Key);
                    if(kv.Key != newKey)
                    {
                        string tmpValue = envValuPair[kv.Key];
                        envValuPair.Remove(kv.Key);
                        envValuPair.Add(newKey, tmpValue);
                    }
                    string newValue = EditorGUILayout.TextField(kv.Value);
                    if (kv.Value != newValue)
                    {
                        envValuPair[kv.Key] = newValue;
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        envValuPair.Remove(kv.Key);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < 5; ++i)
                {
                    EditorGUILayout.Space();
                }
                if (GUILayout.Button("+"))
                {
                    if (!envValuPair.ContainsKey(""))
                    {
                        envValuPair.Add("", "");
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("Save"))
                {
                    DotenvFile.WriteDotenvFile(envValuPair);
                }
            });

            EditorGUILayout.EndScrollView();
        }

        private void WithInFoldoutBlock(string title, ref bool foldout, Action callback)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            foldout = EditorGUILayout.Foldout(foldout, title);
            if (foldout)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.Space();
                callback();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }
}