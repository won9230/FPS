using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RoomManager : MonoBehaviourPunCallbacks
{
	public Transform spawnPoint;
	private void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}
	public override void OnJoinedRoom()
	{
		StartCoroutine(WaitForStart());
	}
	IEnumerator WaitForStart()
	{	
		yield return new WaitForSeconds(0.5f);
		PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
	}

	[ContextMenu("정보")]
	void Info()//디버그 용
	{
		if (PhotonNetwork.InRoom)
		{
			print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
			print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
			print("현재 방 최대 인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);
			string playerStr = "방에 있는 플레이어 목록 : ";
			for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
			{
				playerStr += PhotonNetwork.PlayerList[i].NickName + ",";
			}
			print(playerStr);
		}
		else
		{
			print("접속한 인원 수 :" + PhotonNetwork.CountOfPlayers);
			print("방 개수 :" + PhotonNetwork.CountOfRooms);
			print("모든 방에 있는 인원 수 :" + PhotonNetwork.CountOfPlayersInRooms);
			print("로비에 있는지? :" + PhotonNetwork.InLobby);
			print("연결됬는지? :" + PhotonNetwork.IsConnected);
		}
	}
}
