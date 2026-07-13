using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBegin : Panel
{
	public static PanelBegin Instance { get; private set; }
	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = null;
	}
}
