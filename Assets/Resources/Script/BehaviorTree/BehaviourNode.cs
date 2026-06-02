using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public enum BehaviorStatus
	{
		Failure,
		Running,
		Success
	}

	public class BehaviourNode
	{
		protected BehaviorStatus status;
		public bool IsFailure()
		{
			return status == BehaviorStatus.Failure;
		}
		public bool IsRunning()
		{
			return status == BehaviorStatus.Running;
		}

		public bool IsSuccess()
		{
			return status == BehaviorStatus.Success;
		}

		public BehaviorStatus Tick()
		{
			if (!IsRunning()) OnInit();
			status = OnUpdate();
			if (!IsRunning()) OnEnd();
			return status;
		}

		protected virtual void OnInit()
		{

		}
		protected virtual BehaviorStatus OnUpdate()
		{
			return BehaviorStatus.Failure;
		}
		protected virtual void OnEnd()
		{

		}
		public virtual void AddChild(BehaviourNode node)
		{
			throw new NotImplementedException();
		}
	}
}

