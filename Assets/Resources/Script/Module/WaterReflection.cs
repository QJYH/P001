using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflection : MonoBehaviour
{
	private SpriteRenderer parentSpriteRenderer;
	private SpriteRenderer spriteRenderer;
	// Start is called before the first frame update
	void Start()
    {
		parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sharedMaterial = Resources.Load<Material>("Material/WaterReflection");
		spriteRenderer.color = new Color(0.6f, 0.8f, 1f, 0.6f);

		float verticalOffset = transform.parent.GetComponent<OrderInLayerManager>().verticalOffset;
		transform.localPosition = new Vector3(transform.localPosition.x, (2 * verticalOffset - 10) / 64, 0);

	}

    // Update is called once per frame
    void Update()
    {
		spriteRenderer.sprite = parentSpriteRenderer.sprite;
    }
}
