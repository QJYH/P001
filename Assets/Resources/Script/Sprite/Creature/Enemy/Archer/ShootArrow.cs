using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : EnemyAttack
{
	private const string archerArrowDirName = "Prefab/Sprite/Projectile/Enemy/Archer/ArcherArrow";

	protected override float AttackCoolDownMaxTime { get; } = 2f;
	protected override float AttackHarmValue { get; } = 1f;

	public override void CreateRangedProjectile()
	{
		GameObject archerArrow = Instantiate(Resources.Load<GameObject>(archerArrowDirName));
		archerArrow.transform.position = transform.Find("ProjectilePosition").position;
		archerArrow.GetComponent<ArcherArrow>().Owner = Owner;
		archerArrow.GetComponent<ArcherArrow>().Target = ((Enemy)Owner).Target.gameObject;
		archerArrow.GetComponent<ArcherArrow>().TargetType = TargetType.Hero;
		archerArrow.GetComponent<ArcherArrow>().HarmInfo = new HarmInfo(CurrentHarmInfo);
		Vector2 direction = (transform.Find("ProjectilePosition").position - transform.position).normalized;
		archerArrow.GetComponent<ArcherArrow>().Direction = direction;
	}
}
