using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class WWWLoader : ILoader
    {
        //list of task that uses the loader
        private List<ILoadTask> m_TaskList;
        private WWW m_WWW;

        private bool m_IsDone;
        private int m_Priority;
        private string m_Url;
        private string m_Error;

        public bool IsDone { get { return m_IsDone; } }

        public ELoaderType LoaderType { get { return ELoaderType.WWW_LOADER; } }

        public int Priority { get { return m_Priority; } set { m_Priority = value; } }

        public string Url { get { return m_Url; } }

        public string Error { get { return m_Error; } }

        public object Data { get { return m_WWW.assetBundle; } }

        public WWWLoader()
        {
            m_TaskList = new List<ILoadTask>();
        }

        public void Init(string url, int priority)
        {
            m_Url = url;
            m_Priority = priority;
        }

        public void AddTask(ILoadTask task)
        {
            m_TaskList.Add(task);
        }

        public void Begin()
        {
            m_WWW = new WWW(m_Url);
        }

        public void Update()
        {
            if (m_WWW.isDone && m_IsDone == false)
            {
                m_IsDone = true;
                m_Error = m_WWW.error;
            }
        }

        public void Complete()
        {
            for (int i = 0; i < m_TaskList.Count; ++i)
            {
                m_TaskList[i].OnLoaderComplete(this);
            }
        }

        public void Dispose()
        {
            m_TaskList.Clear();
            m_IsDone = false;
            m_Priority = 0;
            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}