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
	public void BossAttackTrue()
	{
		gameObject.SetActive(true);
	}
	public void BossAttackFalse()
	{
		gameObject.SetActive(false);
	}
}
