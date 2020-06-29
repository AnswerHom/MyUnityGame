using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : BaseManager<MonoManager> {

    private MonoController controller;

	public MonoManager () {
        //添加MonoController脚本
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }

    /// <summary>
    /// 协程
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
}
