using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Keedy.Base.IO;
using System.IO;
using System.Reflection;
using System;

public static class TestSetting
{
    public const string TestMenu = "Neil/Test/";
}
public class IOTestEditor : EditorWindow
{
    [MenuItem("Neil/Test/OpenIOTestWindow", false, 1)]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<IOTestEditor>();
    }

    Type[] myClasses = null;
    MethodInfo currentSelect = null;
    string methodName = null;
    ParameterInfo[] paraInfos = null;
    string[] inputs = new string[0];
    object[] inputParams = new object[0];
    Vector2 _scrollPos = Vector2.zero;
    List<int> outPuts = new List<int>();
    void OnGUI()
    {   
        if(currentSelect == null){
            methodName = null;
            inputParams = new object[0];
            paraInfos = null;
            inputs = new string[0];
            outPuts.Clear();
        }
        else if(methodName != currentSelect.Name){
            methodName = currentSelect.Name;
            paraInfos = currentSelect.GetParameters();
            inputParams = new object[paraInfos.Length];
            inputs = new string[paraInfos.Length];
            CheckOutPut();
            //当前的输入框会保存上次的编辑结果，把选中控件置空，可以清除缓存
            GUI.FocusControl("");
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField("SelectMethod:", methodName);
        if (GUILayout.Button("ReLoadMyClasses"))
        {
            LoadClasses();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical("box");
        GUILayout.Label("Input Parameters:");
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = EditorGUILayout.TextField(new GUIContent(paraInfos[i].Name), inputs[i] == null?"":inputs[i]);
        }
        if (GUILayout.Button("InvokeSelectMethod...."))
        {
            InvokeMethod();
        }
        GUILayout.EndVertical();
        if (GUI.changed)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == null || paraInfos[i].IsOut)
                {
                    //inputParams[i] = null;
                }
                else if(paraInfos[i].ParameterType == typeof(byte[])){
                    inputParams[i] = System.Text.Encoding.UTF8.GetBytes(inputs[i]);
                }
                else
                {
                    try
                    {
                        inputParams[i] = Convert.ChangeType(inputs[i], paraInfos[i].ParameterType);
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                //Debug.LogError(inputs[i] + "    " + paraInfos[i].ParameterType);
                //Debug.LogError(inputParams[i]);
            }

            GUI.changed = false;
        }
        GUILayout.Space(20);
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        if (myClasses != null)
        {
            for (int i = 0; i < myClasses.Length; i++)
            {
                DrawClass(myClasses[i]);
            }
        }
        GUILayout.EndScrollView();

    }

    void CheckOutPut()
    {
        outPuts.Clear();
        if (paraInfos != null)
        {
            for (int i = 0; i < paraInfos.Length; i++)
            {
                if (paraInfos[i].IsOut)
                {
                    outPuts.Add(i);
                }
            }
        }
    }
    void LoadClasses()
    {
        myClasses = LoadClassFromFile(@"E:\GitProject\NeilMyName\Assets\_NeilTest\IOTest\Editor\MyClasses.txt");
    }
    Type[] LoadClassFromFile(string path)
    {
        string assemblyName = ReadLineFromFile(path, 1);
        if (string.IsNullOrEmpty(assemblyName))
        {
            return null;
        }
        else
        {
            List<Type> classes = new List<Type>();
            Assembly assembly = Assembly.Load(assemblyName);
            if (null != assembly)
            {
                string line2 = ReadLineFromFile(path, 2);
                string[] strs = line2.Split(new char[]{' '});
                for (int i = 0; i < strs.Length; i++)
                {
                    Type t = assembly.GetType(strs[i]);
                    if(t != null)
                    {
                        classes.Add(t);
                    }
                }
            }
            return classes.ToArray();
        }
    }

    void DrawClass(Type type)
    {
        if (type == null) return;
        GUILayout.BeginVertical("box");
        GUILayout.Label(type.Name + ":");
        MethodInfo[] methods = type.GetMethods();
        for (int i = 0; i < methods.Length; i++)
        {
            if (methods[i].IsConstructor) continue;
            if (GUILayout.Button(methods[i].Name))
            {
                currentSelect = methods[i];
            }
        }
        GUILayout.EndVertical();
        GUILayout.Space(10f);
    }

    void InvokeMethod()
    {
        if (currentSelect == null)
        {
            Debug.LogError("Please select a method...");
            return;
        }
        object ret = currentSelect.Invoke(null, inputParams);
        Debug.LogError("Result:" + ret);
        for (int i = 0; i < outPuts.Count; i++)
        {
            if (inputParams[outPuts[i]] == null) continue;
            if((inputParams[outPuts[i]]).GetType() == typeof(Byte[]))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                Byte[] bytes = inputParams[outPuts[i]] as Byte[];
                if (bytes != null)
                {
                    for (int index = 0; index < bytes.Length; index++)
                    {
                        sb.Append(bytes[index].ToString());
                    }
                }
                Debug.LogError(sb.ToString());
            }
            else if (inputParams[outPuts[i]].GetType() == typeof(string[]))
            {
                string[] strs = inputParams[outPuts[i]] as string[];
                for (int index = 0; index < strs.Length; index++)
                {
                    Debug.LogError(strs[index]);
                }
            }
            else
            {
                Debug.LogError(inputParams[outPuts[i]]);
            }
        }
    }

    //[MenuItem("Neil/Test/IOTest")]
    static void TestIO()
    {
        Assembly assembly = Assembly.Load("Assembly-CSharp");
        Type type =  assembly.GetType("Keedy.Base.IO.FileHelper");
        Debug.LogError(type.FullName);
        MethodInfo[] methods = type.GetMethods();
        Debug.LogError(methods[0].Name);
        ParameterInfo[] paramInfos = methods[0].GetParameters();
        Debug.LogError(paramInfos.Length);
        object ret = methods[0].Invoke(null, new string[1]{@"E:\GitProject\NeilMyName\Assets\_NeilTest\IOTest\Editor\Trust.txt"});
        Debug.LogError(ret);
    }

    static string ReadLineFromFile(string path, int column)
    {
        //string filePath = Application.dataPath + "/_NeilTest/IOTest/Editor/Trust.txt";

        if (FileHelper.Exists(path) == false) return string.Empty;
        using (StreamReader sr = new StreamReader(path))
        {
            for (int i = 1; i <= column && !sr.EndOfStream; i++)
            {
                string str = sr.ReadLine();
                if (i == column)
                {
                    //Debug.LogError("ReadFromFileLine:" + i + "   " + str);
                    return str;
                }
            }
        }
        return string.Empty;
    }
}
