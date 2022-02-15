using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyMeeleSlot : MonoBehaviour
{
	[SerializeField] Image image;
	[HideInInspector] public string weaponName;
	[HideInInspector] public string weaponType;

	private MyItem _item;
	public MyItem item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
			if (_item != null)
			{
				image.sprite = item.itemImapge;
				image.color = new Color(1, 1, 1, 1);
				weaponName = item.weapon.weaponName;
				weaponType = item.weapon.weaponType.ToString();
			}
			else
			{
				image.color = new Color(1, 1, 1, 0);
			}
		}
	}
}
