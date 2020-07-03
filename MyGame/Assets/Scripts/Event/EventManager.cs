using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}

public class EventInfo<T>: IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}


public class EventManager : BaseManager<EventManager>
{
    public static string EVENT_KEY_DOWN = "按键按下";
    public static string EVENT_KEY_UP = "按键抬起";
    public static string EVENT_MOUSE_DOWN = "鼠标按下";
    public static string EVENT_MOUSE_UP = "鼠标抬起";
    public static string EVENT_MAIN_PLAYER = "主玩家初始化";

    private Dictionary<string, IEventInfo> eventDic;

    public EventManager()
    {
        eventDic = new Dictionary<string, IEventInfo>();
    }

    /// <summary>
    /// 开启监听(有参数)
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">委托</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 开启监听(无参数)
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">委托</param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }

    /// <summary>
    /// 移除监听(有参数)
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">委托</param>
    public void RemoveEventListener<T>(string name ,UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }

    /// <summary>
    /// 移除监听(无参数)
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">委托</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// 事件触发(有参数)
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger<T>(string name , T info)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发(无参数)
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions.Invoke();
        }
    }
}
