using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
	public class Sequence : BehaviourNode
	{
		private LinkedList<BehaviourNode> children = new LinkedList<BehaviourNode>();
		private LinkedListNode<BehaviourNode> currentChild;

		protected override void OnBegin()
		{
			currentChild = children.First;
		}
		protected override BehaviorStatus OnUpdate()
		{
			while (true)
			{
				BehaviorStatus status = currentChild.Value.Tick();
				if (status != BehaviorStatus.Success) return status;
				currentChild = currentChild.Next;
				if (currentChild == null) return BehaviorStatus.Success;
			}
		}
		public override void AddChild(BehaviourNode child)
		{
			children.AddLast(child);
		}
	}
}

