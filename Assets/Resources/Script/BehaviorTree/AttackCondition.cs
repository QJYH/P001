using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace BehaviorTree
{
	public class AttackCondition : BehaviourNode
	{
		private Enemy enemy;
		
		public AttackCondition(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override BehaviorStatus OnUpdate()
		{
			if (!enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (enemy.IsAttackInCoolDown()) return BehaviorStatus.Failure;

			return BehaviorStatus.Success;
		}
	}
}

