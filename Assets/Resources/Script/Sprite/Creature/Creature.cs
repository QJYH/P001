using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
	private const float hurtDuration = 0.4f;

	public virtual float MaxHp { get; } = 0;
	public float Hp { get; private set; }
	public float ExtraAttack { get; protected set; } = 0;
	public float ExtraDefend { get; protected set; } = 0;
	protected virtual float MaxRunSpeed { get; } = 0;
	protected virtual float MaxRunAcceleration { get; } = 0;
	protected GameObject CreatureCanvas { get; private set; }
	
	protected Vector2 Facing { get; set; } = Vector2.right;
	protected UnityEvent<float> OnSubHp = new UnityEvent<float>();
	protected UnityEvent<float> OnAddHp = new UnityEvent<float>();
	protected UnityEvent OnHpChange = new UnityEvent();
	public UnityEvent OnDie = new UnityEvent();
	private Tween hurtEffectTween;

	protected virtual void Awake()
	{
		InitCanvas();
		InitHp();
	}

	protected void TryFlipX(Vector2 velocity)
	{
		if (velocity.x != 0)
		{
			Facing = (velocity.x < 0) ? Vector2.left : Vector2.right;
			Vector3 localScale = transform.localScale;
			float scaleX = (velocity.x < 0) ? -MathF.Abs(localScale.x) : MathF.Abs(localScale.x);
			transform.localScale = new Vector3(scaleX, localScale.y, localScale.z);
		}
	}

	private void InitCanvas()
	{
		CreatureCanvas = new GameObject("MainCanvas");
		Canvas canvas = CreatureCanvas.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.sortingLayerName = "SpriteUI";
		CreatureCanvas.AddComponent<CanvasScaler>();
		CreatureCanvas.AddComponent<GraphicRaycaster>();
		RectTransform rectTransform = CreatureCanvas.GetComponent<RectTransform>();
		rectTransform.SetParent(transform);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 16);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 9);
		rectTransform.anchoredPosition = new Vector3(0, 0, 0);
	}

	private void InitHp()
	{
		Hp = MaxHp;
	}

	public static void HandleAttack(Creature attacker, Creature receiver, HarmInfo harmInfo)
	{
		if (receiver.IsDead()) return;

		float finalHarm = harmInfo.CalFinalHarm(attacker, receiver);
		receiver.ReceiveHarm(finalHarm);
	}

	protected virtual void ReceiveHarm(float harmValue)
	{
		SubHp(harmValue);
	}

	protected void ReceiveHeal(float healValue)
	{
		AddHp(healValue);
	}

	private void SubHp(float value)
	{
		if (IsDead()) return;
		if (value <= 0) return;

		float originalHp = Hp;
		Hp -= value;
		Hp = Mathf.Max(Hp, 0);
		OnSubHp.Invoke(originalHp - Hp);
		OnHpChange.Invoke();

		ShowHurtEffect();
		if (Hp <= 0)
		{
			Die();
		}
	}

	private void AddHp(float value)
	{
		if (IsDead()) return;
		if (value <= 0) return;
		if (Hp == MaxHp) return;

		float originalHp = Hp;
		Hp += value;
		Hp = Mathf.Min(Hp, MaxHp);
		OnAddHp.Invoke(Hp - originalHp);
		OnHpChange.Invoke();
	}

	private void ShowHurtEffect()
	{
		if (Hp <= 0) return;

		hurtEffectTween?.Kill();
		GetComponent<SpriteRenderer>().color = Color.white;
		hurtEffectTween = DOTween.To(
			delegate ()
			{
				return GetComponent<SpriteRenderer>().color;
			},
			delegate (Vector4 color)
			{
				GetComponent<SpriteRenderer>().color = color;
			},
			new Color(1f, 0.5f, 0.5f, 1),
			hurtDuration/2
		).SetLoops(2, LoopType.Yoyo);
	}

	protected virtual void Die()
	{
		GetComponent<Animator>().SetTrigger("Die");
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		OnDie.Invoke();

		enabled = false;
	}

	public bool IsDead()
	{
		return Hp <= 0;
	}
}
