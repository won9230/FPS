using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
	public MyWeaponManager myWeaponManager;
	public Slider playerHp;
	public Text[] texts;
	private void Start()
	{
		playerHp.maxValue = playerCtrl.maxHp;
	}
	private void Update()
	{
		PlayerHpBar();
		PlayerBullet();
	}
	private void PlayerHpBar()
	{
		playerHp.value = playerCtrl.hp;
	}
	private void PlayerBullet()
	{
		if (MyWeaponManager.instance.myWeaponCtrl.currentWeapon != null)
		{
			texts[0].text = MyWeaponManager.instance.myWeaponCtrl.currentWeapon.carryBulletCount.ToString();
			texts[1].text = MyWeaponManager.instance.myWeaponCtrl.currentWeapon.curentBulletCount.ToString();
		}
	}
}
