using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

public sealed class ChangeSceneAndComponentSearchWindow : EditorWindow {

    private List<GameObject> resultObjs = new List<GameObject>();
    public bool isFullMatchName;
    private string mTypeName;
    private string mResult;
    private string cResult;
    private List<string> componentnames = new List<string>();
    private Vector2 mScrollPos;
    private Vector2 cScrollPos;
    private Vector2 caPos;
    Vector2 scrollPos = Vector2.zero;

    public SceneData _scenedata {
        get { return AssetDatabase.LoadAssetAtPath<SceneData>("Assets/Editor_Extension/Editor/SceneData.asset"); }
    }
    public CashGUID _guids {
        get { return AssetDatabase.LoadAssetAtPath<CashGUID>("Assets/Editor_Extension/Editor/CashGUID.asset"); }
    }

    private string objpath = "Assets/Editor_Extension/Editor/SceneData.asset";

    public const int FILTERMODE_ALL = 0;
    public const int FILTERMODE_NAME = 1;
    public const int FILTERMODE_TYPE = 2;

    public string filesize = "0kb";

    public bool show = true;
    public bool showFPS = true;
    public bool showInEditor = false;
    int frameCount;
    float prevTime;
    float fps;

    public string netWorkType {
        get {
            switch (Application.internetReachability) {
                case NetworkReachability.NotReachable:
                    return "NetWorkStatus:ネットワーク未接続";
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return "NetWorkStatus:キャリア接続";
                case NetworkReachability.ReachableViaLocalAreaNetwork:
#if UNITY_ANDROID || UNITY_IOS
                    return "NetWorkStatus:接続中";
#else
                    return "NetWorkStatus:接続";
#endif
                default:
                    return "NetWorkStatus:Unknown";
            }
        }
    }

    [MenuItem("Tools/Serch Window %#w")]
    private static void Init() {
        GetWindow<ChangeSceneAndComponentSearchWindow>();
    }

    void Update() {
        ++frameCount;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f) {
            fps = Mathf.Floor(frameCount / time);
            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// リフレクションによる.
    /// ヒエラルキー検索へのセット.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="filterMode"></param>
    public static void SetSearchFilter(string filter, int filterMode) {

        SearchableEditorWindow hierarchy = null;

        SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));

