using CommonEnum;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BehaviorTree
{
	public class PatrolRun : BehaviourNode
	{
		private Enemy enemy;
		private LinkedList<Vector2> patrolPosLinkedList = new LinkedList<Vector2>();
		private LinkedListNode<Vector2> currentPatrolPos;

		public PatrolRun(Enemy enemy)
		{
			this.enemy = enemy;
			InitPatrolPosLinkedList();
		}

		protected override void OnInit()
		{
			enemy.GetComponent<NavMeshAgent>().SetDestination(currentPatrolPos.Value);
			enemy.GetComponent<Animator>().SetBool("IsMoving", true);
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (ReachTargetPartolPos()) return BehaviorStatus.Success;
			
			return BehaviorStatus.Running;
		}

		private void InitPatrolPosLinkedList()
		{
			Vector2 currentPos = enemy.transform.position;
			List<Vector2> patrolPosList = enemy.GetComponent<EnemyPatrolPosList>().PatrolPosList;
			foreach (Vector2 patrolPos in patrolPosList)
			{
				patrolPosLinkedList.AddLast(new Vector2(currentPos.x + patrolPos.x, currentPos.y + patrolPos.y));
			}
			currentPatrolPos = patrolPosLinkedList.First;
		}

		private bool ReachTargetPartolPos()
		{
			if (CommonFunc.IsNear(enemy.transform.position, currentPatrolPos.Value, null))
			{
				currentPatrolPos = currentPatrolPos.Next ?? patrolPosLinkedList.First;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

