using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponManager : MonoBehaviour
{
	public bool isChangeWeapon = false; //중복실행방지

	[SerializeField] private float changeWeaponDelayTime; //총 바꾸는 시간
	[SerializeField] private float changeWeaponEndTime; //총꾸기 끝나는 시간
	
	[SerializeField] private MyWeapon[] myWeapons; //총오브젝트
	//private MyWeapon[] myWeapons; //총

	private Dictionary<string, MyWeapon> myWeaponTable = new Dictionary<string, MyWeapon>(); //총 정보

	[SerializeField] private string currentWeaponType;  // 현재 총 타입

	private MyInventory myInventory;
	public MyWeaponCtrl myWeaponCtrl;
	public Transform currentWeapon;//총
	public static Animator anim;
	public int currentInt;

	//#region 싱글톤
	//private void Awake()
	//{
	//	if (instance == null)
	//		instance = this;
	//	else if (instance != this)
	//		Destroy(this.gameObject);
	//}
	//#endregion

	private void Start()
	{
		myInventory = GetComponent<MyInventory>();
		for (int i = 0; i < myWeapons.Length; i++)
		{
			myWeaponTable.Add(myWeapons[i].weaponName,myWeapons[i]);
		}
		//currentWeapon = myWeapons[2].gameObject.transform;
	}
	private void Update()
	{
		if (!isChangeWeapon)
		{
			WeaponAway();
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				StartCoroutine(ChangeWeapon(myInventory.mainSlots[0].weaponType, myInventory.mainSlots[0].weaponName));
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				StartCoroutine(ChangeWeapon(myInventory.mainSlots[1].weaponType, myInventory.mainSlots[1].weaponName));
			}	
			else if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				StartCoroutine(ChangeWeapon(myInventory.subSlot.weaponType,myInventory.subSlot.weaponName));
			}	
			else if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				StartCoroutine(ChangeWeapon(myInventory.meeleSlot.weaponType, myInventory.meeleSlot.weaponName));
			}
		}
	}
	public IEnumerator ChangeWeapon(string _type, string _name) //무기 바꾸기 코루틴
	{
		if (_type != "" && _name != "")
		{
			CancelWeaponAciton();
			WeaponChange(_type, _name);
			isChangeWeapon = true;
			currentWeaponType = _type;
			yield return new WaitForSeconds(1.5f);
			isChangeWeapon = false;
		}
		yield return null;
	}
	private void WeaponChange(string _type, string _name) //무기 바꾸기
	{
		if (_type == "GUN" || _type == "Melee")
		{
			if (!isChangeWeapon)
				myWeaponCtrl.WeaponChange(myWeaponTable[_name]);
		}
		if (_type == "Sub" || _type == "Melee")
		{
			if (!isChangeWeapon)
				myWeaponCtrl.WeaponChange(myWeaponTable[_name]);
		}
	}
	private void CancelWeaponAciton() //무기 줌인 캔슬
	{
		switch (currentWeaponType)
		{
			case "GUN":
				myWeaponCtrl.CancelFineSight();
				break;		
			case "Sub":
				myWeaponCtrl.CancelFineSight();
				break;
		}
	}

	private void WeaponAway()//무기 버리기
	{
		if (Input.GetKeyDown(KeyCode.G) && currentWeapon != null)
		{
			//MyWeaponManager.currentWeapon == null;
			myWeaponCtrl.WeaponAway();
			myInventory.DestroyItem();
		}
	}
}
