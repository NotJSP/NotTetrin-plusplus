using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class VersionInfo : MonoBehaviour {
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        var appVersion = PlayerSettings.bundleVersion;
        var versions = appVersion.Split('#');
        var version = versions[0];
        var buildNumberString = versions[1];
        int buildNumber;
        if (int.TryParse(buildNumberString, out buildNumber)) {
            buildNumber++;
            PlayerSettings.bundleVersion = version + '#' + buildNumber.ToString();
        } else {
            throw new SystemException(@"バージョンの書式が不正です。");
        }
    }
}
