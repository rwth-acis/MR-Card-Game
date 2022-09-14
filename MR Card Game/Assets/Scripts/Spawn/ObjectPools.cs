using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;


public class ObjectPools : MonoBehaviour
{

    private static int numberOfEnemyTypes = 9;
    private static int numberOfProjectileTypes = 3;
    private static int numberOfDumpingEnemies = 1;
    private static int numberOfTowerTypes = 5;
    private static int numberOfSpellTypes = 4;
    private static int numberOfTrapTypes = 2;

    public static ArrayList PoolIDs = new();
    public static ObjectPools Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfEnemyTypes; i++)
        {
            PoolIDs.Add(ObjectPool<Enemy>.CreateNewPool());
        }
        for(int i = 0; i < numberOfProjectileTypes; i++)
        {
            PoolIDs.Add(ObjectPool<Projectile>.CreateNewPool());
        }
        for(int i = 0; i < numberOfDumpingEnemies; i++)
        {
            PoolIDs.Add(ObjectPool<Enemy>.CreateNewPool());
        }
        for(var i = 0; i < numberOfTowerTypes; i++)
        {
            PoolIDs.Add(ObjectPool<GameObject>.CreateNewPool());
        }
        for(int i = 0; i < numberOfSpellTypes; i++)
        {
            PoolIDs.Add(ObjectPool<GameObject>.CreateNewPool());
        }
        for(int i = 0; i < numberOfTrapTypes; i++)
        {
            PoolIDs.Add(ObjectPool<Trap>.CreateNewPool());
        }

/*        // Create all enemy pools
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

        int poolId14 = ObjectPool<GameObject>.CreateNewPool(); // Pool for archer tower
        int poolId15 = ObjectPool<GameObject>.CreateNewPool(); // Pool for fire tower
        int poolId16 = ObjectPool<GameObject>.CreateNewPool(); // Pool for earth tower
        int poolId17 = ObjectPool<GameObject>.CreateNewPool(); // Pool for lightning tower
        int poolId18 = ObjectPool<GameObject>.CreateNewPool(); // Pool for wind tower

        int poolId19 = ObjectPool<GameObject>.CreateNewPool(); // Pool for arrow rain
        int poolId20 = ObjectPool<GameObject>.CreateNewPool(); // Pool for meteor impact
        int poolId21 = ObjectPool<GameObject>.CreateNewPool(); // Pool for lightning strike
        int poolId22 = ObjectPool<GameObject>.CreateNewPool(); // Space Distortion

        int poolId22 = ObjectPool<Trap>.CreateNewPool(); // Pool for hole
        int poolId23 = ObjectPool<Trap>.CreateNewPool(); // Pool for swamp*/

