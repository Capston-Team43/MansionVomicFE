using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class EndingSpotATrigger : MonoBehaviour
{
    public TMP_Text requirementText;
    private bool isPlayerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            UpdateRequirementText();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            requirementText.gameObject.SetActive(false);
        }
    }

    public bool IsPlayerInside()
    {
        return isPlayerInside;
    }

    public void UpdateRequirementText()
    {
        if (!isPlayerInside) return;

        if (!EscapeManager.windowBroken)
        {
            requirementText.text = "Use Axe or Pickaxe to break the window.";
        }
        else if (!EscapeManager.IsAtticEscapeReady())
        {
            requirementText.text = $"Need Rope[{EscapeManager.ropeCount}/4], Stake[{EscapeManager.stakeCount}/1]";
        }
        else
        {
            requirementText.text = "Press [R] to escape";
        }

        requirementText.gameObject.SetActive(true);
    }
}

