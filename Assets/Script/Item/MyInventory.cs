using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInventory : MonoBehaviour
{
	public List<MyItem> items;

	[SerializeField]
	private Transform slotParent;

	public MyMainSlot[] mainSlots;
	public MySubSlot subSlot;
	public MyMeeleSlot meeleSlot;

	private void OnValidate()
	{
		mainSlots = slotParent.GetComponentsInChildren<MyMainSlot>();
		subSlot = slotParent.GetComponentInChildren<MySubSlot>();
		meeleSlot = slotParent.GetComponentInChildren<MyMeeleSlot>();
	}
	private void Awake()
	{
		FreshSlot(null);
	}

	public void FreshSlot(MyWeapon _item)
	{
		if (_item == null)
			return;
		int i = items.Count - 1;
		//if (_item.weaponType == WeaponType.GUN)
		//{
		//	for (; i < items.Count && i < mainSlots.Length; i++)
		//	{
		//		mainSlots[i].item = items[i];
		//		Debug.Log("main" + mainSlots[i].item.itemName + " " + _item.weaponType);
		//	}
		//	for (; i < mainSlots.Length; i++)
		//	{
		//		mainSlots[i].item = null;
		//	}
		//}
		//if (_item.weaponType == WeaponType.Sub)
		//{
		//	for (; i < items.Count; i++)
		//	{
		//		subSlot.item = items[i];
		//		Debug.Log("sub"+subSlot.item.itemName+" "+_item.weaponType);
		//	}
		//}
		//if (_item.weaponType == WeaponType.Melee)
		//{
		//	for (; i < items.Count; i++)
		//	{
		//		meeleSlot.item = items[i];
		//		Debug.Log("meele" + meeleSlot.item.itemName + " " + _item.weaponType);
		//	}
		//}
		///==========================================
		for (; i < items.Count; i++)
		{
			for (int j = 0; j < mainSlots.Length; j++)
			{
				if (mainSlots[j].item == null && items[i].weapon.weaponType == WeaponType.GUN)
				{
					mainSlots[j].item = items[i];
					Debug.Log("main" + mainSlots[j].item.itemName + " " + _item.weaponType);
				}
			}
			if(items[i].weapon.weaponType == WeaponType.Sub && subSlot.item == null)
			{
				subSlot.item = items[i];
				Debug.Log("sub" + subSlot.item.itemName + " " + _item.weaponType);
			}
			if(items[i].weapon.weaponType == WeaponType.Melee && meeleSlot.item == null)
			{
				meeleSlot.item = items[i];
				Debug.Log("meele" + meeleSlot.item.itemName + " " + _item.weaponType);
			}
		}
		for (; i < mainSlots.Length; i++)
		{
			mainSlots[i].item = null;
		}

	}
	public void AddItem(MyItem _item)
	{
		if(items.Count < 4)
		{
			items.Add(_item);
			FreshSlot(_item.weapon);
		}
		else
		{
			print("아이템을 더 먹지 못함");
		}
	}
}
