using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Selector : BehaviourNode
	{
		private LinkedList<BehaviourNode> children = new LinkedList<BehaviourNode>();
		private LinkedListNode<BehaviourNode> currentChild;

		protected override BehaviorStatus OnUpdate()
		{
			currentChild = children.First;
			while (true)
			{
				BehaviorStatus status = currentChild.Value.Tick();
				if (status != BehaviorStatus.Failure) return status;
				currentChild = currentChild.Next;
				if (currentChild == null) return BehaviorStatus.Failure;
			}
		}
		public override void AddChild(BehaviourNode child)
		{
			children.AddLast(child);
		}
	}
}

