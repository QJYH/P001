using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Patrol : BehaviourNode
	{
		private Enemy enemy;
		private LinkedList<Vector2> patrolPosList;
		private LinkedListNode<Vector2> targetPatrolPos;
		
		public Patrol(Enemy enemy, LinkedList<Vector2> patrolPosList)
		{
			this.enemy = enemy;
			this.patrolPosList = patrolPosList;
			this.targetPatrolPos = this.patrolPosList.First;
		}

		protected override BehaviorStatus OnUpdate()
		{
			float distance = Vector2.Distance(enemy.transform.position, targetPatrolPos.Value);
			if (distance < 0.1f)
			{
				targetPatrolPos = targetPatrolPos.Next;
				if (targetPatrolPos == null)
				{
					targetPatrolPos = patrolPosList.First;
				}
			}

			enemy.GoToPos(targetPatrolPos.Value);
			return BehaviorStatus.Running;
		}
	}
}

