using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{

	public static CameraMgr instance;

	public Camera cam;
	protected void Awake()
	{
		instance = this;
		cam = this.GetComponent<Camera>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray r = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(r, out hitInfo, 100f))
			{
				FishAI ai = hitInfo.transform.GetComponent<FishAI>();
				ai.Escape();
			}
		}
	}
}
