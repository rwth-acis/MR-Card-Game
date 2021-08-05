using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnEnemy : MonoBehaviour
    {
        //
        public static SpawnEnemy instance;

        // The prefab for the normal enemy
        [SerializeField]
        private Enemy normalEnemy;

        // The method used to access to the normal enemy prefab as a static object
        public static Enemy aNormalEnemy
        {
            get { return instance.normalEnemy; }
        }

        // The prefab for the fast enemy
        [SerializeField]
        private Enemy fastEnemy;

        // The method used to access to the fast enemy prefab as a static object
        public static Enemy aFastEnemy
        {
            get { return instance.fastEnemy; }
        }

        // The prefab for the super fast enemy
        [SerializeField]
        private Enemy superFastEnemy;

        // The method used to access to the super fast enemy prefab as a static object
        public static Enemy aSuperFastEnemy
        {
            get { return instance.superFastEnemy; }
        }

        // The prefab for the flying enemy
        [SerializeField]
        private Enemy flyingEnemy;

        // The method used to access to the flying enemy prefab as a static object
        public static Enemy aFlyingEnemy
        {
            get { return instance.flyingEnemy; }
        }

        // The prefab for the tank enemy
        [SerializeField]
        private Enemy tankEnemy;

        // The method used to access to the tank enemy prefab as a static object
        public static Enemy aTankEnemy
        {
            get { return instance.tankEnemy; }
        }
        
        // The prefab for the slow enemy
        [SerializeField]
        private Enemy slowEnemy;

        // The method used to access to the slow enemy prefab as a static object
        public static Enemy aSlowEnemy
        {
            get { return instance.slowEnemy; }
        }

        // The prefab for the berzerker enemy
        [SerializeField]
        private Enemy berzerkerEnemy;

        // The method used to access to the berzerker enemy prefab as a static object
        public static Enemy aBerzerkerEnemy
        {
            get { return instance.berzerkerEnemy; }
        }

        // The prefab for the berzerker flying enemy
        [SerializeField]
        private Enemy berzerkerFlyingEnemy;

        // The method used to access to the berzerker flying enemy prefab as a static object
        public static Enemy aBerzerkerFlyingEnemy
        {
            get { return instance.berzerkerFlyingEnemy; }
        }

        // The prefab for the berzerker tank enemy
        [SerializeField]
        private Enemy berzerkerTankEnemy;

        // The method used to access to the berzerker tank enemy prefab as a static object
        public static Enemy aBerzerkerTankEnemy
        {
            get { return instance.berzerkerTankEnemy; }
        }

        [SerializeField]
        private static Spawner spawner;

        // // The parent game object, so the board
        // [SerializeField]
        // private GameObject parentObject;

        private string mode = "object pool";

        void Start()
        {
            instance = this;
        }

        private void Update()
        {
            // SpawnAnEnemy(enemy);
            if(Input.GetKeyDown(KeyCode.F4))
            {
                // Debug.Log("Getting enemy from object pool");
                // NormalEnemy enemy1 = ObjectPool<NormalEnemy>.RequestResource(() => {return new NormalEnemy();});
                // enemy.gameObject.SetActive(true);
                // Debug.Log("Enemy was taken from object pool and set active");

                if(mode == "spawn")
                {
                    // Spawn the enemy
                    spawner.Spawn();

                    // Set the enemy as active
                    spawner.MostRecentlySpawnedObject.SetActive(true);

                } else {

                    // Get a new enemy from the object pool 1
                    Enemy enemy1 = ObjectPool<Enemy>.RequestResource(EnemyPools.enemyPoolIds[9], () => {return Instantiate(aNormalEnemy);});

                    // Set the enemy as active
                    enemy1.gameObject.SetActive(true);

                    // Set them as children of the game board
                    enemy1.transform.parent = Waypoints.enemySpawn.transform;

                    // Set the position of the child to the position of the parent object
                    enemy1.transform.position = Waypoints.enemySpawn.transform.position;

                    // Set the health points to max
                    enemy1.ReviveEnemy();
                }
            }
        }

        // Method that spawns an enemy given the enemy type
        public static void SpawnAnEnemy(string type)
        {
            // Get the right object pool index for the enemy type
            int poolIndex = ObjectPools.GetObjectPoolIndex(type);

            Enemy enemyPrefab = GetRightEnemyPrefab(type);

            // Get a new enemy from the object pool 1
            Enemy enemy = ObjectPool<Enemy>.RequestResource(poolIndex, () => {return Instantiate(enemyPrefab);});

            // Set the enemy as active
            enemy.gameObject.SetActive(true);

            // Debug.Log("Enemy spawned at spawn at: " + Waypoints.enemySpawn.transform.position.x);
            // Debug.Log("Enemy spawned at spawn at: " + Waypoints.enemySpawn.transform.position.y);
            // Debug.Log("Enemy spawned at spawn at: " + Waypoints.enemySpawn.transform.position.z);

            // Set them as children of the game board
            enemy.transform.parent = Waypoints.enemySpawn.transform;

            // Set the position of the child to the position of the parent object
            enemy.transform.position = Waypoints.enemySpawn.transform.position + new Vector3(0, enemy.GetFlightHeight, 0);

            // Set the health points to max and make it alive again
            enemy.ReviveEnemy();
        }

        // Method that returns the right enemy prefab given the enemy tpye
        public static Enemy GetRightEnemyPrefab(string type)
        {

            // Depending on the type, return the right prefab
            switch(type)
            {
                case "Normal Enemy":
                    return aNormalEnemy;
                break;
                case "Fast Enemy":
                    return aFastEnemy;
                break;
                case "Super Fast Enemy":
                    return aSuperFastEnemy;
                break;
                case "Flying Enemy":
                    return aFlyingEnemy;
                break;
                case "Tank Enemy":
                    return aTankEnemy;
                break;
                case "Slow Enemy":
                    return aSlowEnemy;
                break;
                case "Berzerker Enemy":
                    return aBerzerkerEnemy;
                break;
                case "Berzerker Flying Enemy":
                    return aBerzerkerFlyingEnemy;
                break;
                case "Berzerker Tank Enemy":
                    return aBerzerkerTankEnemy;
                break;
            }

            // Just in case return normal enemy if the type was not found
            return aNormalEnemy;
        }
    }
}