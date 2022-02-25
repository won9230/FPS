using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviourPunCallbacks
{
	private string gameVersion = "0.02";

	[SerializeField] private GameObject main;
	[SerializeField] private GameObject lobby;
	public Text connectionInfoText; //네트위크 정보를 표시할 텍스트
	public Button joinButton; //룸 접속 버튼
	public InputField roomName, nickName;

	private void Start()
	{
		main.SetActive(true);
		lobby.SetActive(false);
		PhotonNetwork.GameVersion = gameVersion; //접속에 필요한 정보 설정
		PhotonNetwork.ConnectUsingSettings(); //설정한 정보를 가지고 마스터 서버 접속 시도
		
		joinButton.interactable = false;
		connectionInfoText.text = "마스터 서버에 접속중";

	}
	//마스터 서버 접속 성공시 자동 실행
	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		{
			joinButton.interactable = true;
			connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
		}
	}

	//마스터 서버 접속 실패시 자동 실행
	public override void OnDisconnected(DisconnectCause cause)
	{
		joinButton.interactable = false;
		connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도중 ....";
		PhotonNetwork.ConnectUsingSettings();
	}
	public void Connect()
	{
		joinButton.interactable = false;
		if (PhotonNetwork.IsConnected)
		{
			connectionInfoText.text = "룸에 접속...";
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도중 ....";
			PhotonNetwork.ConnectUsingSettings();
		}
	}
	public void JoinLobby()
	{
		main.SetActive(false);
		lobby.SetActive(true);
		PhotonNetwork.JoinLobby();
		connectionInfoText.text = "로비 접속 성공";
	}
	public override void OnJoinedRoom()
	{
		print("방참가 완료");
	}
	public override void OnJoinedLobby()
	{
		print("로비참가 완료");
	}
	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 2 });
		print(roomName.text);
		connectionInfoText.text = roomName.text + "방 만들기완료";
		PhotonNetwork.LocalPlayer.NickName = nickName.text;
		PhotonNetwork.LoadLevel("MainScenes");
	}
	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(roomName.text);
		if(PhotonNetwork.CurrentRoom != null)
			connectionInfoText.text = roomName.text + "방 접속완료";
		PhotonNetwork.LocalPlayer.NickName = nickName.text;
		PhotonNetwork.LoadLevel("MainScenes");
	}
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
		connectionInfoText.text = "빈 방이 없음, 새로운 방 생성";
	}
	public void BackLobby()
	{
		main.SetActive(true);
		lobby.SetActive(false);
		connectionInfoText.text = "로비 나감";
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
	}


	[ContextMenu("정보")]
	void Info()
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