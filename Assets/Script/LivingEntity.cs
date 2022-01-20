using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
	public float hp;
	public float maxHp;
	public bool dead;

	protected virtual void Start()
	{
		maxHp = hp;
	}
	public void TakeHit(float damage)
	{
		hp -= damage;
	}
	public void Die()
	{
		dead = true;
	}
}
