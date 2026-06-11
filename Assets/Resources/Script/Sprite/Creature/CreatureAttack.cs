using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttack : MonoBehaviour
{
	protected HarmInfo currentHarmInfo = new HarmInfo();

	public HarmInfo GetCurrentHarmInfo()
	{
		return currentHarmInfo;
	}

	public virtual void OnAttackBegin()
	{

	}

	public virtual void OnAttackEnd()
	{

	}

	public void SetAttackRangeEnable(string rangeName, bool enable)
	{
		if (enable)
		{
			HarmInfo harmInfo = new HarmInfo(currentHarmInfo);
			transform.Find(rangeName).GetComponent<AttackRange>().EnableAttackRange(harmInfo);
		}
		else
		{
			transform.Find(rangeName).GetComponent<AttackRange>().DisableAttackRange();
		}
	}
}
