using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class InGameVoiceRecorder : MonoBehaviour
{
    public TMP_Text messageText;
    public int maxSpeakCount = 4;
    public int speakDuration = 5;

    private int speakLeft;
    private bool isRecording = false;
    private AudioClip currentClip;

    void Start()
    {
        speakLeft = maxSpeakCount;
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRecording) return;

            if (speakLeft > 0)
            {
                StartCoroutine(RecordAndSendWithCountdown());
            }
            else
            {
                StartCoroutine(ShowMessage("I cannot speak anymore."));
            }
        }
    }

    IEnumerator RecordAndSendWithCountdown()
    {
        isRecording = true;
        currentClip = Microphone.Start(null, false, speakDuration, 44100);

        // 카운트다운 표시
        for (int i = speakDuration; i > 0; i--)
        {
            messageText.text = $"Player is speaking... {i}";
            messageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        messageText.gameObject.SetActive(false);
        Microphone.End(null);
        speakLeft--;

        Debug.Log($"Finish Recording (speakLeft: {speakLeft})");

        byte[] data = WavUtility.FromAudioClip(currentClip, out string path, true);
        StartCoroutine(UploadToAzure(path));

        isRecording = false;
    }

    /*
    IEnumerator UploadToAzure(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", data, "recording.wav", "audio/wav");

        UnityWebRequest req = UnityWebRequest.Post("http://20.41.115.23:8000/voice-assistant/", form);
        Debug.Log("Sending to: /voice-assistant/");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("STT request transmission succeeded");
            yield return new WaitForSeconds(0.2f);

            using (UnityWebRequest audioReq = UnityWebRequestMultimedia.GetAudioClip("http://20.41.115.23:8000/latest-tts-audio/", AudioType.MPEG))
            {
                yield return audioReq.SendWebRequest();

                if (audioReq.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("latest-tts-audio 요청 실패: " + audioReq.error);
                    yield break;
                }
            }
        }
                
                if (req.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("STT request transmission succeeded");
                    yield return new WaitForSeconds(0.2f);

                    MonsterVoice monster = FindObjectOfType<MonsterVoice>();
                    if (monster != null)
                        monster.PlayFromServer("http://20.41.115.23:8000/latest-tts-audio/");
                }
                else
                {
                    Debug.LogError($"STT request transmission failed: {req.error}");
                }
                
    }
    */

    IEnumerator UploadToAzure(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", data, "recording.wav", "audio/wav");

        UnityWebRequest req = UnityWebRequest.Post("http://20.41.115.23:8000/voice-assistant/", form);
        Debug.Log("Sending to: /voice-assistant/");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("STT request transmission succeeded");
            yield return new WaitForSeconds(0.2f);

            using (UnityWebRequest audioReq = UnityWebRequestMultimedia.GetAudioClip("http://20.41.115.23:8000/latest-tts-audio/", AudioType.MPEG))
            {
                yield return audioReq.SendWebRequest();

                if (audioReq.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("latest-tts-audio 요청 실패: " + audioReq.error);
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(audioReq);
                if (clip == null)
                {
                    Debug.LogError("AudioClip 로드 실패: clip is null");
                    yield break;
                }

                Debug.Log($"AudioClip 로드 성공 | length: {clip.length}s, samples: {clip.samples}, frequency: {clip.frequency}");

                AudioSource source = GetComponent<AudioSource>();
                if (source == null)
                {
                    Debug.LogError("AudioSource가 없습니다. 이 오브젝트에 AudioSource 컴포넌트를 붙여주세요.");
                    yield break;
                }

                source.PlayOneShot(clip);
                Debug.Log("TTS 오디오 재생 완료");
            }
        }
        else
        {
            Debug.LogError($"STT request transmission failed: {req.error}");
        }
    }


    IEnumerator ShowMessage(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        messageText.gameObject.SetActive(false);
    }
}

