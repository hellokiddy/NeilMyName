using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public interface ILoadTask
    {
        int LoaderCount { get; }
        int CompletedLoaderCount { get; }
        void SetLoaderCount(int count);
        void AddTaskCallBack(OnLoadTaskComplete callback);
        void OnLoaderComplete(ILoader loader);
        void OnTaskComplete();
        void Dispose();
    }
}