using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ItemSpawner : MonoBehaviour
{
    public Transform originalTransform;
    public GameObject feather;
    public GameObject potion;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnItem(feather);
        }
        for (int i = 0; i < 3; i++)
        {
            SpawnItem(potion);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameObject.FindGameObjectsWithTag("Feather").Length < 5)
        // {
        //     SpawnItem(feather);
        //     Debug.Log()

        // }
        if (GameObject.FindGameObjectsWithTag("Potion").Length < 3)
        {
            SpawnItem(potion);
            // Debug.Log("SpawnPotion");
        }
    }

    public void SpawnItem(GameObject item)
    {

        var spawnPosition = Utility.GetRandomPointOnNavMesh(originalTransform.position, 30f, NavMesh.AllAreas);
        spawnPosition.y = 1f;
        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
