using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public interface ILoadTask
    {
        int LoaderCount { get; }
        int CompletedLoaderCount { get; }
        void AddTaskCallBack(OnLoadTaskComplete callback);
        void OnLoaderComplete(string assetName, object asset);
        void OnTaskComplete();
    }
}