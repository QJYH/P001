using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackRange;

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

	public virtual GameObject CreateAttackRange(Transform parent)
	{
		string attackRangeDirName = GetAttackRangeObjectDirName();
		Transform parentTransform = (parent != null) ? parent : transform;
		Creature attacker = GetAttacker();
		HarmInfo harmInfo = new HarmInfo(currentHarmInfo);
		AttackRange.TargetType targetType = GetAttackRangeTargetType();
		float attackRangeDurationTime = GetAttackRangeDurationTime();
		
		return AttackRange.CreateAttackRangeObject(attackRangeDirName, parentTransform, attacker, harmInfo, targetType, attackRangeDurationTime);
	}

	protected virtual string GetAttackRangeObjectDirName()
	{
		throw new NotImplementedException();
	}

	protected virtual Creature GetAttacker()
	{
		throw new NotImplementedException();
	}

	protected virtual AttackRange.TargetType GetAttackRangeTargetType()
	{
		throw new NotImplementedException();
	}

	protected virtual float GetAttackRangeDurationTime()
	{
		throw new NotImplementedException();
	}

	

	
}
