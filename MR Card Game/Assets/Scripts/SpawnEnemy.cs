using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnEnemy : MonoBehaviour
    {
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
        private Spawner spawner;

        [SerializeField]
        private GameObject parentObject;

        private string mode = "object pool";

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
                    Enemy enemy1 = ObjectPool<Enemy>.RequestResource(EnemyPools.enemyPoolIds[9], () => {return Instantiate(normalEnemy);});

                    // Set the enemy as active
                    enemy1.gameObject.SetActive(true);

                    // Set them as children of the game board
                    enemy1.transform.parent = parentObject.transform;

                    // Set the position of the child to the position of the parent object
                    enemy1.transform.position = parentObject.transform.position;

                    // Set the health points to max
                    enemy1.ResetHealthPoints();
                }
            }
        }

        // Method that spawns an enemy given the enemy type
        public void SpawnAnEnemy(string type)
        {
            // Get the right object pool index for the enemy type
            int poolIndex = ObjectPools.GetObjectPoolIndex(type);

            Enemy enemyPrefab = GetRightEnemyPrefab(type);

            // Get a new enemy from the object pool 1
            Enemy enemy = ObjectPool<Enemy>.RequestResource(poolIndex, () => {return Instantiate(enemyPrefab);});

            // Set the enemy as active
            enemy.gameObject.SetActive(true);

            // Set them as children of the game board
            enemy.transform.parent = parentObject.transform;

            // Set the position of the child to the position of the parent object
            enemy.transform.position = parentObject.transform.position;

            // Set the health points to max
            enemy.ResetHealthPoints();
        }

        // Method that returns the right enemy prefab given the enemy tpye
        public Enemy GetRightEnemyPrefab(string type)
        {

            // Depending on the type, return the right prefab
            switch(type)
            {
                case "Normal Enemy":
                    return normalEnemy;
                break;
                case "Fast Enemy":
                    return fastEnemy;
                break;
                case "Super Fast Enemy":
                    return superFastEnemy;
                break;
                case "Flying Enemy":
                    return flyingEnemy;
                break;
                case "Tank Enemy":
                    return tankEnemy;
                break;
                case "Slow Enemy":
                    return slowEnemy;
                break;
                case "Berzerker Enemy":
                    return berzerkerEnemy;
                break;
                case "Berzerker Flying Enemy":
                    return berzerkerFlyingEnemy;
                break;
                case "Berzerker Tank Enemy":
                    return berzerkerTankEnemy;
                break;
            }

            // Just in case return normal enemy if the type was not found
            return normalEnemy;
        }
    }
}