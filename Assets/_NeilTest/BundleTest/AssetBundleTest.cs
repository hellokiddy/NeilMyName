using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Load;
public class AssetBundleTest : MonoBehaviour {

    string[] paths = new string[]{
        "cube.ab",
        "win0.ab"
    };
    LoadManager mam = new LoadManager();

	// Use this for initialization
	void Start () {

        mam.Init();

	}

    void StartLoad()
    {
        mam.CreateLoadTask(paths, OnTaskComplete, 0, ELoaderType.WWW_LOADER);
    }

    AssetBundle cubeAB;
    AssetBundle picAB;
    GameObject go;
    void OnTaskComplete(ILoadAsset asset)
    {
        object obj;
        string error = asset.TryGetAssetByName("cube.ab", out obj);
        if (string.IsNullOrEmpty(error))
        {
            cubeAB = obj as AssetBundle;
        }
        else
        {
            Debug.LogError(error);
        }
        error = asset.TryGetAssetByName("win0.ab", out obj);
        if (string.IsNullOrEmpty(error))
        {
            picAB = obj as AssetBundle;
        }
        else
        {
            Debug.LogError(error);
        }
        
    }

    void CreateGo()
    {
        GameObject cube = cubeAB.LoadAsset("assets/_neiltest/bundletest/cube.prefab") as GameObject;
        //string[] assets = cubeAB.GetAllAssetNames();
        //for (int i = 0; i < assets.Length; ++i)
        //{
        //    Debug.LogError("assetsName:" + assets[i]);
        //}
        go = GameObject.Instantiate(cube);
    }
    //需要测试 mainAsset
	//mainAsset这个接口基本没用了啊

    void OnGUI()
    {
        if (GUILayout.Button("Start Load"))
        {
            StartLoad();
        }

        if (GUILayout.Button("Load Pic Asset"))
        {
            Object pic = picAB.LoadAllAssets()[0];
            Debug.LogError(pic.name);
        }

        if (GUILayout.Button("Create GO"))
        {
            CreateGo();
        }

        if (GUILayout.Button("Clear Bundles"))
        {
            picAB.Unload(false);
            cubeAB.Unload(false);
        }

        if (GUILayout.Button("Clear Bundles True"))
        {
            picAB.Unload(true);
            cubeAB.Unload(true);
        }

        
        if (GUILayout.Button("Clear Memorys"))
        {
            Destroy(go);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
	// Update is called once per frame
	void Update () {
        mam.Update(Time.deltaTime);
	}
}
