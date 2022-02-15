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
		myInventory.AddItem(item);
		Destroy(gameObject);
	}
}
