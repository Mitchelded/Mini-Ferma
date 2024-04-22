using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public GameObject cactusPrefab;
    public GameObject seedbedPrefab;

    private string savePath;
    private string saveMapPath;
    private string saveSeedbedPath;
    private string savePlayerPath;
    [SerializeField] List<GameObject> cactuses;
    [SerializeField] List<GameObject> seedbeds;

    private void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory Manager").GetComponent<InventoryManager>();


        savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        saveMapPath = Path.Combine(Application.persistentDataPath, "map.json");
        saveSeedbedPath = Path.Combine(Application.persistentDataPath, "seedbed.json");
        savePlayerPath = Path.Combine(Application.persistentDataPath, "player.json");
        Load();
    }

    public void Save()
    {
        SaveMap();
        SaveSeedbed();
        SaveInventory();
        SavePlayer();
    }

    public void Load()
    {
        LoadMap();
        LoadSeedbed();
        LoadPlayer();
        LoadInventory();
    }

    private void SavePlayer()
    {
        Dates.PlayerData data = new Dates.PlayerData
        {
            playerPositions = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().rotation,
            playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().level,
            playerExp = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().levelExp,
        };

        var jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePlayerPath, jsonData);
    }

    private void LoadPlayer()
    {
        if (File.Exists(savePlayerPath))
        {
            var jsonData = File.ReadAllText(savePlayerPath);
            var data = JsonUtility.FromJson<Dates.PlayerData>(jsonData);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = data.playerPositions;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().rotation = data.playerRotation;
            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().level = data.playerLevel;
            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().levelExp = data.playerExp;
            GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>().UpdateText();
        }
    }


    private void SaveInventory()
    {
        Dates.InventoryData data = new Dates.InventoryData
        {
            maxStack = inventoryManager.maxStack,
            selectedSlot = inventoryManager.selectedSlot,
            isStartPlowSelected = inventoryManager.isStartPlowSelected,
            isVegitablesSelected = inventoryManager.isVegitablesSelected,
            items = new List<Item>(),
            counts = new List<int>(),
        };

        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null && item.item != null)
            {
                data.items.Add(item.item);
                data.counts.Add(item.countItem);
            }
            else
            {
                data.items.Add(null);
                data.counts.Add(0);
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
                if (i < data.items.Count && data.items[i] != null && data.counts[i] != 0)
                {
                    inventoryManager.SpawnNewItem(data.items[i], inventoryManager.inventorySlots[i]);
                    var t = inventoryManager.inventorySlots[i].GetComponentsInChildren<InventoryItem>();
                    if (t != null)
                    {
                        foreach (var test in t)
                        {
                            test.countItem = data.counts[i];
                            test.RefreshCount();
                        }
                    }
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
                var t = Instantiate(seedbedPrefab, position: data.seedbedPositions[i],
                    rotation: data.seedbedRotation[i]);
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