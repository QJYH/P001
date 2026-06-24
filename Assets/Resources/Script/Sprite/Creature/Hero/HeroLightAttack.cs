using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroLightAttack<OwnerType> : HeroAttack<OwnerType> where OwnerType : Hero
{
	public virtual void HandleLightAttackInput(InputAction.CallbackContext context)
	{
		throw new NotImplementedException();
	}

	
}
