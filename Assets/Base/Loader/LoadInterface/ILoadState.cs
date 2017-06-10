using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public interface ILoadState
    {
        float LoadProcess { get; }
        bool IsDone { get; }
        void SetTask(ILoadTask task);
    }
}