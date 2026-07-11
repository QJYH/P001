using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GetComponentInParent<SpriteRenderer>().material = Resources.Load<Material>("Material/PerspectiveBlock");
		GetComponentInParent<SpriteRenderer>().material.SetFloat("_ScreenWidth", Screen.width);
		GetComponentInParent<SpriteRenderer>().material.SetFloat("_ScreenHeight", Screen.height);
		SetHeroScreenPos(new Vector2(-Screen.width, -Screen.height));
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetHeroScreenPos(Vector2 screenPos)
	{
		GetComponentInParent<SpriteRenderer>().material.SetVector("_HeroScreenPos", screenPos);
	}
}
