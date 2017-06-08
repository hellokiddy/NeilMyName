using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class LoadTask : ILoadTask
    {
        public delegate void RecycleTask(LoadTask task);
        /// <summary>
        /// the delegate to recycle the task if needed...
        /// </summary>
        public static RecycleTask s_RecycleTask;

        private event OnLoadTaskComplete m_OnTaskComplete;
        private int m_LoaderCount;
        private int m_CompletedLoaderCount;
        private LoadAsset m_LoadAsset;
        public int CompletedLoaderCount { get { return m_CompletedLoaderCount; } }
        public int LoaderCount { get { return m_LoaderCount; } }

        public LoadTask()
        {
            m_LoadAsset = new LoadAsset();
        }
        public void AddTaskCallBack(OnLoadTaskComplete callback)
        {
            if (callback == null)
            {
                Debug.LogError("here comes a null callback to task....");
                return;
            }
            m_OnTaskComplete += callback;
        }
        /// <summary>
        /// you must use the mothod to make the task can complete...
        /// </summary>
        /// <param name="count"></param>
        public void SetLoaderCount(int count)
        {
            m_LoaderCount = count;
            CheckTask();
        }

        public void OnLoaderComplete(ILoader loader)
        {
            //don't call me in a wrong way...
            if (!loader.IsDone) return;

            m_LoadAsset.AddAsset(loader.Url, loader.Data, loader.Error);
            ++m_CompletedLoaderCount;
            CheckTask();
        }
        public void OnTaskComplete()
        {
            if (m_OnTaskComplete != null)
            {
                m_OnTaskComplete(m_LoadAsset);
            }
            if(s_RecycleTask != null)
            {
                s_RecycleTask(this);
            }
        }
        public void Dispose()
        {
            m_LoadAsset = null;
            m_CompletedLoaderCount = 0;
            m_LoaderCount = 0;
            m_OnTaskComplete = null;
        }

        void CheckTask()
        {
            if(m_CompletedLoaderCount >= m_LoaderCount)
            {
                OnTaskComplete();
            }
        }
    }
}