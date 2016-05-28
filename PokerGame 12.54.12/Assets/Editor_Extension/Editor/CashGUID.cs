using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// GUIDを保存するための.
/// ScriptableObject.
/// </summary>
public class CashGUID : ScriptableObject {
    public List<string> guid = new List<string>();
    public List<string> filepath = new List<string>();
    public List<string> cashpath = new List<string>();
    public List<string> scene = new List<string>();
    //public Dictionary<string, List<int>> scene = new Dictionary<string, List<int>>();

    public static void CreateScriptableObject() {
        CreateAsset<CashGUID>();
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
