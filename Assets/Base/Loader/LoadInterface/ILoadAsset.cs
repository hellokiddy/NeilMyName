using System.Collections;
using System.Collections.Generic;
using System;
using Object = System.Object;

namespace Keedy.Common.Load
{
    public interface ILoadAsset
    {
        bool TryGetAssetByName(string assetName, out Object asset);
        void AddAsset(string assetName, Object asset);
    }
}