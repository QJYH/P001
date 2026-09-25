using CommonEnum;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
	public class Rest : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack enemyAttack;

		public Rest(Enemy enemy, EnemyAttack enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{
			enemy.GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);
			enemy.GetComponent<Animator>().SetBool("IsMoving", false);
		}
		protected override BehaviorStatus OnUpdate()
		{
			if (enemyAttack.IsAttackInCoolDown()) return BehaviorStatus.Running;

			return BehaviorStatus.Success;
		}
	}
}

