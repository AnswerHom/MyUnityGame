using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResManager : BaseManager<ResManager> {
    
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadRes<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="func"></param>
    public void LoadResAsync<T>(string path, UnityAction<T> func) where T : Object
    {
        MonoManager.GetInstance().StartCoroutine(ReallyLoadResAsync(path, func));
    }

    private IEnumerator ReallyLoadResAsync<T>(string path, UnityAction<T> func) where T:Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        if(r.asset is GameObject)
            func(GameObject.Instantiate(r.asset) as T);
        else
            func(r.asset as T);
    }
}
