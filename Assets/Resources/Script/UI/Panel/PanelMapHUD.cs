using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMapHUD : Panel
{
	public static PanelMapHUD Instance { get; private set; }
	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = null;
	}
}
