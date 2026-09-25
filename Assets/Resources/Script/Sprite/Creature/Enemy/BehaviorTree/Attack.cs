using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
	public class Attack : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack enemyAttack;

		public Attack(Enemy enemy, EnemyAttack enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{
			enemy.GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);
			enemy.GetComponent<Animator>().SetBool("IsMoving", false);
			enemyAttack.Attack();
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (!enemy.RunningAttack) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}
	}
}

