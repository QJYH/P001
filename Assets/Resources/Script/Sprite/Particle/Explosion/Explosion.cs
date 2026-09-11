using Cinemachine;
using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Particle
{
	const string explosionAttackRangeDirName = "Prefab/Sprite/Particle/Explosion/ExplosionAttackRange";
	const float explosionAttackRangeDurationTime = 0.1f;
	const float explosionHarmValue = 3f;

	public Creature Owner { get; set; }
	public TargetType TargetType { get; set; }

	public void CreateExplosionAttackRange()
	{
		//动画事件调用
		string attackRangeDirName = explosionAttackRangeDirName;
		Creature attacker = Owner;
		HarmInfo harmInfo = new HarmInfo(explosionHarmValue);
		TargetType targetType = TargetType;
		float attackRangeDurationTime = explosionAttackRangeDurationTime;

		GameObject attackRangeObject = AttackRange.CreateAttackRangeObject(attackRangeDirName, attacker, harmInfo, targetType, attackRangeDurationTime);
		attackRangeObject.transform.position = transform.position;

		GetComponent<CinemachineImpulseSource>().GenerateImpulse();
	}
}
