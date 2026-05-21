using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class PatrolCondition : BehaviourNode
	{
		private Enemy enemy;
		
		public PatrolCondition(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override BehaviorStatus OnUpdate()
		{
			if (enemy.HaveTarget()) return BehaviorStatus.Failure;

			return BehaviorStatus.Success;
		}
	}
}

