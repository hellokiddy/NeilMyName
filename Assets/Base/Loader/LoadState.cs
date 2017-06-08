using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class LoadState : ILoadState
    {
        private ILoadTask m_Task;

        public float LoadProcess
        {
            get
            {
                if(m_Task != null && m_Task.LoaderCount > 0 && m_Task.CompletedLoaderCount >= 0)
                {
                    return (float)m_Task.CompletedLoaderCount / m_Task.LoaderCount;
                }
                //when the task is complete, the task will be set to null. so return 1. 
                return 1;
            }
        }

        public void SetTask(ILoadTask task)
        {
            m_Task = task;
            m_Task.AddTaskCallBack(OnTaskComplete);
        }

        //release the refrence of the task to recycle the task...
        void OnTaskComplete(ILoadAsset asset)
        {
            m_Task = null;
        }
    }
}