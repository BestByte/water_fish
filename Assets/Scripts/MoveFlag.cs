using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
/// <summary>
/// 移动相关的控制
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	[SerializeField]
	private bool _movingByTime = false;
	[SerializeField]
	private float _moveTime = 0f;
	[SerializeField]
	private bool _moveToDynamicTarget = false;

	[SerializeField]
	private Transform _dynamicDest;

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
		_movingByTime = false;
		_moveToDynamicTarget = false;
		_dir = dir;
		_callback = cb;
	}
	/// <summary>
	/// 如果移动位置超出鱼缸，会callback
	/// </summary>
	public void MoveByTime(float time, Vector3 dir, Action cb = null)
	{
		_movingByTime = true;
		_moving = false;
		_moveToDynamicTarget = false;
		_dir = dir;
		_moveTime = time;
		_callback = cb;

	}
	public void MoveToDynamicTarget(Transform tr, Action callback)
	{
		_movingByTime = false;
		_moving = false;
		_moveToDynamicTarget = true;
		_dynamicDest = tr;
		_callback = callback;
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
		else if (_movingByTime)
		{

			_MoveByTime(deltaTime);
		}

		if (_moveToDynamicTarget)
		{
			_MoveToDynamicTarget(deltaTime);
		}
	}
	private void _MoveByTime(float deltaTime)
	{
		_moveTime -= deltaTime;
		if (_moveTime <= 0)
		{
			_movingByTime = false;
			_DoCallBack();
			return;
		}
		Vector3 pos = _nowPos + deltaTime * _dir * _speed;
		if (!Tank.instance.InTank(pos))
		{
			_movingByTime = false;
			_DoCallBack();
		}
		else
		{
			_nowPos = pos;
		}

	}

	private void _MoveToDynamicTarget(float deltaTime)
	{
		Vector3 dir = _dynamicDest.position - _nowPos;
		float time = Vector3.Distance(_nowPos, _dynamicDest.position) / _speed;
		if (time > deltaTime) // 需要的时间大于一个deltaTime
		{
			_nowPos = Vector3.Lerp(_nowPos, _dynamicDest.position, deltaTime / time);
		}
		else
		{
			_moveToDynamicTarget = false;
			_dynamicDest = null;
			_DoCallBack();
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
