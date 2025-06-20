using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingSpotB1Trigger : MonoBehaviour
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

        if (!EscapeManager.floorBroken)
        {
            requirementText.text = "Use Axe or Pickaxe to break the floor.";
        }
        else if (!EscapeManager.IsBasementEscapeReady())
        {
            requirementText.text = $"Use Shovel to dig tunnel [{EscapeManager.shovelCount}/6]";
        }
        else
        {
            requirementText.text = "Press [R] to escape";
        }

        requirementText.gameObject.SetActive(true);
    }
}
