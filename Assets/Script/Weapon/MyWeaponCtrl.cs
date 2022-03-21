using System;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyWeaponMoveAnim))]
public class MyWeaponCtrl : MonoBehaviour
{
	public MyWeapon currentWeapon; //플레이거가 들고있는 무기
	//public Transform currentObject; //들고있는 무기 오브젝트
	public GameObject fineSightPos; //조준 위치
	[SerializeField] private Transform playerItemSpawn; //총을 버릴 위치

	private float currentFireRate; //연사속도
	private bool isReload = false; //장전 중
	[HideInInspector] public bool isFineSightMode = false; //조준 중
	private Vector3 originPos; //조준 전 원래 위치
	private Quaternion originRot; //조준 전 원래 위치
	public GameObject camPos; //총알 나가는 위치

	private AudioSource audioSource; //총 소리
	private Animator anim; //애니
	private MyWeaponMoveAnim myWeaponMoveAnim;
	private MyWeaponManager myWeaponManager;
	private PlayerCtrl playerCtrl;
	[SerializeField] private InGameUI inGameUI;


	private void Start()
	{
		audioSource = GetComponentInParent<AudioSource>();
		//anim = currentWeapon.GetComponent<Animator>();
		myWeaponMoveAnim = GetComponent<MyWeaponMoveAnim>();
		myWeaponMoveAnim.anim = anim;
		myWeaponManager = GetComponentInParent<MyWeaponManager>();

		playerCtrl = GetComponentInParent<PlayerCtrl>();
		originPos = fineSightPos.transform.localPosition;
		originRot = camPos.transform.localRotation;
	}
	private void Update()
	{
		if (currentWeapon != null && !inGameUI.isChatMode)
		{
			if (currentWeapon.weaponType == WeaponType.GUN || currentWeapon.weaponType == WeaponType.Sub)
			{
				GunFireRateCalc();
				TryFire();
				Reload();
				TryFineSight();
			}
			if (currentWeapon.weaponType == WeaponType.Melee)
			{
				MeeleAttack();
			}
		}
	}
	private void GunFireRateCalc() //연사 속도 계산
	{
		if (currentFireRate > 0)
			currentFireRate -= Time.deltaTime;
	}
	private void TryFire() //발사(연사 계산)
	{
		if(Input.GetMouseButton(0) && currentFireRate <= 0 && !isReload) 
		{
			Fire();
		}
	}
	private void Fire() //발사(총알 계산)
	{
		if (!isReload)
		{
			if (currentWeapon.curentBulletCount > 0)
			{
				Shoot();
			}
		}
	}
	private void Shoot() // 발사
	{
		StopAllCoroutines();
		currentWeapon.curentBulletCount--; //총알 --
		currentFireRate = currentWeapon.fireRate; // 연사속도
		CerateBullet(); //총알생성
		anim.SetTrigger("Shoot"); //애니
		currentWeapon.gunFlash.Play();
		StartCoroutine(RetroAction()); //총 앞뒤 반동
		StartCoroutine(CamAction2());
		PlaySE(currentWeapon.fire_Sound); //총 소리
		CamAction(); //카메라 에임 조종
	}
	private void Reload() //재장전
	{
		if (Input.GetKeyDown(KeyCode.R) && !isReload && currentWeapon.curentBulletCount < currentWeapon.reloadBulletCount)
		{
			CancelFineSight();
			StartCoroutine(ReloadCoroutime());
		}
	}
	IEnumerator ReloadCoroutime() //재장전 계산
	{
		isReload = true;
		if (currentWeapon.carryBulletCount > 0)
		{
			anim.SetTrigger("Reload");

			currentWeapon.carryBulletCount += currentWeapon.curentBulletCount;
			currentWeapon.curentBulletCount = 0;
			yield return new WaitForSeconds(currentWeapon.reloadTime);
			if(currentWeapon.carryBulletCount > currentWeapon.reloadBulletCount)
			{
				currentWeapon.curentBulletCount = currentWeapon.reloadBulletCount;
				currentWeapon.carryBulletCount -= currentWeapon.reloadBulletCount;
			}
			else
			{
				currentWeapon.curentBulletCount = currentWeapon.carryBulletCount;
				currentWeapon.carryBulletCount = 0;
			}
		}
		else
		{
			Debug.Log("총알 없음");
		}
		isReload = false;
	}

	private void PlaySE(AudioClip _cilp) //총 소리
	{
		audioSource.clip = _cilp;
		audioSource.Play();
	}

