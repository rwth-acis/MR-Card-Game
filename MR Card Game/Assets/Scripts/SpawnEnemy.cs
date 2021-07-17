using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField]
        private NormalEnemy enemy;

        public Spawner spawner;

        private void Update()
        {
            // SpawnAnEnemy(enemy);
            if(Input.GetKeyDown(KeyCode.F4))
            {
                // Debug.Log("Getting enemy from object pool");
                // NormalEnemy enemy1 = ObjectPool<NormalEnemy>.RequestResource(() => {return new NormalEnemy();});
                // enemy.gameObject.SetActive(true);
                // Debug.Log("Enemy was taken from object pool and set active");
                spawner.Spawn();
                spawner.MostRecentlySpawnedObject.SetActive(true);
            }
        }

        // private 
    }
}