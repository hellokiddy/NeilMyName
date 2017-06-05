using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Keedy.Common.Load
{
    public interface ILoader
    {
        ILoadTask Task { get; }
        string Url { get; }
        bool IsDone { get; }
        int Priority { get; }
        void Init(string url, int priority);
        void Begin();
        void Update();
        void Dispose();
        void Complete();
        void Delete();
    }

}
