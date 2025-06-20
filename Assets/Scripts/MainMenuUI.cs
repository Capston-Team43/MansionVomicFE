using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviourPunCallbacks
{
    public Button startButton;

    public void OnClickStartGame()
    {
        startButton.interactable = false; // 중복 클릭 방지

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // 서버 연결 시작
        }
        else
        {
            PhotonNetwork.JoinRandomRoom(); // 이미 연결돼 있으면 방 입장 시도
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방이 없으면 새 방 생성
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("WaitingRoom");
    }

    // 연결 실패 시 호출되는 콜백
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($"Photon 연결 끊김: {cause}");
        startButton.interactable = true; // 버튼 다시 활성화
    }

    // 방 생성 실패 (예: 중복된 방 이름 등)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"방 생성 실패: {message}");
        startButton.interactable = true; // 다시 클릭 가능
    }

    public void OnClickOptions()
    {
        Debug.Log("설정 버튼 눌림 (아직 미구현)");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
