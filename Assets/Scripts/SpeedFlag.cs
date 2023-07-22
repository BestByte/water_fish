using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
[Serializable]
public class SpeedFlag
{
	private Transform _tr;

	[SerializeField]
	private float _curSpeed;    // ��ǰ�ٶ�
	[SerializeField]
	private float _usedTime;    // ��ʹ�õ�ʱ��
	[SerializeField]
	private float _totalTime; // ��ʱ��
	[SerializeField]
	private float _accTime; // �����˶�ʱ��
	[SerializeField]
	private float _decTime; // �����˶�ʱ��
	[SerializeField]
	private float _avgTime; // �����˶�ʱ��
	[SerializeField]
	private bool _varSpeeding = false; // ��ǰ�Ƿ����ڽ��б���

	private Action _callback; // ���ʱ��Ļص�

	[SerializeField]
	private float _speedUpProgress = 0.3f; // ���ٽ׶�ռ����ʱ��
	[SerializeField]
	private float _speedDownProgress = 0.3f; // ���ٽ׶�ռ����ʱ��
	[SerializeField]
	private float _accSpeed = 0; // ���ٶ�
	[SerializeField]
	private float _decSpeed = 0; // ���ٶ�
	[SerializeField]
	private float _maxSpeed = 0;    // ����ٶ�
	[SerializeField]
	private float _minSpeed = 0f;   //��С�ٶ�

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
		_callback = callback;
		_totalTime = totalTime;
		_usedTime = 0f;
		// ����ʱ��
		_accTime = _totalTime * _speedUpProgress;
		// ����ʱ��
		_decTime = _totalTime * _speedDownProgress;
		// ����ʱ��
		_avgTime = _totalTime - _accTime - _decTime;

		_maxSpeed = (2f * distance - _curSpeed * _accTime - _minSpeed * _decTime) / (2f * totalTime - _accTime - _decTime);
		_accSpeed = (_maxSpeed - _curSpeed) / _accTime;
		_decSpeed = (_maxSpeed - _minSpeed) / _decTime;
	}

	public void Update(float deltaTime)
	{
		if (_varSpeeding)
		{
			_varSpeed(deltaTime);
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
			else // ��ǰdeltaTime����ʣ����Ҫ���ٵ�ʱ��
			{
				_curSpeed += _accTime * _accSpeed;
				deltaTime -= _accTime; // deltaTime����ʣ��
				_accTime = 0;
				_varSpeed(deltaTime); // ����ʣ��deltaTime
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
				deltaTime -= _avgTime; // deltaTime����ʣ��
				_avgTime = 0;
				_varSpeed(deltaTime); // ����ʣ��deltaTime
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
