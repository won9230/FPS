using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
	public GameObject roomPanel;
	public PlayerCtrl playerCtrl;
	public Slider playerHp;
	public Text[] texts;
	public static bool isChatMode = false;
	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		roomPanel.SetActive(false);
		playerHp.maxValue = playerCtrl.maxHp;
	}
	private void Update()
	{
		PlayerHpBar();
		PlayerBullet();
		ChatMode();
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
	private void ChatMode()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			isChatMode = !isChatMode;
		}
		if (isChatMode)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			roomPanel.SetActive(true);
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			roomPanel.SetActive(false);
		}
	}
}