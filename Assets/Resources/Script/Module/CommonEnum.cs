using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonEnum
{
	public enum TargetType
	{
		Hero,
		Enemy
	}

	public enum HeroStatus
	{
		None,
		Idle,
		Run,
		Rush,
		LightAttack,
		HeavyAttack,
		Heal,
		Die
	}

	public enum EnemyStatus
	{
		None,
		PatrolRun,
		PatrolWatch,
		Rest,
		Chase,
		Attack,
		Die
	}
}

