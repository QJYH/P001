using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class ThrowBone : EnemyAttack
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
		Vector2 direction = (transform.Find("ProjectilePosition").position - transform.position).normalized;
		gnollBone.GetComponent<GnollBone>().Direction = direction;
	}

}
