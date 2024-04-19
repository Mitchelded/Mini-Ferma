using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedSlot, notSelectedSlot;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.sprite = selectedSlot;
    }

    public void Deselect()
    {
        image.sprite = notSelectedSlot;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
