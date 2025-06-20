using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxSlots = 6;
    public List<GameObject> inventory = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PickUpItem(GameObject item)
    {
        if (inventory.Count >= maxSlots)
        {
            Debug.Log("The inventory is full.");
            return;
        }

        inventory.Add(item);
        Debug.Log($"Add {item.name} into the inventory.");

        DrawerSoundManager.Instance?.PlayPickupSound();

        // 맵에서 제거
        item.SetActive(false);
        InventoryUI.Instance.UpdateUI(); // UI 갱신
    }

    public void DropItem(int index)
    {
        if (index < 0 || index >= inventory.Count) return;

        GameObject item = inventory[index];

        // 플레이어 위치 기준 드롭 위치
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        Vector3 dropStart = playerTransform.position + playerTransform.forward * 1f + Vector3.up * 2f;

        Vector3 dropTarget = dropStart;

        Transform player = GameObject.FindWithTag("Player").transform;
        CharacterController cc = player.GetComponentInParent<CharacterController>();
        float height = cc.height;

        if (height > 1)
        {
            dropTarget.y = playerTransform.position.y - 1.73f;
        }
        else
        {
            dropTarget.y = playerTransform.position.y - 0.7f;
        }

        item.transform.position = dropTarget;
        item.SetActive(true);
        inventory.RemoveAt(index);

        DrawerSoundManager.Instance?.PlayDropSound();

        InventoryUI.Instance.UpdateUI();
    }

    public void UseItemInEscapeZone(int index)
    {
        if (index < 0 || index >= inventory.Count)
            return;

        GameObject item = inventory[index];
        string name = item.name;
        Debug.Log("Item name: " + name);

        bool used = false;

        if (FindObjectOfType<EndingSpotF1Trigger>()?.IsPlayerInside() == true)
        {
            used = EscapeManager.Instance.UseItemForFrontDoor(name);
            if (used) FindObjectOfType<EndingSpotF1Trigger>().UpdateRequirementText();
        }
        else if (FindObjectOfType<EndingSpotATrigger>()?.IsPlayerInside() == true)
        {
            used = EscapeManager.Instance.UseItemForAttic(name);
            if (used) FindObjectOfType<EndingSpotATrigger>().UpdateRequirementText();
        }
        else if (FindObjectOfType<EndingSpotB1Trigger>()?.IsPlayerInside() == true)
        {
            used = EscapeManager.Instance.UseItemForBasement(name);
            if (used) FindObjectOfType<EndingSpotB1Trigger>().UpdateRequirementText();
        }

        if (used)
        {
            if (!name.Contains("Shovel"))
            {
                Destroy(item);
                inventory.RemoveAt(index);
            }
            InventoryUI.Instance.UpdateUI();
        }
        else
        {
            EscapeManager.Instance.ShowTemporaryMessage("Cannot use the item.");
        }
    }
}
