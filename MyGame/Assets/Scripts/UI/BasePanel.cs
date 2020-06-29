using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI面板基类
/// </summary>
public class BasePanel : MonoBehaviour {
    protected Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

	// Use this for initialization
	protected virtual void Awake () {
        FindChildControl<Button>();
        FindChildControl<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; ++i)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }

        return null;
    }

    protected void FindChildControl<T>() where T:UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            T control = controls[i];
            string name = control.gameObject.name;
            if (controlDic.ContainsKey(name))
            {
                controlDic[name].Add(control);
            }else
            {
                controlDic.Add(name, new List<UIBehaviour>() { control });
            }
            //添加点击事件
            if(control is Button)
            {
                (control as Button).onClick.AddListener(()=>
                {
                    OnClick(name);
                });
            }
        }
    }

    protected virtual void OnClick(string name)
    {

    }

    /// <summary>
    /// 显示自己
    /// </summary>
    public virtual void Show()
    {

    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void Hide()
    {

    }
}
