using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTaskCompelte(ResourceTask task);

public class ResourceTask
{
    private BaseResource m_MainRes;
    private List<BaseResource> m_ResList;
    private int m_Priority;
    private event OnTaskCompelte m_OnTaskComplete;


    public int Priority { get { return m_Priority; } set { m_Priority = value; } }
    public float Progress
    {
        get
        {
            int completed = 0;
            int resCount = m_ResList.Count;
            for (int i = 0; i < m_ResList.Count; ++i)
            {
                if (m_ResList[i] == null || m_ResList[i].Valid)
                {
                    ++completed;
                }
            }
            return (float)completed / resCount;
        }
    }

    public bool Done
    {
        get
        {
            for(int i = 0; i < m_ResList.Count; ++i)
            {
                if(m_ResList[i] != null && m_ResList[i].Valid == false)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public ResourceTask()
    {
        m_ResList = new List<BaseResource>();
    }
    /// <summary>
    /// all resources needed have been added to the task...
    /// </summary>
    public void Ready()
    {
        CheckTask();
    }
    public void AddResource(BaseResource resource, bool isMainRes = false)
    {
        if (resource == null) return;
        if (isMainRes)
        {
            m_MainRes = resource;
        }
#if UNITY_EDITOR
        if (Contains(resource.ResKey))
        {
            Debug.LogError(("some resources have the same key:" + resource.ResKey));
        }
#endif
        if (!resource.Valid)
        {
            resource.AppendResourceReadyNotify(OnResourceLoaded);
        }
        resource.AddRefCount();
        m_ResList.Add(resource);
    }

    public void AppendCompleteNotify(OnTaskCompelte onTaskComplete)
    {
        if(onTaskComplete != null)
        {
            m_OnTaskComplete += onTaskComplete;
        }
    }

    public Object LoadAsset(string resName, string assetName)
    {
        for(int i = 0; i < m_ResList.Count; ++i)
        {
            BaseResource res = m_ResList[i];
            if(res.ResKey == resName)
            {
                return res.LoadAsset(assetName);
            }
        }
        Debug.LogError("Can't find the resources:" + resName);
        return null;
    }

    /// <summary>
    /// dispose the task
    /// reduce the refCount of the resources
    /// </summary>
    public void Dispose()
    {
        for(int i = 0; i < m_ResList.Count; ++i)
        {
            if(m_ResList[i] != null)
            {
                m_ResList[i].ReduceRefCount();
            }
        }
        m_ResList.Clear();
        m_OnTaskComplete = null;
    }

    //check weather the task is Done to call the m_OnTaskComplete...
    void OnResourceLoaded(BaseResource res)
    {
        CheckTask();
    }

    void CheckTask()
    {
        if (Done)
        {
            Complete();
        }
    }
    void Complete()
    {
        if(m_OnTaskComplete != null)
        {
            m_OnTaskComplete(this);
        }
    }
    

#if UNITY_EDITOR
    bool Contains(string resKey)
    {
        if (string.IsNullOrEmpty(resKey)) return false;
        for(int i = 0; i < m_ResList.Count; ++i)
        {
            if(m_ResList[i].ResKey.Equals(resKey))
            {
                return true;
            }
        }
        return false;
    }
#endif


}
