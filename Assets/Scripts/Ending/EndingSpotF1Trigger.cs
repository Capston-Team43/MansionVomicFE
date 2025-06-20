using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class EndingSpotF1Trigger : MonoBehaviour
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

        if (!EscapeManager.IsEscapeReady())
        {
            requirementText.text =
                $"Need Fuel[{EscapeManager.fuelCount}/2], Paper[{EscapeManager.paperCount}/5], " +
                $"Lighter or Match[{(EscapeManager.hasLighterOrMatch ? 1 : 0)}/1], Water[{EscapeManager.waterCount}/2]";
        }
        else
        {
            requirementText.text = "Press [R] to escape";
        }

        requirementText.gameObject.SetActive(true);
    }
}


