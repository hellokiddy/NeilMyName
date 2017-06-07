using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.IO;
public class BundleTools 
{
    public class ABFileInfo
    {
        /// <summary>
        /// 即ab文件根目录下路径
        /// </summary>
        public string bundleName;
        public UInt32 CRC;
        public Int64 size;

        public override string ToString()
        {
            return string.Format("[BundleName:{0}]   [CRC:{1}]   [size:{2}kb]", bundleName, CRC.ToString(), (size/1024).ToString());
        }
    }
    [MenuItem("Test/Bundle", true)]
    static bool CheckAsset()
    {
        GameObject go = Selection.activeGameObject;
        return go != null && PrefabUtility.GetPrefabParent(go) != null;
    }


    [MenuItem("Test/Bundle")]
    static void BuildBundle()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogError("Please select a gameobject in the scene....");
        }
        else
        {
            Object obj = PrefabUtility.GetPrefabParent(go);
            if(obj == null)
            {
                Debug.LogError("Make sure the gameobject selected has a prefab....");
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(obj);
                string[] deps = AssetDatabase.GetDependencies(path);
                //准备打包
                BuildStart();
                for(int i = 0; i < deps.Length; i++)
                {
                    string dep = deps[i];
                    
                    if(Path.GetExtension(dep).ToLower() == ".png" || Path.GetExtension(dep).ToLower() == ".prefab")
                    {
                        //配置好准备打包的资源，分配好Bundle名，添加到列表中
                        AddAssetBundleBuildToList(dep, Path.GetFileNameWithoutExtension(dep) + ".ab");
                    }
                }
                //最终打包，并输出ab文件信息
                List<ABFileInfo> list = BuildEnd(Application.dataPath+"/_NeilTest/LoadTest/AB",BuildTarget.StandaloneWindows64, BuildAssetBundleOptions.None, true);
                for(int i = 0, cout = list.Count; i<cout;i++)
                {
                    Debug.LogError(list[i].ToString());
                }
            }
        }
    }



    private static List<AssetBundleBuild> mAssetBundleBuildList;

    /// <summary>
    /// 打包顺序:
    /// 1.BuildStart
    /// 2.AddAssetBundleBuildToList
    /// 3.BuildEnd
    /// </summary>
    public static void BuildStart()
    {
        if(mAssetBundleBuildList == null)
        {
            mAssetBundleBuildList = new List<AssetBundleBuild>();
        }
        else
        {
            mAssetBundleBuildList.Clear();
        }
    }

    /// <summary>
    /// 打包顺序:
    /// 1.BuildStart
    /// 2.AddAssetBundleBuildToList
    /// 3.BuildEnd
    /// outputPath:ab包文件的根目录
    /// </summary>
    static AssetBundleManifest BuildAssetBundles(string outputPath, BuildAssetBundleOptions option, BuildTarget platform)
    {
        AssetBundleManifest manifest = null;
        if(mAssetBundleBuildList == null || mAssetBundleBuildList.Count == 0)
        {
            Debug.LogError("Here find no asset to build...");
        }
        else
        {
            if(Directory.Exists(outputPath) == false)
            {
                Directory.CreateDirectory(outputPath);
            }
            manifest = BuildPipeline.BuildAssetBundles(outputPath, mAssetBundleBuildList.ToArray(), option, platform);
        }
        return manifest;
    }
    /// <summary>
    /// 打包顺序:
    /// 1.BuildStart
    /// 2.AddAssetBundleBuildToList
    /// 3.BuildEnd
    /// outputPath:ab包文件的根目录
    /// </summary>
    public static List<ABFileInfo> BuildEnd(string outputPath, BuildTarget platform, BuildAssetBundleOptions option = BuildAssetBundleOptions.None, bool deleteManifest = false)
    {
        AssetBundleManifest mani = BuildAssetBundles(outputPath, option, platform);
        List<ABFileInfo> infoList = new List<ABFileInfo>();
        if (mani != null)
        {
            for (int i = 0; i < mAssetBundleBuildList.Count; i++)
            {
                ABFileInfo info = new ABFileInfo();

                info.bundleName = mAssetBundleBuildList[i].assetBundleName;
                string filePath = string.Format("{0}/{1}", outputPath, info.bundleName);
                if (File.Exists(filePath))
                {
                    BuildPipeline.GetCRCForAssetBundle(filePath, out info.CRC);
                    info.size = GetFileSize(filePath);
                    //Debug.LogError(info.ToString());
                }
                else
                {
                    Debug.LogError("Can't find assetbundle file : " + filePath);
                }
                infoList.Add(info);

                if (deleteManifest == true)
                {
                    string maniPath = string.Format("{0}{1}", filePath, ".manifest");
                    DeleteFile(maniPath);
                }
            }

            if(deleteManifest == true)
            {
                string abFolderName = Path.GetFileName(outputPath);
                string abFilePath = string.Format("{0}/{1}", outputPath, abFolderName);
                string abManiPath = string.Format("{0}{1}", abFilePath, ".manifest");
                DeleteFile(abFilePath);
                DeleteFile(abManiPath);
            }
        }

        return infoList;
    }

    public static Int64 GetFileSize(string path)
    {
        if(string.IsNullOrEmpty(path) || File.Exists(path) == false)
        {
            return 0;
        }
        else
        {
            FileInfo info = new FileInfo(path);
            if (info != null)
            {
                return info.Length;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 打包顺序:
    /// 1.BuildStart
    /// 2.AddAssetBundleBuildToList
    /// 3.BuildEnd
    /// assetPath:资源路径
    /// bundleName:ab包存放的相对路径(相对于最终输出的根目录)
    /// </summary>
    public static void AddAssetBundleBuildToList(string assetPath, string bundleName)
    {
        AddAssetBundleBuildToList(new string[] { assetPath }, bundleName);
    }

    /// <summary>
    /// 打包顺序:
    /// 1.BuildStart
    /// 2.AddAssetBundleBuildToList
    /// 3.BuildEnd
    /// assetPaths:资源路径
    /// bundleName:ab包存放的相对路径(相对于最终输出的根目录)
    /// </summary>
    public static void AddAssetBundleBuildToList(string[] assetPaths, string bundleName)
    {
        if (assetPaths == null || assetPaths.Length == 0 || string.IsNullOrEmpty(bundleName))
        {
            Debug.LogWarning("here comes wrong assetBundleBuild, please check it ... [BUNDLENAME:" + bundleName + "]");
            return;
        }
        if (mAssetBundleBuildList == null)
        {
            mAssetBundleBuildList = new List<AssetBundleBuild>();
        }

        AssetBundleBuild build = new AssetBundleBuild();
        bool isBundleNameExist = false;

        foreach (AssetBundleBuild tmpBuild in mAssetBundleBuildList)
        {
            if (tmpBuild.assetBundleName == bundleName)
            {
                build = tmpBuild;
                isBundleNameExist = true;
                break;
            }
        }

        if (isBundleNameExist == false)
        {
            build.assetBundleName = bundleName;
            build.assetNames = assetPaths;
            //build.assetBundleVariant = variant;
            mAssetBundleBuildList.Add(build);
        }
        else
        {
            List<string> assetNames = new List<string>();
            foreach (string path in build.assetNames)
            {
                assetNames.Add(path);
            }

            foreach (string path in assetPaths)
            {
                if (assetNames.Contains(path) == false)
                {
                    assetNames.Add(path);
                }
            }
        }
    }

    static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogWarning(string.Format("Can't find [{0}] to delete, you may need check it!", filePath));
        }
    }
}
