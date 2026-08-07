using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Scale();
		SetAlpha();
	}

	private void SetAlpha()
	{
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
	}

	private void Scale()
	{
		Vector2 spriteSize = GetComponent<SpriteRenderer>().sprite.rect.size;
		transform.localScale = new Vector2(Screen.width / spriteSize.x, Screen.height / spriteSize.y);
	}

	// Update is called once per frame
	void Update()
    {
		FollowCamera();
    }

	private void FollowCamera()
	{
		Vector3 cameraPos = SceneDirector.SceneInstance.MainCamera.transform.position;
		transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
	}
}
