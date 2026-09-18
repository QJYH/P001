using AStarPathFinding;
using BehaviorTree;
using CommonEnum;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Creature
{
	public Hero Target { private set; get; }
	protected List<GameObject> AttackList { get; private set; } = new List<GameObject>();

	public GameObject RunningAttack { get; set; }
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
		UpdateVisual();
	}

	private void FixedUpdate()
	{
		UpdateBehaviorTree();
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
			BehaviorTree.Sequence attackSeq = new BehaviorTree.Sequence();
			attackSeq.AddChild(new AttackCondition(this, enemyAttack));
			attackSeq.AddChild(new Chase(this, enemyAttack));
			attackSeq.AddChild(new Attack(this, enemyAttack));
			attackSeq.AddChild(new Rest(this, enemyAttack));
			behaviorTree.AddChild(attackSeq);
		}

		BehaviorTree.Sequence patrolSeq = new BehaviorTree.Sequence();
		patrolSeq.AddChild(new PatrolCondition(this));
		patrolSeq.AddChild(new PatrolRun(this));
		patrolSeq.AddChild(new PatrolWatch(this));
		behaviorTree.AddChild(patrolSeq);
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
		Velocity velocity = new Velocity(Vector2.zero, Vector2.zero, MaxRunAcceleration);
		GetComponent<MoveManager>().AddVelocity("_BaseMove", velocity);
	}

	public void UpdateBaseMoveVelocity(Vector3 targetPos)
	{
		BoxCollider2D creatureBlock = transform.Find("CreatureBlock").GetComponent<BoxCollider2D>();
		Vector2 collisionOffset = creatureBlock.offset;
		Vector2 collisionSize = creatureBlock.size;
		if (!TileMapManager.CanReachDirectly(transform.position, targetPos, collisionOffset, collisionSize))
		{
			targetPos = GetTargetPosByPathfinding(transform.position, targetPos, collisionOffset, collisionSize);
		}

		Vector3 targetVelocity = (targetPos - transform.position).normalized * MaxRunSpeed;
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
		RunningAttack.GetComponent<EnemyAttack>().OnAttackBegin();
	}
	public void OnAttackAnimationEnd()
	{
		RunningAttack.GetComponent<EnemyAttack>().OnAttackEnd();
		RunningAttack = null;
	}
	public void OnDieAnimationBegin()
	{

	}
	public void OnDieAnimationEnd()
	{
		Disappear();
	}

	private void Disappear()
	{
		CommonFunc.FadeSprite(GetComponent<SpriteRenderer>(), 1, 0, 1, () =>
		{
			Destroy(gameObject);
		});
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
		GetComponent<Animator>().SetTrigger("Attack");
	}

	public void CreateMeleeAttackRange()
	{
		//动画事件调用
		RunningAttack.GetComponent<EnemyAttack>().CreateMeleeAttackRange();
	}

	public void CreateRangedProjectile()
	{
		//动画事件调用
		RunningAttack.GetComponent<EnemyAttack>().CreateRangedProjectile();
	}
}
