using AStarPathFinding;
using BehaviorTree;
using CommonEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Enemy : Creature
{
	private const float maxBaseMoveSpeed = 5f;
	private const float maxBaseMoveAcceleration = 50f;

	public EnemyStatus CurrentStatus { get; set; }
	protected List<GameObject> AttackList { get; private set; } = new List<GameObject>();

	private GameObject runningAttack;
	private GameObject enemyHpBar;
	private Hero target;
	private LinkedList<Vector2> patrolPosList = new LinkedList<Vector2>();
	private LinkedListNode<Vector2> currentPatrolPos;
	private Selector behaviorTree;

	protected override void Awake()
	{
		base.Awake();
		InitHpBar();
		InitAttackList();
		InitPatrolPosList();
		AddBehaviorTree();
		AddBaseMoveVelocity();
	}

	private void OnEnable()
	{
		OnDie.AddListener(OnEnemyDie);
		OnHpChange.AddListener(OnEnemyHpChange);
	}

	private void OnDisable()
	{
		OnDie.RemoveListener(OnEnemyDie);
		OnHpChange.RemoveListener(OnEnemyHpChange);
	}

	private void Update()
	{
		UpdateTarget();
		UpdateBehaviorTree();

	}

	private void FixedUpdate()
	{
		UpdateBaseMoveVelocity();
	}

	protected virtual void InitAttackList()
	{
		throw new NotImplementedException();
	}

	private void InitPatrolPosList()
	{
		Vector2 currentPos = transform.position;
		Vector2 patrolPos1 = new Vector2(currentPos.x - 2, currentPos.y);
		Vector2 patrolPos2 = new Vector2(currentPos.x + 2, currentPos.y);
		patrolPosList.AddLast(patrolPos1);
		patrolPosList.AddLast(patrolPos2);
		currentPatrolPos = patrolPosList.First;
	}

	private void InitHpBar()
	{
		enemyHpBar = Instantiate(Resources.Load<GameObject>("Prefab/UI/EnemyHpBar"));
		RectTransform rectTransform = enemyHpBar.GetComponent<RectTransform>();
		rectTransform.SetParent(CreatureCanvas.GetComponent<RectTransform>());
		AdjustHpBarPos(rectTransform);
		enemyHpBar.GetComponent<Slider>().maxValue = 1;
		enemyHpBar.GetComponent<Slider>().value = Hp / MaxHp;
	}

	protected virtual void AdjustHpBarPos(RectTransform rectTransform)
	{
		rectTransform.anchoredPosition = new Vector2(0, 0);
	}

	private void AddBehaviorTree()
	{
		behaviorTree = new Selector();

		foreach (GameObject attackObject in AttackList)
		{
			EnemyAttack enemyAttack = attackObject.GetComponent<EnemyAttack>();
			Sequence attackSeq = new Sequence();
			attackSeq.AddChild(new AttackCondition(this, enemyAttack));
			attackSeq.AddChild(new Chase(this, enemyAttack));
			attackSeq.AddChild(new Attack(this, enemyAttack));
			attackSeq.AddChild(new Rest(this, enemyAttack));
			behaviorTree.AddChild(attackSeq);
		}

		Sequence patrolSeq = new Sequence();
		patrolSeq.AddChild(new PatrolCondition(this));
		patrolSeq.AddChild(new Patrol(this));
		behaviorTree.AddChild(patrolSeq);

		behaviorTree.Tick();
	}

	private void UpdateBehaviorTree()
	{
		behaviorTree.Tick();
	}

	private void UpdateTarget()
	{
		if (target == null) return;

		if (target.IsDead())
		{
			target = null;
		}
	}
	public void SeeHero(Hero hero)
	{
		if (hero.IsDead()) return;

		target = hero;
	}

	private void AddBaseMoveVelocity()
	{
		Velocity velocity = new Velocity(Vector2.zero, Vector2.zero, maxBaseMoveAcceleration);
		GetComponent<MoveManager>().AddVelocity("_BaseMove", velocity);
	}

	private void UpdateBaseMoveVelocity()
	{
		Vector3 targetPos;
		BoxCollider2D creatureBlock = transform.Find("CreatureBlock").GetComponent<BoxCollider2D>();
		Vector2 collisionOffset = creatureBlock.offset;
		Vector2 collisionSize = creatureBlock.size;
		if (CurrentStatus == EnemyStatus.Patrol)
		{
			if (CommonFunc.IsNear(transform.position, currentPatrolPos.Value, null))
			{
				currentPatrolPos = currentPatrolPos.Next;
				if (currentPatrolPos == null)
				{
					currentPatrolPos = patrolPosList.First;
				}
			}
			targetPos = currentPatrolPos.Value;
		}
		else if (CurrentStatus == EnemyStatus.Chase)
		{
			Vector3 heroPos = target.transform.position;
			BoxCollider2D heroCollider = target.GetComponent<BoxCollider2D>();
			heroPos.x += heroCollider.offset.x;
			heroPos.y += heroCollider.offset.y;
			GameObject detectRangeObject = runningAttack.transform.Find("DetectRange").gameObject;
			PolygonCollider2D detectRange = detectRangeObject.GetComponent<PolygonCollider2D>();
			Vector3 potentialTargetPos1 = new Vector3(heroPos.x - detectRange.offset.x, heroPos.y - detectRange.offset.y);
			Vector3 potentialTargetPos2 = new Vector3(heroPos.x + detectRange.offset.x, heroPos.y - detectRange.offset.y);

			bool canStandOnPos1 = TileMapManager.CanStandOnPosition(potentialTargetPos1, collisionOffset, collisionSize);
			bool canStandOnPos2 = TileMapManager.CanStandOnPosition(potentialTargetPos2, collisionOffset, collisionSize);
			if ( canStandOnPos1 && canStandOnPos2)
			{
				float distance1 = Vector2.Distance(transform.position, potentialTargetPos1);
				float distance2 = Vector2.Distance(transform.position, potentialTargetPos2);
				targetPos = (distance1 < distance2) ? potentialTargetPos1 : potentialTargetPos2;
			}
			else if (canStandOnPos1 || canStandOnPos2)
			{
				targetPos = canStandOnPos1 ? potentialTargetPos1 : potentialTargetPos2;
			}
			else
			{
				targetPos = heroPos;
			}
		}
		else
		{
			targetPos = transform.position;
		}

		if (!TileMapManager.CanReachDirectly(transform.position, targetPos, collisionOffset, collisionSize))
		{
			targetPos = GetTargetPosByPathfinding(transform.position, targetPos, collisionOffset, collisionSize);
		}

		Vector3 targetVelocity = (targetPos - transform.position).normalized * maxBaseMoveSpeed;
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", targetVelocity);
		GetComponent<Animator>().SetBool("IsMoving", (targetVelocity != Vector3.zero));
		TryFlipX(targetVelocity);
	}

	private Vector3 GetTargetPosByPathfinding(Vector3 beginWorldPos, Vector3 endWorldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		List<Vector3> path = AStar.FindPath(beginWorldPos, endWorldPos, collisionOffset, collisionSize);

		bool canReach = (TileMapManager.CanReachDirectly(beginWorldPos, path[1], collisionOffset, collisionSize));
		return canReach ? path[1] : path[0];
	}

	public void OnIdleAnimationBegin()
	{

	}
	public void OnIdleAnimationEnd()
	{

	}
	public void OnRunAnimationBegin()
	{

	}
	public void OnRunAnimationEnd()
	{

	}
	public void OnAttackAnimationBegin()
	{
		runningAttack.GetComponent<EnemyAttack>().OnAttackBegin();
	}
	public void OnAttackAnimationEnd()
	{
		runningAttack.GetComponent<EnemyAttack>().OnAttackEnd();
		CurrentStatus = EnemyStatus.None;
		runningAttack = null;
	}
	public void OnDieAnimationBegin()
	{
		CurrentStatus = EnemyStatus.Die;
	}

	public void OnDieAnimationEnd()
	{
		CurrentStatus = EnemyStatus.None;
	}

	public bool HaveTarget()
	{
		return (target != null);
	}

	private void OnEnemyDie()
	{
		enemyHpBar.SetActive(false);
	}

	private void OnEnemyHpChange()
	{
		enemyHpBar.GetComponent<Slider>().value = Hp / MaxHp;
	}

	public void Attack()
	{
		CurrentStatus = EnemyStatus.Attack;
		GetComponent<Animator>().SetTrigger("Attack");
	}

	public void CreateMeleeAttackRange()
	{
		//动画事件调用
		runningAttack.GetComponent<EnemyAttack>().CreateMeleeAttackRange();
	}

	public void CreateRangedProjectile()
	{
		//动画事件调用
		runningAttack.GetComponent<EnemyAttack>().CreateRangedProjectile();
	}

	public void SetRunningAttack(GameObject gameObject)
	{
		runningAttack = gameObject;
	}
}
