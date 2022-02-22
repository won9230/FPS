using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { GUN,Sub,Melee}
public class MyWeapon : MonoBehaviour
{
	[Header("세팅")]

	[Tooltip("무기 타입")]
	public WeaponType weaponType;
	[Tooltip("무기 이름")]
	public string weaponName;
	[Tooltip("공격력")]
	public int damage;
	[Tooltip("공격 딜레이")]
	public float attackDelay; 
	[Tooltip("정확도")]
	public float accuracy;
	[Tooltip("연사 속도")]
	public float fireRate;
	[Tooltip("재장전 속도")]
	public float reloadTime;

	[Tooltip("재장전하는 장탄 수")]
	public int reloadBulletCount;
	[Tooltip("남아 있는 장탄 수")]
	public int curentBulletCount;
	[Tooltip("최대 소유 가능한 장탄 수")]
	public int maxBulletCount;	
	[Tooltip("현재 소유하고 있는 장탄 수")]
	public int carryBulletCount;

	[Tooltip("총 앞뒤 반동 세기")]
	public float retroActionForce;
	[Tooltip("총 앞뒤 정조준시 반동")]
	public float retroActionFineSightForce;
	[Tooltip("카메라 위아래 정조준시 반동")]
	public float camActionForce;
	[Tooltip("카메라 위아래 정조준시 반동")]
	public float camActionFineSightForce;
	[Tooltip("카메라 위아래 정조준시 반동")]
	public float camUpActionForce;
	[Tooltip("카메라 위아래 정조준시 반동")]
	public float camUpActionFineSightForce;
	[Tooltip("에임 위치")]
	public Vector3 fineSightOriginPos;
	[Tooltip("총 위치")]
	public Vector3 gunPos;
	[Tooltip("총알 프리펩")]
	public GameObject bullet;
	[Tooltip("총알 발사 위치")]
	public Transform bulletPos;
	[Tooltip("무기 아이템화 프리펩")]
	public GameObject weaponItemPrefab;
	[Tooltip("무기 애니메이션")]
	public Animator anim;
	[Tooltip("무기 공격 사운드")]
	public AudioClip fire_Sound;
}
