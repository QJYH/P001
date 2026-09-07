using System;
using System.Collections;
using System.Collections.Generic;

public class HarmInfo : Object
{
	public float BaseHarm { get; set; }
	private List<float> harmRateList;
	private List<float> extraHarmList;

	public HarmInfo()
	{
		BaseHarm = 0;
		harmRateList = new List<float>();
		extraHarmList = new List<float>();
	}

	public HarmInfo(HarmInfo original)
	{
		BaseHarm = original.BaseHarm;
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
		BaseHarm = 0;
		harmRateList.Clear();
		extraHarmList.Clear();
	}

	public void AddHarmRate(float value)
	{
		harmRateList.Add(value);
	}

	public void AddExtraHarm(float value)
	{
		extraHarmList.Add(value);
	}

	public float CalFinalHarm(Creature attacker, Creature receiver)
	{
		float finalHarm = BaseHarm;
		foreach (float value in harmRateList)
		{
			finalHarm *= value;
		}
		foreach (float value in extraHarmList)
		{
			finalHarm += value;
		}
		finalHarm += attacker.ExtraAttack;
		finalHarm -= receiver.ExtraDefend;
		return finalHarm;
	}


}
