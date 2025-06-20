using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerSpeech : MonoBehaviour
{
    public TMP_Text countdownText;
    private bool isSpeaking = false;
    private float speechDuration = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpeaking)
        {
            StartCoroutine(SpeechCountdown());
        }
    }

    IEnumerator SpeechCountdown()
    {
        isSpeaking = true;

        float remaining = speechDuration;
        while (remaining > 0)
        {
            countdownText.text = $"Player is speaking... {Mathf.CeilToInt(remaining)}";
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
        }

        countdownText.text = "";
        isSpeaking = false;

        Debug.Log("Speech ended.");
    }
}
