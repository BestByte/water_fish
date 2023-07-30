using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// tank作为管理水箱的脚步
/// </summary>
public class Tank : MonoBehaviour
{
	private static Tank _instance;
	public static Tank instance
	{
		get
		{
			return _instance;
		}
	}
	/// <summary>
	/// 水箱的位置，是鱼缸的中心
	/// </summary>
	private Transform _tr;

	/// <summary>
	///水箱的长宽高
	/// </summary>
	public float width;
	public float height;
	public float depth;

	private float _minWidthPos;
	private float _maxWidthPos;
	private float _minHeightPos;
	private float _maxHeightPos;
	private float _minDepthPos;
	private float _maxDepthPos;

	public float minWidthPos
	{
		get
		{
			return _minWidthPos;
		}
	}
	public float maxWidthPos
	{
		get
		{
			return _maxWidthPos;
		}
	}
	public float minHeightPos
	{
		get
		{
			return _minHeightPos;
		}
	}
	public float maxHeightPos
	{
		get
		{
			return _maxHeightPos;
		}
	}
	public float minDepthPos
	{
		get
		{
			return _minDepthPos;
		}
	}
	public float maxDepthPos
	{
		get
		{
			return _maxDepthPos;
		}
	}
	/// <summary>
	/// 计算边界的时候，使用水箱长宽高的一半
	/// </summary>
	private void Awake()
	{
		_instance = this;
		_tr = transform;

		_minWidthPos = _tr.position.x - width * 0.5f;
		_maxWidthPos = _tr.position.x + width * 0.5f;
		_minHeightPos = _tr.position.y - height * 0.5f;
		_maxHeightPos = _tr.position.y + height * 0.5f;
		_minDepthPos = _tr.position.z - depth * 0.5f;
		_maxDepthPos = _tr.position.z + depth * 0.5f;
	}
	/// <summary>
	/// 鱼的出生点为随机位置
	/// </summary>
	/// <returns></returns>
	public float RandomX()
	{
		return Random.Range(_minWidthPos, _maxWidthPos);

	}
	/// <summary>
	/// 鱼的出生点为随机位置
	/// </summary>
	/// <returns></returns>
	public float RandomY()
	{
		return Random.Range(_minHeightPos, _maxHeightPos);

	}
	/// <summary>
	/// 鱼的出生点为随机位置
	/// </summary>
	/// <returns></returns>
	public float RandomZ()
	{
		return Random.Range(_minDepthPos, _maxDepthPos);

	}
	/// <summary>
	/// 鱼游动的范围只在鱼缸内
	/// 当 xyz小于最小值或大于最大值的时候，都不在鱼缸内
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public bool InTank(Vector3 pos)
	{
		if (pos.x < _minWidthPos || pos.x > _maxWidthPos)
		{
			return false;
		}
		if (pos.y < _minHeightPos || pos.y > _maxHeightPos)
		{
			return false;
		}
		if (pos.z < _minDepthPos || pos.z > _maxDepthPos)
		{
			return false;
		}
		return true;
	}
	/// <summary>
	/// 当xyz小于最小值的时候，就设置xyz为最小值
	/// 当xyz大于最大值的时候，就设置为最大值
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public Vector3 InTankPos(Vector3 pos)
	{
		if (pos.x < _minWidthPos)
		{
			pos.x = _minWidthPos;
		}
		else if (pos.x > _maxWidthPos)
		{
			pos.x = _maxWidthPos;
		}

		if (pos.y < _minHeightPos)
		{
			pos.y = _minHeightPos;
		}
		else if (pos.y > _maxHeightPos)
		{
			pos.y = _maxHeightPos;
		}
		if (pos.z < _minDepthPos)
		{
			pos.z = _minDepthPos;
		}
		else if (pos.z > _maxDepthPos)
		{
			pos.z = _maxDepthPos;
		}
		return pos;
	}
	/// <summary>
	/// 水箱的Gizmos绘制线框立方体
	/// </summary>
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

