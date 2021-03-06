using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LivingEntity : MonoBehaviour
{
	public float hp;
	public float maxHp;
	public bool dead = false;

	protected virtual void Start()
	{
		maxHp = hp;
	}
	public void TakeHit(float damage)
	{
		hp -= damage;
	}
}
