using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunc : MonoBehaviour
{
	public static bool IsNear(float numberA, float numberB, float? threshold)
	{
		threshold = threshold ?? 0.1f;
		return Mathf.Abs(numberA - numberB) < threshold;
	}
    public static bool IsNear(Vector2 beginWorldPos, Vector2 endWorldPos, float? threshold)
	{
		threshold = threshold ?? 0.1f;
		return Vector2.Distance(beginWorldPos, endWorldPos) < threshold;
	}

	public static void FollowMainCamera(GameObject gameObject)
	{
		Vector3 cameraPos = SceneDirector.SceneInstance.MainCamera.transform.position;
		gameObject.transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
	}

	public static void ScaleToFillScreen(GameObject gameObject)
	{
		Camera mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
		Vector3 bottomLeftWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRightWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		Vector2 spriteSize = gameObject.GetComponent<SpriteRenderer>().sprite.rect.size;
		float screenWidth = (topRightWorldPos.x - bottomLeftWorldPos.x) * CommonConst.PixelPerUnit;
		float screenHeight = (topRightWorldPos.y - bottomLeftWorldPos.y) * CommonConst.PixelPerUnit;
		gameObject.transform.localScale = new Vector2(screenWidth / spriteSize.x, screenHeight / spriteSize.y);
	}
}
