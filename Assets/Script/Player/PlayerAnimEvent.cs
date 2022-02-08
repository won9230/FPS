using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
	[SerializeField] private GameObject game;
	public void PlayerMeleeAttackTrue()
	{
		game.SetActive(true);
	}
	public void PlayerMeleeAttackFalse()
	{
		game.SetActive(false);
	}
}
