using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    Dictionary<string, BaseResource>[] m_ResourceDics;
    LoadManager m_LoadManager;
    public void Init()
    {
        int count = (int)EResourceType.COUNT;
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

    


    public ResourceTask ApplyResources(string[] paths, OnTaskCompelte onTaskComplete, EResourceType mgrType)
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

    //remove the res from resource map, and dispose it...
    void OnResourceNoRef(BaseResource res)
    {
        Dictionary<string, BaseResource> resDic = GetResourceDic(res.ResType);
        if (resDic.ContainsKey(res.ResKey))
        {
            resDic.Remove(res.ResKey);
        }
        res.Dispose();
    }

    // get resource from dictionary
    // if failed, create one and assign it a loader
    BaseResource GetBaseResource(string resKey, EResourceType mgrTyep, int priority = 0)
    {
        Dictionary<string, BaseResource> resDic = GetResourceDic(mgrTyep);
        BaseResource res;
        if (resDic.TryGetValue(resKey, out res))
        {

        }
        else
        {
            res = new BaseResource(resKey, mgrTyep);
            res.AppednResourceNoRefNotify(OnResourceNoRef);
        }

        resDic[resKey] = res;
        if(res.NeedLoad)
        {
            Loader loader = m_LoadManager.CreateLoader(resKey, priority);
            loader.AppendCompleteNotify(res.FillResource);            
        }
        return res;
    }

    Dictionary<string, BaseResource> GetResourceDic(EResourceType mgrType)
    {
        return m_ResourceDics[(int)mgrType];
    }



}