/*        // Store the pool ids so that they are not lost
        EnemyPools.enemyPoolIds = new int[23];
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

        EnemyPools.enemyPoolIds[18] = poolId19;
        EnemyPools.enemyPoolIds[19] = poolId20;
        EnemyPools.enemyPoolIds[20] = poolId21;

        EnemyPools.enemyPoolIds[21] = poolId22;
        EnemyPools.enemyPoolIds[22] = poolId23;*/

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

    public static int GetObjectPoolIndex(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => (int)PoolIDs[0],
            EnemyType.Fast => (int)PoolIDs[1],
            EnemyType.SuperFast => (int)PoolIDs[2],
            EnemyType.Flying => (int)PoolIDs[3],
            EnemyType.Tank => (int)PoolIDs[4],
            EnemyType.Slow => (int)PoolIDs[5],
            EnemyType.Berzerker => (int)PoolIDs[6],
            EnemyType.BerzerkerFlying => (int)PoolIDs[7],
            EnemyType.BerzerkerTank => (int)PoolIDs[8],
            _ => (int)PoolIDs[12],// Case the enemy does not have a correct type
        };
    }

    // The method that releses the enemy game objects
    public static void ReleaseEnemy(Enemy enemy)
    {
        // Get the correclt object pool index from the object pools class
        int objectPoolIndex = GetObjectPoolIndex(enemy.EnemyType);

        // Release the enemy into the right object pool
        ObjectPool<Enemy>.ReleaseResource(objectPoolIndex, enemy);

        // Set the enemy as inactive
        enemy.gameObject.SetActive(false);
    }

    // Method that returns the correct object pool index given the tower type
    public static int GetTowerPoolIndex(TowerType type)
    {
        int towerPoolStartIndex = numberOfEnemyTypes + numberOfProjectileTypes + numberOfDumpingEnemies;
        return type switch
        {
            TowerType.Archer => (int)PoolIDs[towerPoolStartIndex],
            TowerType.Fire => (int)PoolIDs[towerPoolStartIndex + 1],
            TowerType.Earth => (int)PoolIDs[towerPoolStartIndex + 2],
            TowerType.Lightning => (int)PoolIDs[towerPoolStartIndex + 3],
            TowerType.Wind => (int)PoolIDs[towerPoolStartIndex + 4],
            _ => (int)PoolIDs[towerPoolStartIndex],
        };
    }

    // The method that releses the tower game objects
    public static void ReleaseTower(GameObject tower)
    {
        TowerType towerType = tower.GetComponentInChildren<Tower>().TowerType;

        // Get the correct object pool index from the object pools class
        int objectPoolIndex = GetTowerPoolIndex(towerType);

        // Release the enemy into the right object pool
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, tower);

        // Set the enemy as inactive
        tower.gameObject.SetActive(false);
    }

    // Method that returns the correct object pool index given the projectile type
    public static int GetProjectilePoolIndex(TowerType type)
    {
        int projectilePoolStartIndex = numberOfEnemyTypes;
        // Return the right projectile pool index given the projectile type
        switch(type)
        {
            case TowerType.Archer:
                return (int)PoolIDs[projectilePoolStartIndex];
            case TowerType.Fire:
                return (int)PoolIDs[projectilePoolStartIndex + 1];
            case TowerType.Earth:
                return (int)PoolIDs[projectilePoolStartIndex + 2];
        }

        // In case the name is incorrect return an arrow
        return (int)PoolIDs[9];
    }

    // The method that releses the projectile game objects
    public static void ReleaseProjectile(Projectile projectile, Tower parent)
    {
        // Get the correctly object pool index from the object pools class
        int objectPoolIndex = GetProjectilePoolIndex(parent.TowerType);

        // Release the projectile into the right object pool
        ObjectPool<Projectile>.ReleaseResource(objectPoolIndex, projectile);

        // Set the projectile as inactive
        projectile.gameObject.SetActive(false);
    }

    // Method that returns the correct object pool index given the spell type
    public static int GetSpellEffectPoolIndex(SpellType type)
    {
        int spellPoolStartIndex = numberOfEnemyTypes + numberOfProjectileTypes + numberOfDumpingEnemies + numberOfTowerTypes;
        // Return the right spell effect pool index given the spell effect type
        return type switch
        {
            SpellType.ArrowRain => (int)PoolIDs[spellPoolStartIndex],
            SpellType.Meteor => (int)PoolIDs[spellPoolStartIndex + 1],
            SpellType.ThunderStrike => (int)PoolIDs[spellPoolStartIndex + 2],
            SpellType.SpaceDistortion => (int)PoolIDs[spellPoolStartIndex + 3],
            _ => (int)PoolIDs[spellPoolStartIndex],
        };
    }

    // The method that releases the spell game objects
    public static void ReleaseSpellEffect(GameObject spellEffect, SpellType type)
    {
        // Get the correctly object pool index from the object pools class
        int objectPoolIndex = GetSpellEffectPoolIndex(type);

        // Release the spell effect into the right object pool
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, spellEffect);

        // Set the spell effect as inactive
        spellEffect.SetActive(false);
    }

     // Method that returns the correct object pool index given the trap type
    public static int GetTrapPoolIndex(TrapType type)
    {
        int trapPoolStartIndex = numberOfEnemyTypes + numberOfProjectileTypes + numberOfDumpingEnemies + numberOfTowerTypes + numberOfSpellTypes;
        // Return the right trap pool index given the trap type
        return type switch
        {
            TrapType.Hole => (int)PoolIDs[trapPoolStartIndex],
            TrapType.Swamp => (int)PoolIDs[trapPoolStartIndex + 1],
            // In case the name is incorrect return a hole
            _ => (int)PoolIDs[trapPoolStartIndex],
        };
    }

    // The method that releases the trap game objects
    public static void ReleaseTrap(Trap trap)
    {
        // Get the correctly object pool index from the object pools class
        int objectPoolIndex = GetTrapPoolIndex(trap.TrapType);

        // Release the trap into the right object pool
        ObjectPool<Trap>.ReleaseResource(objectPoolIndex, trap);

        // Set the trap as inactive
        trap.gameObject.SetActive(false);
    }
}
