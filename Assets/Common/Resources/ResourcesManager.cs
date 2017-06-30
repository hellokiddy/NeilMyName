using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//resource manager type
//easy to find reqiured resource
//start from 0 end with Count
public enum EMgrType
{


    COUNT,
}

public class ResourcesManager
{
    Dictionary<string, BaseResource>[] m_ResourceDics;
    LoadManager m_LoadManager;
    public void Init()
    {
        int count = (int)EMgrType.COUNT;
        m_ResourceDics = new Dictionary<string, BaseResource>[count];
        for(int i = 0; i < count; ++i)
        {
            m_ResourceDics[i] = new Dictionary<string, BaseResource>();
        }

        m_LoadManager = new LoadManager();
        m_LoadManager.Init();
    }

    public void Update(float deltaTime)
    {
        m_LoadManager.Update(deltaTime);
    }

    Dictionary<string, BaseResource> GetResourceDic(EMgrType mgrType)
    {
        return m_ResourceDics[(int)mgrType];
    }


    public ResourceTask ApplyResources(string[] paths, OnTaskCompelte onTaskComplete, EMgrType mgrType)
    {
        ResourceTask task = AllocateTask();
        task.AppendCompleteNotify(onTaskComplete);
        
        for(int i =  0; i < paths.Length; ++i)
        {
            BaseResource res = GetBaseResource(paths[i], mgrType, task.Priority);
            task.AddResource(res);
        }

        task.Ready();//the task is prepared ready...

        return task;
    }

    ResourceTask AllocateTask()
    {
        return new ResourceTask();
    }

    // get resource from dictionary
    // if failed, create one and assign it a loader
    BaseResource GetBaseResource(string resKey, EMgrType mgrTyep, int priority = 0)
    {
        Dictionary<string, BaseResource> resDic = GetResourceDic(mgrTyep);
        BaseResource res;
        if (resDic.TryGetValue(resKey, out res))
        {
            return res;
        }
        else
        {
            res = new BaseResource(resKey);
            Loader loader = m_LoadManager.CreateLoader(resKey, priority);
            loader.AppendCompleteNotify(res.FillResource);

            return res;
        }
    }



    
        
}
