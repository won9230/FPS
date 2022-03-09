using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponSway : MonoBehaviour
{
	private Vector3 originPos; //원래 위치

	private Vector3 currentPos; //현재 위치

	[SerializeField] private Vector3 limitPos;  //sway한계

	[SerializeField] private Vector3 fineSightLimitPos; //정조준 sway

	[SerializeField] private Vector3 smoothSway; //부드러운 정도

	[SerializeField] private MyWeaponCtrl myWeaponCtrl;

	[SerializeField] private GameObject game;

	[SerializeField] private InGameUI inGameUI;

	private void Start()
	{
		originPos = game.transform.localPosition;
	}
	private void Update()
	{
		if (!inGameUI.isChatMode)
		{
			TrySway();
		}
	}
	private void TrySway()
	{
		if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
		{
			Swaying();
		}
		else
		{
			BackToOriginPos();
		}
	}

	private void BackToOriginPos()
	{
		currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
		game.transform.localPosition = currentPos;
	}

	private void Swaying()
	{
		float _moveX = Input.GetAxisRaw("Mouse X");
		float _moveY = Input.GetAxisRaw("Mouse Y");
		if (!myWeaponCtrl.isFineSightMode)
		{
			currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y), originPos.z);
		}
		else
		{
			currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x), Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y), originPos.z);
		}

		game.transform.localPosition = currentPos;
	}
}
