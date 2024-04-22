using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static class Dates
    {
        [System.Serializable]
        public class InventoryData
        {
            public int maxStack;
            public int selectedSlot;
            public bool isStartPlowSelected;
            public bool isVegitablesSelected;
            public List<Item> items;
            public List<int> counts;
        }
        
        [System.Serializable]
        public class PlayerData
        {
            public Vector3 playerPositions;
            public Quaternion playerRotation;
            public int playerLevel;
            public int playerExp;
        }

        [System.Serializable]
        public class CactusData
        {
            public List<Vector3> cactusPositions = new List<Vector3>();
            public List<Quaternion> cactusRotation = new List<Quaternion>();
            public List<Vector3> cactusScale = new List<Vector3>();
        }

        [System.Serializable]
        public class SeedbedData
        {
            public List<string> gameObjectName;
            public List<PlayerInteraction.SeedbedStatus> status;
            public List<PlayerInteraction.SeedbedVegetables> vegetables;
            public List<bool> isPlanted;
            public List<bool> isWeeded;
            public List<Vector3> seedbedPositions = new List<Vector3>();
            public List<Quaternion> seedbedRotation = new List<Quaternion>();
            public List<bool> playerInRange;
            public List<bool> canInteract;
        }

    }
}
