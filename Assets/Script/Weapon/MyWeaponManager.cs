using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponManager : MonoBehaviour
{
	public static bool isChangeWeapon = false; //중복실행방지

	[SerializeField] private float changeWeaponDelayTime; //총 바꾸는 시간
	[SerializeField] private float changeWeaponEndTime; //총꾸기 끝나는 시간

	[SerializeField] private MyWeaponCtrl[] myWeapons; //총
	[SerializeField] private GameObject[] myWeaponsPrefab; //총오브젝트

	private Dictionary<string, GameObject> myWeaponTable = new Dictionary<string, GameObject>(); //총 정보


	[SerializeField]
	private string currentWeaponType;  // 현재 총 타입

	public static MyWeaponCtrl myWeaponCtrl;
	public static Transform currentWeapon;//총
	public static Animator anim;

	private void Start()
	{
		for (int i = 0; i < myWeapons.Length; i++)
		{
			myWeaponTable.Add(myWeapons[i].currentWeapon.weaponName, myWeaponsPrefab[i]);
		}
		myWeaponCtrl = myWeapons[0];
	}
	private void Update()
	{
		if (!isChangeWeapon)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				StartCoroutine(ChangeWeapon("GUN", "AK47"));
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				StartCoroutine(ChangeWeapon("GUN", "Glock"));
			}
		}
	}
	public IEnumerator ChangeWeapon(string _type,string _name)
	{
		isChangeWeapon = true;
		yield return new WaitForSeconds(0.1f);
		CancelWeaponAciton();
		WeaponChange(_type,_name);
		yield return new WaitForSeconds(0.1f);
		currentWeaponType = _type;
		isChangeWeapon = false;
	}
	private void CancelWeaponAciton()
	{
		switch (currentWeaponType)
		{
			case "GUN":
				myWeaponCtrl.CancelFineSight();
				break;
		}
	}
	private void WeaponChange(string _type, string _name)
	{
		if(_type == "GUN")
		{
			myWeaponCtrl.WeaponChange(myWeaponTable[_name]);
		}
	}
}
