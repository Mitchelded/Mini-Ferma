using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour
{

    public GameObject cactusPrefab;
    public int numberOfCacti;
    public Vector3 spawnRadius;
    public Vector3 spawnOffset;
    public Vector2 sizeRange;
    
    void Start()
    {
        SpawnCacti();
    }

    void SpawnCacti()
    {
        for (int i = 0; i < numberOfCacti; i++)
        {
            float randomX = Random.Range(-spawnRadius.x, spawnRadius.x);
            float randomY = Random.Range(-spawnRadius.y, spawnRadius.y);
            Vector3 randomPosition = transform.position + new Vector3(randomX, randomY, 0f) + spawnOffset;


            float randomSizeXY = Random.Range(sizeRange.x, sizeRange.y);
            Vector2 randomSize = new Vector2(randomSizeXY, randomSizeXY);

            GameObject cactus = Instantiate(cactusPrefab, randomPosition, Quaternion.identity);
            cactus.transform.localScale = randomSize;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + spawnOffset, new Vector3(spawnRadius.x * 2, spawnRadius.y * 2, 0));
    }
}
