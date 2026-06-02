using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Enemy;

public class Hero : Creature
{
	private Vector2 moveInput;
	private readonly float maxMoveSpeed = 6f;

	public enum HeroStatus
	{
		None,
		Idle,
		Run,
		LightAttack,
		HeavyAttack,
		Heal

	}
	private HeroStatus currentStatus;
	protected GameObject lightAttack;
	protected GameObject heavyAttack;

	protected override void Awake()
	{
		base.Awake();
		InitAttackModule();
	}

	private void OnEnable()
	{
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered += OnActionTriggered;
	}

	private void OnDisable()
	{
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered -= OnActionTriggered;
	}

	private void Update()
    {
		
	}

	private void FixedUpdate()
	{
		Move();
	}

	protected virtual void InitAttackModule()
	{
		throw new NotImplementedException();
	}

	private void OnActionTriggered(InputAction.CallbackContext context)
	{
		switch (context.action.name)
		{
			case "Move":
				moveInput = context.ReadValue<Vector2>();
				break;
			case "LightAttack":
				HandleLightAttackInput(context);
				break;
			case "HeavyAttack":
				HandleHeavyAttackInput(context);
				break;
			case "Heal":
				Heal(context);
				break;
		}
	}

	private void Heal(InputAction.CallbackContext context)
	{
		if (IsDoingAction()) return;
		if (context.phase != InputActionPhase.Started) return;

		GameObject healEffect = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/HealEffect"));
		healEffect.transform.SetParent(gameObject.transform, false);
	}

	private void Move()
	{
		Vector2 currentMoveInput = new Vector2(moveInput.x, moveInput.y);

		currentMoveInput = IsDoingAction() ? Vector2.zero : currentMoveInput;
		currentMoveInput = currentMoveInput.normalized;
		float horizontalInput = currentMoveInput.x;
		float verticalInput = currentMoveInput.y;
	
		if (horizontalInput < 0)
		{
			GetComponent<SpriteRenderer>().flipX = true;
			FlipAttackX(lightAttack, true);
			FlipAttackX(heavyAttack, true);
		}
		else if (horizontalInput > 0)
		{
			GetComponent<SpriteRenderer>().flipX = false;
			FlipAttackX(lightAttack, false);
			FlipAttackX(heavyAttack, false);
		}

		Animator animator = GetComponent<Animator>();
		if ((horizontalInput != 0) || (verticalInput != 0))
		{
			animator.SetBool("IsMoving", true);
		}
		else
		{
			animator.SetBool("IsMoving", false);
		}

		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.velocity = currentMoveInput * maxMoveSpeed;
	}

	private void FlipAttackX(GameObject attack, bool needFlipX)
	{
		Vector3 localScale = attack.transform.localScale;
		float scaleX = needFlipX ? -MathF.Abs(localScale.x) : MathF.Abs(localScale.x);
		attack.transform.localScale = new Vector3(scaleX, localScale.y, localScale.z);
	}

	public void SetStatus(HeroStatus heroStatus)
	{
		currentStatus = heroStatus;
	}

	public bool IsIdling()
	{
		return currentStatus == HeroStatus.Idle;
	}

	public bool isRunning()
	{
		return currentStatus == HeroStatus.Run;
	}

	public bool isLightAttacking()
	{
		return currentStatus == HeroStatus.LightAttack;
	}

	public bool isHeavyAttacking()
	{
		return currentStatus == HeroStatus.HeavyAttack;
	}

	public bool isHealing()
	{
		return currentStatus == HeroStatus.Heal;
	}

	public bool IsDoingAction()
	{
		return isLightAttacking() || isHeavyAttacking() || isHealing();
	}

	public void OnIdleBegin()
	{
		currentStatus = HeroStatus.Idle;
	}
	public void OnIdleEnd()
	{
		currentStatus = HeroStatus.None;
	}
	public void OnRunEnd()
	{
		currentStatus = HeroStatus.Run;
	}
	public void OnRunBegin()
	{
		currentStatus = HeroStatus.None;
	}
	public void OnHealBegin()
	{
		SetStatus(HeroStatus.Heal);
	}
	public void OnHealEnd()
	{
		SetStatus(HeroStatus.None);
	}
	public void OnLightAttackBegin()
	{
		SetStatus(HeroStatus.LightAttack);
		lightAttack.GetComponent<HeroLightAttack>().OnAttackBegin();
	}
	public void OnLightAttackEnd()
	{
		SetStatus(HeroStatus.None);
		lightAttack.GetComponent<HeroLightAttack>().OnAttackEnd();
	}
	public void OnHeavyAttackBegin()
	{
		SetStatus(HeroStatus.HeavyAttack);
		heavyAttack.GetComponent<HeroHeavyAttack>().OnAttackBegin();
	}
	public void OnHeavyAttackEnd()
	{
		SetStatus(HeroStatus.None);
		heavyAttack.GetComponent<HeroHeavyAttack>().OnAttackEnd();
	}
	private void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		lightAttack.GetComponent<HeroLightAttack>().HandleLightAttackInput(context);
	}
	private void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		heavyAttack.GetComponent<HeroHeavyAttack>().HandleHeavyAttackInput(context);
	}

	
}
