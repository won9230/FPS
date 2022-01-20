using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponCtrl : MonoBehaviour
{
	public MyWeapon weapon;
	public GameObject FineSight;

	private float currentFireRate;
	private bool isReload = false;
	private bool isFineSightMode = false;
	[SerializeField] private Vector3 originPos;
	[SerializeField] private Vector3 fineSightOriginPos;


	private AudioSource audioSource;
	private Animator anim;

	private void Start()
	{
		audioSource = GetComponentInParent<AudioSource>();
		anim = GetComponent<Animator>();
		originPos = transform.localPosition;
	}
	private void Update()
	{
		GunFireRateCalc();
		TryFire();
		Reload();
		TryFineSight();
	}
	private void GunFireRateCalc()
	{
		if (currentFireRate > 0)
			currentFireRate -= Time.deltaTime;
	}
	private void TryFire()
	{
		if(Input.GetMouseButton(0) && currentFireRate <= 0 && !isReload)
		{
			Fire();
		}
	}

	private void Fire()
	{
		if (!isReload)
		{
			if (weapon.curentBulletCount > 0)
			{
				Shoot();
			}
		}
	}
	private void Shoot()
	{
		weapon.curentBulletCount--;
		currentFireRate = weapon.fireRate; // 연사속도
		anim.SetTrigger("Shoot");
		PlaySE(weapon.fire_Sound);
		Debug.Log("발사");
	}
	private void Reload()
	{
		if (Input.GetKeyDown(KeyCode.R) && !isReload && weapon.curentBulletCount < weapon.reloadBulletCount)
		{
			StartCoroutine(ReloadCoroutime());
		}
	}
	IEnumerator ReloadCoroutime()
	{
		isReload = true;
		if (weapon.carryBulletCount > 0)
		{
			anim.SetTrigger("Reload");

			weapon.carryBulletCount += weapon.curentBulletCount;
			weapon.curentBulletCount = 0;
			yield return new WaitForSeconds(weapon.reloadTime);
			if(weapon.carryBulletCount > weapon.reloadBulletCount)
			{
				weapon.curentBulletCount = weapon.reloadBulletCount;
				weapon.carryBulletCount -= weapon.reloadBulletCount;
			}
			else
			{
				weapon.curentBulletCount = weapon.carryBulletCount;
				weapon.carryBulletCount = 0;
			}
			isReload = false;
		}
		else
		{
			Debug.Log("총알 없음");
		}

	}

	private void PlaySE(AudioClip _cilp)
	{
		audioSource.clip = _cilp;
		audioSource.Play();
	}
	
	private void TryFineSight()
	{
		if (Input.GetMouseButtonDown(1))
		{
			FineSight();
		}
	}
	private void FineSight()
	{
		isFineSightMode = !isFineSightMode;
		anim.SetBool("FineSightMode", isFineSightMode);
		if (isFineSightMode)
		{
			StartCoroutine(FineSightActive());
		}
		else
		{
			StartCoroutine(FineSightDective());
		}
	}
	IEnumerator FineSightActive()
	{
			while(this.transform.localPosition != fineSightOriginPos)
		{
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, fineSightOriginPos, 0.2f);
			yield return null;
		}
	}
	IEnumerator FineSightDective()
	{
		while (transform.localPosition != originPos)
		{
			transform.localPosition= Vector3.Lerp(transform.localPosition, originPos, 0.2f);
			yield return null;
		}
	}
}
