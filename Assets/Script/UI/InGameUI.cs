using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
	public MyWeapon myWeapon;
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
		texts[0].text = myWeapon.carryBulletCount.ToString();
		texts[1].text = myWeapon.curentBulletCount.ToString();
	}
}
