using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ItemSpawner : MonoBehaviour
{
    public Transform originalTransform;
    public GameObject feather;
    public GameObject potion;
    private static ItemSpawner instance; // 싱글톤이 할당될 static 변수
    public static ItemSpawner Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (instance == null)
                // 씬에서 GameManager 오브젝트를 찾아 할당
                instance = FindObjectOfType<ItemSpawner>();

            // 싱글톤 오브젝트를 반환
            return instance;
        }
    }
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
        if (GameObject.FindGameObjectsWithTag("Feather").Length < 5)
        {
            SpawnItem(feather);


        }
        if (GameObject.FindGameObjectsWithTag("Potion").Length < 3)
        {
            SpawnItem(potion);
            // Debug.Log("SpawnPotion");
        }
    }

    public void SpawnItem(GameObject item)
    {

        var spawnPosition = Utility.GetRandomPointOnNavMesh(originalTransform.position, 100f, NavMesh.AllAreas);
        spawnPosition.y = 1f;
        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
