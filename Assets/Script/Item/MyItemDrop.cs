using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemDrop : MonoBehaviour
{
	public MyItem item;
	private MyInventory myInventory;

	public void Drop()
	{
		myInventory = FindObjectOfType<MyInventory>();
		for (int i = 0; i < myInventory.mainSlots.Length; i++)
		{
			if (myInventory.mainSlots[i].item == null)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
			else if (myInventory.subSlot.item == null)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
			else if (myInventory.meeleSlot.item == null)
			{
				myInventory.AddItem(item);
				Destroy(gameObject);
				return;
			}
		}
		
	}
}
