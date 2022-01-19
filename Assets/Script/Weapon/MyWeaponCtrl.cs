using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponCtrl : MonoBehaviour
{
	public MyWeapon weapon;

	private float currentFireRate;

	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponentInParent<AudioSource>();
	}
	private void Update()
	{
		GunFireRateCalc();
		TryFire();
	}
	private void GunFireRateCalc()
	{
		if (currentFireRate > 0)
			currentFireRate -= Time.deltaTime;
	}
	private void TryFire()
	{
		if(Input.GetMouseButton(0) && currentFireRate <= 0)
		{
			Fire();
		}
	}

	private void Fire()
	{
		currentFireRate = weapon.fireRate;
		Shoot();
	}
	private void Shoot()
	{
		PlaySE(weapon.fire_Sound);
		Debug.Log("발사");
	}

	private void PlaySE(AudioClip _cilp)
	{
		audioSource.clip = _cilp;
		audioSource.Play();
	}
}
