using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;

public class Test : MonoBehaviour
{

    public Item item;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private PlayerInteraction playerInteraction;

    private LevelSystem levelSystem;

    [SerializeField]
    private bool isCollected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isCollected)
        {
            if (collision.CompareTag("Player"))
            {
                var playerInteraction = gameObject.GetComponentInParent<PlayerInteraction>();
                
                if(playerInteraction!=null)
                {
                    if(playerInteraction.status == SeedbedStatus.READY)
                    {
                        inventoryManager.AddItem(item, gameObject);
                    }
                    playerInteraction.status = SeedbedStatus.EMPTY;
                    playerInteraction.isPlanted = false;
                    playerInteraction.isWeeded = false;
                    playerInteraction.vegetables = SeedbedVegetables.None;
                    levelSystem.levelExp += item.howMuchExp;

                }
                inventoryManager.AddItem(item, gameObject);
                isCollected = true;
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        inventoryManager = GameObject.FindWithTag("Inventory Manager").GetComponent<InventoryManager>();
        levelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
    }
}
