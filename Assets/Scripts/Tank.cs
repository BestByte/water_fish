using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


