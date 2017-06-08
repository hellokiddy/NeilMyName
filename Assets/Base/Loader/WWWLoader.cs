using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class WWWLoader : AbstractLoader
    {
        private WWW m_WWW;
        public override ELoaderType LoaderType { get { return ELoaderType.WWW_LOADER; } }

        //public WWWLoader():base()
        //{            
        //}

        public override void Begin()
        {
            //if inited failed, you will not need to create a www.
            if(m_IsDone == false)
            {
                m_WWW = new WWW(m_Url);
            }
        }

        public override void Update(float deltaTime)
        {
            if (m_IsDone == false)
            {
                //set a max loading time to force complete the loader...
                m_LoadingTime += Time.deltaTime;
                if (LoadConf.c_MaxLoadingTime > 0 && m_LoadingTime >= LoadConf.c_MaxLoadingTime)
                {
                    m_IsDone = true;
                    m_Error = LoadConf.c_OutOfTimeError;
                }

                //when www ready, complete the loader...
                if (m_WWW.isDone)
                {
                    m_IsDone = true;
                    m_Error = m_WWW.error;
                    if (string.IsNullOrEmpty(m_Error))
                    {
                        m_Data = m_WWW.assetBundle;
                    }
                }
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}