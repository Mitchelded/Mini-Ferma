using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public bool playerInRange = false;

    public bool canInteract = false;

    public Sprite selectedSeedbed, unSelectedSeedbed;

    public InventoryManager inventoryManager;

    public SpriteRenderer renderer;

    public bool isPlanted = false;

    public bool isWeeded = false;

    public GameObject seedsPrefab;
    public GameObject eggplantPrefab;
    public GameObject pumkinPrefab;
    public GameObject beetPrefab;
    public GameObject carrotPrefab;

    public GameObject seededPlant;

    public BoxCollider2D boxCollider2D;


    public enum SeedbedStatus { EMPTY, GROW, PLOW, READY }

    public SeedbedStatus status = SeedbedStatus.EMPTY;

    public enum SeedbedVegetables { None, Eggplant, Beet, Carrot, Pumkin }

    public SeedbedVegetables vegetables = SeedbedVegetables.None;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Если объект, вошедший в триггер, является игроком
        {
            var t = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
            if(t != null)
            {
                if ((t.item.type == ItemType.Tool && status == SeedbedStatus.PLOW) || (t.item.type == ItemType.Vegetables && status == SeedbedStatus.EMPTY))
                {
                    playerInRange = true; // Указываем, что игрок находится в пределах грядки
                    canInteract = true; // Устанавливаем возможность взаимодействия
                    renderer.sprite = selectedSeedbed;
                }
            }
        }
    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player")) // Если объект, вышедший из триггера, является игроком
        {
            playerInRange = false; // Указываем, что игрок вышел из пределов грядки
            canInteract = false; // Отключаем возможность взаимодействия
            renderer.sprite = unSelectedSeedbed;
        }
    }

    private void Start()
    {
        inventoryManager = GameObject.FindWithTag("Inventory Manager").GetComponent<InventoryManager>();
        renderer = GetComponent<SpriteRenderer>();

    }

    public void CreateObject(SeedbedVegetables vegetables, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (vegetables == SeedbedVegetables.Eggplant)
        {
            seededPlant = Instantiate(eggplantPrefab, position + new Vector3(0f, 0f, -1f), rotation, parent);
        }
        else if (vegetables == SeedbedVegetables.Pumkin)
        {
            seededPlant = Instantiate(pumkinPrefab, position + new Vector3(0f, 0f, -1f), rotation, parent);
        }
        else if (vegetables == SeedbedVegetables.Beet)
        {
            seededPlant = Instantiate(beetPrefab, position + new Vector3(0f, 0f, -1f), rotation, parent);
        }
        else if (vegetables == SeedbedVegetables.Carrot)
        {
            seededPlant = Instantiate(carrotPrefab, position + new Vector3(0f, 0f, -1f), rotation, parent);
        }
    }

    public IEnumerator UpdateStatus()
    {
        if (status == SeedbedStatus.EMPTY && isPlanted)
        {
            ++status;
            yield return new WaitForSeconds(10f);
            ++status;
            GameObject childObject = transform.Find("seeds(Clone)").gameObject;
            Destroy(childObject);
            if (SeedbedVegetables.Eggplant == vegetables)
            {
                var t = eggplantPrefab.GetComponent<BoxCollider2D>();
                t.enabled = false;
                t.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                CreateObject(SeedbedVegetables.Eggplant,
                    transform.position + new Vector3(0f, 0f, -0.025f),
                    transform.rotation,
                    transform);
            }
            else if (SeedbedVegetables.Beet == vegetables)
            {
                var t = beetPrefab.GetComponent<BoxCollider2D>();
                t.enabled = false;
                t.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                CreateObject(SeedbedVegetables.Beet,
                    transform.position + new Vector3(0f, 0f, -0.025f),
                    transform.rotation,
                    transform);
            }
            else if (SeedbedVegetables.Carrot == vegetables)
            {
                var t = carrotPrefab.GetComponent<BoxCollider2D>();
                t.enabled = false;
                t.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                CreateObject(SeedbedVegetables.Carrot,
                    transform.position + new Vector3(0f, 0f, -0.025f),
                    transform.rotation,
                    transform);
            }   
            else if (SeedbedVegetables.Pumkin == vegetables)
            {
                var t = pumkinPrefab.GetComponent<BoxCollider2D>();
                t.enabled = false;
                t.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                CreateObject(SeedbedVegetables.Pumkin,
                    transform.position + new Vector3(0f, 0f, -0.025f),
                    transform.rotation,
                    transform);
            }
            isWeeded = true;
        }
        else if (status == SeedbedStatus.PLOW && isWeeded && isPlanted)
        {
            yield return new WaitForSeconds(10f); // Wait for 1 second
            ++status;
            boxCollider2D = gameObject.GetComponentInChildren<BoxCollider2D>();
            boxCollider2D.enabled = true;
            seededPlant.transform.localScale = Vector3.one;
        }
    }
}

