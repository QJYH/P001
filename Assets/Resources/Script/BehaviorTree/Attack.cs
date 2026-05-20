using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Attack : BehaviourNode
	{
		private Enemy enemy;
		
		public Attack(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override BehaviorStatus OnUpdate()
		{	
			if (enemy.IsTargetInAttackArea())
			{
				enemy.TryAttack();
				return BehaviorStatus.Running;
			}
			else
			{
				return BehaviorStatus.Failure;
			}
		}
	}
}

