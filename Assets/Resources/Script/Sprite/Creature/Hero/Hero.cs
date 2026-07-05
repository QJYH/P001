using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CommonEnum;

public class Hero : Creature
{
	private const float maxMoveSpeed = 6f;

	public HeroStatus CurrentStatus { get; set; }
	protected GameObject LightAttack { get; set; }
	protected GameObject HeavyAttack { get; set; }

	private Vector2 moveInput;

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

	protected override void Update()
    {
		base.Update();
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
			FlipAttackX(LightAttack, true);
			FlipAttackX(HeavyAttack, true);
		}
		else if (horizontalInput > 0)
		{
			GetComponent<SpriteRenderer>().flipX = false;
			FlipAttackX(LightAttack, false);
			FlipAttackX(HeavyAttack, false);
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
		LevelDirector.OnHeroHpChange(Hp/MaxHp);
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
