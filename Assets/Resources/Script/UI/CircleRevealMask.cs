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
	private float duration = 1f;
	private float minRadius;
	private float maxRadius;
	private float currentTime = 0f;
	private Mode currentMode = Mode.None;
	
	// Start is called before the first frame update
	void Start()
    {
		minRadius = 0;
		maxRadius = MathF.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMode != Mode.None)
		{
			if (currentTime >= duration)
			{
				Destroy(gameObject);
				return;
			}

			currentTime += Time.deltaTime;
			float startRadius = (currentMode == Mode.Expand) ? minRadius : maxRadius;
			float endRadius = (currentMode == Mode.Expand) ? maxRadius : minRadius;
			float currentRadius = Mathf.Lerp(startRadius, endRadius, currentTime/duration);
			GetComponent<Image>().material.SetFloat("_Radius", currentRadius);
		}
	}

	public void Expand(Vector2 centerPos)
	{
		GetComponent<Image>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Expand;
	}

	public void Contract(Vector2 centerPos)
	{
		GetComponent<Image>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Contract;
	}
}
