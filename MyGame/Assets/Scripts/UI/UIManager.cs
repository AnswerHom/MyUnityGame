using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UILayer
{
    BOTTOM,
    MIDDLE,
    TOP,
    SYSTEM
}

public class UIManager : BaseManager<UIManager> {

    public Transform canvas;
    public Transform bottom;
    public Transform middle;
    public Transform top;
    public Transform system;

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    public UIManager()
    {
        GameObject obj = ResManager.GetInstance().LoadRes<GameObject>("UI/Canvas");
        canvas = obj.transform;
        bottom = canvas.Find("Bottom").transform;
        middle = canvas.Find("Middle").transform;
        top = canvas.Find("Top").transform;
        system = canvas.Find("System").transform;
        obj = ResManager.GetInstance().LoadRes<GameObject>("UI/EventSystem");
    }

    /// <summary>
    /// 通过层级枚举 得到对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(UILayer layer)
    {
        switch (layer)
        {
            case UILayer.BOTTOM:
                return bottom;
            case UILayer.MIDDLE:
                return middle;
            case UILayer.TOP:
                return top;
            case UILayer.SYSTEM:
                return system;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="layer"></param>
    /// <param name="func"></param>
    public void ShowPanel<T>(string name, UILayer layer,UnityAction<T> func = null) where T:BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            if (func != null)
            {
                func(panelDic[name] as T);
            }
            panelDic[name].Show();
        }
        else
        {
            ResManager.GetInstance().LoadResAsync<GameObject>("UI/" + name, (obj) =>
            {
                Transform parent = GetLayerFather(layer);
                obj.transform.SetParent(parent);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                T panel = obj.GetComponent<T>();
                if (func != null)
                {
                    func(panel);
                }
                panel.Show();
                panelDic.Add(name, panel);
            });
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="name"></param>
    public void HidePanel(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            panelDic[name].Hide();
            GameObject.Destroy(panelDic[name].gameObject);
            panelDic.Remove(name);
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }
}
