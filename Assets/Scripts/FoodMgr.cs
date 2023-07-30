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
	public List<FoodAI> foodList;

	public void GenFood(Vector3 pos)
	{
		if (foodList == null)
		{
			foodList = new List<FoodAI>();
		}

		GameObject go = GameObject.Instantiate(foodGo);
		go.transform.position = pos;
		foodList.Add(go.GetComponent<FoodAI>());
		EventMgr<FishAI.FishEvent>.instance.TriggerEvent(FishAI.FishEvent.Feed);
	}
	public void DesFood(FoodAI ai)
	{
		foodList.Remove(ai);
	}
	public void Nearest(Vector3 pos, out FoodAI ai, out float dis)
	{
		dis = 9999f;
		ai = null;
		for (int i = 0, imax = foodList.Count; i < imax; ++i)
		{
			FoodAI f = foodList[i];
			if (f != null && f.active)
			{
				float d = Vector3.Distance(pos, f.transform.position);
				if (d < dis)
				{
					ai = f;
					dis = d;
				}
			}
		}
	}
}
