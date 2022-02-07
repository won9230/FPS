using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponBulletDamage : MonoBehaviour
{
	private new Rigidbody rigidbody;
	[SerializeField] private float lifeTime;
	[SerializeField] private float speed;
	private MyWeaponCtrl myWeaponCtrl;
	private void Start()
	{
		myWeaponCtrl = FindObjectOfType<MyWeaponCtrl>();
		rigidbody = GetComponent<Rigidbody>();
		Destroy(gameObject, lifeTime);
	}
	private void Update()
	{
		rigidbody.velocity = transform.forward * Time.deltaTime * speed;
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyCtrl>().TakeHit(myWeaponCtrl.currentWeapon.damage);
			Debug.Log(myWeaponCtrl.currentWeapon.damage);
		}
	}
}
