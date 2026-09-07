using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SwitchSceneAnimation : MonoBehaviour
{
	public static SwitchSceneAnimation Instance { get; private set; }

	private const float duration = 1f;
	private float minRadius;
	private float maxRadius;
	private float currentRadius;
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
		minRadius = 0;
		maxRadius = MathF.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenWidth", Screen.width);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenHeight", Screen.height);
	}

	private void SetRadius(float value)
	{
		currentRadius = value;
		GetComponent<SpriteRenderer>().material.SetFloat("_Radius", value);
	}

	public void PlayOpenAnimation(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		SetRadius(minRadius);
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
		).OnComplete(() => { OnOpenAnimationFinish.Invoke(); });
	}

	public void PlayCloseAnimation(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		SetRadius(maxRadius);
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
