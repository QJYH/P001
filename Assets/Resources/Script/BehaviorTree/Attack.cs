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

		protected override void OnInit()
		{
			enemy.SetStatus(Enemy.EnemyStatus.Attack);
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (!enemy.IsAttacking()) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

