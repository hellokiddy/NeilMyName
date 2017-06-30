using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Keedy.Common.Utility;

public delegate void OnLoadTaskComplete(IAsset[] assets);
public class LoadTask
{
    private static ObjectPool<LoadTask> m_TaskPool;
    protected event OnLoadTaskComplete m_OnLoadTaskComplete;
    protected List<Loader> m_LoaderList;
    protected List<IAsset> m_AssetList;
    protected bool m_IsError;
    protected bool m_CanDelete;
    public bool IsDone
    {
        get
        {
            return LoadingCount == 0;
        }
    }

    public bool IsError
    {
        get
        {
            return m_IsError;
        }
    }

    public bool CanDelete
    {
        get
        {
            return m_CanDelete;
        }
    }

    /// <summary>
    /// the count of loaders undone...
    /// </summary>
    public int LoadingCount
    {
        get
        {
            return m_LoaderList == null ? 0 : m_LoaderList.Count;
        }
    }

    public int CompletedCount
    {
        get
        {
            return m_AssetList == null ? 0 : m_AssetList.Count;
        }
    }

    public float Progress
    {
        get
        {
            int completes = CompletedCount;
            int total = LoadingCount + completes;
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
        m_AssetList = new List<IAsset>();
    }

    public void AddLoader(Loader loader)
    {
        if (loader == null || m_LoaderList.Contains(loader)) return;
        m_LoaderList.Add(loader);
        loader.AddLoadCallback(OnLoaderComplete);
    }

    public void AddTaskCallback(OnLoadTaskComplete onLoadTaskComplete)
    {
        if(onLoadTaskComplete != null)
        {
            m_OnLoadTaskComplete += onLoadTaskComplete;
        }
    }
    protected void OnLoaderComplete(Loader loader)
    {
#if UNITY_EDITOR
        if(loader == null)
        {
            Debug.LogError("loader is null, this should not happen...");
        }
        if (!string.IsNullOrEmpty(loader.Error))
        {
            Debug.LogError(string.Format("some loader has errror:[URL:{0}] [Error:{1}]" , loader.Url, loader.Error));
        }
#endif
        if(m_IsError == false && string.IsNullOrEmpty(loader.Error))
        {
            m_IsError = true;
        }
        m_AssetList.Add(loader.Asset);
        //remove the completed to dispose the loader...
        if(m_LoaderList.Contains(loader)) m_LoaderList.Remove(loader);

        if (IsDone)
        {
            OnTaskComplete();
        }
    }

    protected void OnTaskComplete()
    {
        if(m_OnLoadTaskComplete != null)
        {
            m_OnLoadTaskComplete(m_AssetList.ToArray());
        }
        m_CanDelete = true;
    }

    public void Dispose()
    {
        m_IsError = false;
        m_CanDelete = false;
        m_AssetList.Clear();
        m_LoaderList.Clear();
        m_OnLoadTaskComplete = null;
    }

    /****************************************** objectpool ******************************************/
    public static void InitObjectPool(int size, int growSize = 20)
    {
        if (m_TaskPool == null)
        {
            m_TaskPool = new ObjectPool<LoadTask>(size, growSize);
        }
        else
        {
            m_TaskPool.Resize(size, true);
        }
    }
    
    public static LoadTask Allocate()
    {
        if (m_TaskPool == null)
        {
            InitObjectPool(20);
        }
        return m_TaskPool.Allocate();
    }
    
    public static void Release(LoadTask task)
    {
        if (m_TaskPool == null)
        {
            InitObjectPool(20);
        }
        if (task != null)
        {
            task.Dispose();
            m_TaskPool.Release(task);
        }
    }
}
