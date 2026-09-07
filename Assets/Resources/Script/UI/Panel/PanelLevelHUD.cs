using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelHUD : Panel
{
	public static PanelLevelHUD Instance { get; private set; }
	private Tween goldImageScaleTween;
	private Tween goldCountTextUpdateTween;
	private Tween hpSliderUpdateTween;
	private int realGoldCount = 0;
	private int nominalGoldCount = 0;
	private int realAttackValue = 0;
	private int realDefendValue = 0;

	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = (Instance == this) ? null : Instance;

		goldImageScaleTween?.Kill();
		goldCountTextUpdateTween?.Kill();
		hpSliderUpdateTween?.Kill();

	}

	public static void OnHeroHpChange(float hpRate)
	{
		if (Instance == null) return;

		Instance.hpSliderUpdateTween?.Kill();
		Transform sliderHp = Instance.transform.Find("TopLeft/ImageAvatar/SliderHp");
		Instance.hpSliderUpdateTween = DOTween.To(
			delegate ()
			{
				return sliderHp.GetComponent<Slider>().value;
			},
			delegate (float value)
			{
				sliderHp.GetComponent<Slider>().value = value;
			},
			hpRate,
			1f
		);
	}

	public static Vector2 GetGoldIconScreenPos()
	{
		if (Instance == null) return Vector2.zero;

		Transform imageGold = Instance.transform.Find("TopLeft/ImageAvatar/ImageGold");
		return imageGold.position;
	}

	public static Vector2 GetAttackIconScreenPos()
	{
		if (Instance == null) return Vector2.zero;

		Transform imageAttack = Instance.transform.Find("TopLeft/ImageAvatar/ImageAttack");
		return imageAttack.position;
	}

	public static Vector2 GetDefendIconScreenPos()
	{
		if (Instance == null) return Vector2.zero;

		Transform imageDefend = Instance.transform.Find("TopLeft/ImageAvatar/ImageDefend");
		return imageDefend.position;
	}

	public static void OnPickUpGold(int goldValue)
	{
		if (Instance == null) return;

		Instance.realGoldCount += goldValue;

		Instance.goldImageScaleTween?.Kill();
		Transform imageGold = Instance.transform.Find("TopLeft/ImageAvatar/ImageGold");
		imageGold.localScale = new Vector3(1, 1, 1);
		Instance.goldImageScaleTween = imageGold.DOScale(1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

		Instance.goldCountTextUpdateTween?.Kill();
		TextMeshProUGUI textGoldCount = imageGold.Find("TextGoldCount").GetComponent<TextMeshProUGUI>();
		Instance.goldCountTextUpdateTween = DOTween.To(
			delegate ()
			{
				return Instance.nominalGoldCount;
			},
			delegate (int value)
			{
				Instance.nominalGoldCount = value;
				textGoldCount.text = Instance.nominalGoldCount.ToString();
			},
			Instance.realGoldCount,
			1f
		);
	}

	public static void OnPickUpAttackTreasure(int attackValue)
	{
		if (Instance == null) return;

		Instance.realAttackValue += attackValue;
		Transform imageAttack = Instance.transform.Find("TopLeft/ImageAvatar/ImageAttack");
		imageAttack.localScale = new Vector3(1, 1, 1);
		imageAttack.DOScale(1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

		TextMeshProUGUI textAttackValue = imageAttack.Find("TextAttackValue").GetComponent<TextMeshProUGUI>();
		textAttackValue.text = Instance.realAttackValue.ToString();
	}

	public static void OnPickUpDefendTreasure(int defendValue)
	{
		if (Instance == null) return;

		Instance.realDefendValue += defendValue;
		Transform imageDefend = Instance.transform.Find("TopLeft/ImageAvatar/ImageDefend");
		imageDefend.localScale = new Vector3(1, 1, 1);
		imageDefend.DOScale(1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

		TextMeshProUGUI textDefendValue = imageDefend.Find("TextDefendValue").GetComponent<TextMeshProUGUI>();
		textDefendValue.text = Instance.realDefendValue.ToString();
	}
}
