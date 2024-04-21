using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    InventoryManager inventoryManager;
    public GameObject cactusPrefab;
    public GameObject seedbedPrefab;

    private string savePath;
    private string saveMapPath;
    private string saveSeedbedPath;
    [SerializeField]
    List<GameObject> cactuses;
    [SerializeField]
    List<GameObject> seedbeds;

    private void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory Manager").GetComponent<InventoryManager>();


        savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        saveMapPath = Path.Combine(Application.persistentDataPath, "map.json");
        saveSeedbedPath = Path.Combine(Application.persistentDataPath, "seedbed.json");
        Load();
    }

    public void Save()
    {
        SaveInventory();
        SaveMap();
        SaveSeedbed();
    }

    public void Load()
    {
        LoadInventory();
        LoadMap();
        LoadSeedbed();
    }

    private void SaveInventory()
    {
        Dates.InventoryData data = new Dates.InventoryData
        {
            maxStack = inventoryManager.maxStack,
            selectedSlot = inventoryManager.selectedSlot,
            isStartPlowSelected = inventoryManager.isStartPlowSelected,
            isVegitablesSelected = inventoryManager.isVegitablesSelected,
            items = new List<Item>()
        };

        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null && item.item != null)
            {
                data.items.Add(item.item);
            }
            else
            {
                data.items.Add(null);
            }
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, jsonData);
    }

    private void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            Dates.InventoryData data = JsonUtility.FromJson<Dates.InventoryData>(jsonData);

            inventoryManager.maxStack = data.maxStack;
            inventoryManager.selectedSlot = data.selectedSlot;
            inventoryManager.isStartPlowSelected = data.isStartPlowSelected;
            inventoryManager.isVegitablesSelected = data.isVegitablesSelected;

            foreach (InventorySlot slot in inventoryManager.inventorySlots)
            {
                inventoryManager.RemoveItem(slot);
            }


            for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
            {
                if (i < data.items.Count && data.items[i] != null)
                {

                    inventoryManager.SpawnNewItem(data.items[i], inventoryManager.inventorySlots[i]);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void SaveMap()
    {
        Dates.CactusData data = new Dates.CactusData
        {
            cactusPositions = new List<Vector3>(),
            cactusRotation = new List<Quaternion>(),
            cactusScale = new List<Vector3>()
        };

        cactuses = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cactus"));

        foreach (GameObject cactus in cactuses)
        {
            Transform item = cactus.GetComponent<Transform>();
            if (item != null)
            {
                data.cactusPositions.Add(item.position);
                data.cactusRotation.Add(item.rotation);
                data.cactusScale.Add(item.localScale);
            }
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(saveMapPath, jsonData);
    }

    private void LoadMap()
    {
        if (File.Exists(saveMapPath))
        {
            string jsonData = File.ReadAllText(saveMapPath);
            Dates.CactusData data = JsonUtility.FromJson<Dates.CactusData>(jsonData);
            cactuses = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cactus"));
            foreach (var cactus in cactuses)
            {
                Destroy(cactus);
            }

            for (int i = 0; i < data.cactusPositions.Count; i++)
            {
                var t = Instantiate(cactusPrefab, position: data.cactusPositions[i], rotation: data.cactusRotation[i]);
                t.transform.localScale = data.cactusScale[i];

            }
        }
    }


    private void SaveSeedbed()
    {
        Dates.SeedbedData data = new Dates.SeedbedData
        {
            seedbedPositions = new List<Vector3>(),
            seedbedRotation = new List<Quaternion>(),
            isWeeded = new List<bool>(),
            isPlanted = new List<bool>(),
            status = new List<PlayerInteraction.SeedbedStatus>(),
            vegetables = new List<PlayerInteraction.SeedbedVegetables>(),
            canInteract = new List<bool>(),
            playerInRange = new List<bool>()
        };

        seedbeds = new List<GameObject>(GameObject.FindGameObjectsWithTag("SeedBed"));

        foreach (GameObject seedbed in seedbeds)
        {
            Transform transformData = seedbed.GetComponent<Transform>();
            PlayerInteraction playerInt = seedbed.GetComponent<PlayerInteraction>();
            if (transformData != null &&
                playerInt != null)
            {
                data.seedbedPositions.Add(transformData.position);
                data.seedbedRotation.Add(transformData.rotation);
                data.isWeeded.Add(playerInt.isWeeded);
                data.isPlanted.Add(playerInt.isPlanted);
                data.status.Add(playerInt.status);
                data.canInteract.Add(playerInt.canInteract);
                data.vegetables.Add(playerInt.vegetables);
                data.playerInRange.Add(playerInt.playerInRange);
            }
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(saveSeedbedPath, jsonData);
    }

    private void LoadSeedbed()
    {
        if (File.Exists(saveSeedbedPath))
        {
            string jsonData = File.ReadAllText(saveSeedbedPath);
            Dates.SeedbedData data = JsonUtility.FromJson<Dates.SeedbedData>(jsonData);
            seedbeds = new List<GameObject>(GameObject.FindGameObjectsWithTag("SeedBed"));
            foreach (var seedbed in seedbeds)
            {
                Destroy(seedbed);
            }

            for (int i = 0; i < data.seedbedPositions.Count; i++)
            {
                var t = Instantiate(seedbedPrefab, position: data.seedbedPositions[i], rotation: data.seedbedRotation[i]);
                var q = t.GetComponentInChildren<PlayerInteraction>();
                q.isWeeded = data.isWeeded[i];

                q.isPlanted = data.isPlanted[i];

                q.status = data.status[i];

                q.canInteract = data.canInteract[i];

                q.vegetables = data.vegetables[i];

                q.playerInRange = data.playerInRange[i];

            }
        }
    }


}
