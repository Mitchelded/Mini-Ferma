using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts;

public class InventoryManager : MonoBehaviour
{
    public int maxStack = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public PlayerInteraction playerInteraction;
    public int selectedSlot = -1;
    public bool isStartPlowSelected = false;
    public bool isVegitablesSelected = false;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }


    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        InventorySlot selectedInventorySlot = inventorySlots[newValue];

        InventoryItem selectedItem = selectedInventorySlot.GetComponentInChildren<InventoryItem>();

        if (selectedItem != null && selectedItem.item.name == "startPlow")
        {
            isStartPlowSelected = true;
        }
        else if (selectedItem != null && selectedItem.item.type == ItemType.Vegetables)
        {
            isVegitablesSelected = true;
        }
        else
        {
            isStartPlowSelected = false;
            isVegitablesSelected = false;
        }
    }


    public bool AddItem(Item item, GameObject parent)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot != null && itemSlot.item == item && itemSlot.countItem < maxStack)
            {
                if (parent != null)
                {
                    Destroy(parent);
                }
                itemSlot.countItem++;
                itemSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void RemoveItem(InventorySlot slot)
    {
        InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null)
        {
            Destroy(inventoryItem.gameObject);
        }
    }
}
