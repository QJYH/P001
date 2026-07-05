using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

namespace BehaviorTree
{
	public class Chase : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack enemyAttack;

		public Chase(Enemy enemy, EnemyAttack enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{
			enemy.CurrentStatus = EnemyStatus.Chase;
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemyAttack.CanLaunchAttack()) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

