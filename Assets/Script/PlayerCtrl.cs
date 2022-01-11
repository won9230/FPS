using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	[SerializeField]
	private float walkspeed;
	
	[SerializeField]
	private float lookSensitivity;
	[SerializeField]
	private float cameraRotationLimit;
	private float currentCameraRotationX = 0;

	[SerializeField]
	private Camera camera;

	private Rigidbody rigidbody;
	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	private void Update()
	{
		PlayerMove();
		CameraRotation();
		CharacterRotation();
	}
	private void CharacterRotation()
	{
		float rotationY = Input.GetAxisRaw("Mouse X");
		Vector3 characterRotationY = new Vector3(0f, rotationY, 0f) * lookSensitivity;
		rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(characterRotationY));
	}
	private void CameraRotation()
	{
		float rotationX = Input.GetAxisRaw("Mouse Y");
		float cameraRotationX = rotationX * lookSensitivity;
		currentCameraRotationX -= cameraRotationX;
		//currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f,0f);
	}

	private void PlayerMove()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		Vector3 moveX = transform.right * x;
		Vector3 moveY = transform.forward * y;

		Vector3 velocity = (moveX + moveY).normalized * walkspeed;

		rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
	}
}
