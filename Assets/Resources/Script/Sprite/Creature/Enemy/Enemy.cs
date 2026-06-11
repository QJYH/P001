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
	protected List<GameObject> attackList = new List<GameObject>();
	private GameObject runningAttack;
	private readonly float maxMoveSpeed = 5f;
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
		rectTransform.SetParent(creatureCanvas.GetComponent<RectTransform>());
		rectTransform.anchoredPosition = new Vector2(0, 0);
		enemyHpBar.GetComponent<Slider>().maxValue = maxHp;
		enemyHpBar.GetComponent<Slider>().value = hp;
	}

	private void AddBehaviorTree()
	{
		behaviorTree = new Selector();

		foreach (GameObject attackObject in attackList)
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
			SetTarget(null);
		}
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
		foreach (GameObject attackObject in attackList)
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
		runningAttack.GetComponent<EnemyAttack>().OnAttackBegin();
	}
	public void OnAttackAnimationEnd()
	{
		SetStatus(EnemyStatus.None);
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

	public bool HaveTarget()
	{
		return target != null;
	}

	public void Attack()
	{
		SetStatus(EnemyStatus.Attack);
		bool isTargetInLeft = (target.transform.position.x - transform.position.x) < 0;
		SetForward(isTargetInLeft);
		Animator animator = GetComponent<Animator>();
		animator.SetTrigger("Attack");
	}

	public void SetAttackRangeEnable(int status)
	{
		bool enable = (status == 1) ? true : false;
		runningAttack.GetComponent<EnemyAttack>().SetAttackRangeEnable("AttackRange", enable);
	}

	protected override void OnHpChange()
	{
		enemyHpBar.GetComponent<Slider>().value = hp;
	}

	
}
