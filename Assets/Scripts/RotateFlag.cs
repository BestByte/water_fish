using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RotateFlag
{
	private Transform _tr;
	private bool _rotating = false;
	private float _rotaSpeed;
	[SerializeField]
	private Vector3 _tarDir;
	[SerializeField]
	private Quaternion _tarQuat;
	public Vector3 curDir
	{
		get
		{
			return _tr.localEulerAngles;
		}
		private set
		{

			_tr.localEulerAngles = value;
		}
	}
	public RotateFlag(Transform tr)
	{
		_tr = tr;
	}
	public void SetRotate(float rotaSpeed, Vector3 tarDir)
	{
		_rotating = true;
		_rotaSpeed = rotaSpeed;
		_tarDir = tarDir;
		_tarQuat = Quaternion.LookRotation(tarDir);
	}
	public void Update(float deltaTime)
	{
		if (_rotating)
		{
			_rotate(deltaTime);
		}
	}
	private void _rotate(float deltaTime)
	{
		if (Mathf.Abs(_rotaSpeed) < 1e-5)
		{
			_rotating = false;
			return;
		}
		if (_tr.localRotation != _tarQuat)
		{
			_tr.localRotation = Quaternion.Lerp(_tr.localRotation, _tarQuat, _rotaSpeed * deltaTime);
		}
		else
		{
			_rotating = false;
		}
	}
	

}
