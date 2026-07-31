using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonEnum;
using AStarPathFinding;

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
		if (CurrentStatus == EnemyStatus.Patrol)
		{
			if (CommonFunc.IsNear(transform.position, currentPatrolPos.Value))
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
			Vector3 targetPos1 = new Vector3(heroPos.x - detectRange.offset.x, heroPos.y - detectRange.offset.y);
			Vector3 targetPos2 = new Vector3(heroPos.x + detectRange.offset.x, heroPos.y - detectRange.offset.y);
			float distance1 = Vector3.Distance(targetPos1, transform.position);
			float distance2 = Vector3.Distance(targetPos2, transform.position);
			targetPos = (distance1 < distance2) ? targetPos1 : targetPos2;
		}
		else
		{
			targetPos = transform.position;
		}

		if (!TileMapManager.CanReachDirectly(transform.position, targetPos))
		{
			targetPos = GetTargetPosByPathfinding(transform.position, targetPos);
		}

		Vector3 targetVelocity = (targetPos - transform.position).normalized * maxBaseMoveSpeed;
		GetComponent<MoveManager>().ChangeTargetVelocity("_BaseMove", targetVelocity);
		GetComponent<Animator>().SetBool("IsMoving", (targetVelocity != Vector3.zero));
		TryFlipX(targetVelocity);
	}

	private Vector3 GetTargetPosByPathfinding(Vector3 currentPos, Vector3 targetPos)
	{
		List<Vector3> path = AStar.FindPath(currentPos, targetPos);

		return CommonFunc.IsNear(currentPos, path[0]) ? path[1] : path[0];
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
		CurrentStatus = EnemyStatus.None;
		runningAttack.GetComponent<EnemyAttack>().OnAttackEnd();
		runningAttack = null;
	}
	public void OnDieAnimationBegin()
	{
		enemyHpBar.SetActive(false);
	}

	public void OnDieAnimationEnd()
	{
		
	}

	public bool HaveTarget()
	{
		return (target != null);
	}

	protected override void OnHpChange()
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
