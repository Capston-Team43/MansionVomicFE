using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject playerSlotLeft;
    public GameObject playerSlotRight;
    public Button readyButton;
    public TMP_Text countdownText;

    public bool testMode = false;

    private bool isReady = false;
    [SerializeField] private float countdown = 10f;
    private bool countdownStarted = false;

    void Start()
    {
        UpdatePlayerSlots();

        if (testMode)
        {
            readyButton.interactable = true;
            //playerSlotLeft.SetActive(true);
            playerSlotRight.SetActive(true);
            countdownText.gameObject.SetActive(false);
            readyButton.onClick.AddListener(OnClickReady);
            return;
        }

        readyButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount == 2;
        countdownText.gameObject.SetActive(false);
        readyButton.onClick.AddListener(OnClickReady);
    }

    void Update()
    {
        if (countdownStarted)
        {
            //Debug.Log("카운트다운 진행 중... " + countdown);
            countdown -= Time.deltaTime;
            countdownText.text = Mathf.CeilToInt(countdown).ToString();

            if (countdown <= 0)
            {
                countdownStarted = false;
                PhotonNetwork.LoadLevel("MainGame");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerSlots();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            readyButton.interactable = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerSlots();
        readyButton.interactable = false;
        isReady = false;
        countdownStarted = false;
        countdown = 10f;
        countdownText.gameObject.SetActive(false);
    }

    void UpdatePlayerSlots()
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;

        playerSlotLeft.SetActive(count >= 1);
        playerSlotRight.SetActive(count == 2);
    }

    void OnClickReady()
    {
        isReady = true;
        readyButton.interactable = false;
        if (photonView == null)
            Debug.LogError("PhotonView가 연결되어 있지 않습니다!");

        photonView.RPC("SetReady", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void SetReady(Player player)
    {
        if (testMode)
        {
            Debug.Log("테스트 모드에서 카운트다운 강제 시작!");
            countdownStarted = true;
            countdownText.gameObject.SetActive(true);

            FindObjectOfType<VoiceRecorder>().StartRecording(); //녹음 시작
            return;
        }

        var props = new ExitGames.Client.Photon.Hashtable { { "IsReady", true } };
        player.SetCustomProperties(props);

        int readyCount = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isReadyObj;
            if (p.CustomProperties.TryGetValue("IsReady", out isReadyObj) && (bool)isReadyObj)
            {
                readyCount++;
            }
        }

        if (readyCount == 2 && !countdownStarted)
        {
            Debug.Log("두 플레이어 모두 준비됨. 카운트다운 시작!");
            countdownStarted = true;
            countdownText.gameObject.SetActive(true);

            FindObjectOfType<VoiceRecorder>().StartRecording(); //녹음 시작
        }
    }
}
