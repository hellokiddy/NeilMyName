using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Utility;

public class WWWLoader : Loader
{
    private static ObjectPool<WWWLoader> m_Pool;
    private WWW m_WWW;
    public override ELoaderType LoaderType { get { return ELoaderType.WWW_LOADER; } }

    //objectpool need a constructor without refrences...
    public WWWLoader() { }
    public override void Begin()
    {
        m_WWW = new WWW(ConfigPathManager.GetAbsolutePath(Url));
    }

    public override void Update()
    {
        if(m_WWW == null)
        {
            return;
        }
        if(m_IsDone == false && m_WWW.isDone)
        {
            m_Error = m_WWW.error;
            m_Asset = new WWWAsset(m_Url, m_WWW.assetBundle);
            m_IsDone = true;

            Complete();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if(m_WWW != null)
        {
            m_WWW.Dispose();
            m_WWW = null;
        }
    }

    /****************************************** ObjectPool ******************************************/

    public static void InitObjectPool(int size, int growSize = 20)
    {
        if(m_Pool == null)
        {
            m_Pool = new ObjectPool<WWWLoader>(size, growSize);
        }
        else
        {
            m_Pool.Resize(size, true);
        }
    }


    public static WWWLoader Allocate()
    {
        if(m_Pool == null)
        {
            InitObjectPool(20);
        }
        return m_Pool.Allocate();
    }


    public static void Release(WWWLoader loader)
    {
        if (m_Pool == null)
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

public class WWWAsset : IAsset
{
    private string m_Url;
    public AssetBundle m_AssetBudle;

    private WWW m_WWW;
    public string Name
    {
        get
        {
            return m_Url;
        }
    }
    public WWWAsset(string url, AssetBundle ab)
    {
        m_Url = url;
        m_AssetBudle = ab;
    }

    public UnityEngine.Object MainAsset
    {
        get
        {
            if(m_AssetBudle != null)
            {
                return m_AssetBudle.LoadAllAssets()[0];
            }
            return null;
        }
    }
    public UnityEngine.Object LoadAsset(string name)
    {
        if(m_AssetBudle == null)
        {
            return null;
        }
        return m_AssetBudle.LoadAsset(name);
    }

    public void Dispose(bool force = false)
    {
        if(m_AssetBudle != null)
        {
            m_AssetBudle.Unload(force);
        }
    }
}
