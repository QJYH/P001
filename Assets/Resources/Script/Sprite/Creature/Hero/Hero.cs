using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CommonEnum;

public class Hero : Creature
{
	private const float maxInputMoveSpeed = 6f;
	private const float maxInputMoveAcceleration = 60f;

	public HeroStatus CurrentStatus { get; set; }
	protected GameObject LightAttack { get; set; }
	protected GameObject HeavyAttack { get; set; }

	private Vector2 moveInput;

	protected override void Awake()
	{
		base.Awake();
		InitAttackModule();
		AddInputMoveVelocity();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered += OnActionTriggered;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered -= OnActionTriggered;
	}

	private void FixedUpdate()
	{
		UpdateInputMoveVelocity();
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

	private void AddInputMoveVelocity()
	{
		Velocity velocity = new Velocity(Vector2.zero, Vector2.zero, maxInputMoveAcceleration);
		GetComponent<MoveManager>().AddVelocity("_InputMove", velocity);
	}

	private void UpdateInputMoveVelocity()
	{
		Vector2 currentInputMoveDirection = new Vector2(moveInput.x, moveInput.y);
		currentInputMoveDirection = IsDoingAction() ? Vector2.zero : currentInputMoveDirection;
		currentInputMoveDirection = currentInputMoveDirection.normalized;
		Vector2 targetVelocity = currentInputMoveDirection * maxInputMoveSpeed;
		GetComponent<MoveManager>().ChangeTargetVelocity("_InputMove", targetVelocity);
	}

	protected override void OnSetVelocity(Vector2 velocity)
	{
		base.OnSetVelocity(velocity);

		FlipChildHorizontally(LightAttack.name, (velocity.x < 0));
		FlipChildHorizontally(HeavyAttack.name, (velocity.x < 0));
	}

	public bool IsDoingAction()
	{
		return (CurrentStatus == HeroStatus.LightAttack) || (CurrentStatus == HeroStatus.HeavyAttack) || (CurrentStatus == HeroStatus.Heal);
	}

	public void OnIdleAnimationBegin()
	{
		CurrentStatus = HeroStatus.Idle;
	}
	public void OnIdleAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
	}
	public void OnRunAnimationBegin()
	{
		CurrentStatus = HeroStatus.Run;
	}
	public void OnRunAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
	}
	public void OnHealAnimationBegin()
	{
		CurrentStatus = HeroStatus.Heal;
	}
	public void OnHealAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
	}
	public void OnLightAttackAnimationBegin()
	{
		CurrentStatus = HeroStatus.LightAttack;
		LightAttack.GetComponent<HeroLightAttack>().OnAttackBegin();
	}
	public void OnLightAttackAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
		LightAttack.GetComponent<HeroLightAttack>().OnAttackEnd();
	}
	public void OnHeavyAttackAnimationBegin()
	{
		CurrentStatus = HeroStatus.HeavyAttack;
		HeavyAttack.GetComponent<HeroHeavyAttack>().OnAttackBegin();
	}
	public void OnHeavyAttackAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
		HeavyAttack.GetComponent<HeroHeavyAttack>().OnAttackEnd();
	}
	public void OnDieAnimationBegin()
	{
		LevelDirector.OnHeroDie();
	}

	public void OnDieAnimationEnd()
	{
		
	}
	private void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		LightAttack.GetComponent<HeroLightAttack>().HandleLightAttackInput(context);
	}
	private void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		HeavyAttack.GetComponent<HeroHeavyAttack>().HandleHeavyAttackInput(context);
	}

	protected override void OnHpChange()
	{
		PanelLevelHUD.OnHeroHpChange(Hp/MaxHp);
	}

	public void CreateMeleeAttackRange()
	{
		//called by animation event
		if (CurrentStatus == HeroStatus.LightAttack)
		{
			LightAttack.GetComponent<HeroLightAttack>().CreateMeleeAttackRange();
		}
		else if (CurrentStatus == HeroStatus.HeavyAttack)
		{
			HeavyAttack.GetComponent<HeroHeavyAttack>().CreateMeleeAttackRange();
		}
	}

	public void CreateRangedProjectile()
	{
		//called by animation event
		if (CurrentStatus == HeroStatus.LightAttack)
		{
			LightAttack.GetComponent<HeroLightAttack>().CreateRangedProjectile();
		}
		else if ( CurrentStatus == HeroStatus.HeavyAttack)
		{
			HeavyAttack.GetComponent<HeroHeavyAttack>().CreateRangedProjectile();
		}
	}
}
