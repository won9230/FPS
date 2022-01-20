using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { SMG, SniperRiffle, Pistol, Shotgun, Melee, Grenade, RocketLauncher }

[CreateAssetMenu(fileName = "MyWeapon", menuName = "MyWeapon Data", order = 0)]
public class MyWeapon : ScriptableObject
{
	[Header("세팅")]

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

	[Tooltip("반동 세기")]
	public float retroActionForce;
	[Tooltip("정조준시 반동")]
	public float retroActionFineSightForce;
	[Tooltip("에임 위치")]
	public Vector3 fineSightOriginPos;
	//[Tooltip("장전 딜레이")]
	//public float gunReload;

	//public Animator anim;
	public AudioClip fire_Sound;

}
