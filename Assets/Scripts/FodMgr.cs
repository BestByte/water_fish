using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMgr : MonoBehaviour
{
	private static FoodMgr _instance;
	public static FoodMgr instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = new GameObject("FoodMgr");
				_instance = go.AddComponent<FoodMgr>();
				DontDestroyOnLoad(go);
			}
			return _instance;
		}
	}
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			this.gameObject.name = "FoodMgr";
			DontDestroyOnLoad(this.gameObject);
		}
		else if (_instance != this)
		{
			DestroyImmediate(this, true);
		}
	}

	public GameObject foodGo;
	public float downSpeed = 5f;

	public void GenFood(Vector3 pos)
	{
		GameObject go = GameObject.Instantiate(foodGo);
		go.transform.position = pos;
	}
}
