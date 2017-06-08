using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Keedy.Base.IO;

namespace Keedy.Common.Load
{
    public abstract class AbstractLoader : ILoader
    {
        //list of task that uses the loader
        protected List<ILoadTask> m_TaskList;

        protected bool m_IsDone;
        protected int m_Priority;
        protected string m_Url;
        protected string m_Error;
        protected object m_Data;
        protected float m_LoadingTime;

        public bool IsDone { get { return m_IsDone; } }

        public abstract ELoaderType LoaderType { get; }

        public int Priority { get { return m_Priority; } set { m_Priority = value; } }

        public string Url { get { return m_Url; } }

        public string Error { get { return m_Error; } }

        public object Data { get { return m_Data; } }

        public AbstractLoader()
        {
            m_TaskList = new List<ILoadTask>();
        }

        public virtual void Init(string url, int priority)
        {
            m_Url = url;
            m_Priority = priority;
            //if (string.IsNullOrEmpty(m_Url))
            //{
            //    m_Error = LoadConf.c_EmptyLoadPathError;
            //    m_IsDone = true;
            //}
        }

        public virtual void AddTask(ILoadTask task)
        {
            //can't de-emphasis, because a task may sign some same loaders...
            m_TaskList.Add(task);
        }

        public abstract void Begin();

        public abstract void Update(float deltaTime);

        public void Complete()
        {
            for (int i = 0; i < m_TaskList.Count; ++i)
            {
                m_TaskList[i].OnLoaderComplete(this);
            }
        }

        public virtual void Dispose()
        {
            m_TaskList.Clear();
            m_IsDone = false;
            m_Priority = 0;
            m_LoadingTime = 0;
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }
    }
}