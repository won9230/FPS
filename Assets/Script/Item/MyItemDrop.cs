using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItemDrop : MonoBehaviour
{
	public MyItem item;
	private MyInventory myInventory;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			myInventory = FindObjectOfType<MyInventory>();
			myInventory.AddItem(item);
			Destroy(gameObject);
		}
	}
}
