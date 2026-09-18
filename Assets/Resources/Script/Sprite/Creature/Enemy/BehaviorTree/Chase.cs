using CommonEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BehaviorTree
{
	public class Chase : BehaviourNode
	{
		private Enemy enemy;
		private EnemyAttack enemyAttack;

		public Chase(Enemy enemy, EnemyAttack enemyAttack)
		{
			this.enemy = enemy;
			this.enemyAttack = enemyAttack;
		}

		protected override void OnInit()
		{

		}
		protected override BehaviorStatus OnUpdate()
		{
			enemy.UpdateBaseMoveVelocity(GetChaseTargetPos());

			if (!enemy.HaveTarget()) return BehaviorStatus.Failure;
			if (enemyAttack.CanLaunchAttack()) return BehaviorStatus.Success;

			return BehaviorStatus.Running;
		}

		private Vector3 GetChaseTargetPos()
		{
			Vector3 targetPos;
			BoxCollider2D creatureBlock = enemy.transform.Find("CreatureBlock").GetComponent<BoxCollider2D>();
			Vector2 collisionOffset = creatureBlock.offset;
			Vector2 collisionSize = creatureBlock.size;

			Vector3 heroPos = enemy.Target.transform.position;
			BoxCollider2D heroCollider = enemy.Target.GetComponent<BoxCollider2D>();
			heroPos.x += heroCollider.offset.x;
			heroPos.y += heroCollider.offset.y;
			GameObject detectRangeObject = enemy.RunningAttack.transform.Find("DetectRange").gameObject;
			PolygonCollider2D detectRange = detectRangeObject.GetComponent<PolygonCollider2D>();
			Vector3 potentialTargetPos1 = new Vector3(heroPos.x - detectRange.offset.x, heroPos.y - detectRange.offset.y);
			Vector3 potentialTargetPos2 = new Vector3(heroPos.x + detectRange.offset.x, heroPos.y - detectRange.offset.y);

			bool canStandOnPos1 = TileMapManager.CanStandOnPosition(potentialTargetPos1, collisionOffset, collisionSize);
			bool canStandOnPos2 = TileMapManager.CanStandOnPosition(potentialTargetPos2, collisionOffset, collisionSize);
			if (canStandOnPos1 && canStandOnPos2)
			{
				float distance1 = Vector2.Distance(enemy.transform.position, potentialTargetPos1);
				float distance2 = Vector2.Distance(enemy.transform.position, potentialTargetPos2);
				targetPos = (distance1 < distance2) ? potentialTargetPos1 : potentialTargetPos2;
			}
			else if (canStandOnPos1 || canStandOnPos2)
			{
				targetPos = canStandOnPos1 ? potentialTargetPos1 : potentialTargetPos2;
			}
			else
			{
				targetPos = heroPos;
			}

			return targetPos;
		}
	}
}

