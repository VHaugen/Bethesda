using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
	Hit,
	Squash,
}

public struct DamageParams
{
	public float amount;
	public Element element;
	public DamageType damageType;
	public Vector3 knockback;

	public DamageParams(float amount, Element element = Element.None, DamageType damageType = DamageType.Hit)
	{
		this.amount = amount;
		this.element = element;
		this.damageType = damageType;
		this.knockback = Vector3.zero;
	}

	public DamageParams(float amount, Element element, DamageType damageType, Vector3 knockback)
	{
		this.amount = amount;
		this.element = element;
		this.damageType = damageType;
		this.knockback = knockback;
	}
}

public interface IAttackable
{
	void TakeDamage(DamageParams args);
}
