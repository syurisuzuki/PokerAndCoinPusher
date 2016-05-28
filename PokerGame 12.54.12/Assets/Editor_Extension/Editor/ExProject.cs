using System.IO;
using UnityEditor;
using UnityEngine;

public static class ExProject {

    

    private const int WIDTH = 16;
    [InitializeOnLoadMethod]
    private static void Example() {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI(string guid, Rect selectionRect) {
        var path = AssetDatabase.GUIDToAssetPath(guid);

        var pos = selectionRect;
        pos.x = 0;
        pos.xMax = selectionRect.xMax;

        var color = GUI.color;
        GUI.color = GetIconForFile(path);
        GUI.Box(pos, string.Empty);
        GUI.color = color;
    }

    public static Color GetIconForFile(string fileName) {
        return new Color(1f, 1f, 1f, .1f);
        int num = fileName.LastIndexOf('.');
        string text = (num != -1) ? fileName.Substring(num + 1).ToLower() : string.Empty;
        string text2 = text;
        switch (text2) {
            case "boo":
                return new Color(.8f, .2f, .6f, .3f);
            case "cginc":
                return new Color(.8f, .2f, .6f, .3f);
            case "cs":
                return new Color(.8f, 1f, 1f, .3f);
            case "guiskin":
                return new Color(.8f, .2f, .6f, .3f);
            case "js":
                return new Color(.8f, .2f, .6f, .3f);
            case "mat":
                return new Color(0f, 0f, .8f, .3f);
            case "prefab":
                return new Color(.8f, .2f, .8f, .3f);
            case "shader":
                return new Color(0f, 1f, .5f, .3f);
            case "txt":
                return new Color(0f, 0f, 0f, .3f);
            case "unity":
                return new Color(.8f, 0f, 0f, .3f);
            case "prefs":
                return new Color(.8f, .9f, .6f, .3f);
            case "anim":
                return new Color(.2f, .3f, 1f, .3f);
            case "meta":
                return new Color(.8f, .2f, .6f, .3f);
            case "ttf":
            case "otf":
            case "fon":
            case "fnt":
                return new Color(0f, 0f, 0f, .3f);
            case "aac":
            case "aif":
            case "aiff":
            case "au":
            case "mid":
            case "midi":
            case "mp3":
            case "mpa":
            case "ra":
            case "ram":
            case "wma":
            case "wav":
            case "wave":
            case "ogg":
                return new Color(.3f, .3f, 0f, .3f);
            case "ai":
            case "apng":
            case "png":
            case "bmp":
            case "cdr":
            case "dib":
            case "eps":
            case "exif":
            case "gif":
            case "ico":
            case "icon":
            case "j":
            case "j2c":
            case "j2k":
            case "jas":
            case "jiff":
            case "jng":
            case "jp2":
            case "jpc":
            case "jpe":
            case "jpeg":
            case "jpf":
            case "jpg":
            case "jpw":
            case "jpx":
            case "jtf":
            case "mac":
            case "omf":
            case "qif":
            case "qti":
            case "qtif":
            case "tex":
            case "tfw":
            case "tga":
            case "tif":
            case "tiff":
            case "wmf":
            case "psd":
            case "exr":
                return new Color(0f, 1f, 0f, .3f);
            case "3df":
            case "3dm":
            case "3dmf":
            case "3ds":
            case "3dv":
            case "3dx":
            case "blend":
            case "c4d":
            case "lwo":
            case "lws":
            case "ma":
            case "max":
            case "mb":
            case "mesh":
            case "obj":
            case "vrl":
            case "wrl":
            case "wrz":
            case "fbx":
                return new Color(1f, .3f, .6f, .3f);
            case "asf":
            case "asx":
            case "avi":
            case "dat":
            case "divx":
            case "dvx":
            case "mlv":
            case "m2l":
            case "m2t":
            case "m2ts":
            case "m2v":
            case "m4e":
            case "m4v":
            case "mjp":
            case "mov":
            case "movie":
            case "mp21":
            case "mp4":
            case "mpe":
            case "mpeg":
            case "mpg":
            case "mpv2":
            case "ogm":
            case "qt":
            case "rm":
            case "rmvb":
            case "wmw":
            case "xvid":
                return new Color(.8f, .2f, .6f, .3f);
            case "colors":
            case "gradients":
            case "curves":
            case "curvesnormalized":
            case "particlecurves":
            case "particlecurvessigned":
            case "particledoublecurves":
            case "particledoublecurvessigned":
            case "asset":
                return new Color(1f, .2f, 1f, .3f);
            case "":
                return new Color(1f, 1f, 1f, .3f);
        }
        return new Color(0f, 0f, 0f, .3f);
    }
}