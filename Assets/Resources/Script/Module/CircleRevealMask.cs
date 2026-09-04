using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class CircleRevealMask : MonoBehaviour
{	
	enum Mode
	{
		None,
		Expand,
		Contract
	}	
	private float minRadius;
	private float maxRadius;
	private float currentTime = 0f;
	private Mode currentMode = Mode.None;
	public float Duration { get; set; } = 1f;

	// Start is called before the first frame update
	void Start()
    {
		CommonFunc.ScaleToFillScreen(gameObject);
		minRadius = 0;
		maxRadius = MathF.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenWidth", Screen.width);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenHeight", Screen.height);
		GetComponent<SpriteRenderer>().material.SetFloat("_Radius", 0);
    }

	// Update is called once per frame
	void Update()
    {
		CommonFunc.FollowMainCamera(gameObject);
		UpdateRadius();
	}

	private void UpdateRadius()
	{
		if (currentMode != Mode.None)
		{
			if (currentTime >= Duration)
			{
				Destroy(gameObject);
				return;
			}

			currentTime += Time.deltaTime;
			float startRadius = (currentMode == Mode.Expand) ? minRadius : maxRadius;
			float endRadius = (currentMode == Mode.Expand) ? maxRadius : minRadius;
			float currentRadius = Mathf.Lerp(startRadius, endRadius, currentTime / Duration);
			GetComponent<SpriteRenderer>().material.SetFloat("_Radius", currentRadius);
		}
	}

	public void Expand(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Expand;
	}

	public void Contract(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Contract;
	}
}
