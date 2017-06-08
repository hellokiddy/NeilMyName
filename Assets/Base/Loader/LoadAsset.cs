using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public static class LoadConf
    {
        public const string c_EmptyLoadPathError = "the path to load asset is empty...";
        public const string c_NoSuchLoaderError = "you haven't create a loader to load the asset";
        public const string c_OutOfTimeError = "failed to load the asset in MaxLoadingTime...";
        public const float c_MaxLoadingTime = 20f;
    }
    public class LoadAsset : ILoadAsset
    {
        class Asset
        {
            public string Error { get; set; }
            public string AssetName { get; set; }
            public object Data { get; set; }

            public Asset(string assetName, string error, object asset)
            {
                Error = error;
                AssetName = assetName;
                Data = asset;
            }
        }

        private List<Asset> m_AssetList;
        public LoadAsset()
        {
            m_AssetList = new List<Asset>();
        }

        /// <summary>
        /// Add load result to loadAsset
        /// </summary>
        /// <param name="assetName">load path</param>
        /// <param name="asset">loaded asset</param>
        /// <param name="error">error when loading</param>
        /// <returns>if loadPath exists in list already, return false</returns>
        public bool AddAsset(string assetName, object asset, string error)
        {
            for(int i = 0; i < m_AssetList.Count; ++i)
            {
                if(m_AssetList[i].AssetName == assetName)
                {
                    return false;
                }
            }
            m_AssetList.Add(new Asset(assetName, error, asset));
            return true;
        }

        /// <summary>
        /// Try get the asset with the path
        /// </summary>
        /// <param name="assetName">asset path</param>
        /// <param name="asset">the loaded asset</param>
        /// <returns>error code! when loaded success, it's null or empty.</returns>
        public string TryGetAssetByName(string assetName, out object asset)
        {
            for(int i = 0; i < m_AssetList.Count; ++i)
            {
                if(m_AssetList[i].AssetName == assetName)
                {
                    asset = m_AssetList[i].Data;
                    return m_AssetList[i].Error;
                }
            }
            asset = null;
            return LoadConf.c_NoSuchLoaderError;
        }
        /// <summary>
        /// Dispose the refrence of the loaded assets
        /// </summary>
        public void Dispose()
        {
            m_AssetList.Clear();
        }
    }
}