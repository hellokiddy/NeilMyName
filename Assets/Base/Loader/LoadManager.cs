using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    /// <summary>
    /// call back when load completed...
    /// </summary>
    /// <param name="asset">loaded asset</param>
    public delegate void OnLoadTaskComplete(ILoadAsset asset);

    public class LoadManager 
    {
        private List<ILoader> m_WaitingLoaderList;
        private List<ILoader> m_AcitveLoaderList;
        private int m_MaxLoadCount = 4;
        bool m_Pause = false;
        public void Init()
        {
            m_AcitveLoaderList = new List<ILoader>();
            m_WaitingLoaderList = new List<ILoader>();
        }


        public void Clear()
        {
            
        }


        public void Update()
        {
            for (int i = m_AcitveLoaderList.Count - 1; i >= 0; --i)
            {
                ILoader loader = m_AcitveLoaderList[i];
                loader.Update();
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

        public ILoadState CreateLoadTask()
        {
            return null;
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