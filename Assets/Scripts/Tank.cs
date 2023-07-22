using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// tank作为管理水箱的脚步
/// </summary>
public class Tank : MonoBehaviour
{
	private static Tank _instance;
	public static Tank instance;
	private Transform _tr;


	private void Awake()
	{
		_instance = this;
		_tr = transform;

	}
}


