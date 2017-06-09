using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keedy.Common.Load
{
    public class ResourceLoader : AbstractLoader
    {
        public override ELoaderType LoaderType { get { return ELoaderType.RESOURCE_LOADER; } }

        public override void Begin()
        {
#if TEST_CODE
            m_Data = Resources.Load(GlobalSetting.GetResourcePath(m_Url));
#else
            m_Data = Resources.Load(m_Url);
#endif
            m_IsDone = true;
            m_Error = m_Data == null ? LoadConf.c_LoadError :null;
        }
        public override void Update(float deltaTime)
        {

        }
    }
}