using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEevnt : MonoBehaviour
{
	public new GameObject gameObject;

	private void Start()
	{
		gameObject.SetActive(false);
	}
	public void BossAttackTrue() //공격 콜라이더 켜짐(애니메이션 이벤트)
	{
		gameObject.SetActive(true);
	}
	public void BossAttackFalse() //공격 콜라이더 켜짐(애니메이션 이벤트)
	{
		gameObject.SetActive(false);
	}
}
