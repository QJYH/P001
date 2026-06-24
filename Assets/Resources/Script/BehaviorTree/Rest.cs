using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using CommonEnum;

namespace BehaviorTree
{
	public class Rest : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack<Enemy> enemyAttack;

		public Rest(Enemy enemy, EnemyAttack<Enemy> enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{
			enemy.CurrentStatus = EnemyStatus.Rest;
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemyAttack.IsAttackInCoolDown()) return BehaviorStatus.Running;

			return BehaviorStatus.Success;
		}
	}
}

