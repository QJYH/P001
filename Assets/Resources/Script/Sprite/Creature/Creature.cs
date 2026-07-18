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
	
	private float hurtTimer = 0;

	protected virtual void Awake()
	{
		InitCanvas();
		InitHp();
	}

	protected virtual void Update()
	{
		UpdateHurtTimer();
	}

	protected virtual void OnEnable()
	{
		GetComponent<MoveManager>().OnSetVelocity.AddListener(OnSetVelocity);
	}

	protected virtual void OnDisable()
	{
		GetComponent<MoveManager>().OnSetVelocity.RemoveListener(OnSetVelocity);
		
	}

	private void OnSetVelocity(Vector2 velocity)
	{
		GetComponent<Animator>().SetBool("IsMoving", (velocity != Vector2.zero));

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
		canvas.sortingLayerName = "Sprite";
		CreatureCanvas.AddComponent<CanvasScaler>();
		CreatureCanvas.AddComponent<GraphicRaycaster>();
		RectTransform rectTransform = CreatureCanvas.GetComponent<RectTransform>();
		rectTransform.SetParent(transform);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 16);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 9);
		rectTransform.anchoredPosition = new Vector3(0, 0, 0);
	}

	private void UpdateHurtTimer()
	{
		if (hurtTimer > 0)
		{
			hurtTimer -= Time.deltaTime;
		}
		float hurtColorRate = (hurtTimer > hurtDuration / 2) ? ((hurtDuration - hurtTimer) * 4) : (hurtTimer * 4);
		GetComponent<SpriteRenderer>().material.SetFloat("_HurtColorRate", hurtTimer > 0 ? hurtColorRate : 0);

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
		hurtTimer = hurtDuration;

		if (Hp <= 0) Die();
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
		hurtTimer = 0;
		GetComponent<Animator>().SetTrigger("Die");
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

		enabled = false;
	}

	public bool IsDead()
	{
		return Hp <= 0;
	}
}
