using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    public TMP_Text endingText;
    [TextArea] public string fullText;
    public float typingSpeed = 0.05f;
    private bool isTypingDone = false;

    void Start()
    {
        switch (EscapeManager.currentEscape)
        {
            case EscapeManager.EscapeType.FrontDoor:
                fullText = "You successfully burnt down the mansion. \nYou escaped through the front door.";
                break;
            case EscapeManager.EscapeType.Attic:
                fullText = "You successfully climbed down from the attic window. \nYou escaped through the attic.";
                break;
            case EscapeManager.EscapeType.Basement:
                fullText = "You successfully digged a tunnel from the basement. \nYou escaped through the basement.";
                break;
            default:
                fullText = "You failed to escape.";
                break;
        }
        Debug.Log("Start() called");
        Debug.Log("Escape type: " + EscapeManager.currentEscape);
        Debug.Log("Full text: " + fullText);
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        Debug.Log("Coroutine started.");
        endingText.text = "";
        foreach (char c in fullText)
        {
            endingText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingDone = true;
    }

    void Update()
    {
        if (isTypingDone && Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
