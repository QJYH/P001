using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveBlock : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private void Awake()
	{
		spriteRenderer = GetComponentInParent<SpriteRenderer>();
	}
	// Start is called before the first frame update
	void Start()
    {
		spriteRenderer.material = Resources.Load<Material>("Material/PerspectiveBlock");
		spriteRenderer.material.SetFloat("_ScreenWidth", Screen.width);
		spriteRenderer.material.SetFloat("_ScreenHeight", Screen.height);
		SetHeroScreenPos(new Vector2(-Screen.width, -Screen.height));
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetHeroScreenPos(Vector2 screenPos)
	{
		if (spriteRenderer == null) return;

		spriteRenderer.material.SetVector("_HeroScreenPos", screenPos);
	}
}
