using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

// The class of the castle game object
static class EnemyPools
{
    // The maximum and current health point of the castle
    public static int[] enemyPoolIds;
}

public class ObjectPools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Create all enemy pools
        int poolId1 = ObjectPool<Enemy>.CreateNewPool();
        int poolId2 = ObjectPool<Enemy>.CreateNewPool();
        int poolId3 = ObjectPool<Enemy>.CreateNewPool();
        int poolId4 = ObjectPool<Enemy>.CreateNewPool();
        int poolId5 = ObjectPool<Enemy>.CreateNewPool();
        int poolId6 = ObjectPool<Enemy>.CreateNewPool();
        int poolId7 = ObjectPool<Enemy>.CreateNewPool();
        int poolId8 = ObjectPool<Enemy>.CreateNewPool();
        int poolId9 = ObjectPool<Enemy>.CreateNewPool();

        // Store the pool ids so that they are not lost
        EnemyPools.enemyPoolIds = new int[9];
        EnemyPools.enemyPoolIds[0] = poolId1;
        EnemyPools.enemyPoolIds[1] = poolId2;
        EnemyPools.enemyPoolIds[2] = poolId3;
        EnemyPools.enemyPoolIds[3] = poolId4;
        EnemyPools.enemyPoolIds[4] = poolId5;
        EnemyPools.enemyPoolIds[5] = poolId6;
        EnemyPools.enemyPoolIds[6] = poolId7;
        EnemyPools.enemyPoolIds[7] = poolId8;
        EnemyPools.enemyPoolIds[8] = poolId9;

        // Add a normal enemy to the first enemy pool
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private GameObject[] objectPrefabs;

    // Get an object form the pool
    public GameObject GetObject(string type)
    {
        // Go through the array
        for(int i = 0; i < objectPrefabs.Length; i++)
        {
            // Check if there is an object that has the name of the type
            if(objectPrefabs[i].name == type)
            {
                // Instantiate the object
                GameObject newObject = Instantiate(objectPrefabs[i]);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }

    // //
    // public void ReleaseObject(GameObject object) 
    // {
    //     gameObject.SetActive(false);
    // }
}
