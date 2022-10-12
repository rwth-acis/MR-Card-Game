using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;
using TMPro;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnEnemy : MonoBehaviour
    {
        public static SpawnEnemy instance;

        [Header("Enemy Prefabs")]
        [SerializeField]
        private Enemy normalEnemy;
        [SerializeField]
        private Enemy fastEnemy;
        [SerializeField]
        private Enemy superFastEnemy;
        [SerializeField]
        private Enemy flyingEnemy;
        [SerializeField]
        private Enemy tankEnemy;
        [SerializeField]
        private Enemy slowEnemy;
        [SerializeField]
        private Enemy berzerkerEnemy;
        [SerializeField]
        private Enemy berzerkerFlyingEnemy;
        [SerializeField]
        private Enemy berzerkerTankEnemy;

        private static Spawner spawner;

        private string mode = "object pool";

        public static Enemy NormalEnemy
        {
            get { return instance.normalEnemy; }
        }

        public static Enemy FastEnemy
        {
            get { return instance.fastEnemy; }
        }

        public static Enemy SuperFastEnemy
        {
            get { return instance.superFastEnemy; }
        }

        public static Enemy FlyingEnemy
        {
            get { return instance.flyingEnemy; }
        }

        public static Enemy TankEnemy
        {
            get { return instance.tankEnemy; }
        }

        public static Enemy SlowEnemy
        {
            get { return instance.slowEnemy; }
        }

        public static Enemy BerzerkerEnemy
        {
            get { return instance.berzerkerEnemy; }
        }

        public static Enemy BerzerkerFlyingEnemy
        {
            get { return instance.berzerkerFlyingEnemy; }
        }

        public static Enemy BerzerkerTankEnemy
        {
            get { return instance.berzerkerTankEnemy; }
        }

        void Start()
        {
            instance = this;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F4))
            {
                if(mode == "spawn")
                {
                    spawner.Spawn();
                    spawner.MostRecentlySpawnedObject.SetActive(true);
                } else {
                    // Get a new enemy from the object pool 1
                    Enemy enemy1 = ObjectPool<Enemy>.RequestResource((int)ObjectPools.PoolIDs[9], () => {return Instantiate(NormalEnemy);});
                    enemy1.gameObject.SetActive(true);
                    enemy1.transform.parent = Waypoints.enemySpawn.transform;
                    enemy1.transform.position = Waypoints.enemySpawn.transform.position;
                    enemy1.Initialize();
                }
            }
        }
        /// <summary>
        /// Spawns an enemy given the enemy type
        /// </summary>
        public static Enemy SpawnEnemyType(EnemyType type)
        {
            int poolIndex = ObjectPools.GetEnemyPoolIndex(type);
            Enemy enemyPrefab = GetEnemyPrefabWithType(type);
            Enemy enemy = ObjectPool<Enemy>.RequestResource(poolIndex, () => {return Instantiate(enemyPrefab);});
            enemy.gameObject.SetActive(true);
            enemy.enemySlowFactor = 1;
            enemy.FirstLife++;
            enemy.Initialize();
            return enemy;
        }

        /// <summary>
        /// Get the enemy prefab given the enemy type
        /// </summary>
        public static Enemy GetEnemyPrefabWithType(EnemyType type)
        {
            // Depending on the type, return the right prefab
            return type switch
            {
                EnemyType.Normal => NormalEnemy,
                EnemyType.Fast => FastEnemy,
                EnemyType.SuperFast => SuperFastEnemy,
                EnemyType.Flying => FlyingEnemy,
                EnemyType.Tank => TankEnemy,
                EnemyType.Slow => SlowEnemy,
                EnemyType.Berzerker => BerzerkerEnemy,
                EnemyType.BerzerkerFlying => BerzerkerFlyingEnemy,
                EnemyType.BerzerkerTank => BerzerkerTankEnemy,
                _ => NormalEnemy,
            };
        }
    }
}