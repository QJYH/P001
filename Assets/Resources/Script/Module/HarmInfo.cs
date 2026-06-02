using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmInfo : Object
{
	private float baseHarm = 0;
	private List<float> harmRateList = new List<float>();
	private List<float> extraHarmList = new List<float>();

	public void Rsset()
	{
		baseHarm = 0;
		harmRateList.Clear();
		extraHarmList.Clear();
	}
	public void SetBaseHarm(float value)
	{
		baseHarm = value;
	}

	public void AddHarmRate(float value)
	{
		harmRateList.Add(value);
	}

	public void AddExtraHarm(float value)
	{
		extraHarmList.Add(value);
	}

	public float CalFinalHarm()
	{
		float finalHarm = baseHarm;
		foreach (float value in harmRateList)
		{
			finalHarm *= value;
		}
		foreach (float value in extraHarmList)
		{
			finalHarm += value;
		}
		return finalHarm;
	}


}
