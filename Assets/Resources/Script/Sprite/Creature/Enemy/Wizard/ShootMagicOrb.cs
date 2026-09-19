using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMagicOrb : EnemyAttack
{
	private const string magicOrbDirName = "Prefab/Sprite/Projectile/Enemy/Wizard/MagicOrb";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 2f;

	public override void CreateRangedProjectile()
	{
		GameObject magicOrb = Instantiate(Resources.Load<GameObject>(magicOrbDirName));
		magicOrb.transform.position = transform.Find("ProjectilePosition").position;
		magicOrb.GetComponent<MagicOrb>().Owner = Owner;
		magicOrb.GetComponent<MagicOrb>().Target = ((Enemy)Owner).Target.gameObject;
		magicOrb.GetComponent<MagicOrb>().TargetType = TargetType.Hero;
		magicOrb.GetComponent<MagicOrb>().HarmInfo = new HarmInfo(CurrentHarmInfo);
		Vector2 direction = (transform.Find("ProjectilePosition").position - transform.position).normalized;
		magicOrb.GetComponent<MagicOrb>().Direction = direction;
	}
}
