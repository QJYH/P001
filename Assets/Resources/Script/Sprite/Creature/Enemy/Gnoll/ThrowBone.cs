using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

public class ThrowBone : EnemyAttack
{
	private const string gnollBoneDirName = "Prefab/Sprite/Projectile/Enemy/Gnoll/GnollBone";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 2f;

	public override void CreateRangedProjectile()
	{
		Vector2 originalDirection = (transform.Find("ProjectilePosition").position - transform.position).normalized;
		List<float> angleOffsetList = new List<float>{ -25, 0, 25 };
		foreach (float agnleOffset in angleOffsetList)
		{
			GameObject gnollBone = Instantiate(Resources.Load<GameObject>(gnollBoneDirName));
			gnollBone.transform.position = transform.Find("ProjectilePosition").position;
			gnollBone.GetComponent<GnollBone>().Owner = Owner;
			gnollBone.GetComponent<GnollBone>().TargetType = TargetType.Hero;
			gnollBone.GetComponent<GnollBone>().HarmInfo = new HarmInfo(CurrentHarmInfo);
			gnollBone.GetComponent<GnollBone>().Direction = Vector3.RotateTowards(originalDirection, Vector2.up, agnleOffset * Mathf.Deg2Rad, 0).normalized;
		}
	}

}