        foreach (SearchableEditorWindow window in windows) {

            if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow") {
                hierarchy = window;
            }

        }

        if (hierarchy == null)
            return;

        MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = new object[] { filter, filterMode, false };

        setSearchType.Invoke(hierarchy, parameters);
    }

    /// <summary>
    /// プロジェクトウィンドウの検索に代入.
    /// </summary>
    /// <param name="filter"></param>
    public static void SetProjectFilter(string filter) {
        var project = Resources
        .FindObjectsOfTypeAll<EditorWindow>()
        .First(c => c.GetType().ToString() == "UnityEditor.ProjectBrowser");
        ;
        var sceneViewType = Types.GetType("UnityEditor.ProjectBrowser", "UnityEditor.dll");
        MethodInfo shoeMethod = null;
        MethodInfo[] sceneViewMethods = sceneViewType.GetMethods();

        foreach (var method in sceneViewMethods) {
            if (method.Name == "SetSearch" ) {
                shoeMethod = method;
                break;
            }
        }

        object s = filter;
        object[] parameters = new object[] {filter };

        shoeMethod.Invoke(project, parameters);
    }

    private float lastCollect = 0;
    private float lastCollectNum = 0;
    private float delta = 0;
    private float lastDeltaTime = 0;
    private int allocRate = 0;
    private int lastAllocMemory = 0;
    private float lastAllocSet = -9999;
    private int allocMem = 0;
    private int collectAlloc = 0;
    private int peakAlloc = 0;

    private void OnGUI() {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(400));

        GUILayout.Label("Scene", GUILayout.Width(100));

        if (_scenedata != null) {
            if (_scenedata.scenename.Count == 0) {
                foreach (var s in EditorBuildSettings.scenes) {
                    string sceneName = Path.GetFileName(s.path).Replace(".unity", "");
                    _scenedata.scenename.Add(sceneName);
                    _scenedata.scenepath.Add(s.path);
                }
                EditorUtility.SetDirty(_scenedata);
                AssetDatabase.Refresh();
            }

            var stylex = new GUIStyle(GUI.skin.button);
            stylex.alignment = TextAnchor.MiddleLeft;
            stylex.fontSize = 14;
            int index = 0;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUI.skin.box, GUILayout.Width(260), GUILayout.Height(370));
            {
                foreach (string s in _scenedata.scenename) {
                    if (Application.loadedLevelName == s) {
                        stylex.normal.textColor = Color.red;
                    } else {
                        stylex.normal.textColor = Color.blue;
                    }
                    if (GUILayout.Button(s, stylex)) {
                        if (EditorApplication.SaveCurrentSceneIfUserWantsTo()) {
                            EditorApplication.OpenScene(_scenedata.scenepath[index]);
                        }
                    }
                    index++;
                }
            }
            if (GUILayout.Button("シーンデータを再生成する")) {
                AssetDatabase.DeleteAsset("Assets/Editor_Extension/Editor/SceneData.asset");
                AssetDatabase.Refresh();
                SceneData.CreateScriptableObject();
            }
            EditorGUILayout.EndScrollView();

        } else {
            if (GUILayout.Button("シーンデータを作成する")) {
                SceneData.CreateScriptableObject();
            }
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(260), GUILayout.Height(400));
        EditorGUI.BeginChangeCheck();
        mTypeName = EditorGUILayout.TextField("オブジェクトネーム", mTypeName);
        isFullMatchName = EditorGUILayout.Toggle("名前の完全一致", isFullMatchName);
        if (EditorGUI.EndChangeCheck()) {
            mResult = CheckObject(mTypeName);
        }
        mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos, GUI.skin.box, GUILayout.Width(260), GUILayout.Height(100));
        EditorGUILayout.TextArea(mResult);
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("検索結果全てを選択する")) {
            Selection.objects = resultObjs.ToArray();
        }

        if (GUILayout.Button("Sync Hierarchy")) {
            SetSearchFilter(mTypeName, 1);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("コンポーネント一覧更新")) {
            cResult = CheckCompornent();
        }
        cScrollPos = EditorGUILayout.BeginScrollView(cScrollPos, GUI.skin.box, GUILayout.Width(260), GUILayout.Height(150));
        foreach (string s in componentnames) {
            if (GUILayout.Button(s)) {
                SetSearchFilter("t:" + s, 0);
            }

        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Select Warning Object")) {
            Selection.objects = SelectWarningObject();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(540));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Data Init")) {
            CreateAllPathGUIDProject();
        }
        if (GUILayout.Button("Cache Hierarchy")) {
            CashHierarchyInstanceID();
        }
        if (GUILayout.Button("Cache Project")) {
            CashProjectFolderGUID();
        }
        EditorGUILayout.EndHorizontal();

        var style = new GUIStyle(GUI.skin.button);
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = 14;
        style.fixedHeight = 30;
        var content = new GUIContent();

        caPos = EditorGUILayout.BeginScrollView(caPos, GUI.skin.box, GUILayout.Width(530), GUILayout.Height(150));
        foreach (string s in _guids.cashpath) {
            content.image = GetIconForFile(s);
            content.text = s;
            var split = s.Split(new[] { "/" }, StringSplitOptions.None);
            var filename = split[split.Length - 1];
            var split_ext = filename.Split(new[] { "." }, StringSplitOptions.None);
            if (GUILayout.Button(content, style)) {
                SetProjectFilter(split_ext[0]);
            }
        }
        string scenename = Application.loadedLevelName;
        var currentscene = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        foreach (string i in _guids.scene) {
            var split = i.Split(new[] { "/" }, StringSplitOptions.None);
            var objectv = currentscene.FirstOrDefault(id => id.GetInstanceID() == int.Parse(split[1]));
            if (objectv != null) {
                if (GUILayout.Button(i, style)) {
                    SelectObjectFromID(int.Parse(split[1]));
                }
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(540));

        GUILayout.Label(netWorkType, GUILayout.Width(530));

        if (EditorApplication.isUpdating) {
            GUILayout.Label("更新中", GUILayout.Width(530));
        }

        if (!show || (!Application.isPlaying && !showInEditor)) {
            GUILayout.Label("ProfilerMemInfo", GUILayout.Width(530));
            uint monoUsed = Profiler.GetMonoUsedSize();
            uint monoSize = Profiler.GetMonoHeapSize();
            uint totalUsed = Profiler.GetTotalAllocatedMemory();
            uint totalSize = Profiler.GetTotalReservedMemory();
            string memsize = string.Format(
            "mono:{0}/{1} kb({2:f1}%)\n" +
            "total  :{3}/{4} kb({5:f1}%)\n",
            monoUsed / 1024, monoSize / 1024, 100.0 * monoUsed / monoSize,
            totalUsed / 1024, totalSize / 1024, 100.0 * totalUsed / totalSize);
            GUILayout.Label(memsize, GUILayout.Width(530));
            if (GUILayout.Button(filesize)) {
                filesize = AllAssetSize();
            }
        } else {
            int collCount = System.GC.CollectionCount(0);

            if (lastCollectNum != collCount) {
                lastCollectNum = collCount;
                delta = Time.realtimeSinceStartup - lastCollect;
                lastCollect = Time.realtimeSinceStartup;
                lastDeltaTime = Time.deltaTime;
                collectAlloc = allocMem;
            }

            allocMem = (int)System.GC.GetTotalMemory(false);

            peakAlloc = allocMem > peakAlloc ? allocMem : peakAlloc;

            if (Time.realtimeSinceStartup - lastAllocSet > 0.3F) {
                int diff = allocMem - lastAllocMemory;
                lastAllocMemory = allocMem;
                lastAllocSet = Time.realtimeSinceStartup;

                if (diff >= 0) {
                    allocRate = diff;
                }
            }

            StringBuilder text = new StringBuilder();
            text.Append("Currently allocated :");
            text.Append((allocMem / 1000000F).ToString("0"));
            text.Append("mb\n");
            text.Append("Peak allocated       :");
            text.Append((peakAlloc / 1000000F).ToString("0"));
            text.Append("mb (last collect:");
            text.Append((collectAlloc / 1000000F).ToString("0"));
            text.Append(" mb)\n");
            text.Append("Allocation rate       :");
            text.Append((allocRate / 1000000F).ToString("0.0"));
            text.Append("mb\n");
            text.Append("Collection frequency:");
            text.Append(delta.ToString("0.00"));
            text.Append("s\n");
            text.Append("Last collect delta  :");
            text.Append(lastDeltaTime.ToString("0.000"));
            text.Append("s (");
            text.Append(fps.ToString("0.0"));
            text.Append(" fps)");
            GUILayout.Label(text.ToString(), GUILayout.Width(530));

            GUI.Box(new Rect(5, 5, 380, 100 + (showFPS ? 16 : 0)), "");
            GUI.Label(new Rect(10, 5, 1000, 200), text.ToString());
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(">", GUILayout.Width(40))) {
            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        }
        if (GUILayout.Button("||", GUILayout.Width(40))) {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    public string AllAssetSize() {
        int size = 0;
        string[] pathes = AssetDatabase.GetAllAssetPaths();
        foreach (string s in pathes) {
            if (!File.Exists(s)) {
                continue;
            }
            var fileInfo = new FileInfo(s);
            if (fileInfo != null) {
                var fileSize = fileInfo.Length;
                size += (int)fileSize;
            }
        }
        var ws = GetFormatSizeString(size, 1024, "#,##0.##");
        return ws;
    }
    private static string GetFormatSizeString(int size, int p, string specifier) {
        var suffix = new[] { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
        int index = 0;
        while (size >= p) {
            size /= p;
            index++;
        }

        return string.Format(
            "{0}{1}B",
            size.ToString(specifier),
            index < suffix.Length ? suffix[index] : "-"
        );
    }

    public void SelectObjectFromID(int id) {
        var result = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).FirstOrDefault(s=>s.GetInstanceID()==id);
        List<GameObject> l = new List<GameObject>();
        l.Add((GameObject)result);
        Selection.objects = l.ToArray();
    }

    public void CreateAllPathGUIDProject() {
        if (_guids == null) {
            CashGUID.CreateScriptableObject();
        } else {
            _guids.filepath.Clear();
            _guids.guid.Clear();
            _guids.scene.Clear();
            _guids.cashpath.Clear();

            var guids = AssetDatabase.FindAssets("", new[] { "Assets" });
            foreach (var guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                _guids.filepath.Add(path);
                _guids.guid.Add(guid);
            }
        }
    }

    /// <summary>
    /// ヒエラルキーのキャッシュオブジェクト保存.
    /// </summary>
    public void CashHierarchyInstanceID() {

        if (Selection.objects.Length != 0) {
            var ids = new List<string>();
            foreach (UnityEngine.Object obj in Selection.objects) {
                ids.Add(obj.name+"/"+obj.GetInstanceID().ToString());
            }

            foreach (string i in ids) {
                if (!_guids.scene.Any(id => id == i)) {
                    _guids.scene.Add(i);
                }
            }
        }
    }

    /// <summary>
    /// プロジェクトのキャッシュオブジェクト保存.
    /// </summary>
    public void CashProjectFolderGUID() {
        if (Selection.objects.Length != 0) {
            var ids = new List<string>();
            foreach (UnityEngine.Object obj in Selection.objects) {
                var s = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                ids.Add(s);
                foreach (string i in ids) {
                    if (i.Length == 0) continue;
                    if (!_guids.cashpath.Any(id => id == i)) {
                        _guids.cashpath.Add(i);
                    }
                }
            }
        }
    }

    private string CheckObject(string typeName) {
        resultObjs.Clear();
        var builder = new StringBuilder();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
            if (!obj.name.Contains(typeName)) continue;
            if (isFullMatchName)
                if (!obj.name.Equals(typeName)) continue;

            resultObjs.Add(obj);
            builder.AppendLine(obj.name);
        }
        return builder.ToString();
    }

    /// <summary>
    /// Warningの出ているオブジェクト.
    /// </summary>
    /// <returns></returns>
    GameObject[] SelectWarningObject() {
        List<GameObject> _tmp = new List<GameObject>();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
            if (obj == null) {
                continue;
            }
            var isWarning = obj.GetComponents<MonoBehaviour>().Any(c => c == null);
            if (isWarning) {
                _tmp.Add(obj);
            }
        }
        return _tmp.ToArray();
    }

    /// <summary>
    /// コンポーネントの一覧.
    /// </summary>
    /// <returns></returns>
    private string CheckCompornent() {
        var builder = new StringBuilder();
        List<object> s = new List<object>();
        componentnames.Clear();

        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
            foreach (var component in obj.GetComponents<Component>()) {
                s.Add(component);
            }
        }
        MatchCollection mc;
        foreach (object obj in s) {

            if (obj == null) {
                Debug.LogError("Warning Object Find!!");
                Selection.objects = SelectWarningObject();
                break;
            }

            mc = Regex.Matches(obj.ToString(), @"\((\D+?)\)");
            if (mc.Count != 0) {
                var name = mc[0].ToString();
                name = name.Remove(name.Length - 1, 1);
                name = name.Remove(0, 1);
                if (name.IndexOf(".") != 0) {
                    var split = name.Split(new string[] { "." }, StringSplitOptions.None);
                    name = split[split.Length - 1];
                }

                if (!componentnames.Any(l => l == name)) {
                    componentnames.Add(name);
                    builder.AppendLine(name);
                }
            }
        }
        return builder.ToString();
    }

    public static Texture2D GetIconForFile(string fileName) {
        int num = fileName.LastIndexOf('.');
        string text = (num != -1) ? fileName.Substring(num + 1).ToLower() : string.Empty;
        string text2 = text;
        switch (text2) {
            case "boo":
                return EditorGUIUtility.FindTexture("boo Script Icon");
            case "cginc":
                return EditorGUIUtility.FindTexture("CGProgram Icon");
            case "cs":
                return EditorGUIUtility.FindTexture("cs Script Icon");
            case "guiskin":
                return EditorGUIUtility.FindTexture("GUISkin Icon");
            case "js":
                return EditorGUIUtility.FindTexture("Js Script Icon");
            case "mat":
                return EditorGUIUtility.FindTexture("Material Icon");
            case "prefab":
                return EditorGUIUtility.FindTexture("PrefabNormal Icon");
            case "shader":
                return EditorGUIUtility.FindTexture("Shader Icon");
            case "txt":
                return EditorGUIUtility.FindTexture("TextAsset Icon");
            case "unity":
                return EditorGUIUtility.FindTexture("SceneAsset Icon");
            case "prefs":
                return EditorGUIUtility.FindTexture("GameManager Icon");
            case "anim":
                return EditorGUIUtility.FindTexture("Animation Icon");
            case "meta":
                return EditorGUIUtility.FindTexture("MetaFile Icon");
            case "ttf":
            case "otf":
            case "fon":
            case "fnt":
                return EditorGUIUtility.FindTexture("Font Icon");
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
                return EditorGUIUtility.FindTexture("AudioClip Icon");
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
                return EditorGUIUtility.FindTexture("Texture Icon");
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
                return EditorGUIUtility.FindTexture("Mesh Icon");
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
                return EditorGUIUtility.FindTexture("MovieTexture Icon");
            case "colors":
            case "gradients":
            case "curves":
            case "curvesnormalized":
            case "particlecurves":
            case "particlecurvessigned":
            case "particledoublecurves":
            case "particledoublecurvessigned":
            case "asset":
                return EditorGUIUtility.FindTexture("ScriptableObject Icon");
            case "":
                return EditorGUIUtility.FindTexture("Folder Icon");
        }
        return EditorGUIUtility.FindTexture("DefaultAsset Icon");
    }
}