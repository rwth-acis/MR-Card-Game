using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField]
        private Enemy enemy;

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
                    Enemy enemy1 = ObjectPool<Enemy>.RequestResource(EnemyPools.enemyPoolIds[9], () => {return Instantiate(enemy);});

                    // Set the enemy as active
                    enemy1.gameObject.SetActive(true);

                    // Set them as children of the game board
                    enemy1.transform.parent = parentObject.transform;

                    // Set the position of the child to the position of the parent object
                    enemy1.transform.position = parentObject.transform.position;

                    // // Scale them down
                    // enemy1.transform.localScale = new Vector3((float)0.2 * enemy1.size, (float)0.1 * enemy1.size, (float)0.01 * enemy1.size);

                }
            }
        }
    }
}