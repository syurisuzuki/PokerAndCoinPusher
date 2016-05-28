using UnityEditor;
using UnityEngine;
using System.Linq;

public class CommandBuild {

    private static string[] GetAllScenePaths() {
        return EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
    }

    public static void BuildAndroid() {

        string errorMessage = BuildPipeline.BuildPlayer(
                GetAllScenePaths(),
                Application.dataPath + "/../commandbuild.apk",
                BuildTarget.Android,
                BuildOptions.None           
        );

        if (!string.IsNullOrEmpty(errorMessage))
            Debug.LogError("[Error!] " + errorMessage);
        else
            Debug.Log("[Success!]");

        EditorApplication.Exit(string.IsNullOrEmpty(errorMessage) ? 0 : 1);
    }

    public static void BuildiOS() {

        //ProjectDataSize Reduction
        EditorUserBuildSettings.symlinkLibraries = true;
        //Prevent SimulatorBuild
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;

        string errorMessage = BuildPipeline.BuildPlayer(
                GetAllScenePaths(),
                "Device",   
                BuildTarget.iOS,
                BuildOptions.SymlinkLibraries
        );

        if (!string.IsNullOrEmpty(errorMessage))
            Debug.LogError("[Error!] " + errorMessage);
        else
            Debug.Log("[Success!]");

        EditorApplication.Exit(string.IsNullOrEmpty(errorMessage) ? 0 : 1);
    }

    /*public static void BuildAndroidAndiOS() {
        
        string errorMessage = BuildPipeline.BuildPlayer(
                GetAllScenePaths(),
                Application.dataPath + "/../commandbuild.apk",
                BuildTarget.Android,
                BuildOptions.None
        );

        if (!string.IsNullOrEmpty(errorMessage))
            Debug.LogError("[Error!] " + errorMessage);
        else
            Debug.Log("[Success!]");

        //ProjectDataSize Reduction
        EditorUserBuildSettings.symlinkLibraries = true;
        //Prevent SimulatorBuild
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;

        errorMessage = BuildPipeline.BuildPlayer(
                GetAllScenePaths(),
                "Device",
                BuildTarget.iOS,
                BuildOptions.SymlinkLibraries
        );

        if (!string.IsNullOrEmpty(errorMessage))
            Debug.LogError("[Error!] " + errorMessage);
        else
            Debug.Log("[Success!]");

        EditorApplication.Exit(string.IsNullOrEmpty(errorMessage) ? 0 : 1);
    }*/
}
