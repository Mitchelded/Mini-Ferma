using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    public GameObject mainInventory;



    public void OnInventoryClick()
    {
        mainInventory.SetActive(!mainInventory.activeSelf);
    }
}
