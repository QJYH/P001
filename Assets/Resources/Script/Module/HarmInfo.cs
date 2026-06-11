using System;
using System.Collections;
using System.Collections.Generic;

public class HarmInfo : Object
{
	public float baseHarm;
	private List<float> harmRateList;
	private List<float> extraHarmList;

	public HarmInfo()
	{
		baseHarm = 0;
		harmRateList = new List<float>();
		extraHarmList = new List<float>();
	}

	public HarmInfo(HarmInfo original)
	{
		baseHarm = original.baseHarm;
		harmRateList = new List<float>();
		foreach (float value in original.harmRateList)
		{
			harmRateList.Add(value);
		}
		extraHarmList = new List<float>();
		foreach (float value in original.extraHarmList)
		{
			extraHarmList.Add(value);
		}
	}

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
