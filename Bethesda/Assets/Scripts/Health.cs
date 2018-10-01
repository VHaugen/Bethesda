using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	float maxHp;
	public float MaxHp
	{
		get
		{
			return maxHp;
		}
		set
		{
			maxHp = value;
		}
	}

	float hp;
	public float Hp
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
			if (hp > MaxHp)
			{
				hp = MaxHp;
			}
			if (hp <= 0)
			{
				Die();
			}
		}
	}

	protected virtual void Die()
	{

	}

}
