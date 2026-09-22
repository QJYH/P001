using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SwitchSceneAnimation : MonoBehaviour
{
	public static SwitchSceneAnimation Instance { get; private set; }

	private const float duration = 0.5f;
	private const float transitionDistance = 255;
	private float minRadius;
	private float maxRadius;
	private float currentRadius;
	public UnityEvent OnOpenAnimationBegin = new UnityEvent();
	public UnityEvent OnCloseAnimationBegin = new UnityEvent();
	public UnityEvent OnOpenAnimationFinish = new UnityEvent();
	public UnityEvent OnCloseAnimationFinish = new UnityEvent();
	

	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = (Instance == this) ? null : Instance;
	}

	void Start()
    {
		SetParamas();
		CommonFunc.ScaleToFillScreen(gameObject);
	}

	void Update()
	{
		CommonFunc.FollowMainCamera(gameObject);
	}

	private void SetParamas()
	{
		SetRadius(0);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenWidth", Screen.width);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenHeight", Screen.height);
	}

	private void SetMinMaxRadius(Vector2 centerPos)
	{
		minRadius = 0;

		float distance1 = Vector2.Distance(centerPos, new Vector2(0, 0));
		float distance2 = Vector2.Distance(centerPos, new Vector2(Screen.width, 0));
		float distance3 = Vector2.Distance(centerPos, new Vector2(0, Screen.height));
		float distance4 = Vector2.Distance(centerPos, new Vector2(Screen.width, Screen.height));
		float maxDistance1 = Mathf.Max(distance1, distance2);
		float maxDistance2 = Mathf.Max(distance3, distance4);
		maxRadius = Mathf.Max(maxDistance1, maxDistance2) + transitionDistance;
	}

	private void SetRadius(float value)
	{
		currentRadius = value;
		GetComponent<SpriteRenderer>().material.SetFloat("_Radius", value);
	}

	public void PlayOpenAnimation(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		SetMinMaxRadius(centerPos);
		SetRadius(minRadius);
		OnOpenAnimationBegin.Invoke();
		DOTween.To(
			delegate ()
			{
				return currentRadius;
			},
			delegate (float value)
			{
				SetRadius(value);
			},
			maxRadius,
			duration
		).OnComplete(
			delegate ()
			{
				OnOpenAnimationFinish.Invoke();
				GetComponent<SpriteRenderer>().enabled = false;
			}
		);
		
		
	}

	public void PlayCloseAnimation(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		SetMinMaxRadius(centerPos);
		SetRadius(maxRadius);
		OnCloseAnimationBegin.Invoke();
		DOTween.To(
			delegate ()
			{
				return currentRadius;
			},
			delegate (float value)
			{
				SetRadius(value);
			},
			minRadius,
			duration
		).OnComplete(() => { OnCloseAnimationFinish.Invoke(); });
	}
}
