using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveBlock : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private MaterialPropertyBlock propertyBlock;

	void Start()
    {
		spriteRenderer = GetComponentInParent<SpriteRenderer>();
		spriteRenderer.sharedMaterial = Resources.Load<Material>("Material/PerspectiveBlock");
		propertyBlock = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(propertyBlock);

		propertyBlock.SetFloat("_ScreenWidth", Screen.width);
		propertyBlock.SetFloat("_ScreenHeight", Screen.height);
		SetHeroScreenPos(new Vector2(-Screen.width, -Screen.height));
	}

	public void SetHeroScreenPos(Vector2 screenPos)
	{
		if (spriteRenderer == null) return;

		propertyBlock.SetVector("_HeroScreenPos", screenPos);
		spriteRenderer.SetPropertyBlock(propertyBlock);
	}
}
