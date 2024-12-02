using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerInputFinder : EditorWindow
{
    private Vector2 scrollPosition;
    private string[] scriptPaths;
    private bool[] showScriptContents;

    [MenuItem("Window/PlayerInput Finder")]
    public static void ShowWindow()
    {
        GetWindow<PlayerInputFinder>("PlayerInput Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Searching for scripts using PlayerInput class...", EditorStyles.boldLabel);

        if (GUILayout.Button("Find Scripts"))
        {
            FindScriptsUsingPlayerInput();
        }

        if (scriptPaths != null && scriptPaths.Length > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Label("Scripts found:");

            for (int i = 0; i < scriptPaths.Length; i++)
            {
                string scriptPath = scriptPaths[i];
                string scriptName = Path.GetFileName(scriptPath);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(scriptName);

                showScriptContents[i] = EditorGUILayout.ToggleLeft("Show Contents", showScriptContents[i]);

                if (GUILayout.Button("Open Folder", GUILayout.Width(100)))
                {
                    string folderPath = Path.GetDirectoryName(scriptPath);
                    EditorUtility.RevealInFinder(folderPath);
                }

                if (GUILayout.Button("Open Script", GUILayout.Width(100)))
                {
                    UnityEngine.Object scriptObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scriptPath);
                    AssetDatabase.OpenAsset(scriptObject);
                }

                EditorGUILayout.EndHorizontal();

                if (showScriptContents[i])
                {
                    DisplayScriptContents(scriptPath);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Label("No scripts found.");
        }
    }

    private void FindScriptsUsingPlayerInput()
    {
        string[] allScriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        List<string> matchingScriptPaths = new List<string>();
        List<bool> showContents = new List<bool>();

        foreach (string scriptPath in allScriptPaths)
        {
            string scriptContents = File.ReadAllText(scriptPath);
            if (scriptContents.Contains("PlayerInput"))
            {
                matchingScriptPaths.Add(scriptPath);
                showContents.Add(false);
            }
        }

        scriptPaths = matchingScriptPaths.ToArray();
        showScriptContents = showContents.ToArray();
    }

    private void DisplayScriptContents(string scriptPath)
    {
        string[] lines = File.ReadAllLines(scriptPath);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        foreach (string line in lines)
        {
            GUILayout.Label(line);
        }

        EditorGUILayout.EndVertical();
    }
}
