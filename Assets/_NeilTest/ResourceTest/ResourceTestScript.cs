using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTestScript : MonoBehaviour
{

    LoadManager m_LoadMgr;
    GameObject m_Cube;
    Material m_Mat;

    GameObject m_InitiateCube;
    Object m_OrignalObj;
    IAsset m_CubeAsset;

    string m_CubeName = "cube.ab";

    void Start()
    {
        m_LoadMgr = new LoadManager();
        m_LoadMgr.Init();
    }

    void Update()
    {
        m_LoadMgr.Update(Time.deltaTime);
    }
    string m_TextName = "fuck.png";
    void OnGUI()
    {
        m_TextName = GUILayout.TextField(m_TextName);

        if(GUILayout.Button("Init Cube"))
        {
            m_Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_Mat = m_Cube.GetComponent<MeshRenderer>().material;
            m_Cube.GetComponent<MeshRenderer>().material = m_Mat;
        }

        if (GUILayout.Button("Load AB"))
        {
            m_LoadMgr.CreateLoadTask(("testpic.ab"), OnLoadComplete);
        }

        if (GUILayout.Button("Load Pic"))
        {
            m_Tex = m_Asset.LoadAsset(m_TextName) as Texture2D;
            m_Mat.mainTexture = m_Tex;

            //Object obj = m_Asset.LoadAsset(m_TextName);
            //GameObject go = GameObject.Instantiate(obj) as GameObject;
            //go.name = "hello";
        }

        if(GUILayout.Button("UnLoad AB"))
        {
            m_Asset.Dispose();
        }

        if (GUILayout.Button("Clear Pic"))
        {
            m_Tex = null;
        }

        if (GUILayout.Button("Clear Mat"))
        {
            GameObject.DestroyImmediate(m_Mat);
        }

        if (GUILayout.Button("Clear mem"))
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        GUILayout.Label("-------------------------------------------------");

        if(GUILayout.Button("Load Prefab"))
        {
            m_LoadMgr.CreateLoadTask((m_CubeName), OnLoadPrefabComplete);
                
        }

        if(GUILayout.Button("Initiate Cube"))
        {
            m_OrignalObj = m_CubeAsset.LoadAsset("cube");
            m_InitiateCube = GameObject.Instantiate(m_OrignalObj) as GameObject;
            m_InitiateCube.name = "Hello World!";
        }

        if(GUILayout.Button("UnLoad cube ab"))
        {
            m_CubeAsset.Dispose(true);
        }
        
        if(GUILayout.Button("Destory Orignal"))
        {
            GameObject.DestroyImmediate(m_OrignalObj, true);
        }

        if(GUILayout.Button("Destroy InitiateObj"))
        {
            DestroyImmediate(m_InitiateCube, true);
        }
    }

    IAsset m_Asset;
    Texture2D m_Tex;
    void OnLoadComplete(IAsset[] assets)
    {
        m_Asset = assets[0];
        //string[] names = m_Asset.m_AssetBudle.GetAllAssetNames();
        //for (int i = 0; i < names.Length; ++i)
        //{
        //    Debug.LogError(names[i]);
        //}
        //Debug.LogError(m_Asset.MainAsset.name);
        Debug.LogError("Load Complete");
    }

    void OnLoadPrefabComplete(IAsset[] assets)
    {
        m_CubeAsset = assets[0];
    }





    //void Start()
    //{
    //    StartCoroutine(Load());
    //}

    AssetBundle ab;
    //void OnGUI()
    //{
    //    if (GUILayout.Button("Init"))
    //    {
    //        Object obj = ab.LoadAsset("cube.prefab");
    //        Debug.LogError(obj == null);
    //        GameObject go = GameObject.Instantiate(obj) as GameObject;
    //        go.name = "fuck";
    //    }
    //}
    IEnumerator Load()
    {
        WWW www = new WWW(ConfigPathManager.GetAbsolutePath("testpic.ab"));
        yield return www;

        ab = www.assetBundle;
        //Object obj = ab.LoadAsset("cube.prefab");
        //GameObject go = GameObject.Instantiate(obj) as GameObject;
        //go.name = "Fuck";
        //Debug.LogError(obj == null);
        www.Dispose();
    }
}
