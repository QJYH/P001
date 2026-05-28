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

	public void OnLightAttackBegin()
	{
		isLightAttacking = true;
		lightAttack.GetComponent<HeroLightAttack>().OnAttackBegin();
	}

	public void OnLightAttackEnd()
	{
		isLightAttacking = false;
		lightAttack.GetComponent<HeroLightAttack>().OnAttackEnd();
	}

	public void OnHeavyAttackBegin()
	{
		isHeavyAttacking = true;
		heavyAttack.GetComponent<HeroHeavyAttack>().OnAttackBegin();
	}

	public void OnHeavyAttackEnd()
	{
		isHeavyAttacking = false;
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
