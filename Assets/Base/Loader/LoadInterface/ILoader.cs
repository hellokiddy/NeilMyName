using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Keedy.Common.Load
{
    public interface ILoader
    {
        ELoaderType LoaderType { get; }
        string Url { get; }
        string Error { get; }
        bool IsDone { get; }
        int Priority { get; set; }
        object Data { get; }
        void Init(string url, int priority);
        void AddTask(ILoadTask task);
        void Begin();
        void Update(float deltaTime);
        void Dispose();
        void Complete();
        void Delete();
    }

}
