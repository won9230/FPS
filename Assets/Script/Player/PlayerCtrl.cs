using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : LivingEntity
{

	[SerializeField] private float walkspeed; // 걷기 속도
	[SerializeField] private float runspeed; // 달리기 속도
	[SerializeField] private float crouchspeed; // 앉기 속도
	private float anyspeed;
	[SerializeField] private float lookSensitivity;//마우스 감도
	[SerializeField]private float jumpForce;
	[HideInInspector]public bool isRun = false; //달리기 감지
	private bool isGround = true; // 바닥감지
	[HideInInspector] public bool isCrouch = false; //앉기 감지
	//private bool isDie =false;

	[SerializeField] private float cameraRotationLimit; //걸림
	private float currentCameraRotationX = 0;
	private float currentCameraRotationY = 0;

	[SerializeField] private float crouchPosY;
	private float originPosY;
	private float applyCrouchPosY;
	[SerializeField] private Vector3 deadPos;

	[SerializeField] private new Camera camera;
	[SerializeField] private GameObject miniCamera;
	//[SerializeField] private GameObject trunCamera;
	//기타
	private new Rigidbody rigidbody;
	private CapsuleCollider capsuleCollider;
	private RaycastHit hit;
	//public static PlayerCtrl instance = null;
	[HideInInspector]public float x, y;
	private MyWeaponCtrl myWeapon;
	private MyInventory myInventory;
	private InGameUI inGameUI;
	[SerializeField] private PhotonView PV;
	[SerializeField] private Transform holder;
	protected override void Start()
	{
		capsuleCollider = GetComponent<CapsuleCollider>();
		rigidbody = GetComponent<Rigidbody>();
		myWeapon = GetComponentInChildren<MyWeaponCtrl>();
		PV = GetComponent<PhotonView>();
		myInventory = GetComponent<MyInventory>();
		inGameUI = GetComponentInChildren<InGameUI>();
		originPosY = camera.transform.localPosition.y;
		applyCrouchPosY = originPosY;
		anyspeed = walkspeed;
	}
	private void Update()
	{
		if (!PV.IsMine)
		{
			camera.gameObject.SetActive(false);
			miniCamera.SetActive(false);
		}
		if (!dead && !inGameUI.isChatMode && PV.IsMine)
		{
			IsGround();
			TryJump();
			TryCrouch();
			PlayerMove();
			CharacterRotation();
			CameraRotation();
			PlayerRaycast();
		}
		PlayerDie();
	}
	private void PlayerRaycast() //플레이어 레이캐스트
	{
		if(Physics.Raycast(camera.transform.position,camera.transform.forward,out hit, 1000))
		{
			if (hit.collider.CompareTag("Item"))
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					hit.collider.GetComponent<MyItemDrop>().Drop(myInventory);
				}
			}
			Debug.DrawRay(camera.transform.position, camera.transform.forward * hit.distance, Color.red);
		}
	}
	private void CharacterRotation() //좌우 회전
	{
		float yRotation = Input.GetAxisRaw("Mouse X");
		float rotation = yRotation * lookSensitivity;
		currentCameraRotationY += rotation;
		transform.localEulerAngles = new Vector3(0f, currentCameraRotationY, 0f);

		#region 임시
		//float _yRotation = Input.GetAxisRaw("Mouse X");
		//Vector3 charaterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
		//rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(charaterRotationY));
		#endregion
	}
	private void CameraRotation() //상하 회전
	{
		float rotationX = Input.GetAxisRaw("Mouse Y");
		float cameraRotationX = rotationX * lookSensitivity;
		currentCameraRotationX -= cameraRotationX;
		currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		//holder.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
	}

	private void PlayerMove() //걷기 달리기
	{
		x = Input.GetAxisRaw("Horizontal");
		y = Input.GetAxisRaw("Vertical");


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
	private void Running() //달리기
	{
		if (!isCrouch)
		{
			myWeapon.CancelFineSight();
			isRun = true;
			anyspeed = runspeed;
		}
	}
	private void RunningStop() //달리기 멈추기
	{
		if (!isCrouch)
		{
			isRun = false;
			anyspeed = walkspeed;
		}
	}
	private void TryJump() //점프
	{
		if(Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			PlayerJump();
		}
	}
	private void PlayerJump() //플레이어 점프
	{
		if (isCrouch)
		{
			return;
		}
			
		rigidbody.velocity = transform.up * jumpForce;
	}
	private void TryCrouch() //앉기
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

	IEnumerator CrouchCoroutine() //플레이어 코루틴
	{
		float _posY = camera.transform.localPosition.y;
		int count = 0;

		while(_posY != applyCrouchPosY)
		{
			count++;
			_posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.2f);
			camera.transform.localPosition = new Vector3(0, _posY, 0);
			if (count > 30)
				break;
			yield return null;
		}
		camera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
	}
	private void IsGround() //바닥 체크
	{
		isGround = Physics.Raycast(transform.position,Vector3.down,capsuleCollider.bounds.extents.y+0.1f);
	}
	private void PlayerDie() //죽는거
	{
		if (hp <= 0)
			dead = true;

		if (dead)
		{
			StopAllCoroutines();
			StartCoroutine(DeadCoroutine());
		}
	}
	IEnumerator DeadCoroutine() //플레이어 죽음
	{
		Debug.Log("시작");
		Quaternion rot = new Quaternion(0, camera.transform.localRotation.y, camera.transform.localRotation.z, 0);
		while (camera.transform.localPosition != deadPos)
		{
			camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, deadPos, 0.2f);
			yield return null;
		}
		camera.enabled = false;
	}
	public void CamUp() //총 반동의한 에임 흔들림
	{
		currentCameraRotationX -= Random.Range(0f, myWeapon.currentWeapon.camActionForce);
		currentCameraRotationY -= Random.Range(-myWeapon.currentWeapon.camActionForce-0.1f, myWeapon.currentWeapon.camActionForce-0.1f);
	}
	public void CamSightForce() //총 반동의한 에임 흔들림(조준시)
	{
		currentCameraRotationX -= Random.Range(0f, myWeapon.currentWeapon.camActionFineSightForce);
		currentCameraRotationY -= Random.Range(-myWeapon.currentWeapon.camActionFineSightForce - 0.1f, myWeapon.currentWeapon.camActionFineSightForce - 0.1f);
	}
}