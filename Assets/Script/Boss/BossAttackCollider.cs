using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCollider : MonoBehaviour
{
	private BossCtrl bossCtrl;

	private void Start()
	{
		bossCtrl = GetComponentInParent<BossCtrl>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<PlayerCtrl>().TakeHit(bossCtrl.damage);
		}
	}
}
