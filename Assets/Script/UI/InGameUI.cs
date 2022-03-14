using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InGameUI : MonoBehaviour
{
	private ChatManager chatManager;
	public PlayerCtrl playerCtrl;
	public Slider playerHp;
	public Text[] texts;
	private MyWeaponManager weaponManager;
	private PhotonView PV;
	public bool isChatMode = false;
	private void Start()
	{
		PV = GetComponentInParent<PhotonView>();
		if (!PV.IsMine)
		{
			this.gameObject.SetActive(false);
			return;
		}
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		weaponManager = GetComponentInParent<MyWeaponManager>();
		chatManager = FindObjectOfType<ChatManager>();
		//playerHp.maxValue = playerCtrl.maxHp;
	}
	private void Update()
	{
		if (playerCtrl != null && PV.IsMine)
		{
			PlayerHpBar();
			PlayerBullet();
			if (Input.GetKeyDown(KeyCode.T))
			{
				ChatMode();
			}
		}
	}
	private void PlayerHpBar() //플레이어 HP
	{
		playerHp.value = playerCtrl.hp;
	}
	private void PlayerBullet() //플레이어 총알 갯수
	{
		if (weaponManager.myWeaponCtrl.currentWeapon != null)
		{
			texts[0].text = weaponManager.myWeaponCtrl.currentWeapon.carryBulletCount.ToString();
			texts[1].text = weaponManager.myWeaponCtrl.currentWeapon.curentBulletCount.ToString();
		}
	}
	private void ChatMode() //플레이어 채팅 모드
	{
		isChatMode = !isChatMode;
		if (isChatMode)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			chatManager.chatPanel.SetActive(true);
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			chatManager.chatPanel.SetActive(false);
		}
	}
}