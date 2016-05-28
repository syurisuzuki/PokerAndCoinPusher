using UnityEngine;
using UnityEditor;
using System.Collections;

public class Window : EditorWindow
{

    [MenuItem("DebugTool/ブラウザーを開く")]
    static void Open()
    {
        System.Diagnostics.Process.Start("http://google.jp");
    }

}