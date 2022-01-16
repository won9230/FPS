using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
	private Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	public void OnMovement(float h, float v) //움직임 애니
	{
		anim.SetFloat("horizontal", h);
		anim.SetFloat("vertical", v);
	}
}
