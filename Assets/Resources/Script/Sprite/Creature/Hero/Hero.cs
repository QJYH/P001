using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : Creature
{
	private Vector2 moveInput;
	private readonly float maxMoveSpeed = 6f;

	private bool isHealing;
	private bool isLightAttacking;
	private bool isHeavyAttacking;

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
		}
		else if (horizontalInput > 0)
		{
			GetComponent<SpriteRenderer>().flipX = false;
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

	public bool IsDoingAction()
	{
		return isLightAttacking || isHeavyAttacking || isHealing;
	}

	public void OnHealBegin()
	{
		isHealing = true;
	}

	public void OnHealEnd()
	{
		isHealing = false;
	}

	public virtual void OnLightAttackBegin()
	{
		isLightAttacking = true;
	}

	public virtual void OnLightAttackEnd()
	{
		isLightAttacking = false;
	}

	public virtual void OnHeavyAttackBegin()
	{
		isHeavyAttacking = true;
	}

	public virtual void OnHeavyAttackEnd()
	{
		isHeavyAttacking = false;
	}

	protected virtual void HandleLightAttackInput(InputAction.CallbackContext context)
	{

	}

	protected virtual void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{

	}
}
