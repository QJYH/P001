using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundTileMap : MonoBehaviour
{
	public static UnityEvent<GameObject> OnGroundTileMapStart = new UnityEvent<GameObject>();
	// Start is called before the first frame update
	void Start()
    {
		OnGroundTileMapStart.Invoke(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
