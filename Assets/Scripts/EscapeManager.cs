using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EscapeManager : MonoBehaviour
{
    public enum EscapeType { None, FrontDoor, Attic, Basement }

    public static EscapeManager Instance;
    public static EscapeType currentEscape = EscapeType.None;

    [Header("정문 탈출 조건")]
    public static int fuelCount = 0;
    public static int paperCount = 0;
    public static int waterCount = 0;
    public static bool hasLighterOrMatch = false;

    [Header("다락 탈출 조건")]
    public static bool windowBroken = false;
    public static int ropeCount = 0;
    public static int stakeCount = 0;

    [Header("지하 탈출 조건")]
    public static bool floorBroken = false;
    public static int shovelCount = 0;

    public TMP_Text feedbackText;
    private Coroutine messageRoutine;

    void Awake()
    {
        Instance = this;
        feedbackText.gameObject.SetActive(false);
    }

    public static void ResetEscapeConditions()
    {
        fuelCount = 0;
        paperCount = 0;
        waterCount = 0;
        hasLighterOrMatch = false;

        windowBroken = false;
        ropeCount = 0;
        stakeCount = 0;

        floorBroken = false;
        shovelCount = 0;

        currentEscape = EscapeType.None;
    }

    public static bool IsEscapeReady()
    {
        return fuelCount >= 2 && paperCount >= 5 && waterCount >= 2 && hasLighterOrMatch;
    }

    public static bool IsAtticEscapeReady()
    {
        return windowBroken && ropeCount >= 4 && stakeCount >= 1;
    }

    public static bool IsBasementEscapeReady()
    {
        return floorBroken && shovelCount >= 6;
    }

    public void ShowTemporaryMessage(string message, float duration = 1.5f)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    IEnumerator ShowMessageRoutine(string message, float duration)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        feedbackText.gameObject.SetActive(false);
    }

    public bool UseItemForFrontDoor(string name)
    {
        if (name.Contains("Fuel") && fuelCount < 2) { fuelCount++; return true; }
        if (name.Contains("Paper") && paperCount < 5) { paperCount++; return true; }
        if ((name.Contains("Lighter") || name.Contains("Match")) && !hasLighterOrMatch) { hasLighterOrMatch = true; return true; }
        if (name.Contains("Water") && waterCount < 2) { waterCount++; return true; }
        return false;
    }

    public bool UseItemForAttic(string name)
    {
        if (!windowBroken)
        {
            if (name.Contains("Axe") || name.Contains("Pickaxe"))
            {
                windowBroken = true;
                return true;
            }
        }
        else
        {
            if (name.Contains("Rope") && ropeCount < 4) { ropeCount++; return true; }
            if (name.Contains("Stake") && stakeCount < 1) { stakeCount++; return true; }
        }
        return false;
    }

    public bool UseItemForBasement(string name)
    {
        if (!floorBroken)
        {
            if (name.Contains("Axe") || name.Contains("Pickaxe"))
            {
                floorBroken = true;
                return true;
            }
        }
        else
        {
            if (name.Contains("Shovel") && shovelCount < 6)
            {
                shovelCount++;
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (InventoryUI.IsInventoryOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsEscapeReady() && FindObjectOfType<EndingSpotF1Trigger>()?.IsPlayerInside() == true)
            {
                currentEscape = EscapeType.FrontDoor;
                SceneManager.LoadScene("EndingScene");
            }
            else if (IsAtticEscapeReady() && FindObjectOfType<EndingSpotATrigger>()?.IsPlayerInside() == true)
            {
                currentEscape = EscapeType.Attic;
                SceneManager.LoadScene("EndingScene");
            }
            else if (IsBasementEscapeReady() && FindObjectOfType<EndingSpotB1Trigger>()?.IsPlayerInside() == true)
            {
                currentEscape = EscapeType.Basement;
                SceneManager.LoadScene("EndingScene");
            }
        }

    }
}

