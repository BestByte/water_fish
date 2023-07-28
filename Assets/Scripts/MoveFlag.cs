using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
/// <summary>
/// 移动相关的控制
/// </summary>
[System.Serializable]
public class MoveFlag
{

	private Transform _tr;

	[SerializeField]
	private float _speed;
	[SerializeField]
	private Vector3 _dir;
	[SerializeField]
	private bool _moving = false;

	private Action _callback;
	public bool moving
	{
		get
		{
			return _moving;
		}
	}
	private Vector3 _nowPos
	{
		get
		{
			return _tr.position;
		}
		set
		{
			_tr.position = value;
		}
	}

	public MoveFlag(Transform tr)
	{
		_tr = tr;
	}
	public void SetSpeed(float speed)
	{
		_speed = speed;
	}
	public void Move(Vector3 dir, Action cb = null)
	{
		_moving = true;
		_dir = dir;
		_callback = cb;
	}
	public void Update(float deltaTime)
	{
		if (_moving)
		{
			Vector3 pos = _nowPos + deltaTime * _dir * _speed;
			if (!Tank.instance.InTank(pos))
			{
				_moving = false;
				_DoCallBack();
			}
			else
			{
				_nowPos = pos;
			}
		}
	}
	private void _DoCallBack()
	{
		if (_callback != null)
		{
			Action cb = _callback;
			_callback = null;
			cb();
		}
	}
}
