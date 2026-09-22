using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialize : MonoBehaviour
{
	public Texture2D cursorTexture;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Cursor.SetCursor(cursorTexture, new Vector2(10, 10), CursorMode.Auto);

		SceneManager.LoadScene("SceneBegin");
	}
}
