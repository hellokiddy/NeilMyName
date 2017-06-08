using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Load;

public class LoadTestMonoBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mam = new LoadManager();
        mam.Init();
	}
	
	// Update is called once per frame
	void Update () {
		if(loader != null)
        {
            loader.Update();
        }
	}
    WWWLoader loader;
    void OnGUI()
    {
        if(GUILayout.Button("Start"))
        {
            //loader = new WWWLoader();
            //loader.Init(string.Format("file://" + Application.dataPath + "/_NeilTest/LoadTest/AB/cube.ab"), 10);
            //loader.Begin();
            // 
            state = mam.CreateLoadTask("", null, 0, ELoaderType.WWW_LOADER);
        }

        GUILayout.Label((state == null?0:state.LoadProcess).ToString());
    }

    ILoadState state;
    LoadManager mam;
    IEnumerator StartWWW()
    {
        WWW www = new WWW(null);
        yield return www;
        Debug.LogError(www.error);
    }
    
    void OnTaskComplete(ILoadAsset asset)
    {
        Debug.LogError("Loading is completed....");
        
    }
}
