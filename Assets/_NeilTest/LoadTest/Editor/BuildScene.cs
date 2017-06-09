using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using LitJson;
using Keedy.Base.IO;
public class BuildScene 
{
    [MenuItem(TestSetting.TestMenu + "BuildScene")]
    static void BuildAll()
    {
        Scene scene = SceneManager.GetActiveScene();
        string[] deps = AssetDatabase.GetDependencies(scene.path);
        SeceneSource source = new SeceneSource();
        source.sceneName = scene.name;
        source.sceneDeps = GetPrefabPath(scene);

        string data = JsonMapper.ToJson(source);
        data = GlobalSetting.FormatJson(data);
        Keedy.Base.IO.FileHelper.SaveFile(GlobalSetting.c_ConfFileRoot+"scene1.json", data);

        SeceneSource source2 = new SeceneSource();
        source2.sceneName = scene.name;
        source2.sceneDeps = new string[deps.Length];

        BundleTools.BuildStart();
        for (int i = 0; i < deps.Length; i++)
        {
            //if (deps[i].EndsWith(".png") == false && deps[i].EndsWith(".jpg") == false && deps[i].EndsWith(".unity") == false) continue;
            source2.sceneDeps[i] = Keedy.Base.IO.FilePathHelper.GetFileNameWithoutExtension(deps[i]) + "_" + FilePathHelper.GetFileExtension(deps[i]).Substring(1) + ".ab";
            BundleTools.AddAssetBundleBuildToList(deps[i], source2.sceneDeps[i]);
        }
        string data2 = JsonMapper.ToJson(source2);
        data2 = GlobalSetting.FormatJson(data2);
        Keedy.Base.IO.FileHelper.SaveFile(GlobalSetting.c_ConfFileRoot+"scene2.json", data2);
        BundleTools.BuildEnd(GlobalSetting.c_ABRoot, BuildTarget.StandaloneWindows64, BuildAssetBundleOptions.CompleteAssets, true);
    }

    static string[] GetPrefabPath(Scene scene)
    {
        GameObject[] gos = scene.GetRootGameObjects();
        List<string> prefaPath = new List<string>();
        for (int i = 0; i < gos.Length; i++)
        {
            FindPrefaPath(gos[i].transform, prefaPath);
        }
        return prefaPath.ToArray();
    }

    static void FindPrefaPath(Transform trans, List<string> prefabPath)
    {
        Object prefab = PrefabUtility.GetPrefabParent(trans);
        if (prefab != null)
        {
            prefabPath.Add(AssetDatabase.GetAssetPath(prefab));
        }
        else
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                FindPrefaPath(trans.GetChild(i), prefabPath);
            }
        }
    }
}

