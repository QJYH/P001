using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class ThrowBone : EnemyAttack
{
	private const float gnollBoneHarmValue = 1f;
	private const float throwBoneCoolDownMaxTime = 2f;
	private const string gnollBoneDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBone";

	protected override void InitAttackHarmValue()
	{
		attackHarmValue = gnollBoneHarmValue;
	}

	protected override void InitAttackCoolDownMaxTime()
	{
		attackCoolDownMaxTime = throwBoneCoolDownMaxTime;
	}
	public override void CreateRangedProjectile()
	{
		GameObject gnollBone = Instantiate(Resources.Load<GameObject>(gnollBoneDirName));
		gnollBone.transform.position = transform.Find("ProjectilePosition").position;
		gnollBone.GetComponent<GnollBone>().Owner = enemy;
		gnollBone.GetComponent<GnollBone>().TargetType = AttackTargetType.Hero;
		gnollBone.GetComponent<GnollBone>().HarmInfo = new HarmInfo(CurrentHarmInfo);
		if (enemy.GetComponent<SpriteRenderer>().flipX == true)
		{
			gnollBone.GetComponent<GnollBone>().Direction = new Vector2(-1, 0);
		}
		else
		{
			gnollBone.GetComponent<GnollBone>().Direction = new Vector2(1, 0);
		}
	}

}
