/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Tablet
{
    /// <summary>
    /// This class will make a custom editor window and generate the text file that
    /// uses for entering the max poeng for every aktivitet
    /// </summary>
    class FerdighetPoengGenerator : EditorWindow
    {

        string mainFolder = "Assets/Tablet/";

        [MenuItem("IMTEL Tools/Ferdighet Poeng Generator")]

        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(FerdighetPoengGenerator));
        }

        void OnGUI()
        {
            EditorGUILayout.HelpBox("OBS! The existing file text will be overwritten!\nPlease create a backup file before generating" +
                "\na new text file.", MessageType.Warning);
            GUILayout.Space(5);
            if (GUILayout.Button("Gererate Text File", GUILayout.Width(400), GUILayout.Height(50)))
            {
                string path = mainFolder + "Resources/FerdighetPoenger.txt";
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "");
                }

                Oppgave[] oppgaver = GameObject.FindObjectsOfType<Oppgave>();
                if (File.Exists(path))
                    File.Delete(path);

                foreach (string ferdighet in GameObject.Find("FerdighetManager").GetComponent<FerdighetManager>().ferdighter)
                {
                    File.AppendAllLines(path, new string[] { "{" });

                    if (ferdighet != "")
                    {
                        File.AppendAllLines(path, new string[] { "**." + ferdighet + ".**" });
                    }
                    foreach (Oppgave oppgave in oppgaver)
                    {
                        File.AppendAllLines(path, new string[] { "  (" });
                        File.AppendAllLines(path, new string[] { "  --." + oppgave.oppgaveName + ".--" });
                        foreach (string aktivitet in oppgave.aktiviteter)
                        {
                            if (aktivitet != "")
                                File.AppendAllLines(path, new string[] { "      <" + aktivitet + ":>" });
                        }
                        File.AppendAllLines(path, new string[] { "  )" });
                    }

                    File.AppendAllLines(path, new string[] { "}" });
                    File.AppendAllLines(path, new string[] { "" });
                }

                EditorUtility.DisplayDialog("Done", "The text file is created in Assets/Resources/FerdighetPoenger.txt", "Ok");
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Backup", GUILayout.Width(200), GUILayout.Height(50)))
            {
                if (File.Exists(mainFolder + "Resources/FerdighetPoenger.txt"))
                {
                    //delete back file first
                    if (File.Exists(mainFolder + "Resources/FerdighetPoenger_Backup.bk"))
                        File.Delete(mainFolder + "Resources/FerdighetPoenger_Backup.bk");

                    FileUtil.CopyFileOrDirectory(mainFolder + "Resources/FerdighetPoenger.txt", mainFolder + "Resources/FerdighetPoenger_Backup.bk");
                    EditorUtility.DisplayDialog("Done", "The backup file is created in " + mainFolder + "Resources/FerdighetPoenger_Backup.bk", "Ok");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "File Not Found!", "Ok");
                    return;
                }

            }
            if (GUILayout.Button("Recover Backup", GUILayout.Width(200), GUILayout.Height(50)))
            {
                if (File.Exists("Assets/Resources/FerdighetPoenger_Backup.bk"))
                {
                    //dele file first
                    if (File.Exists(mainFolder + "Resources/FerdighetPoenger.txt"))
                        File.Delete(mainFolder + "Resources/FerdighetPoenger.txt");

                    FileUtil.CopyFileOrDirectory(mainFolder + "Resources/FerdighetPoenger_Backup.bk", mainFolder + "Resources/FerdighetPoenger.txt");
                    EditorUtility.DisplayDialog("Done", "Backup File is recoverd", "Ok");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "File Not Found!", "Ok");
                    return;
                }
            }
            GUILayout.EndHorizontal();

        }
    }
}