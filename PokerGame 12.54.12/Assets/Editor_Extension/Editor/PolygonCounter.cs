using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MeshFilter))]
public class PolygonCounter : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        MeshFilter filter = target as MeshFilter;
        //string polygons = "Triangles: " + filter.sharedMesh.triangles.Length / 3;
        //EditorGUILayout.LabelField(polygons);
    }
}


[CustomEditor(typeof(SkinnedMeshRenderer))]
public class SkinPolygonCounter : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SkinnedMeshRenderer skin = target as SkinnedMeshRenderer;
        string polygons = "Triangles: " + skin.sharedMesh.triangles.Length / 3;
        EditorGUILayout.LabelField(polygons);
    }
}