	public void CancelReload() //재장전 캔슬
	{
		if (isReload)
		{
			isReload = false;
			StopAllCoroutines();
		}
	}
	public void CancelFineSight() //조준 캔슬
	{
		if (isFineSightMode)
		{
			isFineSightMode = false;
			StopAllCoroutines();
			StartCoroutine(FineSightDective());
		}
	}
	private void TryFineSight() //조준1
	{
		if (!isReload && !playerCtrl.isRun)
		{
			FineSight();
		}
	}
	private void FineSight() //조준
	{
		if (Input.GetMouseButton(1))
		{
			isFineSightMode = true;
			StopAllCoroutines();
			StartCoroutine(FineSightActive());
		}
		if(Input.GetMouseButtonUp(1))
		{
			isFineSightMode = false;
			StopAllCoroutines();
			StartCoroutine(FineSightDective());
		}
		anim.SetBool("FineSightMode", isFineSightMode);
	}
	IEnumerator FineSightActive() //줌 활성화
	{
		while (fineSightPos.transform.localPosition != currentWeapon.fineSightOriginPos)
		{
			fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, currentWeapon.fineSightOriginPos, Time.deltaTime * 10f);
			yield return null;
		}
	}
	IEnumerator FineSightDective()//줌 비활성화
	{
		while (fineSightPos.transform.localPosition != originPos)
		{
			fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, originPos, Time.deltaTime * 10f);
			yield return null;
		}
	}
	IEnumerator RetroAction() //총 앞뒤 반동
	{
		Vector3 recoilBack = new Vector3(originPos.x, originPos.y,currentWeapon.retroActionForce);
		Vector3 retroActionRecoil = new Vector3(currentWeapon.fineSightOriginPos.x, currentWeapon.fineSightOriginPos.y, currentWeapon.retroActionFineSightForce);
		if (!isFineSightMode)
		{
			fineSightPos.transform.localPosition = originPos;
			while (fineSightPos.transform.localPosition.z >= currentWeapon.retroActionForce + 0.02f) //반동
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
			fineSightPos.transform.localPosition = currentWeapon.fineSightOriginPos;
			while (fineSightPos.transform.localPosition.z >= currentWeapon.retroActionFineSightForce + 0.02f) //반동
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, retroActionRecoil, 0.4f);
				yield return null;
			}
			while (fineSightPos.transform.localPosition != originPos) //원위치
			{
				fineSightPos.transform.localPosition = Vector3.Lerp(fineSightPos.transform.localPosition, currentWeapon.fineSightOriginPos, 0.1f);
				yield return null;
			}
		}
	}
	IEnumerator CamAction2() //총 위아래 반동
	{
		Quaternion recoilBack = new Quaternion(currentWeapon.camUpActionForce, originRot.y, originRot.z,0f);
		Quaternion retroActionRecoil = new Quaternion(currentWeapon.camUpActionFineSightForce, originRot.y, originRot.z,0f);
		if (!isFineSightMode)
		{
			camPos.transform.localRotation = originRot;
			while (camPos.transform.localRotation.x >= currentWeapon.camUpActionForce) //반동
			{
				camPos.transform.localRotation = Quaternion.Slerp(camPos.transform.localRotation, recoilBack,Time.deltaTime * 10f);
				yield return null;
			}
			while (camPos.transform.localRotation != originRot) //원위치
			{
				camPos.transform.localRotation = Quaternion.Slerp(camPos.transform.localRotation, originRot, Time.deltaTime * 10f);
				yield return null;
			}
			camPos.transform.localRotation = originRot;
		}
		else
		{
			camPos.transform.localRotation = originRot;
			while (camPos.transform.localRotation.x >= currentWeapon.camUpActionForce) //반동
			{
				camPos.transform.localRotation = Quaternion.Slerp(camPos.transform.localRotation, retroActionRecoil, Time.deltaTime * 10f);
				//Debug.Log("asdasd");
				yield return null;
			}
			while (camPos.transform.localRotation != originRot) //원위치
			{
				camPos.transform.localRotation = Quaternion.Slerp(camPos.transform.localRotation, originRot, Time.deltaTime * 10f);
				yield return null;
			}
			camPos.transform.localRotation = originRot;
		}
	}
	private void CerateBullet() //총알 발사 생성
	{
		Instantiate(currentWeapon.bullet,currentWeapon.bulletPos.transform.position,transform.rotation);
		//GameObject t_object = ObjectPoolingManager.instance.GetQueue();
		//if (t_object == null)
			//Debug.Log("없음");
		//t_object.transform.position = bulletPos.transform.position;
		//t_object.transform.rotation = transform.rotation;
	}
	private void CamAction() //총 카메라 반동
	{
		if (!isFineSightMode)
			playerCtrl.CamUp();
		else
			playerCtrl.CamSightForce();
	}


//======================================근접구현=============================

	public void MeeleAttack() //플레이어 근접 공격
	{
		if (Input.GetMouseButtonDown(0))
		{
			TryMeeleAttack();
		}
	}

	private void TryMeeleAttack() //플레이어 근접 공격(애니메이션)
	{
		anim.SetTrigger("Shoot");
	}

	public void WeaponChange(MyWeapon _myWeapon) //플레이어 무기 변경
	{
		if (!isReload)
		{
			CancelReload();
			if (myWeaponManager.currentWeapon != null)
				myWeaponManager.currentWeapon.gameObject.SetActive(false);

			currentWeapon = _myWeapon;
			anim = currentWeapon.anim;
			myWeaponMoveAnim.anim = anim;
			myWeaponManager.currentWeapon = currentWeapon.GetComponent<Transform>();
			fineSightPos.transform.localPosition = currentWeapon.gunPos;
			originPos = currentWeapon.gunPos;
			currentWeapon.gameObject.SetActive(true);
		}
	}
	public void WeaponAway() //아이템 버리기
	{
		if (!isReload && currentWeapon != null)
		{
			CancelReload();
			myWeaponManager.currentWeapon.gameObject.SetActive(false);
			Instantiate(currentWeapon.GetComponent<MyWeapon>().weaponItemPrefab,playerItemSpawn.position,Quaternion.identity);
			currentWeapon = null;
		}
	}
}