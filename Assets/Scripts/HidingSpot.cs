using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public Transform hidingPoint;

    private bool isPlayerNear = false;
    private PlayerHiding playerHiding;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerHiding = other.GetComponentInParent<PlayerHiding>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerHiding = null;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (playerHiding != null)
                playerHiding.ToggleHiding(hidingPoint);
                DrawerSoundManager.Instance?.PlayHidingSound();
        }
    }
}
