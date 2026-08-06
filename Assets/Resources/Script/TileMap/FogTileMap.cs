using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogTileMap : MonoBehaviour
{
	private const float disappearDuration = 5f;

	private TilemapRenderer tilemapRenderer;
	private MaterialPropertyBlock propertyBlock;
	private float disappearThreshold;
	private Tween disappearTween;

	private void Start()
	{
		tilemapRenderer = GetComponentInParent<TilemapRenderer>();
		propertyBlock = new MaterialPropertyBlock();
		tilemapRenderer.GetPropertyBlock(propertyBlock);
		SetDisappearThreshold(0);
		SetTileMapData();
		StartDisappear();
	}

	private void SetTileMapData()
	{
		Tilemap tilemap = GetComponent<Tilemap>();
		Bounds localBounds = tilemap.localBounds;
		Vector3 bottomLeftWorldPos = transform.TransformPoint(tilemap.localBounds.min);
		Vector3 topRightWorldPos = transform.TransformPoint(tilemap.localBounds.max);

		propertyBlock.SetVector("_BottomLeftWorldPos", bottomLeftWorldPos);
		propertyBlock.SetVector("_TopRightWorldPos", topRightWorldPos);

	}

	private void SetDisappearThreshold(float value)
	{
		disappearThreshold = value;
		propertyBlock.SetFloat("_DisappearThreshold", value);
		tilemapRenderer.SetPropertyBlock(propertyBlock);
	}

	public void StartDisappear()
	{
		DOTween.To(
			delegate ()
			{
				return disappearThreshold;
			},
			delegate (float value)
			{
				SetDisappearThreshold(value);
			},
			1f,
			disappearDuration
		);
	}

	private void OnDestroy()
	{
		disappearTween?.Kill();
	}
}
