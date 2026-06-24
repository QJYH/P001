using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonEnum;

public class Enemy : Creature
{
	private const float maxMoveSpeed = 5f;

	public EnemyStatus CurrentStatus { get; set; }
	protected List<GameObject> AttackList { get; private set; } = new List<GameObject>();

	private GameObject runningAttack;
	private GameObject enemyHpBar;
	private Hero target;
	private LinkedList<Vector2> patrolPosList;
	private LinkedListNode<Vector2> currentPatrolPos;
	private Selector behaviorTree;
	

	protected override void Awake()
	{
		base.Awake();
		InitHpBar();
		InitAttackList();
		InitPatrolPosList();
		AddBehaviorTree();
	}

	protected override void Update()
	{
		base.Update();
		UpdateTarget();
		UpdateBehaviorTree();
	}

	private void FixedUpdate()
	{
		Move();
	}

	protected virtual void InitAttackList()
	{
		throw new NotImplementedException();
	}
	public void SetRunningAttack(GameObject gameObject)
	{
		runningAttack = gameObject;
	}
	private void InitHpBar()
	{
		enemyHpBar = Instantiate(Resources.Load<GameObject>("Prefab/UI/EnemyHpBar"));
		RectTransform rectTransform = enemyHpBar.GetComponent<RectTransform>();
		rectTransform.SetParent(CreatureCanvas.GetComponent<RectTransform>());
		AdjustHpBarPos(rectTransform);
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
			EnemyAttack<Enemy> enemyAttack = attackObject.GetComponent<EnemyAttack<Enemy>>();
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

	private void InitPatrolPosList()
	{
		patrolPosList = new LinkedList<Vector2>();
		Vector2 currentPos = transform.position;
		Vector2 patrolPos1 = new Vector2(currentPos.x - 2, currentPos.y);
		Vector2 patrolPos2 = new Vector2(currentPos.x + 2, currentPos.y);
		patrolPosList.AddLast(patrolPos1);
		patrolPosList.AddLast(patrolPos2);
		currentPatrolPos = patrolPosList.First;
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

	private void Move()
	{
		bool isMoving;
		Vector2 targetPos;
		if (CurrentStatus == EnemyStatus.Patrol)
		{
			isMoving = true;
			float distance = Vector2.Distance(transform.position, currentPatrolPos.Value);
			if (distance < 0.1f)
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
			isMoving = true;
			Vector2 heroPos = target.transform.position;
			BoxCollider2D heroCollider = target.GetComponent<BoxCollider2D>();
			heroPos.x += heroCollider.offset.x;
			heroPos.y += heroCollider.offset.y;
			GameObject detectRangeObject = runningAttack.transform.Find("DetectRange").gameObject;
			PolygonCollider2D detectRange = detectRangeObject.GetComponent<PolygonCollider2D>();
			Vector2 targetPos1 = new Vector2(heroPos.x - detectRange.offset.x, heroPos.y - detectRange.offset.y);
			Vector2 targetPos2 = new Vector2(heroPos.x + detectRange.offset.x, heroPos.y - detectRange.offset.y);
			float distance1 = Vector2.Distance(targetPos1, transform.position);
			float distance2 = Vector2.Distance(targetPos2, transform.position);
			targetPos = (distance1 < distance2) ? targetPos1 : targetPos2;
		}
		else
		{
			isMoving = false;
			targetPos = transform.position;
		}

		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		Animator animator = GetComponent<Animator>();
		if (isMoving)
		{
			rigidbody.velocity = (targetPos - (Vector2)transform.position).normalized * maxMoveSpeed;
			SetForward(rigidbody.velocity.x < 0);
			animator.SetBool("IsMoving", true);
		}
		else
		{
			rigidbody.velocity = Vector2.zero;
			animator.SetBool("IsMoving", false);
		}
	}

	private void SetForward(bool isFacingLeft)
	{
		GetComponent<SpriteRenderer>().flipX = isFacingLeft;
		FlipChildX("VisionRange", isFacingLeft);
		foreach (GameObject attackObject in AttackList)
		{
			FlipChildX(attackObject.name, isFacingLeft);
		}
	}

	private void FlipChildX(String childName, bool needFlipX)
	{
		Transform childTrans = transform.Find(childName);
		if (childTrans == null) return;
		Vector3 localScale = childTrans.localScale;
		float scaleX = needFlipX ? -MathF.Abs(localScale.x) : MathF.Abs(localScale.x);
		childTrans.localScale = new Vector3(scaleX, localScale.y, localScale.z);
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
		runningAttack.GetComponent<EnemyAttack<Enemy>>().OnAttackBegin();
	}
	public void OnAttackAnimationEnd()
	{
		CurrentStatus = EnemyStatus.None;
		runningAttack.GetComponent<EnemyAttack<Enemy>>().OnAttackEnd();
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
		enemyHpBar.GetComponent<Slider>().value = Hp;
	}

	public void Attack()
	{
		CurrentStatus = EnemyStatus.Attack;
		bool isTargetInLeft = (target.transform.position.x - transform.position.x) < 0;
		SetForward(isTargetInLeft);
		Animator animator = GetComponent<Animator>();
		animator.SetTrigger("Attack");
	}

	public void CreateMeleeAttackRange()
	{
		//called by animation event
		runningAttack.GetComponent<EnemyAttack<Enemy>>().CreateMeleeAttackRange();
	}

	public void CreateRangedProjectile()
	{
		//called by animation event
		runningAttack.GetComponent<EnemyAttack<Enemy>>().CreateRangedProjectile();
	}
}
