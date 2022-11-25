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

    public static ArrayList PoolIDs;
    public static ObjectPools Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PoolIDs = new ArrayList();
        // Create Pools
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
            PoolIDs.Add(ObjectPool<GameObject>.CreateNewPool());
        }
    }

    /// <summary>
    /// Get the correct object pool index given the enemy type
    /// </summary>
    /// <param name="type">enemy pool ID</param>

    public static int GetEnemyPoolIndex(EnemyType type)
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
            _ => (int)PoolIDs[12],
        };
    }

    /// <summary>
    /// Releses the enemy game objects to the pool
    /// </summary>
    public static void ReleaseEnemy(Enemy enemy)
    {
        int objectPoolIndex = GetEnemyPoolIndex(enemy.EnemyType);
        ObjectPool<Enemy>.ReleaseResource(objectPoolIndex, enemy);
        enemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get the correct object pool index given the tower type
    /// </summary>
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

    /// <summary>
    /// Releses the tower game objects to the pool
    /// </summary>
    public static void ReleaseTower(GameObject tower)
    {
        TowerType towerType = tower.GetComponentInChildren<Tower>().TowerType;
        int objectPoolIndex = GetTowerPoolIndex(towerType);
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, tower);
        tower.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get the correct object pool index given the Tower type
    /// </summary>
    /// <param name="type">projectile pool ID</param>
    public static int GetProjectilePoolIndex(TowerType type)
    {
        int projectilePoolStartIndex = numberOfEnemyTypes;
        switch(type)
        {
            case TowerType.Archer:
                return (int)PoolIDs[projectilePoolStartIndex];
            case TowerType.Fire:
                return (int)PoolIDs[projectilePoolStartIndex + 1];
            case TowerType.Earth:
                return (int)PoolIDs[projectilePoolStartIndex + 2];
            default:
                return (int)PoolIDs[9];
        }
    }

    /// <summary>
    /// Releses the projectile game object to the pool given the parent tower and the projectile
    /// </summary>
    public static void ReleaseProjectile(Projectile projectile, Tower parent)
    {
        int objectPoolIndex = GetProjectilePoolIndex(parent.TowerType);
        ObjectPool<Projectile>.ReleaseResource(objectPoolIndex, projectile);
        projectile.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns the correct object pool index given the spell type
    /// </summary>
    /// <param name="type">spell effect pool ID</param>
    /// <returns></returns>
    public static int GetSpellEffectPoolIndex(SpellType type)
    {
        int spellPoolStartIndex = numberOfEnemyTypes + numberOfProjectileTypes + numberOfDumpingEnemies + numberOfTowerTypes;
        return type switch
        {
            SpellType.ArrowRain => (int)PoolIDs[spellPoolStartIndex],
            SpellType.Meteor => (int)PoolIDs[spellPoolStartIndex + 1],
            SpellType.ThunderStrike => (int)PoolIDs[spellPoolStartIndex + 2],
            SpellType.SpaceDistortion => (int)PoolIDs[spellPoolStartIndex + 3],
            _ => (int)PoolIDs[spellPoolStartIndex],
        };
    }

    /// <summary>
    /// Releases the spell game objects to the pool given the spell effect object and the spell type
    /// </summary>
    public static void ReleaseSpellEffect(GameObject spellEffect, SpellType type)
    {
        int objectPoolIndex = GetSpellEffectPoolIndex(type);
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, spellEffect);
        spellEffect.SetActive(false);
    }

     /// <summary>
     /// Returns the correct object pool index given the trap type
     /// </summary>
    public static int GetTrapPoolIndex(TrapType type)
    {
        int trapPoolStartIndex = numberOfEnemyTypes + numberOfProjectileTypes + numberOfDumpingEnemies + numberOfTowerTypes + numberOfSpellTypes;
        return type switch
        {
            TrapType.Hole => (int)PoolIDs[trapPoolStartIndex],
            TrapType.Swamp => (int)PoolIDs[trapPoolStartIndex + 1],
            // In case the name is incorrect return a hole
            _ => (int)PoolIDs[trapPoolStartIndex],
        };
    }

    /// <summary>
    /// Releases the trap game objects
    /// </summary>
    public static void ReleaseTrap(Trap trap)
    {
        int objectPoolIndex = GetTrapPoolIndex(trap.TrapType);
        ObjectPool<GameObject>.ReleaseResource(objectPoolIndex, trap.gameObject);
        trap.gameObject.SetActive(false);
    }
}
