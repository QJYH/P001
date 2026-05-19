using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	private static TimerManager instance;
	public static TimerManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject timerManager = new GameObject("TimerManager");
				instance = timerManager.AddComponent<TimerManager>();
				DontDestroyOnLoad(timerManager);
			}
			return instance;
		}
	}

	public void DelayCall(float delayTime, Action callback)
	{
		StartCoroutine(DelayCoroutine(delayTime, callback));
	}

	private IEnumerator DelayCoroutine(float delayTime, Action callback)
	{
		yield return new WaitForSeconds(delayTime);
		callback.Invoke();
	}
}
