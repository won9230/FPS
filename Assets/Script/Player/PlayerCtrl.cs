using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

	[SerializeField] private float walkspeed; // 걷기 속도
	[SerializeField] private float runspeed; // 달리기 속도
	[SerializeField] private float crouchspeed; // 앉기 속ㄷ도
	private float anyspeed;
	[SerializeField] private float lookSensitivity;//마우스 감도
	[SerializeField]private float jumpForce;
	private bool isRun = false; //달리기 감지
	private bool isGround = true; // 바닥감지
	private bool isCrouch = false; //앉기 감지

	[SerializeField] private float cameraRotationLimit; //걸림
	private float currentCameraRotationX = 0;
	private float currentCameraRotationY = 0;

	[SerializeField] private float crouchPosY;
	private float originPosY;
	private float applyCrouchPosY;

	[SerializeField] private new Camera camera;
	//기타
	private new Rigidbody rigidbody;
	private PlayerAnim anim;
	private CapsuleCollider capsuleCollider;
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
		capsuleCollider = GetComponent<CapsuleCollider>();
		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<PlayerAnim>();
		originPosY = camera.transform.localPosition.y;
		applyCrouchPosY = originPosY;
	}
	private void Update()
	{
		IsGround();
		TryJump();
		TryCrouch();
		PlayerMove();
		CharacterRotation();
		CameraRotation();
	}



	private void CharacterRotation() //좌우 회전
	{
		float yRotation = Input.GetAxisRaw("Mouse X");
		float rotation = yRotation * lookSensitivity;
		currentCameraRotationY += rotation;
		transform.localEulerAngles = new Vector3(0f, currentCameraRotationY, 0f);
	}
	private void CameraRotation() //상하 회전
	{
		float rotationX = Input.GetAxisRaw("Mouse Y");
		float cameraRotationX = rotationX * lookSensitivity;
		currentCameraRotationX -= cameraRotationX;
		currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
	}

	private void PlayerMove() //걷기 달리기
	{
		float x, y;
		x = Input.GetAxisRaw("Horizontal");
		y = Input.GetAxisRaw("Vertical");
		anyspeed = walkspeed;

		Vector3 moveX = transform.right * x;
		Vector3 moveY = transform.forward * y;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			Running();
		}
		else
		{
			RunningStop();
		}

		Vector3 velocity = (moveX + moveY).normalized * anyspeed;
		rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
	}
	private void Running()
	{
		isRun = true;
		anyspeed = runspeed;
	}
	private void RunningStop()
	{
		isRun = false;
		anyspeed = walkspeed;
	}
	private void TryJump() //점프
	{
		if(Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			PlayerJump();
		}
	}
	private void PlayerJump()
	{
		if (isCrouch)
		{
			return;
		}
			
		rigidbody.velocity = transform.up * jumpForce;
	}
	private void TryCrouch()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			isCrouch = true;
			anyspeed = crouchspeed;
			applyCrouchPosY = crouchPosY;
		}
		else
		{
			isCrouch = false;
			anyspeed = walkspeed;
			applyCrouchPosY = originPosY;
		}
		StartCoroutine(CrouchCoroutine());
	}

	IEnumerator CrouchCoroutine()
	{
		float _posY = camera.transform.localPosition.y;
		int count = 0;

		while(_posY != applyCrouchPosY)
		{
			count++;
			_posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
			camera.transform.localPosition = new Vector3(0, _posY, 0);
			if (count > 15)
				break;
			yield return null;
		}
		camera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
	}
	private void IsGround()
	{
		isGround = Physics.Raycast(transform.position,Vector3.down,capsuleCollider.bounds.extents.y+0.1f);
	}
}