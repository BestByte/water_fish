using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System; //Action所在的命名空间

public class EventMgr<T> where T : struct
{
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
