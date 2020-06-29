using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesManager : BaseManager<ScenesManager> {

	public void LoadScene(string name,UnityAction func)
    {
        SceneManager.LoadScene(name);
        func();
    }

    public void LoadSceneAsync(int idx,UnityAction func)
    {
        MonoManager.GetInstance().StartCoroutine(OnLoadScene(idx, func));
    }

    private static IEnumerator OnLoadScene(int idx, UnityAction func)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(idx);
        while (!ao.isDone) { 
            EventManager.GetInstance().EventTrigger("进度条变化", ao.progress);
            yield return ao.progress;
        }
        func();
    }
}
