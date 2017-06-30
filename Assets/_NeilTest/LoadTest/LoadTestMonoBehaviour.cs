using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Load;
using LitJson;
using Keedy.Base.IO;
using UnityEngine.SceneManagement;
public class LoadTestMonoBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        mam = new LoadManager();
        mam.Init();

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if(loader != null)
        //{
        //    loader.Update();
        //}

        mam.Update(Time.deltaTime);
    }
    float timer = 0;
    void OnGUI()
    {
        if (GUILayout.Button("Start"))
        {
            //loader = new WWWLoader();
            //loader.Init(string.Format("file://" + Application.dataPath + "/_NeilTest/LoadTest/AB/cube.ab"), 10);
            //loader.Begin();
            // 
            string str;
            FileHelper.LoadFile(GlobalSetting.c_ConfFileRoot + "scene2.json", out str);
            source = JsonMapper.ToObject<SeceneSource>(str);

            //for(int i = 0; i < source.sceneDeps.Length; ++i)
            //{
            //    source.sceneDeps[i] = null;
            //}

            timer = Time.realtimeSinceStartup;
            task = mam.CreateLoadTask(source.sceneDeps, OnTaskComplete);
        }

        if (GUILayout.Button("Unload scene & ABs"))
        {
            SceneManager.UnloadSceneAsync(source.sceneName);
            for (int i = 0; i < abs.Count; ++i)
            {
                if (abs[i] == null) continue;
                abs[i].Unload(false);
            }
            abs.Clear();
        }

        if (GUILayout.Button("TestResouceLoader"))
        {

        }

        if (GUILayout.Button("TestResourcePath"))
        {
            //路径中有多个Resources时，相对于任何一个Resources的路径都可以
            //Shading/Shaders/PostProcess/Resources/
            Object obj = Resources.Load<Texture>("Shading/Shaders/PostProcess/Resources/Textures/VignetteMask");
            if (obj != null)
            {
                Debug.LogError(obj.name);
            }
            else
            {
                Debug.LogError("Load Failed...");
            }
        }
        GUILayout.Label((task == null ? 0 : task.Progress).ToString());


    }

    void OnTaskComplete2(ILoadAsset asset)
    {
        object data;
        string error = asset.TryGetAssetByName("Scenelight", out data);
        if (string.IsNullOrEmpty(error))
        {
            GameObject go = GameObject.Instantiate((Object)data) as GameObject;
            go.name = "check me!";
        }
        else
        {
            Debug.LogError(error);
        }
    }

    LoadTask task;
    LoadManager mam;
    SeceneSource source;
    IEnumerator StartWWW()
    {
        WWW www = new WWW(null);
        yield return www;
        Debug.LogError(www.error);
    }

    List<Object> caches = new List<Object>();
    List<AssetBundle> abs = new List<AssetBundle>();
    void AddToCaches(Object[] assets)
    {
        for (int i = 0; i < assets.Length; i++)
        {
            caches.Add(assets[i]);
        }
    }
    void OnTaskComplete(IAsset[] assets)
    {

        Debug.LogError("Use time:" + (Time.realtimeSinceStartup - timer).ToString());
        //for (int i = 0; i < source.sceneDeps.Length; i++)
        //{
        //    if (source.sceneDeps[i] == null || source.sceneDeps[i].Contains("unity")) continue;

        //    for(int j = 0; j < assets.Length; ++j)
        //    {
        //        if(assets[j].Name == source.sceneDeps[i])
        //        {
        //            //abs.Add(assets[j].LoadAsset)
        //        }
        //    }
            
        //}
        Debug.LogError("Task Complete...");

        SceneManager.LoadScene(source.sceneName);
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
                rens[i].sharedMaterial.shader = shader;
            }
        }
    }

}
