using CommonEnum;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BehaviorTree
{
	public class PatrolRun : BehaviourNode
	{
		private Enemy enemy;
		private LinkedList<Vector2> patrolPosList = new LinkedList<Vector2>();
		private LinkedListNode<Vector2> currentPatrolPos;

		public PatrolRun(Enemy enemy)
		{
			this.enemy = enemy;
			InitPatrolPosList();
		}

		protected override void OnInit()
		{
			enemy.CurrentStatus = EnemyStatus.PatrolRun;
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (ReachTargetPartolPos()) return BehaviorStatus.Success;		
			return BehaviorStatus.Running;
		}

		private void InitPatrolPosList()
		{
			Vector2 currentPos = enemy.transform.position;
			Vector2 patrolPos1 = new Vector2(currentPos.x - 2, currentPos.y);
			Vector2 patrolPos2 = new Vector2(currentPos.x + 2, currentPos.y);
			patrolPosList.AddLast(patrolPos1);
			patrolPosList.AddLast(patrolPos2);
			SetCurrentPatrolPosition(patrolPosList.First);
			
		}

		private void SetCurrentPatrolPosition(LinkedListNode<Vector2> node)
		{
			currentPatrolPos = node;
			enemy.CurrentPatrolPos = currentPatrolPos.Value;
		}

		private bool ReachTargetPartolPos()
		{
			if (CommonFunc.IsNear(enemy.transform.position, currentPatrolPos.Value, null))
			{
				SetCurrentPatrolPosition(currentPatrolPos.Next ?? patrolPosList.First);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

