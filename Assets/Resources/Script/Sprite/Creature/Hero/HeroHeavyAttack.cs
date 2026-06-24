using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroHeavyAttack<OwnerType> : HeroAttack<OwnerType> where OwnerType : Hero
{
	public virtual void HandleHeavyAttackInput(InputAction.CallbackContext context)
	{
		throw new NotImplementedException();
	}
}
