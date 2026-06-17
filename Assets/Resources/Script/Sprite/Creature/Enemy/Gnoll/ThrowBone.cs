using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBone : EnemyAttack
{
	private readonly float gnollBoneHarmValue = 1f;
	private readonly float throwBoneCoolDownMaxTime = 2f;
	private readonly string gnollBoneDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBone";

	protected override void InitAttackHarmValue()
	{
		attackHarmValue = gnollBoneHarmValue;
	}

	protected override void InitAttackCoolDownMaxTime()
	{
		attackCoolDownMaxTime = throwBoneCoolDownMaxTime;
	}
	public override GameObject CreateRangedProjectile()
	{
		GameObject gnollBone = Instantiate(Resources.Load<GameObject>(gnollBoneDirName));
		gnollBone.transform.position = transform.Find("ProjectilePosition").position;
		gnollBone.GetComponent<GnollBone>().SetOwner(enemy);
		gnollBone.GetComponent<GnollBone>().SetHarmInfo(currentHarmInfo);
		if (enemy.GetComponent<SpriteRenderer>().flipX == true)
		{
			gnollBone.GetComponent<GnollBone>().SetDirection(new Vector2(-1, 0));
		}
		else
		{
			gnollBone.GetComponent<GnollBone>().SetDirection(new Vector2(1, 0));
		}

		return gnollBone;
	}

}
