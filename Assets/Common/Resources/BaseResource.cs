using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EResourceType
{
    DEFAULT = 0,

    AUDIO,
    TEXTURE,
    //...
}

public enum EResourceState
{
    INDEXED = 0,
    LOADED,//valid state
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
public class BaseResource
{
    private string m_Key;
    private EResourceState m_State;
    private EResourceType m_ResourceType;
    private EAssetPathType m_PathType;
    private IAsset m_Asset;
    private int m_RefCount;
    private event OnBaseResourceLoaded m_OnResLoaded;

    public string ResKey { get { return m_Key; } }
    public EResourceState State { get { return m_State; } }
    public EResourceType ResType { get { return m_ResourceType; } }
    public EAssetPathType AssetPathType { get { return m_PathType; } }
    public bool Valid { get { return m_State == EResourceState.LOADED; } }
    public int RefCount { get { return m_RefCount; } }
    public BaseResource(string key, EResourceType resType)
    {
        m_Key = key;
        m_ResourceType = resType;
        m_State = EResourceState.INDEXED;
        m_RefCount = 0;
    }

    public BaseResource(string key)
    {
        m_Key = key;
        m_State = EResourceState.INDEXED;
        m_RefCount = 0;
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
    public BaseResource RefResource()
    {
        ++m_RefCount;
        return this;
    }
    public void FillResource(IAsset asset)
    {
        m_Asset = asset;
        m_State = EResourceState.LOADED;
    }

}
