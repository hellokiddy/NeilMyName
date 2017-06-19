using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLoadTaskComplete();
public class LoadTask
{
    protected event OnLoadTaskComplete m_OnLoadTaskComplete;
    protected List<Loader> m_LoaderList;

    public bool IsDone
    {
        get
        {
            for(int i = 0; i < m_LoaderList.Count; ++i)
            {
                if(m_LoaderList[i].IsDone == false)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsError
    {
        get
        {
            for(int i = 0; i < m_LoaderList.Count; ++i)
            {
                if(string.IsNullOrEmpty(m_LoaderList[i].Error) == false)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public int AssetCount
    {
        get
        {
            return m_LoaderList.Count;
        }
    }

    public int CompletedCount
    {
        get
        {
            int count = 0;
            for(int i = 0; i < m_LoaderList.Count; ++i)
            {
                if (m_LoaderList[i].IsDone)
                {
                    ++count;
                }
            }
            return count;
        }
    }

    public float Progress
    {
        get
        {
            int total = AssetCount;
            int completes = CompletedCount;
            if(total <=0 || completes <= 0)
            {
                return 0;
            }
            return (float)completes / total;
        }
    }

    public LoadTask()
    {
        m_LoaderList = new List<Loader>();
    }

    public void AddLoader(Loader loader)
    {
        if (loader == null || m_LoaderList.Contains(loader)) return;
        m_LoaderList.Add(loader);
        loader.AddLoadCallback(OnLoaderComplete);
    }

    protected void OnLoaderComplete(Loader loader)
    {

    }

    protected void OnTaskComplete()
    {

    }
}
