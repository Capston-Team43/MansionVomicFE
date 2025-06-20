using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    //public Animator animator;
    public Image[] slots;
    public Image selectionIndicator;
    public static bool IsInventoryOpen { get; private set; }

    private int selectedIndex = -1;

    public static InventoryUI Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    void Update()
    {
        // 열기/닫기
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (!inventoryPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.A))
            MoveSelection(-1);

        if (Input.GetKeyDown(KeyCode.D))
            MoveSelection(1);

        if (Input.GetKeyDown(KeyCode.W))
            MoveSelection(3);

        if (Input.GetKeyDown(KeyCode.S))
            MoveSelection(-3);

        if (Input.GetKeyDown(KeyCode.Q))
            DropSelectedItem();

        if (Input.GetKeyDown(KeyCode.E))
            UseItemInEscapeZone();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
        {
            CloseInventory();
        }
    }

    void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        if (!isOpen) OpenInventory();
        else CloseInventory();
    }

    void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        IsInventoryOpen = true;
        //animator.SetBool("isOpen", true);

        if (InventoryManager.Instance.inventory.Count > 0)
        {
            selectedIndex = 0;
            UpdateSelection();
        }
        else
        {
            selectedIndex = -1;
            selectionIndicator.enabled = false;
        }
    }

    void CloseInventory()
    {
        //animator.SetBool("isOpen", false);
        inventoryPanel.SetActive(false);
        IsInventoryOpen = false;
        selectedIndex = -1;
        selectionIndicator.enabled = false;
        Invoke(nameof(DeactivatePanel), 0.5f); // 애니메이션 시간만큼 지연
    }

    void DeactivatePanel()
    {
        inventoryPanel.SetActive(false);
    }

    void MoveSelection(int dir)
    {
        if (InventoryManager.Instance.inventory.Count == 0) return;

        selectedIndex += dir;
        if (selectedIndex < 0) selectedIndex = InventoryManager.Instance.inventory.Count - 1;
        if (selectedIndex >= InventoryManager.Instance.inventory.Count) selectedIndex = 0;

        UpdateSelection();
    }

    void UpdateSelection()
    {
        selectionIndicator.transform.position = slots[selectedIndex].transform.position;
        selectionIndicator.enabled = true;
    }

    void DropSelectedItem()
    {
        InventoryManager.Instance.DropItem(selectedIndex);
        CloseInventory();
    }

    void UseItemInEscapeZone()
    {
        InventoryManager.Instance.UseItemInEscapeZone(selectedIndex);
    }
    

    public void UpdateUI()
    {
        var items = InventoryManager.Instance.inventory;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                var instance = items[i].GetComponent<ItemInstance>();
                if (instance != null && instance.itemIcon != null)
                {
                    slots[i].sprite = instance.itemIcon;
                    slots[i].color = Color.white;
                }
                else
                {
                    slots[i].sprite = null;
                    slots[i].color = new Color(1, 1, 1, 0); // 투명하게
                }
            }
            else
            {
                slots[i].sprite = null;
                slots[i].color = new Color(1, 1, 1, 0);
            }
        }
    }


}
