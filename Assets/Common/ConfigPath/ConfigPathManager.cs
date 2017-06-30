using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigPathManager
{
    public static string GetAbsolutePath(string url)
    {
        return "file://E:/GitProject/NeilMyName/DataPC/" + url;
    }

    public static string GetResourcesPath(string url)
    {
        return url;
    }
}
