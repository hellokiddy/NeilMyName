using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Keedy.Base.IO;

public class BundleBuildTest {

    [MenuItem(TestSetting.TestMenu + "BundleTest")]
    static void TestBundle()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        string path = AssetDatabase.GetAssetPath(go);
        if (string.IsNullOrEmpty(path)) return;
        string[] deps = AssetDatabase.GetDependencies(path);
        BundleTools.BuildStart();
        for (int i = 0; i < deps.Length; i++)
        {
            if (deps[i].EndsWith(".mat")) continue;
            BundleTools.AddAssetBundleBuildToList(deps[i], FilePathHelper.GetFileNameWithoutExtension(deps[i]) + ".ab");
        }
        BundleTools.BuildEnd(GlobalSetting.c_ABRoot, BuildTarget.StandaloneWindows64, BuildAssetBundleOptions.ForceRebuildAssetBundle, true);
    }
}
