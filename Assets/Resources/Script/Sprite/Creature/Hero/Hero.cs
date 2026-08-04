using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CommonEnum;
using System.Collections;
using Cinemachine;

public class Hero : Creature
{
	private const float maxInputMoveSpeed = 5f;
	private const float maxInputMoveAcceleration = 50f;
	private const float maxRushSpeed = 20f;
	private const float maxRushAcceleration = 200f;
	private const float healValue = 1f;

	public HeroStatus CurrentStatus { get; set; }
	protected GameObject LightAttack { get; set; }
	protected GameObject HeavyAttack { get; set; }

	private Vector2 moveInput = Vector2.zero;

	protected override void Awake()
	{
		base.Awake();
		InitAttackModule();
		AddInputMoveVelocity();
	}

	private void OnEnable()
	{
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered += OnActionTriggered;
		OnDie.AddListener(OnHeroDie);
		OnSubHp.AddListener(OnHeroSubHp);
		OnHpChange.AddListener(OnHeroHpChange);
	}

	private void OnDisable()
	{
		PlayerInput heroInput = GetComponent<PlayerInput>();
		heroInput.onActionTriggered -= OnActionTriggered;
		OnDie.RemoveListener(OnHeroDie);
		OnSubHp.RemoveListener(OnHeroSubHp);
		OnHpChange.RemoveListener(OnHeroHpChange);
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
			case "Rush":
				Rush(context);
				break;
		}
	}

	private void Heal(InputAction.CallbackContext context)
	{
		if (IsDoingAction()) return;
		if (context.phase != InputActionPhase.Started) return;

		GetComponent<Animator>().SetTrigger("Heal");
		ReceiveHeal(healValue);
	}

	private void Rush(InputAction.CallbackContext context)
	{
		if (IsDoingAction()) return;
		if (context.phase != InputActionPhase.Started) return;

		Vector2 rushDirection = new Vector2(moveInput.x, moveInput.y);
		rushDirection = (rushDirection == Vector2.zero) ? Facing : rushDirection;
		Vector2 rushVelocity = rushDirection.normalized * maxRushSpeed;
		Velocity velocity = new Velocity(Vector2.zero, rushVelocity, maxRushAcceleration);
		GetComponent<MoveManager>().AddVelocity("_Rush", velocity);

		GetComponent<Animator>().SetTrigger("Rush");
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
		GetComponent<Animator>().SetBool("IsMoving", (targetVelocity != Vector2.zero));
		TryFlipX(targetVelocity);
	}

	public bool IsDoingAction()
	{
		return (CurrentStatus == HeroStatus.LightAttack)
			|| (CurrentStatus == HeroStatus.HeavyAttack)
			|| (CurrentStatus == HeroStatus.Heal)
			|| (CurrentStatus == HeroStatus.Rush);
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
	public void OnRushAnimationBegin()
	{
		CurrentStatus = HeroStatus.Rush;
	}

	public void OnRushAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
		GetComponent<MoveManager>().RemoveVelocity("_Rush");

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
		CurrentStatus = HeroStatus.Die;
	}

	public void OnDieAnimationEnd()
	{
		CurrentStatus = HeroStatus.None;
	}
	private void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		LightAttack.GetComponent<HeroLightAttack>().HandleLightAttackInput(context);
	}
	private void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		HeavyAttack.GetComponent<HeroHeavyAttack>().HandleHeavyAttackInput(context);
	}

	private void OnHeroDie()
	{
		StartCoroutine(DelayCreatePanelHeroDie());
	}
	private IEnumerator DelayCreatePanelHeroDie()
	{
		yield return new WaitForSeconds(1.1f);
		UICommonFunc.CreatePanel("PanelHeroDie", null);
	}

	private void OnHeroSubHp(float value)
	{
		GetComponent<CinemachineImpulseSource>().GenerateImpulse();
	}

	private void OnHeroHpChange()
	{
		PanelLevelHUD.OnHeroHpChange(Hp / MaxHp);
	}

	public void CreateMeleeAttackRange()
	{
		//动画事件调用
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
		//动画事件调用
		if (CurrentStatus == HeroStatus.LightAttack)
		{
			LightAttack.GetComponent<HeroLightAttack>().CreateRangedProjectile();
		}
		else if ( CurrentStatus == HeroStatus.HeavyAttack)
		{
			HeavyAttack.GetComponent<HeroHeavyAttack>().CreateRangedProjectile();
		}
	}

	public void CreateHeroPhantom()
	{
		//动画事件调用
		GameObject heroPhantom = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/HeroPhantom"));
		heroPhantom.transform.position = transform.position;
		heroPhantom.transform.localScale = transform.localScale;
		heroPhantom.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		heroPhantom.GetComponent<OrderInLayerManager>().verticalOffset = GetComponent<OrderInLayerManager>().verticalOffset;
	}

}
