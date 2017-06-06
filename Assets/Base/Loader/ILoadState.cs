using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public interface ILoadState
    {
        //这个还是不暴露出来了
        //ILoadTask Task { get; }
        float LoadProcess { get; }

    }
}