using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FishAI : MonoBehaviour
{

	private Transform _tr;
	[SerializeField]
	private Vector3 tarPos;
	[SerializeField]
	private Vector3 tarDir;
	[SerializeField]
	private float tarSpeed;
	[SerializeField]
	private float tarTime;
	[SerializeField]
	private float curTime;

	[SerializeField]
	private SpeedFlag speed;

	[SerializeField]
	private RotateFlag rota;
	// ...

	[SerializeField]
	private MoveFlag move;
	// ...
	[SerializeField]
	private AnimFlag anim;


	public enum ActionState { Swimming, Float, Escape }
	public ActionState _curState;
	public ActionState _lastState;

	private void Swimming()
	{
		_lastState = _curState;
		_curState = ActionState.Swimming;
		
    }

	void Update()
	{

		float deltaTime = Time.deltaTime;
		speed.Update(deltaTime); // 每帧更新
		rota.Update(deltaTime);
		move.SetSpeed(speed.curSpeed);
		move.Update(deltaTime);
		anim.SetSpeed(speed.curSpeed);
		anim.Update(deltaTime);
	}


	void Start()
	{
		_tr = transform;
		speed = new SpeedFlag(_tr);
		RandomBorn();
		GetTarget();

		rota = new RotateFlag(_tr);

		
		// ...
		move = new MoveFlag(_tr);
		// ...

		anim = new AnimFlag(_tr);
	}

	private void RandomBorn()
	{
		_tr.position = new Vector3(Tank.instance.RandomX(), Tank.instance.RandomY(), Tank.instance.RandomZ());
		tarDir = _tr.transform.forward;
	}
	private void GetTarget()
	{
		move.Move(tarDir, GetTarget);

		float turnSpeed = Random.Range(2f, 5f);
		rota.SetRotate(turnSpeed, tarDir);


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
		speed.StartVarSpeed(tarDistance, tarTime, GetTarget);
		curTime = 0f;
	}
	
}
