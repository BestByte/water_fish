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
	private void OnDrawGizmos()
	{
		if (this.enabled)
		{
			if (_tr == null)
			{
				_tr = this.transform;
			}
			//��¼��ɫ
			Color c = Gizmos.color;
			Gizmos.color = Color.green;
			//��¼matrix
			Matrix4x4 m = Gizmos.matrix;
			//��ֵΪtransform��matrix
			Gizmos.matrix = _tr.localToWorldMatrix;
			//�����߿�������
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, depth));
			//�ָ�matirx��color
			Gizmos.matrix = m;
			Gizmos.color = c;
		}
	}

}


