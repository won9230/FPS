using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPoolingManager))]
public class MyWeaponCtrl : MonoBehaviour
{
	public MyWeapon weapon;
	public GameObject fineSightPos;

	private float currentFireRate;
	private bool isReload = false;
	private bool isFineSightMode = false;
	[SerializeField] private Vector3 originPos;
	[SerializeField] private Vector3 fineSightOriginPos;
	public GameObject bulletPos;


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
		StopAllCoroutines();
		weapon.curentBulletCount--;
		currentFireRate = weapon.fireRate; // 연사속도
		CerateBullet();
		anim.SetTrigger("Shoot");
		StartCoroutine(RetroAction());
		PlaySE(weapon.fire_Sound);
	}
	private void Reload()
	{
		if (Input.GetKeyDown(KeyCode.R) && !isReload && weapon.curentBulletCount < weapon.reloadBulletCount)
		{
			CancelFineSight();
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
		if (Input.GetMouseButtonDown(1) && !isReload)
		{
			FineSight();
		}
	}
	public void CancelFineSight()
	{
		if (isFineSightMode)
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
			StopAllCoroutines();
			StartCoroutine(FineSightActive());
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(FineSightDective());
		}
	}
	IEnumerator FineSightActive()
	{
			while(fineSightPos.transform.localPosition != weapon.fineSightOriginPos)
		{
			fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, weapon.fineSightOriginPos, 0.2f);
			yield return null;
		}
	}
	IEnumerator FineSightDective()
	{
		while (fineSightPos.transform.localPosition != originPos)
		{
			fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, originPos, 0.2f);
			yield return null;
		}
	}
	IEnumerator RetroAction()
	{
		Vector3 recoilBack = new Vector3(originPos.x, originPos.y,weapon.retroActionForce);
		Vector3 retroActionRecoil = new Vector3(weapon.fineSightOriginPos.x, weapon.fineSightOriginPos.y, weapon.retroActionFineSightForce);
		if (!isFineSightMode)
		{
			fineSightPos.transform.localPosition = originPos;
			while (fineSightPos.transform.localPosition.z >= weapon.retroActionForce + 0.02f) //반동
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, recoilBack, 0.4f);
				yield return null;
			}
			while(fineSightPos.transform.localPosition != originPos) //원위치
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, originPos, 0.1f);
				yield return null;
			}
		}
		else
		{
			fineSightPos.transform.localPosition = weapon.fineSightOriginPos;
			while (fineSightPos.transform.localPosition.z >= weapon.retroActionFineSightForce + 0.02f) //반동
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, retroActionRecoil, 0.4f);
				yield return null;
			}
			while (fineSightPos.transform.localPosition != originPos) //원위치
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, weapon.fineSightOriginPos, 0.1f);
				yield return null;
			}
		}
	}
	private void CerateBullet()
	{
		//Instantiate(weapon.bullet,bulletPos.transform.position,transform.rotation);
		GameObject t_object = ObjectPoolingManager.instance.GetQueue();
		t_object.transform.position = bulletPos.transform.position;
		t_object.transform.rotation = transform.rotation;
	}
}
