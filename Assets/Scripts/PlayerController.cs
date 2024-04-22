using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInteraction;

public class PlayerController : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public float normalSpeed;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Camera cam;
    public Vector3 offset;
    [SerializeField]
    private Animator animator;
    public GameObject[] playerInteractionGameObject;
    public InventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        speedX = 0f;
        speedY = 0f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInteractionGameObject = GameObject.FindGameObjectsWithTag("SeedBed");
        inventoryManager = GameObject.FindWithTag("Inventory Manager").GetComponent<InventoryManager>();
    }

    public void Interaction()
    {
        var selectedItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
        if (selectedItem.item.name != "shovel")
        {
            playerInteractionGameObject = GameObject.FindGameObjectsWithTag("SeedBed");
            foreach (var player in playerInteractionGameObject)
            {
                PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
                if (playerInteraction.canInteract
                    && playerInteraction.playerInRange
                    && inventoryManager.isVegitablesSelected
                    && playerInteraction.status == SeedbedStatus.EMPTY
                    && playerInteraction.vegetables == SeedbedVegetables.None)
                {
                    Instantiate(playerInteraction.seedsPrefab, player.transform.position + new Vector3(0f, 0f, -0.025f), player.transform.rotation, player.transform);
                    var t = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>();
                    if (t != null)
                    {
                        if (t.countItem == 1)
                        {
                            inventoryManager.RemoveItem(inventoryManager.inventorySlots[inventoryManager.selectedSlot]);
                        }
                        else
                        {
                            t.countItem--;
                            t.RefreshCount();
                        }

                        if (t.item.name == "eggplant")
                        {
                            playerInteraction.vegetables = SeedbedVegetables.Eggplant;
                        }
                        else if (t.item.name == "pumkin")
                        {
                            playerInteraction.vegetables = SeedbedVegetables.Pumkin;
                        }
                        else if (t.item.name == "carrot")
                        {
                            playerInteraction.vegetables = SeedbedVegetables.Carrot;
                        }
                        else if (t.item.name == "beet")
                        {
                            playerInteraction.vegetables = SeedbedVegetables.Beet;
                        }
                    }
                    playerInteraction.isPlanted = true;
                    StartCoroutine(playerInteraction.UpdateStatus());
                }
                else if (playerInteraction.canInteract
                    && playerInteraction.playerInRange
                    && inventoryManager.isStartPlowSelected
                    && playerInteraction.status == SeedbedStatus.PLOW)
                {
                    StartCoroutine(playerInteraction.UpdateStatus());
                }
            }
        }
    }




    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speedX, speedY);
        cam.transform.position = rb.transform.position + offset;
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnUpButtonDown();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnRightButtonDown();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnLeftButtonDown();
        }
    }

    public void OnLeftButtonDown()
    {
        if (speedX >= 0f)
        {
            animator.SetBool("LeftMovement", true);
            animator.SetBool("DownMovement", false);
            animator.SetBool("UpMovement", false);
            animator.SetBool("RightMovement", false);
            speedX = -normalSpeed;
        }
    }

    public void OnRightButtonDown()
    {
        if (speedX <= 0f)
        {
            animator.SetBool("DownMovement", false);
            animator.SetBool("LeftMovement", false);
            animator.SetBool("UpMovement", false);
            animator.SetBool("RightMovement", true);
            speedX = normalSpeed;
        }
    }

    public void OnUpButtonDown()
    {
        if (speedY >= 0f)
        {
            animator.SetBool("DownMovement", false);
            animator.SetBool("LeftMovement", false);
            animator.SetBool("UpMovement", true);
            animator.SetBool("RightMovement", false);
            speedY = normalSpeed;
        }
    }

    public void OnDownButtonDown()
    {
        if (speedY <= 0f)
        {
            animator.SetBool("DownMovement", true);
            animator.SetBool("LeftMovement", false);
            animator.SetBool("UpMovement", false);
            animator.SetBool("RightMovement", false);
            speedY = -normalSpeed;
        }
    }

    public void OnButtonUp()
    {
        speedX = 0f;
        speedY = 0f;
    }
}
