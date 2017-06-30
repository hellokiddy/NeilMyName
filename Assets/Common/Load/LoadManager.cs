using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager
{
    //start one loader one frame... so maintain a waiting list...
    private List<Loader> m_WatingLoaderList;
    private List<Loader> m_LoadingList;//the current loading loaders...
    private List<LoadTask> m_TaskList;
    private int m_Priority;//a auto-incremental priority
    public void Init()
    {
        m_WatingLoaderList = new List<Loader>();
        m_LoadingList = new List<Loader>();
        m_TaskList = new List<LoadTask>();
        //Init object pool for loader...
        //WWWLoader.InitObjectPool(20);
        //ResourcesLoader.InitObjectPool(20);
        //LoadTask.InitObjectPool(20);
    }
    public void Update(float deltaTime)
    {
        for(int i = m_LoadingList.Count - 1; i >=0; --i)
        {
            Loader loader = m_LoadingList[i];
            loader.Update();
            if (loader.CanDelete)
            {
                //recycle loader here...
                m_LoadingList.RemoveAt(i);
                ReleaseLoader(loader);
            }
        }

        for(int i  = m_TaskList.Count - 1; i >= 0; --i)
        {
            LoadTask task = m_TaskList[i];
            if (task.CanDelete)
            {
                //recycle task here...
                m_TaskList.RemoveAt(i);
                LoadTask.Release(task);
            }
        }

        //start one loader one frame...
        if (m_WatingLoaderList.Count > 0)
        {
            Loader loader = PopHighestPriorityLoader();
            m_LoadingList.Add(loader);
            loader.Begin();
        }
    }

    public LoadTask CreateLoadTask(string assetPath, OnLoadTaskComplete onTaskComplete)
    {
        return CreateLoadTask(new string[] { assetPath }, onTaskComplete);
    }

    public LoadTask CreateLoadTask(List<string> assetPaths, OnLoadTaskComplete onTaskComplete)
    {
        return CreateLoadTask(assetPaths.ToArray(), onTaskComplete);
    }

    public LoadTask CreateLoadTask(string[] assetPaths, OnLoadTaskComplete onTaskComplete)
    {
        //task will be released automatic after complete...
        LoadTask task = LoadTask.Allocate();
        //m_TaskList.Add(task);//暂时屏蔽掉，看需不需要管理task
        task.AddTaskCallback(onTaskComplete);

        if(assetPaths != null)
        {
            for(int i= 0; i < assetPaths.Length; ++i)
            {
                Loader loader = GetLoader(assetPaths[i], ++m_Priority);
                task.AddLoader(loader);
            }
        }

        return task;
    }
    
    public Loader CreateLoader(string url, int priority)
    {
        return GetLoader(url, priority);
    }
    Loader GetLoader(string url, int priority)
    {
        Loader loader = null;

        // it's impossible to get the same url as the resource is unique...

        //for(int i = 0; i < m_LoadingList.Count; ++i)
        //{
        //    loader = m_LoadingList[i];
        //    if (loader.Url == url)
        //    {
        //        return loader;
        //    }
        //}

        //for (int i = 0; i < m_WatingLoaderList.Count; ++i)
        //{
        //    loader = m_WatingLoaderList[i];
        //    if (loader.Url == url)
        //    {
        //        loader.Priority = priority;
        //        return loader;
        //    }
        //}

        loader = AllocateLoader(url);
        loader.Init(url, priority);
        m_WatingLoaderList.Add(loader);

        return loader;
    }

    //maybe we can diffrence it by editor mode or others...
    Loader AllocateLoader(string url)
    {
        //Debug.LogError(url);
        if (url.EndsWith(".ab"))
        {
            return WWWLoader.Allocate();
        }
        else
        {
            return ResourcesLoader.Allocate();
        }
    }
    void ReleaseLoader(Loader loader)
    {
        if(loader.LoaderType == ELoaderType.RESOURCES_LOADER)
        {
            ResourcesLoader.Release((ResourcesLoader)loader);
        }
        else if(loader.LoaderType == ELoaderType.WWW_LOADER)
        {
            WWWLoader.Release((WWWLoader)loader);
        }
    }

    //make sure the m_WatingLoaderList is not empty...
    Loader PopHighestPriorityLoader()
    {
        Loader loader = null;
        for(int i = m_WatingLoaderList.Count - 1; i >= 0; --i)
        {
            if(loader == null)
            {
                loader = m_WatingLoaderList[i];
            }
            else if(m_WatingLoaderList[i].Priority > loader.Priority)
            {
                loader = m_WatingLoaderList[i];
            }
        }
        m_WatingLoaderList.Remove(loader);
        return loader;
    }
}
