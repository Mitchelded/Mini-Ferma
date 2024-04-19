using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShovelScript : MonoBehaviour
{

    public InventoryManager inventoryManager;
    [SerializeField]
    private InventoryItem selectedItem;
    public GameObject seedbedPrefubForShovel;
    public GameObject seedbedPrefub;
    public GameObject seedbed;
    public PlayerController playerController;
    public float radius;
    public bool isInit = false;

    public bool canSpawn = true;



    private void Start()
    {
        inventoryManager = GameObject.FindWithTag("Inventory Manager").GetComponent<InventoryManager>();
        playerController = GetComponent<PlayerController>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(1.5f, 0, 0), radius);
    }

    public void SpawnSeedbed()
    {
        selectedItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
        if (canSpawn && selectedItem.item.name == "shovel")
        {
            if(seedbed.GetComponent<SeedBedSpawn>().nearCactus !=null)
            {

                Destroy(seedbed.GetComponent<SeedBedSpawn>().nearCactus);
            }
            var t =Instantiate(seedbedPrefub, seedbed.transform.position + new Vector3(0,0,0.5f),
                    transform.rotation);
            playerController.playerInteractionGameObject= GameObject.FindGameObjectsWithTag("SeedBed");
        
        }

    }

    // Update is called once per frame
    void Update()
    {
        selectedItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
        if (selectedItem != null)
        {
            if (selectedItem.item.name == "shovel")
            {


                if (!isInit)
                {
                    seedbed = Instantiate(seedbedPrefubForShovel,transform.position + new Vector3(1.5f, 0f, 0f),
                    transform.rotation);
                    isInit = true;
                }
                else if (playerController.speedX > 0)
                {
                    seedbed.transform.position = transform.position + new Vector3(1.5f, 0, 0);
                }
                else if (playerController.speedX < 0)
                {
                    seedbed.transform.position = transform.position + new Vector3(-1.5f, 0, 0);
                }
                else if (playerController.speedY < 0)
                {
                    seedbed.transform.position = transform.position + new Vector3(0f, -1.5f, 0);
                }
                else if (playerController.speedY > 0)
                {
                    seedbed.transform.position = transform.position + new Vector3(0f, 1.5f, 0);
                }
            }
            else
            {
                isInit = false;
                if(seedbed!=null)
                {
                    Destroy(seedbed);
                }
            }
        }
        else
        {
            isInit = false;
            if (seedbed != null)
            {
                Destroy(seedbed);
            }
        }
    }
}
