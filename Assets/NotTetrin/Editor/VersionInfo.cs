using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class VersionInfo : MonoBehaviour {
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        float versionFloat;

        if (float.TryParse(PlayerSettings.bundleVersion, out versionFloat)) {
            versionFloat += 0.01f;
            PlayerSettings.bundleVersion = versionFloat.ToString();
        }
    }
}
