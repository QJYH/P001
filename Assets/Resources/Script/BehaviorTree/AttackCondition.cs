using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace BehaviorTree
{
	public class AttackCondition : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack<Enemy> enemyAttack;
		
		public AttackCondition(Enemy enemy, EnemyAttack<Enemy> enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override BehaviorStatus OnUpdate()
		{
			if (!enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (enemyAttack.IsAttackInCoolDown()) return BehaviorStatus.Failure;

			enemy.SetRunningAttack(enemyAttack.gameObject);
			return BehaviorStatus.Success;
		}
	}
}

