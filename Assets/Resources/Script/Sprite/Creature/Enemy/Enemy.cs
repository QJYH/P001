using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using static Hero;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : Creature
{
	public enum EnemyStatus
	{
		None,
		Patrol,
		Rest,
		Chase,
		Attack,
	}
	private Hero target;
	private GameObject enemyHpBar;
	private EnemyStatus currentStatus;
	private bool isDetectAreaActive;
	private float attackCoolDownTime = 0f;
	private float attackCoolDownMaxTime = 2f;
	private readonly float maxMoveSpeed = 5f;
	private LinkedList<Vector2> patrolPosList;
	private LinkedListNode<Vector2> currentPatrolPos;
	private Selector behaviorTree;
	protected float attackHarmValue;

	protected override void Awake()
	{
		base.Awake();
		InitHpBar();
		InitAttackHarm();
		initPatrolPosList();
		AddBehaviorTree();
	}
	protected override void Update()
	{
		base.Update();
		UpdateTarget();
		UpdateAttackCoolDown();
		UpdateBehaviorTree();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void InitHpBar()
	{
		enemyHpBar = Instantiate(Resources.Load<GameObject>("Prefab/UI/EnemyHpBar"));
		RectTransform rectTransform = enemyHpBar.GetComponent<RectTransform>();
		rectTransform.SetParent(creatureCanvas.GetComponent<RectTransform>());
		rectTransform.anchoredPosition = new Vector2(0, 0);
		enemyHpBar.GetComponent<Slider>().maxValue = maxHp;
		enemyHpBar.GetComponent<Slider>().value = hp;
	}

	private void AddBehaviorTree()
	{
		behaviorTree = new Selector();

		Sequence attackSeq = new Sequence();
		attackSeq.AddChild(new AttackCondition(this));
		attackSeq.AddChild(new Chase(this));
		attackSeq.AddChild(new Attack(this));
		behaviorTree.AddChild(attackSeq);

		Sequence restSeq = new Sequence();
		restSeq.AddChild(new RestCondition(this));
		restSeq.AddChild(new Rest(this));
		behaviorTree.AddChild(restSeq);

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

	private void initPatrolPosList()
	{
		patrolPosList = new LinkedList<Vector2>();
		Vector2 currentPos = transform.position;
		Vector2 patrolPos1 = new Vector2(currentPos.x - 2, currentPos.y);
		Vector2 patrolPos2 = new Vector2(currentPos.x + 2, currentPos.y);
		patrolPosList.AddLast(patrolPos1);
		patrolPosList.AddLast(patrolPos2);
		currentPatrolPos = patrolPosList.First;
	}

	private void UpdateAttackCoolDown()
	{
		if (attackCoolDownTime > 0)
		{
			attackCoolDownTime -= Time.deltaTime;
		}
	}

	private void UpdateTarget()
	{
		if (target == null) return;

		if (target.IsDead())
		{
			SetTarget(null);
		}
	}
	public bool IsAttackInCoolDown()
	{
		return attackCoolDownTime > 0;
	}

	private void Move()
	{
		bool isMoving;
		Vector2 targetPos;
		if (IsPatroling())
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
		else if (IsChasing())
		{
			isMoving = true;
			Vector2 heroPos = target.transform.position;
			BoxCollider2D heroCollider = target.GetComponent<BoxCollider2D>();
			heroPos.x += heroCollider.offset.x;
			heroPos.y += heroCollider.offset.y;
			GameObject enemyDetectArea = transform.Find("EnemyDetectRange").gameObject;
			PolygonCollider2D detectArea = enemyDetectArea.GetComponent<PolygonCollider2D>();
			Vector2 targetPos1 = new Vector2(heroPos.x - detectArea.offset.x, heroPos.y - detectArea.offset.y);
			Vector2 targetPos2 = new Vector2(heroPos.x + detectArea.offset.x, heroPos.y - detectArea.offset.y);
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

		if (isFacingLeft)
		{
			GetComponent<SpriteRenderer>().flipX = true;
			FlipChildX("EnemyVisionRange", true);
			FlipChildX("EnemyDetectRange", true);
			FlipChildX("EnemyAttackRange", true);
		}
		else
		{
			GetComponent<SpriteRenderer>().flipX = false;
			FlipChildX("EnemyVisionRange", false);
			FlipChildX("EnemyDetectRange", false);
			FlipChildX("EnemyAttackRange", false);
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

	public void SetStatus(EnemyStatus enemyStatus)
	{
		currentStatus = enemyStatus;
	}
	public bool IsPatroling()
	{
		return currentStatus == EnemyStatus.Patrol;
	}
	public bool IsResting()
	{
		return currentStatus == EnemyStatus.Rest;
	}
	public bool IsChasing()
	{
		return currentStatus == EnemyStatus.Chase;
	}
	public bool IsAttacking()
	{
		return currentStatus == EnemyStatus.Attack;
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
		
	}
	public void OnAttackAnimationEnd()
	{
		SetStatus(EnemyStatus.None);
	}
	public void OnDieAnimationBegin()
	{
		enemyHpBar.SetActive(false);
	}

	public void OnDieAnimationEnd()
	{
		
	}
	public void SetTarget(Hero hero)
	{
		if ((hero) && (!hero.IsDead()))
		{
			target = hero;
		}
		else
		{
			target = null;
		}
	}

	public void SetDetectAreaActive(bool v)
	{
		isDetectAreaActive = v;
	}
	public bool HaveTarget()
	{
		return target != null;
	}

	public Hero GetTarget()
	{
		return target;
	}

	public bool HaveTargetInAttackArea()
	{
		return HaveTarget() && isDetectAreaActive;
	}

	public void Attack()
	{
		SetStatus(EnemyStatus.Attack);
		bool isTargetInLeft = (target.transform.position.x - transform.position.x) < 0;
		SetForward(isTargetInLeft);
		Animator animator = GetComponent<Animator>();
		animator.SetTrigger("Attack");
		attackCoolDownTime = attackCoolDownMaxTime;
	}

	public void EnableAttackCollider(int status)
	{
		EnemyAttackRange range = GetComponentInChildren<EnemyAttackRange>();
		if (range == null) return;
		range.GetComponent<PolygonCollider2D>().enabled = (status == 1) ? true : false;
	}

	protected virtual void InitAttackHarm()
	{
		throw new NotImplementedException();
	}

	public void OnAttackHitHero(Hero hero)
	{
		HarmInfo harmInfo = new HarmInfo();
		harmInfo.SetBaseHarm(attackHarmValue);
		Creature.HandleAttack(this, hero, harmInfo);
	}

	protected override void OnHpChange()
	{
		enemyHpBar.GetComponent<Slider>().value = hp;
	}
}
