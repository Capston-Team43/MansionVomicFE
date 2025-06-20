using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            // 인벤토리에 이 아이템 전달
            InventoryManager.Instance.PickUpItem(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
