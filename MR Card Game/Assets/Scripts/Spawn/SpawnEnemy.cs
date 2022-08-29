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

        // The prefab for the fast enemy
        [SerializeField]
        private Enemy fastEnemy;

        // The prefab for the super fast enemy
        [SerializeField]
        private Enemy superFastEnemy;

        // The prefab for the flying enemy
        [SerializeField]
        private Enemy flyingEnemy;

        // The prefab for the tank enemy
        [SerializeField]
        private Enemy tankEnemy;

        // The prefab for the slow enemy
        [SerializeField]
        private Enemy slowEnemy;

        // The prefab for the berzerker enemy
        [SerializeField]
        private Enemy berzerkerEnemy;

        // The prefab for the berzerker flying enemy
        [SerializeField]
        private Enemy berzerkerFlyingEnemy;

        // The prefab for the berzerker tank enemy
        [SerializeField]
        private Enemy berzerkerTankEnemy;

        [SerializeField]
        private static Spawner spawner;

        private string mode = "object pool";

        // The method used to access to the normal enemy prefab as a static object
        public static Enemy NormalEnemy
        {
            get { return instance.normalEnemy; }
        }
        // The method used to access to the fast enemy prefab as a static object
        public static Enemy FastEnemy
        {
            get { return instance.fastEnemy; }
        }
        // The method used to access to the super fast enemy prefab as a static object
        public static Enemy SuperFastEnemy
        {
            get { return instance.superFastEnemy; }
        }
        // The method used to access to the flying enemy prefab as a static object
        public static Enemy FlyingEnemy
        {
            get { return instance.flyingEnemy; }
        }
        // The method used to access to the tank enemy prefab as a static object
        public static Enemy TankEnemy
        {
            get { return instance.tankEnemy; }
        }
        // The method used to access to the slow enemy prefab as a static object
        public static Enemy SlowEnemy
        {
            get { return instance.slowEnemy; }
        }
        // The method used to access to the berzerker enemy prefab as a static object
        public static Enemy BerzerkerEnemy
        {
            get { return instance.berzerkerEnemy; }
        }
        // The method used to access to the berzerker flying enemy prefab as a static object
        public static Enemy BerzerkerFlyingEnemy
        {
            get { return instance.berzerkerFlyingEnemy; }
        }
        // The method used to access to the berzerker tank enemy prefab as a static object
        public static Enemy BerzerkerTankEnemy
        {
            get { return instance.berzerkerTankEnemy; }
        }
        // // The parent game object, so the board
        // [SerializeField]
        // private GameObject parentObject;



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
                    Enemy enemy1 = ObjectPool<Enemy>.RequestResource(EnemyPools.enemyPoolIds[9], () => {return Instantiate(NormalEnemy);});

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
        public static Enemy SpawnAnEnemy(EnemyType type)
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

            // // Set them as children of the game board
            // enemy.transform.parent = Waypoints.enemySpawn.transform;

            // Remove the enemy slow
            enemy.enemySlowFactor = 1;

            // // Set the position of the child to the position of the parent object
            // enemy.transform.position = Waypoints.enemySpawn.transform.position;

            // // Rotate the enemies like the game board
            // enemy.transform.rotation = Board.gameBoard.transform.rotation;

            // // Set the position of the child to the position of the parent object
            // enemy.transform.position = Waypoints.enemySpawn.transform.position + enemy.transform.up * enemy.GetFlightHeight;

            // // Set the position of the child to the position of the parent object
            // enemy.transform.position = Waypoints.enemySpawn.transform.position  - enemy.transform.up * enemy.GetFlightHeight;

            enemy.firstLife = enemy.firstLife + 1;

            // Set the health points to max and make it alive again
            enemy.ReviveEnemy();

            // Return the enemy object
            return enemy;
        }

        // Method that returns the right enemy prefab given the enemy tpye
        public static Enemy GetRightEnemyPrefab(EnemyType type)
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