using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBedSpawn : MonoBehaviour
{
    public ShovelScript shovelScript;

    public GameObject nearCactus;

    private void Start()
    {
        shovelScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ShovelScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        else if (collision.CompareTag("Cactus") || collision.CompareTag("SeedBed") || collision.gameObject!=null)
        {
            shovelScript.canSpawn = false;
            if (collision.CompareTag("Cactus"))
            {
                nearCactus = collision.gameObject;
                shovelScript.canSpawn = true;
            }
        }
        else
        {
            shovelScript.canSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        shovelScript.canSpawn = true;
        nearCactus = null;
    }
}
