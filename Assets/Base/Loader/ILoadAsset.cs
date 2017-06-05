using System.Collections;
using System.Collections.Generic;
using System;
using Object = System.Object;

namespace Keedy.Common.Load
{
    public interface ILoadAsset
    {

        Object GetAssetByName(string assetName);
    }
}