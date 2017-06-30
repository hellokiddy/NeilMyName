using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Utility;

public class ResourcesLoader : Loader
{
    private static ObjectPool<ResourcesLoader> m_Pool;
    private ResourceRequest m_Request;
    public override ELoaderType LoaderType { get { return ELoaderType.RESOURCES_LOADER; } }

    //object pool needs a constructor without refrences...
    public ResourcesLoader() { }

    public override void Begin()
    {
        m_Request = Resources.LoadAsync(ConfigPathManager.GetResourcesPath(Url));
    }

    public override void Update()
    {
        if(m_Request == null)
        {
            return;
        }
        if(m_Request.isDone && m_IsDone == false)
        {
            m_IsDone = true;
            m_Error = m_Request.asset == null ? "ReLoad Failed..." : "";
            m_Asset = new ResourcesAsset(m_Url, m_Request.asset);

            Complete();
        }
    }

    /****************************************** Object Pool ******************************************/
    public static void InitObjectPool(int size, int growSize = 20)
    {
        if (m_Pool == null)
        {
            m_Pool = new ObjectPool<ResourcesLoader>(size, growSize);
        }
        else
        {
            m_Pool.Resize(size, true);
        }
    }

    public static ResourcesLoader Allocate()
    {
        if(m_Pool == null)
        {
            InitObjectPool(20);
        }
        return m_Pool.Allocate();
    }

    public static void Release(ResourcesLoader loader)
    {
        if(m_Pool == null)
        {
            InitObjectPool(20);
        }
        if(loader != null)
        {
            loader.Dispose();
            m_Pool.Release(loader);
        }
    }
}

public class ResourcesAsset : IAsset
{
    private UnityEngine.Object m_Resource;
    private string m_Url;

    public string Name
    {
        get
        {
            return m_Url;
        }
    }
    public ResourcesAsset(string url, UnityEngine.Object resource)
    {
        m_Url = url;
        m_Resource = resource;
    }

    public UnityEngine.Object LoadAsset(string name)
    {
        return m_Resource;
    }

    public void Dispose(bool force = false)
    {
        m_Url = null;
        if(m_Resource != null)
        {
            GameObject.DestroyObject(m_Resource);
        }
    }
}
