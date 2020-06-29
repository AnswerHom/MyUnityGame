using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理器基类
/// </summary>
public class BaseManager<T> where T:new() {
    private static T _instance;
	
    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = new T();
        }
        return _instance;
    }
}
