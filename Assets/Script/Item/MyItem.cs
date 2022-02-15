using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MyItem", menuName = "New MyItem/Myitem")]
public class MyItem : ScriptableObject
{
	public string itemName;
	public Sprite itemImapge;
	public MyWeapon weapon;
	//public GameObject itemPrefab;
	//public GameObject itemObject;

	//public string weponType;
}
