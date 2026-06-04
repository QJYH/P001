using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
	protected float hp;
	protected float maxHp;
	private readonly float hurtDuration = 0.4f;
	private float hurtTimer = 0;
	protected GameObject creatureCanvas;

	protected virtual void Awake()
	{
		InitCanvas();
		InitHp();
	}

	protected virtual void Update()
	{
		UpdateHurtTimer();
	}

	private void InitCanvas()
	{
		creatureCanvas = new GameObject("MainCanvas");
		Canvas canvas = creatureCanvas.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.sortingLayerName = "UI";
		creatureCanvas.AddComponent<CanvasScaler>();
		creatureCanvas.AddComponent<GraphicRaycaster>();
		RectTransform rectTransform = creatureCanvas.GetComponent<RectTransform>();
		rectTransform.SetParent(transform);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 16);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 9);
		rectTransform.anchoredPosition = new Vector3(0, 1, 0);
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

	protected virtual void InitHp()
	{
		throw new NotImplementedException();
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

		ChangeHp(hp - harmValue);
		hurtTimer = hurtDuration;

		if (hp <= 0) Die();
	}

	private void ChangeHp(float v)
	{
		hp = v;
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

		OnDie();
		enabled = false;
	}

	protected virtual void OnDie()
	{
		 
	}

	public bool IsDead()
	{
		return hp <= 0;
	}
}
