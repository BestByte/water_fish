using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼能够游动起来
/// 鱼需要有一个出生位置
/// 出生位置要在鱼缸里面
/// update方法中，更新鱼的位置，让鱼游到目标位置
/// </summary>
public class FishAI : MonoBehaviour
{
	/// <summary>
	/// 定义状态
	/// </summary>
	public enum ActionState { Swimming, Float, Escape, Feed, FeedOver }
	/// <summary>
	/// 鱼的状态
	/// </summary>
	public enum FishEvent
	{
		Feed,
		FoodDestroy,
	}
	/// <summary>
	/// 鱼的transform的引用
	/// </summary>
	private Transform _tr;

	/// <summary>
	/// 目标位置
	/// </summary>
	[SerializeField]
	private Vector3 tarPos;
	/// <summary>
	/// 目标方向
	/// </summary>
	[SerializeField]
	private Vector3 tarDir;
	/// <summary>
	/// 目标速度
	/// </summary>
	[SerializeField]
	private float tarSpeed;
	/// <summary>
	/// 移动的目标时间
	/// </summary>
	[SerializeField]
	private float tarTime;
	/// <summary>
	/// 当前的运行时间
	/// </summary>
	[SerializeField]
	private float curTime;
	/// <summary>
	/// speedflag的初始化
	/// </summary>
	[SerializeField]
	private SpeedFlag speed;
	[SerializeField]
	private RotateFlag rota;
	[SerializeField]
	private MoveFlag move;
	[SerializeField]
	private AnimFlag anim;
	[SerializeField]
	private FeedFlag feed;

	private FoodAI _foodTarget;


	public ActionState _curState;
	public ActionState _lastState;

	public float findFoodRange = 20f;


	void Start()
	{
		//喂食事件的监听
		EventMgr<FishEvent>.instance.AddListener(FishEvent.Feed, OnFeed);
		//获得transform的位置
		_tr = transform;
		speed = new SpeedFlag(_tr);
		rota = new RotateFlag(_tr);
		move = new MoveFlag(_tr);
		anim = new AnimFlag(_tr);
		feed = new FeedFlag(_tr);
		feed.feedDuration = 5f;
		RandomBorn();
		Swimming();
	}

	private void OnDestroy()
	{
		EventMgr<FishEvent>.instance.RemListener(FishEvent.Feed, OnFeed);
	}
	/// <summary>
	/// 随机出生的位置，将AI的位置，随机到鱼缸内的一个点
	/// </summary>
	private void RandomBorn()
	{
		_tr.position = new Vector3(Tank.instance.RandomX(), Tank.instance.RandomY(), Tank.instance.RandomZ());
		tarDir = _tr.transform.forward;
	}
	void Update()
	{
		float deltaTime = Time.deltaTime;
		feed.Update(deltaTime);
		speed.Update(deltaTime); // 每帧更新
		rota.Update(deltaTime);
		move.SetSpeed(speed.curSpeed);
		move.Update(deltaTime);
		anim.SetSpeed(speed.curSpeed);
		anim.Update(deltaTime);
	}
	private void Swimming()
	{
		_lastState = _curState;
		_curState = ActionState.Swimming;

		float tarX = Tank.instance.RandomX();
		float tarZ = Tank.instance.RandomZ();
		float angle = Random.Range(-35, 35);
		float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
		float tarY = tarX * tanAngle;
		if (tarY < Tank.instance.minHeightPos)
		{
			tarY = Tank.instance.minHeightPos;
		}
		else if (tarY > Tank.instance.maxHeightPos)
		{
			tarY = Tank.instance.maxHeightPos;
		}

		tarPos = new Vector3(tarX, tarY, tarZ);
		tarDir = tarPos - _tr.position;
		float tarDistance = tarDir.magnitude; // 目标距离
		tarDir.Normalize();
		tarTime = Random.Range(5f, 10f);
		//        tarSpeed = Random.Range(5f, 10f);
		speed.SetVarMinSpeed(Random.Range(0f, 2f));
		speed.SetVarSpeedConfig(Random.Range(0.1f, 0.5f), Random.Range(0.2f, 0.5f));
		speed.StartVarSpeed(tarDistance, tarTime, StartAction);

		float turnSpeed = Random.Range(2f, 5f);
		rota.SetRotate(turnSpeed, tarDir);

		move.Move(tarDir, Swimming);

		curTime = 0f;
	}

