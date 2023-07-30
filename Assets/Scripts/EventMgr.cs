using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //Action���ڵ������ռ�

/// <summary>
/// ����Ҫ�ĵط� ��Ӽ����¼�
/// �ڲ���Ҫ�����ĵط����Ƴ������¼�
/// ���¼������ĵط������ô����¼�
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventMgr<T> where T : struct
{
	/// <summary>
	/// ����ģʽ
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
	/// ֱ��ʹ���޷���ֵ���͵�ί��action
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
