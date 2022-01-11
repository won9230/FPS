using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

	[SerializeField] private float walkspeed; // 걷기 속도
	[SerializeField] private float runspeed; // 달리기 속도
	[SerializeField] private float lookSensitivity;//마우스 감도

	[SerializeField] private float cameraRotationLimit; //걸림
	private float currentCameraRotationX = 0;

	[SerializeField] private new Camera camera;
	//기타
	private new Rigidbody rigidbody;
	private PlayerAnim anim;
	public static PlayerCtrl instance = null;
	#region Singleton
	private void Awake() //싱글톤
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(this.gameObject);
	}
	#endregion
	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<PlayerAnim>();
	}
	private void Update()
	{
		PlayerMove();
		CharacterRotation();
	}
	private void CharacterRotation()
	{
		float rotationY = Input.GetAxisRaw("Mouse X");
		Vector3 characterRotationY = new Vector3(0f, rotationY, 0f) * lookSensitivity;
		rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(characterRotationY));
		CameraRotation();
	}
	private void CameraRotation()
	{
		float rotationX = Input.GetAxisRaw("Mouse Y");
		float cameraRotationX = rotationX * lookSensitivity;
		currentCameraRotationX -= cameraRotationX;
		currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f,0f);
	}

	private void PlayerMove()
	{
		float anyspeed,x,y;
		x = Input.GetAxisRaw("Horizontal");
		y = Input.GetAxisRaw("Vertical");
		anyspeed = walkspeed;

		Vector3 moveX = transform.right * x;
		Vector3 moveY = transform.forward * y;
		if (Input.GetKey(KeyCode.LeftShift))
			anyspeed = runspeed;
		else if (Input.GetKeyUp(KeyCode.LeftShift))
			anyspeed = walkspeed;

		anim.OnMovement(x, y);

		Vector3 velocity = (moveX + moveY).normalized * anyspeed;
		rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
	}
}
