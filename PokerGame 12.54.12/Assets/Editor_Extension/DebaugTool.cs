using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;
using System.Text;



public class DebaugTool : MonoBehaviour {

    public float time;

    private AndroidJavaObject androidJavaObject = null;
    private long memsize;
    private long maxsize;

    private string currentMemInfo;

    

    public string netWorkType {
        get {
            switch (Application.internetReachability) {
                case NetworkReachability.NotReachable:
                    return "NetWorkStatus:ネットワーク未接続";
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return "NetWorkStatus:キャリア接続";
                case NetworkReachability.ReachableViaLocalAreaNetwork:
#if UNITY_ANDROID || UNITY_IOS
                    return "NetWorkStatus:Wi-fi接続";
#else
                    return "NetWorkStatus:接続";
#endif
                default:
                    return "NetWorkStatus:Unknown";
            }
        }
    }

    public string platform {
        get {
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    return "Platform:Android";
                case RuntimePlatform.IPhonePlayer:
                    return "Platform:iOS";
                default:
                    return "Platform:Others";
            }
        }
    }



    public bool show = true;
    public bool showFPS = true;
    public bool showInEditor = false;
    int frameCount;
    float prevTime;
    float fps;
    public void Start() {
#if !UNITY_EDITOR && UNITY_ANDROID
        androidJavaObject = new AndroidJavaObject("unityplugin.android.memoryinfo.MemoryInfo");
#endif
        
        useGUILayout = false;
    }

    public string UpdateMemoryInfo() {
        memsize = androidJavaObject.CallStatic<long>("getUsedMemorySize");
        maxsize = androidJavaObject.CallStatic<long>("totalMemorySize");
        currentMemInfo = memsize / 1024 + "MB/" + maxsize / 1024 / 1024 + "MB";
        return currentMemInfo;
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

    // Use this for initialization
    public void OnGUI() {
        if (!show || (!Application.isPlaying && !showInEditor)) {
            return;
        }

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
        text.Append(netWorkType);
        text.Append("\n");
#if !UNITY_EDITOR && UNITY_ANDROID
        text.Append(UpdateMemoryInfo());
        text.Append("\n");
#endif

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

        if (showFPS) {
            //text.Append("\n" + fps.ToString("0.0") + " fps");
        }

        

        //Debug.Log("mem: " + memsize);

        GUI.Box(new Rect(5, 5, 380, 100 + (showFPS ? 16 : 0)), "");
        GUI.Label(new Rect(10, 5, 1000, 200), text.ToString());
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

}
