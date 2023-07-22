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

	/// <summary>
	///  水箱的长宽高
	/// </summary>
	public float width;
	public float height;
	public float depth;


	private void Awake()
	{
		_instance = this;
		_tr = transform;

	}
}


