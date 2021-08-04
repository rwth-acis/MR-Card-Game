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
        int poolId1 = ObjectPool<Enemy>.CreateNewPool(); // Pool for normal enemies
        int poolId2 = ObjectPool<Enemy>.CreateNewPool(); // Pool for fast enemies
        int poolId3 = ObjectPool<Enemy>.CreateNewPool(); // Pool for super fast enemies
        int poolId4 = ObjectPool<Enemy>.CreateNewPool(); // Pool for flying enemies
        int poolId5 = ObjectPool<Enemy>.CreateNewPool(); // Pool for tanky enemies
        int poolId6 = ObjectPool<Enemy>.CreateNewPool(); // Pool for slow enemies
        int poolId7 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker enemies
        int poolId8 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker flying enemies
        int poolId9 = ObjectPool<Enemy>.CreateNewPool(); // Pool for berzerker tanky enemies

        int poolId10 = ObjectPool<Projectile>.CreateNewPool(); // Pool for arrows
        int poolId11 = ObjectPool<Projectile>.CreateNewPool(); // Pool for fire balls
        int poolId12 = ObjectPool<Projectile>.CreateNewPool(); // Pool for stones

        int poolId13 = ObjectPool<Enemy>.CreateNewPool(); // Pool for dumping enemies with wrong enemy type

        int poolId14 = ObjectPool<Tower>.CreateNewPool(); // Pool for archer tower
        int poolId15 = ObjectPool<Tower>.CreateNewPool(); // Pool for fire tower
        int poolId16 = ObjectPool<Tower>.CreateNewPool(); // Pool for earth tower
        int poolId17 = ObjectPool<Tower>.CreateNewPool(); // Pool for lightning tower
        int poolId18 = ObjectPool<Tower>.CreateNewPool(); // Pool for wind tower

        // Store the pool ids so that they are not lost
        EnemyPools.enemyPoolIds = new int[18];
        EnemyPools.enemyPoolIds[0] = poolId1;
        EnemyPools.enemyPoolIds[1] = poolId2;
        EnemyPools.enemyPoolIds[2] = poolId3;
        EnemyPools.enemyPoolIds[3] = poolId4;
        EnemyPools.enemyPoolIds[4] = poolId5;
        EnemyPools.enemyPoolIds[5] = poolId6;
        EnemyPools.enemyPoolIds[6] = poolId7;
        EnemyPools.enemyPoolIds[7] = poolId8;
        EnemyPools.enemyPoolIds[8] = poolId9;

        EnemyPools.enemyPoolIds[9] = poolId10;
        EnemyPools.enemyPoolIds[10] = poolId11;
        EnemyPools.enemyPoolIds[11] = poolId12;

        EnemyPools.enemyPoolIds[12] = poolId13;
        
        EnemyPools.enemyPoolIds[13] = poolId14;
        EnemyPools.enemyPoolIds[14] = poolId15;
        EnemyPools.enemyPoolIds[15] = poolId16;
        EnemyPools.enemyPoolIds[16] = poolId17;
        EnemyPools.enemyPoolIds[17] = poolId18;

        // Debug.Log("Pool index: " + poolId1);
        // Debug.Log("Pool index: " + poolId2);
        // Debug.Log("Pool index: " + poolId3);
        // Debug.Log("Pool index: " + poolId4);
        // Debug.Log("Pool index: " + poolId5);
        // Debug.Log("Pool index: " + poolId6);
        // Debug.Log("Pool index: " + poolId7);
        // Debug.Log("Pool index: " + poolId8);
        // Debug.Log("Pool index: " + poolId9);
        // Debug.Log("Pool index: " + poolId10);
        // Debug.Log("Pool index: " + poolId11);
        // Debug.Log("Pool index: " + poolId12);
        // Debug.Log("Pool index: " + poolId13);
        // Debug.Log("Pool index: " + poolId14);
        // Debug.Log("Pool index: " + poolId15);
        // Debug.Log("Pool index: " + poolId16);
        // Debug.Log("Pool index: " + poolId17);
        // Debug.Log("Pool index: " + poolId18);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // [SerializeField]
    // private GameObject[] objectPrefabs;

    // // Get an object form the pool
    // public GameObject GetObject(string type)
    // {
    //     // Go through the array
    //     for(int i = 0; i < objectPrefabs.Length; i++)
    //     {
    //         // Check if there is an object that has the name of the type
    //         if(objectPrefabs[i].name == type)
    //         {
    //             // Instantiate the object
    //             GameObject newObject = Instantiate(objectPrefabs[i]);
    //             newObject.name = type;
    //             return newObject;
    //         }
    //     }
    //     return null;
    // }

    // //
    // public void ReleaseObject(GameObject object) 
    // {
    //     gameObject.SetActive(false);
    // }

    // Method that returns the correct object pool index given the enemy type
    public static int GetObjectPoolIndex(string type)
    {
        switch(type)
        {
            case "Normal Enemy":
                return EnemyPools.enemyPoolIds[0];
            break;
            case "Fast Enemy":
                return EnemyPools.enemyPoolIds[1];
            break;
            case "Super Fast Enemy":
                return EnemyPools.enemyPoolIds[2];
            break;
            case "Flying Enemy":
                return EnemyPools.enemyPoolIds[3];
            break;
            case "Tank Enemy":
                return EnemyPools.enemyPoolIds[4];
            break;
            case "Slow Enemy":
                return EnemyPools.enemyPoolIds[5];
            break;
            case "Berzerker Enemy":
                return EnemyPools.enemyPoolIds[6];
            break;
            case "Berzerker Flying Enemy":
                return EnemyPools.enemyPoolIds[7];
            break;
            case "Berzerker Tank Enemy":
                return EnemyPools.enemyPoolIds[8];
            break;
        }

        // Case the enemy does not have a correct type
        return EnemyPools.enemyPoolIds[12];
    }

    // The method that releses the enemy game objects
    public static void ReleaseEnemy(Enemy enemy)
    {
        // Get the correclt object pool index from the object pools class
        int objectPoolIndex = GetObjectPoolIndex(enemy.GetEnemyType);

        // Release the enemy into the right object pool
        ObjectPool<Enemy>.ReleaseResource(objectPoolIndex, enemy);

        // Set the enemy as inactive
        enemy.gameObject.SetActive(false);
    }

    // Method that returns the correct object pool index given the tower type
    public static int GetTowerPoolIndex(string type)
    {
        switch(type)
        {
            case "Archer Tower":
                return EnemyPools.enemyPoolIds[13];
            break;
            case "Fire Tower":
                return EnemyPools.enemyPoolIds[14];
            break;
            case "Earth Tower":
                return EnemyPools.enemyPoolIds[15];
            break;
            case "Lightning Tower":
                return EnemyPools.enemyPoolIds[16];
            break;
            case "Wind Tower":
                return EnemyPools.enemyPoolIds[17];
            break;
        }

        // Case the enemy does not have a correct type
        return EnemyPools.enemyPoolIds[12];
    }

    // The method that releses the tower game objects
    public static void ReleaseTower(GameObject tower)
    {
        string towerType = tower.GetComponent<Tower>().GetTowerType;

        // Get the correclt object pool index from the object pools class
        int objectPoolIndex = GetTowerPoolIndex(towerType);

        // Release the enemy into the right object pool
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, tower);

        // Set the enemy as inactive
        tower.gameObject.SetActive(false);
    }

    // Method that returns the correct object pool index given the projectile type
    public static int GetProjectilePoolIndex(string type)
    {
        // Return the right projectile pool index given the projectile type
        switch(type)
        {
            case "Arrow":
                return EnemyPools.enemyPoolIds[9];
            break;
            case "Fire Ball":
                return EnemyPools.enemyPoolIds[10];
            break;
            case "Stone":
                return EnemyPools.enemyPoolIds[11];
            break;
        }

        // In case the name is incorrect return an arrow
        return EnemyPools.enemyPoolIds[9];
    }

    // The method that releses the projectile game objects
    public static void ReleaseProjectile(Projectile projectile)
    {
        string projectileType = projectile.GetComponent<Projectile>().GetProjectileType;

        // Get the correclt object pool index from the object pools class
        int objectPoolIndex = GetProjectilePoolIndex(projectile.GetComponent<Projectile>().GetProjectileType);

        // Release the enemy into the right object pool
        ObjectPool<Projectile>.ReleaseResource(objectPoolIndex, projectile);

        // Set the enemy as inactive
        projectile.gameObject.SetActive(false);
    }
}
