using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using CommonEnum;

namespace BehaviorTree
{
	public class Patrol : BehaviourNode
	{
		private Enemy enemy;
		
		public Patrol(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override void OnInit()
		{
			enemy.CurrentStatus = EnemyStatus.Patrol;
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemy.HaveTarget()) return BehaviorStatus.Success;
			
			return BehaviorStatus.Running;
		}
	}
}

