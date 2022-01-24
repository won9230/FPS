using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
	public new GameObject gameObject;

	private void Start()
	{
		gameObject.SetActive(false);
	}
	public void EnemyAttackTrue()
	{
		gameObject.SetActive(true);
	}
	public void EnemyAttackFalse()
	{
		gameObject.SetActive(false);
	}
}
