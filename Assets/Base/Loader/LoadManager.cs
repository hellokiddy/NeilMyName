using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Keedy.Common.Load
{
    public static class LoadConf
    {
        public const string c_EmptyLoadPathError = "the path to load asset is empty...";
        public const string c_NoSuchLoaderError = "you haven't create a loader to load the asset";
        public const string c_OutOfTimeError = "failed to load the asset in MaxLoadingTime...";
        public const string c_LoadError = "load error...";
        
        public const float c_MaxLoadingTime = 20f;

    }

    /// <summary>
    /// call back when load completed...
    /// </summary>
    /// <param name="asset">loaded asset</param>
    public delegate void OnLoadTaskComplete(ILoadAsset asset);

    public class LoadManager 
    {
        /****************************************** Todo List ******************************************/
        /*
            1.ILoader,ILoadTask的对象池缓存
            2.防止内存碎片化的处理
            3.加载错误机制
            4.函数调用异常机制
        */
        private List<ILoader> m_WaitingLoaderList;
        private List<ILoader> m_AcitveLoaderList;
        private int m_MaxLoadCount = 4;
        bool m_Pause = false;
        public void Init()
        {
            m_AcitveLoaderList = new List<ILoader>();
            m_WaitingLoaderList = new List<ILoader>();
            m_Pause = false;
            LoadTask.s_RecycleTask = RecycleTask;
        }


        public void Clear()
        {
            m_AcitveLoaderList.Clear();
            m_WaitingLoaderList.Clear();
            m_Pause = false;
        }


        public void Update(float deltaTime)
        {
            for (int i = m_AcitveLoaderList.Count - 1; i >= 0; --i)
            {
                ILoader loader = m_AcitveLoaderList[i];
                loader.Update(deltaTime);
                if (loader.IsDone)
                {
                    m_AcitveLoaderList.RemoveAt(i);
                    loader.Complete();
                    RecycleLoader(loader);
                }
            }

            if (m_Pause) return;

            while(m_AcitveLoaderList.Count < m_MaxLoadCount && m_WaitingLoaderList.Count > 0)
            {
                ILoader loader = PopHighestPriorityLoader();
                m_AcitveLoaderList.Add(loader);
                loader.Begin();
            }
        }

        public ILoadState CreateLoadTask(string assetPath, OnLoadTaskComplete onTaskComplete, int priority, ELoaderType loaderType)
        {
            return CreateLoadTask(new string[] { assetPath }, onTaskComplete, priority, loaderType);
        }

        public ILoadState CreateLoadTask(string[] assetPaths, OnLoadTaskComplete onTaskComplete, int priority, ELoaderType loaderType)
        {
            ILoadTask task = GetTask();
            task.AddTaskCallBack(onTaskComplete);
            int loaderCount = assetPaths == null ? 0 : assetPaths.Length;

            //set loader count first, then the task could check if completed...
            task.SetLoaderCount(loaderCount);

            if (assetPaths != null)
            {
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    ILoader loader = GetLoader(assetPaths[i], priority, loaderType);
                    loader.AddTask(task);
                }
            }

            ILoadState state = GetState();
            state.SetTask(task);
            return state;
        }
        public ILoadState CreateLoadTask(List<string> assetPaths, OnLoadTaskComplete onTaskComplete, int priority, ELoaderType loaderType)
        {
            return CreateLoadTask(assetPaths.ToArray(), onTaskComplete, priority, loaderType);
        }

        /****************************************** prepare to use object pool ******************************************/
        ILoadState GetState()
        {
            return new LoadState();
        }

        ILoadTask GetTask()
        {
            return new LoadTask();
        }

        ILoader GetLoader(string path, int priority, ELoaderType loaderType)
        {
            for (int i = 0; i < m_AcitveLoaderList.Count; i++)
            {
                if (path == m_AcitveLoaderList[i].Url)
                {
                    return m_AcitveLoaderList[i];
                }
            }
            for (int i = 0; i < m_WaitingLoaderList.Count; i++)
            {
                if (path == m_WaitingLoaderList[i].Url)
                {
                    if(m_WaitingLoaderList[i].Priority < priority)
                    {
                        m_WaitingLoaderList[i].Priority = priority;
                    }
                    return m_WaitingLoaderList[i];
                }
            }
            ILoader loader = AddNewLoader(loaderType);
            loader.Init(path, priority);
            return loader;
        }

        ILoader AddNewLoader(ELoaderType loaderType)
        {
            ILoader loader = CreateLoader(loaderType);
            m_WaitingLoaderList.Add(loader);
            return loader;
        }

        //loader factory
        ILoader CreateLoader(ELoaderType loaderType)
        {
            ILoader loader = null;
            switch (loaderType)
            {
                case ELoaderType.WWW_LOADER:
                    loader = new WWWLoader();
                    break;
                case ELoaderType.RESOURCE_LOADER:
                    break;
            }
            return loader;
        }

        //Recycle loader here
        void RecycleLoader(ILoader loader)
        {
            loader.Dispose();
        }        
        void RecycleTask(LoadTask task)
        {
            Debug.LogError("A task is completed...");
        }
        /// <summary>
        /// Pop the highest priority loader from m_WaitingLoaderList
        /// </summary>
        /// <returns></returns>
        ILoader PopHighestPriorityLoader()
        {
            int index = -1;
            int piority = -1;
            ILoader loader = null;
            for(int i = 0; i < m_WaitingLoaderList.Count; i++)
            {
                if(m_WaitingLoaderList[i].Priority > piority)
                {
                    index = i;
                    loader = m_WaitingLoaderList[i];
                    piority = loader.Priority;
                }
            }
            m_WaitingLoaderList.RemoveAt(index);
            return loader;
        }
    }
}