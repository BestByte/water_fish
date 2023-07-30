using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //Action所在的命名空间

/// <summary>
/// 在需要的地方 添加监听事件
/// 在不需要监听的地方，移除监听事件
/// 在事件发生的地方，调用触发事件
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventMgr<T> where T : struct
{
	/// <summary>
	/// 单例模式
	/// </summary>
	private static EventMgr<T> _instance;
	public static EventMgr<T> instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EventMgr<T>();
			}
			return _instance;
		}
	}
	/// <summary>
	/// 直接使用无返回值类型的委托action
	/// </summary>
	private Dictionary<T, Action<object[]>> eventDict;
	public EventMgr()
	{
		eventDict = new Dictionary<T, Action<object[]>>();
	}
	public void AddListener(T eventName, Action<object[]> cb)
	{
		if (eventDict.ContainsKey(eventName))
		{
			eventDict[eventName] += cb;
		}
		else
		{
			eventDict[eventName] = cb;
		}

	}
	public void RemListener(T eventName, Action<object[]> cb)
	{
		if (eventDict.ContainsKey(eventName))
		{
			eventDict[eventName] -= cb;
		}

	}
	public void TriggerEvent(T eventName, params object[] param)
	{
		if (eventDict.ContainsKey(eventName))
		{
			eventDict[eventName](param);
		}
	}
}
