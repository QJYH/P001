using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Attack : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack<Enemy> enemyAttack;

		public Attack(Enemy enemy, EnemyAttack<Enemy> enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{
			enemyAttack.Attack();
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (!(enemy.CurrentStatus == EnemyStatus.Attack)) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

