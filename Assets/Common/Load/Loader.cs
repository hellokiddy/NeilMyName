using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLoaderComplete(Loader loader);
public abstract class Loader
{
    protected bool m_IsDone;
    protected int m_Priority;
    protected string m_Url;
    protected string m_Error;
    protected IAsset m_Asset;

    public bool IsDone { get { return m_IsDone; } }

    public int Priority { get { return m_Priority; } set { m_Priority = value; } }

    public string Url { get { return m_Url; } }

    public string Error { get { return m_Error; } }

    public IAsset Asset { get { return m_Asset; } }

    protected event OnLoaderComplete m_OnLoaderComplete;

    public void Init(string url, int priority = 0)
    {
        m_Url = url;
        m_Priority = priority;
    }

    public void AddLoadCallback(OnLoaderComplete onLoaderComplete)
    {
        if(onLoaderComplete != null)
        {
            m_OnLoaderComplete += onLoaderComplete;
        }
    }
    public abstract IEnumerator Begin();

    public virtual void Dispose()
    {
        m_Url = null;
        m_Asset = null;
        m_Error = null;
        m_Priority = 0;
        m_IsDone = false;
        m_OnLoaderComplete = null;
    }

    protected void Complete()
    {
        if(m_OnLoaderComplete != null)
        {
            m_OnLoaderComplete(this);
        }
    }
}
