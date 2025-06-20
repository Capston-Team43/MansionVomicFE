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
        startButton.interactable = false; // �ߺ� Ŭ�� ����

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� ���� ����
        }
        else
        {
            PhotonNetwork.JoinRandomRoom(); // �̹� ����� ������ �� ���� �õ�
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("WaitingRoom");
    }

    // ���� ���� �� ȣ��Ǵ� �ݹ�
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($"Photon ���� ����: {cause}");
        startButton.interactable = true; // ��ư �ٽ� Ȱ��ȭ
    }

    // �� ���� ���� (��: �ߺ��� �� �̸� ��)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"�� ���� ����: {message}");
        startButton.interactable = true; // �ٽ� Ŭ�� ����
    }

    public void OnClickOptions()
    {
        Debug.Log("���� ��ư ���� (���� �̱���)");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
