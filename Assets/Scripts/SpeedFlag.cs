using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 专注于速度
/// </summary>

[Serializable]
public class SpeedFlag
{
	/// <summary>
	/// 拿到transform的引用
	/// </summary>
	private Transform _tr;

	[SerializeField]
	private float _curSpeed;    // 当前速度
	[SerializeField]
	private float _usedTime;    // 已使用的时间
	[SerializeField]
	private float _totalTime; // 总时长
	[SerializeField]
	private float _accTime; // 加速运动时间
	[SerializeField]
	private float _decTime; // 减速运动时间
	[SerializeField]
	private float _avgTime; // 匀速运动时间
	[SerializeField]
	private bool _varSpeeding = false; // 当前是否正在进行变速
	[SerializeField]
	private bool _isToTarSpeed = false; // 当前是否正在向目标速度改变
	[SerializeField]
	private bool _isSpeedUp = false;
	[SerializeField]
	private bool _isSpeedDown = false;

	private Action _callback; // 完成时候的回调

	[SerializeField]
	private float _speedUpProgress = 0.3f; // 加速阶段占的总时长
	[SerializeField]
	private float _speedDownProgress = 0.3f; // 减速阶段占的总时长
	[SerializeField]
	private float _accSpeed = 0; // 加速度
	[SerializeField]
	private float _decSpeed = 0; // 减速度
	[SerializeField]
	private float _maxSpeed = 0;    // 最大速度
	[SerializeField]
	private float _minSpeed = 0f;   //最小速度

	public float curSpeed
	{
		get
		{
			return _curSpeed;
		}
	}
	public float totalTime
	{
		get
		{
			return _totalTime;
		}
	}
	public float usedTime
	{
		get
		{
			return _usedTime;
		}
	}
	public SpeedFlag(Transform tr)
	{
		_tr = tr;
	}
	public void SetVarSpeedConfig(float speedUpProgress, float speedDownProfress)
	{
		_speedUpProgress = speedUpProgress;
		_speedDownProgress = speedDownProfress;
	}
	public void SetVarMinSpeed(float minSpeed)
	{
		_minSpeed = minSpeed;
	}
	public void StartVarSpeed(float distance, float totalTime, Action callback)
	{
		_varSpeeding = true;
		_isToTarSpeed = false;
		_isSpeedUp = false;
		_isSpeedDown = false;
		_callback = callback;
		_totalTime = totalTime;
		_usedTime = 0f;
		// 加速时间
		_accTime = _totalTime * _speedUpProgress;
		// 减速时间
		_decTime = _totalTime * _speedDownProgress;
		// 匀速时间
		_avgTime = _totalTime - _accTime - _decTime;

		_maxSpeed = (2f * distance - _curSpeed * _accTime - _minSpeed * _decTime) / (2f * totalTime - _accTime - _decTime);
		_accSpeed = (_maxSpeed - _curSpeed) / _accTime;
		_decSpeed = (_maxSpeed - _minSpeed) / _decTime;
	}

	/// <summary>
	/// 目标速度
	/// </summary>
	public void StartToTarSpeed(float tarSpeed, float time, Action cb = null)
	{
		_isToTarSpeed = true;
		_varSpeeding = false;
		_isSpeedUp = false;
		_isSpeedDown = false;
		_maxSpeed = tarSpeed;
		_totalTime = time;
		_callback = cb;
	}


	public void StartSpeedUp(float accSpeed, float maxSpeed, Action cb = null)
	{
		_isSpeedUp = true;
		_varSpeeding = false;
		_isSpeedDown = false;
		_isToTarSpeed = false;
		_accSpeed = accSpeed;
		_maxSpeed = maxSpeed;
		_callback = cb;

	}
	public void StartSpeedDown(float decSpeed, float minSpeed, Action cb = null)
	{
		_isSpeedUp = false;
		_varSpeeding = false;
		_isSpeedDown = true;
		_isToTarSpeed = false;
		_decSpeed = decSpeed;
		_minSpeed = minSpeed;
		_callback = cb;

	}

	public void Update(float deltaTime)
	{
		if (_varSpeeding)
		{
			_varSpeed(deltaTime);
		}
		else if (_isToTarSpeed)  // 目标速度
		{
			_toTarSpeed(deltaTime);
		}
		else if (_isSpeedUp)
		{
			_speedUp(deltaTime);
		}
		else if (_isSpeedDown)
		{
			_speedDown(deltaTime);
		}
	}
	private void _varSpeed(float deltaTime)
	{
		_usedTime = _usedTime + deltaTime;
		if (_accTime > 0)
		{
			if (_accTime > deltaTime)
			{
				_curSpeed += deltaTime * _accSpeed;
				_accTime -= deltaTime;
			}
			else // 当前deltaTime大于剩余需要加速的时间
			{
				_curSpeed += _accTime * _accSpeed;
				deltaTime -= _accTime; // deltaTime还有剩余
				_accTime = 0;
				_varSpeed(deltaTime); // 处理剩余deltaTime
			}
		}
		else if (_avgTime > 0)
		{
			if (_avgTime > deltaTime)
			{
				_avgTime -= deltaTime;
			}
			else
			{
				deltaTime -= _avgTime; // deltaTime还有剩余
				_avgTime = 0;
				_varSpeed(deltaTime); // 处理剩余deltaTime
			}
		}
		else if (_decTime > 0)
		{
			if (_decTime > deltaTime)
			{
				_curSpeed -= deltaTime * _decSpeed;
				_decTime -= deltaTime;
			}
			else
			{
				_curSpeed -= _decTime * _decSpeed;
				deltaTime -= _decTime;
				_decTime = 0;
				_varSpeeding = false;
				_DoCallback();
			}
		}
	}

	private void _toTarSpeed(float deltaTime)
	{
		if (Mathf.Abs(_curSpeed - _maxSpeed) < 1e-5)
		{
			_isToTarSpeed = false;
			return;
		}
		if (_totalTime > deltaTime)
		{
			_curSpeed = Mathf.Lerp(_curSpeed, _maxSpeed, deltaTime / _totalTime);
			_totalTime -= deltaTime;
		}
		else
		{
			_curSpeed = _maxSpeed;
			_isToTarSpeed = false;
			_DoCallback();
		}

	}
	private void _speedUp(float deltaTime)
	{
		if (_curSpeed < _maxSpeed)
		{
			_curSpeed += _accSpeed * deltaTime;
		}
		else
		{
			_curSpeed = _maxSpeed;
			_isSpeedUp = false;
			_DoCallback();
		}

	}
	private void _speedDown(float deltaTime)
	{
		if (_curSpeed > _minSpeed)
		{
			_curSpeed -= _decSpeed * deltaTime;
		}
		else
		{
			_curSpeed = _minSpeed;
			_isSpeedDown = false;
			_DoCallback();
		}

	}
	private void _DoCallback()
	{
		if (_callback != null)
		{
			Action cb = _callback;
			_callback = null;
			cb();
		}
	}
}

