using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// tank��Ϊ����ˮ��ĽŲ�
/// </summary>
public class Tank : MonoBehaviour
{
	private static Tank _instance;
	public static Tank instance;
	private Transform _tr;

	/// <summary>
	///  ˮ��ĳ����
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


