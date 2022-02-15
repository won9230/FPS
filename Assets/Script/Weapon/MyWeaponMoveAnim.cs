using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponMoveAnim : MonoBehaviour
{
	private MyWeaponCtrl myWeaponCtrl;
	public Animator anim;
	float x = 0;
	private void Start()
	{
		myWeaponCtrl = GetComponent<MyWeaponCtrl>();
		//anim = myWeaponCtrl.currentWeapon.GetComponent<Animator>();
	}
	private void Update()
	{
		if (myWeaponCtrl.currentWeapon != null)
		{
			AnimRun();
		}
	}
	private void AnimMove()
	{
		if(Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Vertical") > 0)
		{
			if(x < 1f)
			{
				x += Time.deltaTime*5;
			}
		}
		else
		{
			if(x > 0)
				x -= Time.deltaTime*5;
		}
	}
	private void AnimRun()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			if(x < 2f)
			{
				x += Time.deltaTime * 5;
			}
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			StartCoroutine(RunDown());
		}
		else if(Input.GetKey(KeyCode.LeftControl))
		{
			if(x > -1f)
			{
				x -= Time.deltaTime * 10;
			}
		}
		else if(Input.GetKeyUp(KeyCode.LeftControl))
		{
			StartCoroutine(CruchUp());
		}
		else
		{
			AnimMove();
		}
		anim.SetFloat("Move", x);
	}
	IEnumerator RunDown()
	{
		while (x > 1.1f)
		{
			x -= Time.deltaTime*5;
			yield return null;
		}
	}
	IEnumerator CruchUp()
	{
		while(x < -0.1f)
		{
			x += Time.deltaTime * 5;
			yield return null;
		}
	}
}
//Input.GetAxisRaw("Vertical")        Input.GetAxisRaw("Horizontal")