using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class LoadTask : ILoadTask
    {
        private event OnLoadTaskComplete m_OnTaskComplete;
        private int m_LoaderCount;
        private int m_CompletedLoaderCount;
        public int CompletedLoaderCount { get { return m_CompletedLoaderCount; } }
        public int LoaderCount { get { return m_LoaderCount; } }


        public void AddTaskCallBack(OnLoadTaskComplete callback)
        {
            if (callback == null)
            {
                Debug.LogError("here comes a null callback to task....");
                return;
            }
            m_OnTaskComplete += callback;
        }

        public void SetLoaderCount(int count)
        {
            m_LoaderCount = count;
        }

        public void OnLoaderComplete(ILoader loader)
        {
            ++m_CompletedLoaderCount;
            
        }

        public void OnTaskComplete()
        {
            if (m_OnTaskComplete != null)
            {
                m_OnTaskComplete(null);
            }
        }
    }
}