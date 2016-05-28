using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// シーンデータを保存するための.
/// ScriptableObject.
/// </summary>
public class SceneData : ScriptableObject {
    public List<string> scenename = new List<string>();
    public List<string> scenepath = new List<string>();

    public static void CreateScriptableObject() {
        CreateAsset<SceneData>();
    }

    public static void CreateAsset<Type>() where Type : ScriptableObject {
        Type item = CreateInstance<Type>();

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Editor_Extension/Editor/" + typeof(Type) + ".asset");

        AssetDatabase.CreateAsset(item, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = item;

    }
}