	// 漂浮
	private void Float()
	{
		_lastState = _curState;
		_curState = ActionState.Float;
		float turnSpeed = Random.Range(2f, 5f);
		float dis = Random.Range(1, 2);
		tarDir.y = 0;
		Vector3 tarPos = _tr.position + tarDir * dis;
		float tarDistance = 0;
		if (Tank.instance.InTank(tarPos))
		{
			tarDistance = dis;
		}
		else
		{
			tarPos = Tank.instance.InTankPos(tarPos);
			tarDistance = Vector3.Distance(_tr.position, tarPos);
		}
		speed.StartToTarSpeed(Random.Range(-1f, 1f), Random.Range(0.5f, 2f));
		rota.SetRotate(turnSpeed, tarDir);
		move.MoveByTime(Random.Range(2f, 5f), tarDir, StartAction);
	}
	private void StartAction()
	{
		if (Random.Range(0, 10) > 3)
		{

			Swimming();
		}
		else
		{
			Float();
		}
	}
	public void Escape()
	{
		_lastState = _curState;
		_curState = ActionState.Escape;

		if (Random.Range(0, 5) > 2)
		{
			Escape_SpeedUp();
		}
		else
		{
			Escape_Turn();
		}
	}
	private void Escape_SpeedUp()
	{
		float dis = Random.Range(10f, 20f);
		tarPos = _tr.position + tarDir * dis;
		Tank.instance.InTankPos(tarPos);
		tarDir = tarPos - _tr.position;
		tarDir.Normalize();
		float tarDistance = (tarPos - _tr.position).magnitude;

		float turnSpeed = Random.Range(4f, 10f);
		tarTime = Random.Range(1f, 3f);
		tarDir.Normalize();
		speed.SetVarMinSpeed(Random.Range(0f, 2f));
		speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
		speed.StartVarSpeed(tarDistance, tarTime, StartAction);
		rota.SetRotate(turnSpeed, tarDir);
		move.Move(tarDir, StartAction);
	}
	private void Escape_Turn()
	{
		float turnSpeed = Random.Range(4f, 10f);

		float tarX = Tank.instance.RandomX();
		float tarZ = Tank.instance.RandomZ();
		float angle = Random.Range(-35, 35);
		float tanAngle = Mathf.Tan(angle * Mathf.Deg2Rad);
		float tarY = tarX * tanAngle;
		if (tarY < Tank.instance.minHeightPos)
		{
			tarY = Tank.instance.minHeightPos;
		}
		else if (tarY > Tank.instance.maxHeightPos)
		{
			tarY = Tank.instance.maxHeightPos;
		}

		tarTime = Random.Range(3f, 5f);
		tarPos = new Vector3(tarX, tarY, tarZ);
		tarDir = tarPos - _tr.position;
		float tarDistance = tarDir.magnitude;
		tarDir.Normalize();
		speed.SetVarMinSpeed(Random.Range(0f, 2f));
		speed.SetVarSpeedConfig(Random.Range(0.02f, 0.1f), Random.Range(0.2f, 0.5f));
		speed.StartVarSpeed(tarDistance, tarTime, StartAction);
		rota.SetRotate(turnSpeed, tarDir);
		move.Move(tarDir);
	}

	private void Feed()
	{
		_lastState = _curState;
		_curState = ActionState.Feed;
		feed.StartFeed(_foodTarget, FeedOver);
		speed.StartSpeedUp(Random.Range(3f, 10f), Random.Range(6f, 10f));

		float turnSpeed = Random.Range(4f, 10f);
		rota.SetRotateToTarget(turnSpeed, _foodTarget.transform);
		move.MoveToDynamicTarget(_foodTarget.transform, null);
	}
	private void FeedOver(bool getFood)
	{
		_lastState = _curState;
		_curState = ActionState.FeedOver;
		speed.StartSpeedDown(Random.Range(1f, 4f), Random.Range(1f, 2f), null);
		float turnSpeed = Random.Range(4f, 10f);
		tarDir = _tr.forward;
		tarDir.Normalize();
		rota.SetRotate(turnSpeed, tarDir);
		move.Move(tarDir, StartAction);
	}

	private void OnFeed(object[] param)
	{

		if (feed.isHungry && !feed.feeding)
		{
			// 寻找距离最近的食物，并且冲过去
			float dis;
			FoodMgr.instance.Nearest(_tr.position, out _foodTarget, out dis);
			if (_foodTarget != null)
			{
				if (dis <= findFoodRange)
				{
					Feed();

				}
				else
				{
					_foodTarget = null;
				}
			}
		}
	}

}

