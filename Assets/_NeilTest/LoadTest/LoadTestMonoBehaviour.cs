using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Load;

public class LoadTestMonoBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		

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
            loader = new WWWLoader();
            loader.Init(string.Format("file://" + Application.dataPath + "/_NeilTest/LoadTest/AB/cube.ab"), 10);
            loader.Begin();
        }
    }
}
