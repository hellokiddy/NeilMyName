using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Keedy.Common.Load
{
    public interface ILoader
    {
        ILoadTask Task { get; }
        bool IsDone { get; }
        int Priority { get; set; }
        void Init();
        void Begin();
        void Update();
        void Dispose();
        void Complete();
    }

}
