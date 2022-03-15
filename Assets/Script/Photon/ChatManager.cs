using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ChatManager : MonoBehaviourPunCallbacks
{
	public Text[] chatText;
	public InputField chatInput;
	public PhotonView PV;
	public GameObject chatPanel;

	private void Start()
	{
		chatPanel.SetActive(false);
	}
	private void Update()
	{
		if (PV.IsMine)
		{
			if (Input.GetKeyDown(KeyCode.Return) && chatInput.text != "")
			{
				Send();
			}
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
	}
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
	}
	public void Send()
	{
		string msg = PhotonNetwork.NickName + " : " + chatInput.text;
		PV.RPC("ChatRPC",RpcTarget.All,PhotonNetwork.NickName + " : " + chatInput.text);
		chatInput.text = "";
	}

	[PunRPC]
	private void ChatRPC(string msg)
	{
		bool isInput = false;
		for (int i = 0; i < chatText.Length; i++)
		{
			if (chatText[i].text == "")
			{
				isInput = true;
				chatText[i].text = msg;
				break;
			}
		}
		if (!isInput)
		{
			for (int i = 0; i < chatText.Length; i++)
			{
				chatText[i - 1].text = chatText[i].text;
				chatText[chatText.Length - 1].text = msg;
			}
		}
	}
}
