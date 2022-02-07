using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponManager : MonoBehaviour
{
	public static MyWeaponManager instance = null;
	public static bool isChangeWeapon = false; //중복실행방지

	[SerializeField] private float changeWeaponDelayTime; //총 바꾸는 시간
	[SerializeField] private float changeWeaponEndTime; //총꾸기 끝나는 시간
	
	[SerializeField] private MyWeapon[] myWeapons; //총오브젝트
	//private MyWeapon[] myWeapons; //총

	private Dictionary<string, MyWeapon> myWeaponTable = new Dictionary<string, MyWeapon>(); //총 정보

	[SerializeField] private string currentWeaponType;  // 현재 총 타입

	public MyWeaponCtrl myWeaponCtrl;
	public static Transform currentWeapon;//총
	public static Animator anim;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(this.gameObject);
	}
	private void Start()
	{
		for (int i = 0; i < myWeapons.Length; i++)
		{
			myWeaponTable.Add(myWeapons[i].weaponName,myWeapons[i]);
		}
		currentWeapon = myWeapons[0].gameObject.transform;
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
