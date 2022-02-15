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
		int i = 0;
		if (_item.weaponType == WeaponType.GUN)
		{
			for (; i < items.Count && i < mainSlots.Length; i++)
			{
				mainSlots[i].item = items[i];
			}
			for (; i < mainSlots.Length; i++)
			{
				mainSlots[i].item = null;
			}
		}
		if (_item.weaponType == WeaponType.Sub)
		{
			subSlot.item = items[i];
		}
		if (_item.weaponType == WeaponType.Melee)
		{
			meeleSlot.item = items[i];
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
