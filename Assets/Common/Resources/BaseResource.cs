using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EResourceType
{
    DEFAULT = 0,

    AUDIO,
    TEXTURE,

    COUNT
    //...
}

public enum EResourceState
{
    INDEXED = 0,
    LOADING,
    LOADED,//valid state
    UNLOADED,
}

public enum EAssetPathType
{
    DEFAULT = 0,

    STREAMING_ASSET,
    STREAMING_ASSET_BUNDLE,
    LOCAL,
    LOCAL_BUNDLE,
    LOCAL_WWW,
}

public delegate void OnBaseResourceLoaded(BaseResource res);
public delegate void OnBaseResourceNoRef(BaseResource res);
public class BaseResource
{
    private string m_Key;
    private EResourceState m_State;
    private EResourceType m_ResourceType;
    private EAssetPathType m_PathType;
    private IAsset m_Asset;
    private int m_RefCount;
    private event OnBaseResourceLoaded m_OnResLoaded;
    private event OnBaseResourceNoRef m_OnResNoRef;

    public string ResKey { get { return m_Key; } }
    public EResourceState State { get { return m_State; } }
    /// <summary>
    /// the resouce has no resource or loader, you need load res for it
    /// </summary>
    public bool NeedLoad { get { return m_State != EResourceState.LOADING && m_State != EResourceState.LOADED; } }
    public EResourceType ResType { get { return m_ResourceType; } }
    public EAssetPathType AssetPathType { get { return m_PathType; } }
    public bool Valid { get { return m_State == EResourceState.LOADED; } }
    public int RefCount { get { return m_RefCount; } }    

    public BaseResource(string key, EResourceType resType)
    {
        m_Key = key;
        m_State = EResourceState.INDEXED;
        m_RefCount = 0;
        m_ResourceType = resType;
    }

    public Object LoadAsset(string assetName)
    {
        if (Valid) return m_Asset.LoadAsset(assetName);
        else return null;
    }

    public void AppendResourceReadyNotify(OnBaseResourceLoaded onResLoaded)
    {
        if(onResLoaded != null)
        {
            m_OnResLoaded += onResLoaded;
        }
    }

    public void AppednResourceNoRefNotify(OnBaseResourceNoRef onResNoRef)
    {
        if(onResNoRef != null)
        {
            m_OnResNoRef += onResNoRef;
        }
    }

    public void StartDownload()
    {
        m_State = EResourceState.LOADING;
    }
    public void AddRefCount()
    {
        ++m_RefCount;
    }

    public void ReduceRefCount()
    {
        --m_RefCount;
        //if no ref, release the asset and set the resource state to INDEXED...
        if(m_RefCount <= 0)
        {
            if(m_OnResNoRef != null)
            {
                m_OnResLoaded(this);
            }
        }
    }

    public void FillResource(IAsset asset)
    {
        m_Asset = asset;
        m_State = EResourceState.LOADED;
    }

    public void Dispose()
    {
        m_State = EResourceState.UNLOADED;
        m_Key = "";
        if(m_Asset != null)
        {
            m_Asset.Dispose();
            m_Asset = null;
        }
        m_OnResLoaded = null;
        m_OnResNoRef = null;
        m_RefCount = 0;
        m_ResourceType = EResourceType.DEFAULT;
    }

}
