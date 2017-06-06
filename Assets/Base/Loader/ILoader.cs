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
        bool IsDone { get; }
        int Priority { get; set; }
        void Init(string url, int priority);
        void AddTask(ILoadTask task);
        void Begin();
        void Update();
        void Dispose();
        void Complete();
        void Delete();
    }

}
