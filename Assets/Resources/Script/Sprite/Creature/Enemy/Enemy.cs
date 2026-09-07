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
	public Vector3 CurrentPatrolPos { set; get; }
	public Hero Target { private set; get; }
	protected List<GameObject> AttackList { get; private set; } = new List<GameObject>();

	private GameObject runningAttack;
	private GameObject enemyHpBar;
	private Selector behaviorTree;

	protected override void Awake()
	{
		base.Awake();
		SetMaterial();
		InitHpBar();
		InitAttackList();
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
		UpdateVisual();
	}

	private void FixedUpdate()
	{
		UpdateBaseMoveVelocity();
	}

	private void SetMaterial()
	{
		GetComponent<SpriteRenderer>().sharedMaterial = Resources.Load<Material>("Material/Enemy");
	}

	protected virtual void InitAttackList()
	{
		throw new NotImplementedException();
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
		patrolSeq.AddChild(new PatrolRun(this));
		patrolSeq.AddChild(new PatrolWatch(this));
		behaviorTree.AddChild(patrolSeq);

		behaviorTree.Tick();
	}

	private void UpdateBehaviorTree()
	{
		behaviorTree.Tick();
	}

	private void UpdateVisual()
	{
		LayerMask obstacleLayer = LayerMask.GetMask("EnvironmentBlock");
		Vector3 heroPos = LevelDirector.LevelInstance.CurrentHero.transform.position;
		bool haveBlock = Physics2D.Linecast(transform.position, heroPos, obstacleLayer);
		enemyHpBar?.SetActive(!haveBlock);
	}

	private void UpdateTarget()
	{
		if (Target == null) return;

		if (Target.IsDead())
		{
			Target = null;
		}
	}
	public void SeeHero(Hero hero)
	{
		if (hero.IsDead()) return;

		Target = hero;
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
		if (CurrentStatus == EnemyStatus.PatrolRun)
		{
			targetPos = CurrentPatrolPos;
		}
		else if (CurrentStatus == EnemyStatus.Chase)
		{
			Vector3 heroPos = Target.transform.position;
			BoxCollider2D heroCollider = Target.GetComponent<BoxCollider2D>();
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
		CreatureCanvas.GetComponent<RectTransform>().localScale = transform.localScale;
	}

	private Vector3 GetTargetPosByPathfinding(Vector3 beginWorldPos, Vector3 endWorldPos, Vector2 collisionOffset, Vector2 collisionSize)
	{
		List<Vector3> path = AStar.FindPath(beginWorldPos, endWorldPos, collisionOffset, collisionSize);

		bool canReach = TileMapManager.CanReachDirectly(beginWorldPos, path[1], collisionOffset, collisionSize);
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
		return (Target != null);
	}

	private void OnEnemyDie()
	{
		Destroy(enemyHpBar);
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
