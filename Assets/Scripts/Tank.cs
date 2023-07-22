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
	private void OnDrawGizmos()
	{
		if (this.enabled)
		{
			if (_tr == null)
			{
				_tr = this.transform;
			}
			//记录颜色
			Color c = Gizmos.color;
			Gizmos.color = Color.green;
			//记录matrix
			Matrix4x4 m = Gizmos.matrix;
			//赋值为transform的matrix
			Gizmos.matrix = _tr.localToWorldMatrix;
			//绘制线框立方体
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, depth));
			//恢复matirx和color
			Gizmos.matrix = m;
			Gizmos.color = c;
		}
	}

}


