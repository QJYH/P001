using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerManager : MonoBehaviour
{
	[SerializeField] public float verticalOffset;
	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		spriteRenderer.sortingOrder = (int)-(transform.position.y * 64 + verticalOffset);
	}
}
