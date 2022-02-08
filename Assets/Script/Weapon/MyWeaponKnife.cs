using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponKnife : MonoBehaviour
{
	private MyWeaponCtrl myWeaponCtrl;
	private void Start()
	{
		myWeaponCtrl = FindObjectOfType<MyWeaponCtrl>();	
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyCtrl>().TakeHit(myWeaponCtrl.currentWeapon.damage);
		}
	}
}
