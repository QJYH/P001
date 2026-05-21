using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Rest : BehaviourNode
	{
		private Enemy enemy;
		
		public Rest(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override void OnInit()
		{
			enemy.SetStatus(Enemy.EnemyStatus.Rest);
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (!enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (!enemy.IsAttackInCoolDown()) return BehaviorStatus.Failure;

			return BehaviorStatus.Running;
		}
	}
}

