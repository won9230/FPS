using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInventory : MonoBehaviour
{
	public List<MyItem> items;

	[SerializeField]
	private Transform slotParent;
	private MyWeaponManager myWeaponManager;

	public MyMainSlot[] mainSlots;
	public MySubSlot subSlot;
	public MyMeeleSlot meeleSlot;

	private void Start()
	{
		mainSlots = slotParent.GetComponentsInChildren<MyMainSlot>();
		subSlot = slotParent.GetComponentInChildren<MySubSlot>();
		meeleSlot = slotParent.GetComponentInChildren<MyMeeleSlot>();
		myWeaponManager = GetComponent<MyWeaponManager>();
	}

	public void FreshSlot() //슬롯 구분 및 정렬
	{
		int i = items.Count - 1;
		int j = 0;
		for (; i < items.Count; i++)
		{
			for (; j < mainSlots.Length; j++)
			{
				if (mainSlots[j].item == null && items[i].weapon.weaponType == WeaponType.GUN)
				{
					mainSlots[j].item = items[i];
					//Debug.Log("main" + mainSlots[j].item.itemName + " " + _item.weaponType);
					return;
				}
			}
			for (; j < mainSlots.Length; j++)
			{
				mainSlots[j].item = null;
			}
			if (items[i].weapon.weaponType == WeaponType.Sub && subSlot.item == null)
			{
				subSlot.item = items[i];
				//Debug.Log("sub" + subSlot.item.itemName + " " + _item.weaponType);
			}
			if(items[i].weapon.weaponType == WeaponType.Melee && meeleSlot.item == null)
			{
				meeleSlot.item = items[i];
				//Debug.Log("meele" + meeleSlot.item.itemName + " " + _item.weaponType);
			}
		}

	}

	public void AddItem(MyItem _item) //아이템 추가
	{
		if(items.Count < 4)
		{
			items.Add(_item);
			FreshSlot();
		}
		else
		{
			print("아이템을 더 먹지 못함");
		}
	}
	public void DestroyItem() //아이템 삭제체크
	{
		if (items.Count > 0)
		{
			DestroySlotItem();	
		}
		else
		{
			return;
		}
	}
	private void DestroySlotItem() //아이템 슬롯 체크 및 삭제
	{
		if (myWeaponManager.currentWeapon == null)
			return;
		string weaponName = myWeaponManager.currentWeapon.gameObject.GetComponent<MyWeapon>().weaponName;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].weapon.weaponName == weaponName)
			{
				if(items[i].weapon.weaponType == WeaponType.GUN)
				{
					for (int j = 0; j < mainSlots.Length; j++)
					{
						if (mainSlots[j].item.itemName == weaponName)
						{
							mainSlots[j].item = null;
							items.RemoveAt(i);
							return;
						}
					}
					return;
				}	
				if(items[i].weapon.weaponType == WeaponType.Sub)
				{
					subSlot.item = null;
					items.RemoveAt(i);
					return;
				}
				if(items[i].weapon.weaponType == WeaponType.Melee)
				{
					meeleSlot.item = null;
					items.RemoveAt(i);
					return;
				}

			}
		}
		myWeaponManager.currentWeapon = null;
	}
}
