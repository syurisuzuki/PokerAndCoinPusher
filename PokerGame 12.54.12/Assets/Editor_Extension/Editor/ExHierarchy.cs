using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ExHierarchy{

    private const int WIDTH = 16;
    [InitializeOnLoadMethod]
    private static void Init() {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        
    }

    private static void OnGUI(int instanceID, Rect selectionRect) {

        var basepos = selectionRect;
        basepos.x = 0;
        basepos.xMax = selectionRect.xMax;
        var basecolor = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, .1f);
        GUI.Box(basepos, string.Empty);
        GUI.color = basecolor;

        var pos = selectionRect;
        pos.x = 0;
        pos.xMax = selectionRect.x;
        var color = GUI.color;
        
        if (instanceID < 0) {
            GUI.color = new Color(0f, 0f, 1f, .3f);
        } else {
            GUI.color = new Color(1f, 0f, 0f, .3f);
        }
        GUI.Box(pos, string.Empty);
        GUI.color = color;


        if (selectionRect.x / 14 > 1) {
            GUIStyle styled = new GUIStyle();
            styled.normal.textColor = new Color(1, 1, 0);
            styled.fontSize = 10;
            styled.alignment = TextAnchor.MiddleLeft;
            GUI.Label(pos, (selectionRect.x/14 - 1).ToString(), styled);
        }

        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go == null) {
            return;
        }

        var isWarning = go
            .GetComponents<MonoBehaviour>()
            .Any(c => c == null);

        if (!isWarning) {
            return;
        }

        pos = selectionRect;
        pos.x = 0;
        pos.xMax = selectionRect.x;
        //pos.width = WIDTH;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(1,1,0);
        style.fontSize = 10;
        style.alignment = TextAnchor.MiddleLeft;

        color = GUI.color;
        GUI.color = new Color(1f, 1f, .0f, .5f);
        GUI.Box(pos, string.Empty);
        GUI.color = color;

        pos.x = 10;

        GUI.Label(pos, "!",style);
    }
}
