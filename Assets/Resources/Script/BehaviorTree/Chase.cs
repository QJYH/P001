using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Chase : BehaviourNode
	{
		private Enemy enemy;
		
		public Chase(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override void OnInit()
		{
			enemy.SetStatus(Enemy.EnemyStatus.Chase);
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemy.HaveTargetInAttackArea()) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

