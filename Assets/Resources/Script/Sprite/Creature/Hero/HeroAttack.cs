using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAttack : MonoBehaviour
{
	protected HarmInfo currentHarmInfo = new HarmInfo();
	public virtual void OnAttackBegin()
	{

	}

	public virtual void OnAttackEnd()
	{

	}

	public HarmInfo GetCurrentHarmInfo()
	{
		return currentHarmInfo;
	}
}
