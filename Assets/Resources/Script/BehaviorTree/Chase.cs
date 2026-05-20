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

		protected override BehaviorStatus OnUpdate()
		{	
			if (enemy.Target != null)
			{
				enemy.GoToPos(enemy.Target.transform.position);
				return BehaviorStatus.Running;
			}
			else
			{
				
				return BehaviorStatus.Failure;
			}
		}
	}
}

