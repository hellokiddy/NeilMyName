using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLoaderComplete(Loader loader);
public delegate void OnLoaderCompleteForRes(IAsset asset);
public enum ELoaderType
{
    WWW_LOADER,
    RESOURCES_LOADER,
}
public abstract class Loader
{
    protected bool m_IsDone;
    protected int m_Priority;
    protected string m_Url;
    protected string m_Error;
    protected IAsset m_Asset;
    protected bool m_CanDelete;

    /// <summary>
    /// the loader completed and called back, so it's ready to recycle...
    /// </summary>
    public bool IsDone { get { return m_IsDone; } }

    public int Priority { get { return m_Priority; } set { m_Priority = value; } }

    public string Url { get { return m_Url; } }

    public string Error { get { return m_Error; } }

    public IAsset Asset { get { return m_Asset; } }

    public bool CanDelete { get { return m_CanDelete; } }

    public abstract ELoaderType LoaderType { get; }

    protected event OnLoaderComplete m_OnLoaderComplete;
    protected event OnLoaderCompleteForRes m_OnLoaderCompleteForRes;

    public void Init(string url, int priority = 0)
    {
        m_Url = url;
        m_Priority = priority;
    }

    /// <summary>
    /// when loader completes, the loader will be disposed.
    /// so don't use loader again after callback...
    /// </summary>
    /// <param name="onLoaderComplete"></param>
    public void AddLoadCallback(OnLoaderComplete onLoaderComplete)
    {
        if (onLoaderComplete != null)
        {
            m_OnLoaderComplete += onLoaderComplete;
        }
    }

    public void AppendCompleteNotify(OnLoaderCompleteForRes onLoaderComplete)
    {
        if(onLoaderComplete != null)
        {
            m_OnLoaderCompleteForRes += onLoaderComplete;
        }
    }
    
    public abstract void Begin();

    public abstract void Update();
    public virtual void Dispose()
    {
        m_Url = null;
        m_Asset = null;
        m_Error = null;
        m_Priority = 0;
        m_IsDone = false;
        m_CanDelete = false;
        m_OnLoaderComplete = null;
        m_OnLoaderCompleteForRes = null;
    }

    protected void Complete()
    {
        if(m_OnLoaderComplete != null)
        {
            m_OnLoaderComplete(this);
        }
        if(m_OnLoaderCompleteForRes != null)
        {
            m_OnLoaderCompleteForRes(Asset);
        }
        //make it canDelete after callback...
        m_CanDelete = true;
    }
}
