using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemDrop : MonoBehaviour
{
	public MyItem item;
//	private MyInventory myInventory;

	public void Drop(MyInventory myInventory) //플레이어 아이템 드롭
	{
		for (int i = 0; i < myInventory.mainSlots.Length; i++)
		{
			if (myInventory.mainSlots[i].item == null && item.weapon.weaponType == WeaponType.GUN)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
			else if (myInventory.subSlot.item == null && item.weapon.weaponType == WeaponType.Sub)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
			else if (myInventory.meeleSlot.item == null && item.weapon.weaponType == WeaponType.Melee)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
		}
	}
}
