using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StringTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StringTestFunc();

        PrintTime(TestUnBox);

        PrintTime(TestInherit);

        PrintTime(TestHello);

        PrintTime(TestGo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    string str1 = "Hello,World!";
    string str2 = "Hello,World!";

    string str3 = string.Empty;
    string str4 = "";

    unsafe void StringTestFunc()
    {
        //Debug.LogError((int)&str1);
        fixed(char *p = str1)
        {
            Debug.LogError((int)p);
        }
        fixed(char *p = str2)
        {
            Debug.LogError((int)p);
        }
        fixed (char* p = str3)
        {
            Debug.LogError((int)p);
        }
        fixed (char* p = str4)
        {
            Debug.LogError((int)p);
        }
    }

    void PrintTime(Action action)
    {
        float time = Time.realtimeSinceStartup;
        action();
        Debug.LogError("Time Used:" + (Time.realtimeSinceStartup - time));
    }

    List<int> intList = new List<int>();
    List<Hello> helloList = new List<Hello>();
    List<GameObject> goList = new List<GameObject>();

    void TestUnBox()
    {
        int hi = 100;

        for (int i = 0; i < 100000; i++)
        {
            AddToIntList(hi);
        }
    }

    void AddToIntList(object obj)
    {
        int a = (int)obj;
        intList.Add(a);
    }

    void TestInherit()
    {
        Hello hi = new Hello();
        for (int i = 0; i < 100000; i++)
        {
            AddToHelloList((object)hi);
        }
    }

    void TestHello()
    {
        Hello hi = new Hello();
        for (int i = 0; i < 100000; i++)
        {
            AddToHelloList(hi);
        }
    }

    void AddToHelloList(object obj)
    {
        Hello hello = obj as Hello;
        helloList.Add(hello);
    }

    void AddToHelloList(Hello obj)
    {
        Hello hi = obj;
        helloList.Add(hi);
    }

    void TestGo()
    {
        GameObject go = new GameObject();
        for(int i = 0;i < 100000; i++)
        {
            AddToGoList((object)go);
        }
    }

    void AddToGoList(object obj)
    {
        GameObject go = obj as GameObject;
        goList.Add(go);
    }
    class Hello
    {
        public int a;
        public Hello()
        {
            a = 100;
        }
    }
}
