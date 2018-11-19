using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageParams
{
	public float amount;
	public Element element;

	public DamageParams(float amount, Element element = Element.None)
	{
		this.amount = amount;
		this.element = element;
	}
}

public interface IAttackable
{
	void TakeDamage(DamageParams args);
}
