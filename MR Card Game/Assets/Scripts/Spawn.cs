using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Spawners;

public class Spawn : MonoBehaviour
{
    public Spawner spawner;

    private int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            {
                spawner.Spawn();
                spawner.MostRecentlySpawnedObject.transform.position = new Vector3(0, spawnCount, 0);
                spawnCount++;
            }
    }
}
