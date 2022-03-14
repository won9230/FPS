using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    private PlayerCtrl player;
	[SerializeField] float offsetY;
	private void Start()
	{
		player = GetComponentInParent<PlayerCtrl>();
	}

	private void LateUpdate()
	{
		Vector3 offset = new Vector3(player.transform.position.x, offsetY, player.transform.position.z);
		transform.position = Vector3.Slerp(transform.position,offset,Time.deltaTime * 3f);
	}
}
