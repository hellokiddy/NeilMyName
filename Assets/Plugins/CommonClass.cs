using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonClass {

}

public static class GlobalSetting
{
    //test code...
    public const string c_ABRoot = @"E:\GitProject\NeilMyName\DataPC\";
    public const string c_ABRootWWW = "file://E:/GitProject/NeilMyName/DataPC/";
    public const string c_ConfFileRoot = "E:/GitProject/NeilMyName/ConfigFile/";

    public static string GetWWWPath(string path)
    {
        return c_ABRootWWW + path;
    }

    public static string GetResourcePath(string path)
    {
        int index = path.IndexOf("/Resources/");
        return path.Substring(index + 11);
    }

    public static string FormatJson(string json)
    {
        return json.Replace("\",\"", "\",\n\"");
    }
}

public class SeceneSource
{
    public string sceneName;
    public string[] sceneDeps;
}
