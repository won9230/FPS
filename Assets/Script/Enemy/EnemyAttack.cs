using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	private EnemyCtrl enemyCtrl;
	private EnemyAnimEvent animEvent;
	private void Start()
	{
		enemyCtrl = GetComponentInParent<EnemyCtrl>();
		animEvent = GetComponentInParent<EnemyAnimEvent>();
	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("asd");
		if(other.CompareTag("Player"))
		{
			//Debug.Log("asd");
			other.GetComponent<PlayerCtrl>().TakeHit(enemyCtrl.damage);
			animEvent.gameObject.SetActive(false);
		}
	}
}
