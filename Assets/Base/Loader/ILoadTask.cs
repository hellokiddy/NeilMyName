﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public interface ILoadTask
    {
        void OnLoaderComplete(string assetName, object asset);
        void OnTaskComplete();
    }
}