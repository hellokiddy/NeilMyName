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

    //[MenuItem(TestSetting.TestMenu + "CharacterBundleTest")]
    //static void TestCharBundle()
    //{
    //    Debug.LogError("the method is empty now");
    //    return;

    //    GameObject go = Selection.activeGameObject;
    //    if (go == null) return;
    //    string path = AssetDatabase.GetAssetPath(go);
    //    if (string.IsNullOrEmpty(path)) return;
    //    string[] deps = AssetDatabase.GetDependencies(path);
    //    BundleTools.BuildStart();
    //    GameObject insGo = GameObject.Instantiate(go) as GameObject;
    //    SkinnedMeshRenderer[] renders = insGo.GetComponentsInChildren<SkinnedMeshRenderer>();
    //    for(int i = 0; i < renders.Length; ++i)
    //    {
    //        SkinnedMeshRenderer ren = renders[i];
    //        BoneIndex bones = ren.gameObject.AddComponent<BoneIndex>();
    //        Transform[] trans = ren.bones;

    //    }

    //    BundleTools.BuildEnd(GlobalSetting.c_ABRoot, BuildTarget.StandaloneWindows64);
    //} 

    [MenuItem(TestSetting.TestMenu + "TestMem")]
    static void TestMem()
    {
        Object obj = Selection.activeObject;
        if (obj == null) return;
        string path = AssetDatabase.GetAssetPath(obj);
        BundleTools.BuildStart();
        BundleTools.AddAssetBundleBuildToList(path, "testPic.ab");
        string path2 = path.Substring(0, path.Length - 4) + "2.png";
        Debug.LogError(path2);
        BundleTools.AddAssetBundleBuildToList(path2, "testPic.ab");
        BundleTools.BuildEnd(GlobalSetting.c_ABRoot, BuildTarget.StandaloneWindows64, BuildAssetBundleOptions.ForceRebuildAssetBundle);
    }

    [MenuItem(TestSetting.TestMenu + "BundleSingle")]
    static void BundleSingle()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;
        string path = AssetDatabase.GetAssetPath(go);
        if (string.IsNullOrEmpty(path)) return;
        BundleTools.BuildStart();
        string name = Keedy.Base.IO.FilePathHelper.GetFileNameWithoutExtension(path) + ".ab";
        BundleTools.AddAssetBundleBuildToList(path, name);
        BundleTools.BuildEnd(GlobalSetting.c_ABRoot, BuildTarget.StandaloneWindows64);
    }
}
