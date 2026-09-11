using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPhantom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		DOTween.ToAlpha(
			delegate ()
			{
				return GetComponent<SpriteRenderer>().color;
			},
			delegate (Color value)
			{
				GetComponent<SpriteRenderer>().color = value;
			},
			0,
			0.3f
		).OnComplete(
			delegate ()
			{
				Destroy(gameObject);
			}
		);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
