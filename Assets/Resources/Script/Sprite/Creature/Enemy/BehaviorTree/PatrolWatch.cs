using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using CommonEnum;

namespace BehaviorTree
{
	public class PatrolWatch : BehaviourNode
	{
		private Enemy enemy;
		private float watchMaxTime = 2f;
		private float watchTime;

		public PatrolWatch(Enemy enemy)
		{
			this.enemy = enemy;
		}

		protected override void OnInit()
		{
			enemy.CurrentStatus = EnemyStatus.PatrolWatch;
			watchTime = 0f;
		}
		protected override BehaviorStatus OnUpdate()
		{
			watchTime += Time.deltaTime;
			if (enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (watchTime >= watchMaxTime) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

