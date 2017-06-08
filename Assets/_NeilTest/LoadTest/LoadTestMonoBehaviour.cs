using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Load;
using LitJson;
using Keedy.Base.IO;
using UnityEngine.SceneManagement;
public class LoadTestMonoBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mam = new LoadManager();
        mam.Init();

        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        //if(loader != null)
        //{
        //    loader.Update();
        //}

        mam.Update(Time.deltaTime);
	}
    void OnGUI()
    {
        if(GUILayout.Button("Start"))
        {
            //loader = new WWWLoader();
            //loader.Init(string.Format("file://" + Application.dataPath + "/_NeilTest/LoadTest/AB/cube.ab"), 10);
            //loader.Begin();
            // 
            string str;
            FileHelper.LoadFile(Application.dataPath + "/_NeilTest/LoadTest/SceneData2.json", out str);
            source = JsonMapper.ToObject<SeceneSource>(str);

            state = mam.CreateLoadTask(source.sceneDeps, OnTaskComplete, 0, ELoaderType.WWW_LOADER);
        }

        if(GUILayout.Button("Replace"))
        {
            replaceShader();
        }

        GUILayout.Label((state == null?0:state.LoadProcess).ToString());
    }

    ILoadState state;
    LoadManager mam;
    SeceneSource source;
    IEnumerator StartWWW()
    {
        WWW www = new WWW(null);
        yield return www;
        Debug.LogError(www.error);
    }

    List<Object> caches = new List<Object>();
    void AddToCaches(Object[] assets)
    {
        for (int i = 0; i < assets.Length; i++)
        {
            caches.Add(assets[i]);
        }
    }
    void OnTaskComplete(ILoadAsset asset)
    {
        for (int i = 0; i < source.sceneDeps.Length; i++)
        {
            if (source.sceneDeps[i].Contains("unity")) continue;

            object ab;
            string error = (asset.TryGetAssetByName(source.sceneDeps[i], out ab));
            if (string.IsNullOrEmpty(error))
            {
                AssetBundle ab2 = ab as AssetBundle;
                if(ab2 != null){
                    AddToCaches(ab2.LoadAllAssets());
                    ab2.Unload(false);
                }
            }
            else{
                Debug.LogError(error);
            }
        }
        Debug.LogError("Task Complete...");

        UnityEngine.SceneManagement.SceneManager.LoadScene(source.sceneName);

        GameObject go = new GameObject("hello world");
        go.AddComponent<Camera>();
    }

    void replaceShader()
    {
        Debug.LogError(SceneManager.GetActiveScene().name);

        Renderer[] rens = GameObject.FindObjectsOfType<Renderer>();
        //Debug.LogError(rens.Length);
        Shader shader = Shader.Find("Spark/Ground/SimpleGroundBumped");
        //Debug.LogError(shader != null);
        for (int i = 0; i < rens.Length; i++)
        {
            if (rens[i].sharedMaterial != null)
            {
                //rens[i].sharedMaterial.shader = shader;
            }
        }
    }

}
