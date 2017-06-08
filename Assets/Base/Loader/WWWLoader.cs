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
        private object m_Data;
        private float m_LoadingTime;
        public bool IsDone { get { return m_IsDone; } }

        public ELoaderType LoaderType { get { return ELoaderType.WWW_LOADER; } }

        public int Priority { get { return m_Priority; } set { m_Priority = value; } }

        public string Url { get { return m_Url; } }

        public string Error { get { return m_Error; } }

        public object Data { get { return m_Data; } }

        public WWWLoader()
        {
            m_TaskList = new List<ILoadTask>();
        }

        public void Init(string url, int priority)
        {
            m_Url = url;
            m_Priority = priority;
            if (string.IsNullOrEmpty(m_Url))
            {
                m_Error = LoadConf.c_EmptyLoadPathError;
                m_IsDone = true;
            }
        }

        public void AddTask(ILoadTask task)
        {
            //can't de-emphasis, because a task may sign some same loaders...
            m_TaskList.Add(task);
        }

        public void Begin()
        {
            //if inited failed, you will not need to create a www.
            if(m_IsDone == false)
            {
                m_WWW = new WWW("file://" + Application.dataPath + "/_NeilTest/LoadTest/AB/" + m_Url);
            }
        }

        public void Update(float deltaTime)
        {
            
            if (m_IsDone == false)
            {
                //set a max loading time to force complete the loader...
                m_LoadingTime += Time.deltaTime;
                if (LoadConf.c_MaxLoadingTime > 0 && m_LoadingTime >= LoadConf.c_MaxLoadingTime)
                {
                    m_IsDone = true;
                    m_Error = LoadConf.c_OutOfTimeError;
                }

                //when www ready, complete the loader...
                if (m_WWW.isDone)
                {
                    m_IsDone = true;
                    m_Error = m_WWW.error;
                    m_Data = m_WWW.assetBundle;
                }
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
            m_LoadingTime = 0;
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