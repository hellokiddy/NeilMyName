using System.Collections;
using System.Collections.Generic;
using System;
using Object = System.Object;

namespace Keedy.Common.Load
{
    public interface ILoadAsset
    {
        string TryGetAssetByName(string assetName, out Object asset);
        bool AddAsset(string assetName, Object asset, string error);
        void Dispose();
    }
}