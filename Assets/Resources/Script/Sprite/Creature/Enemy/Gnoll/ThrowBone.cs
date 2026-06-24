using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class ThrowBone : EnemyAttack<Gnoll>
{
	private const string gnollBoneDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBone";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 1f;

	public override void CreateRangedProjectile()
	{
		GameObject gnollBone = Instantiate(Resources.Load<GameObject>(gnollBoneDirName));
		gnollBone.transform.position = transform.Find("ProjectilePosition").position;
		gnollBone.GetComponent<GnollBone>().Owner = Owner;
		gnollBone.GetComponent<GnollBone>().TargetType = TargetType.Hero;
		gnollBone.GetComponent<GnollBone>().HarmInfo = new HarmInfo(CurrentHarmInfo);
		if (Owner.GetComponent<SpriteRenderer>().flipX == true)
		{
			gnollBone.GetComponent<GnollBone>().Direction = new Vector2(-1, 0);
		}
		else
		{
			gnollBone.GetComponent<GnollBone>().Direction = new Vector2(1, 0);
		}
	}

}
