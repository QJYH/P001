using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
	private const float hurtDuration = 0.4f;

	protected virtual float MaxHp { get; } = 0;
	protected GameObject CreatureCanvas { get; private set; }
	protected float Hp { get; set; }
	
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

		float finalHarm = harmInfo.CalFinalHarm();
		receiver.ReceiveHarm(finalHarm);
	}

	protected virtual void ReceiveHarm(float harmValue)
	{
		if (harmValue <= 0) return;

		ChangeHp(Hp - harmValue);
		ShowHurtEffect();

		if (Hp <= 0)
		{
			Die();
		}
	}

	private void ShowHurtEffect()
	{
		if (Hp <= 0) return;

		hurtEffectTween = DOTween.To(
			delegate ()
			{
				return GetComponent<SpriteRenderer>().color;
			},
			delegate (Vector4 color)
			{
				GetComponent<SpriteRenderer>().color = color;
			},
			new Vector4(1f, 0.5f, 0.5f, 1),
			hurtDuration/2
		).SetLoops(2, LoopType.Yoyo);
	}

	private void ChangeHp(float v)
	{
		Hp = v;
		OnHpChange();
	}

	protected virtual void OnHpChange()
	{

	}

	protected virtual void Die()
	{
		GetComponent<Animator>().SetTrigger("Die");
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

		enabled = false;
	}

	public bool IsDead()
	{
		return Hp <= 0;
	}
}
