using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadTileMap : MonoBehaviour
{
	public static UnityEvent<GameObject> OnRoadTileMapStart;
    // Start is called before the first frame update
    void Start()
    {
		OnRoadTileMapStart.Invoke(gameObject);

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
