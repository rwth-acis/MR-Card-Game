using i5.Toolkit.Core.Spawners;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnProjectile : MonoBehaviour
    {

        public static float towerDownScaleCounter = 5;
        
        /// <summary>
        /// Spawn the projectile for the tower
        /// </summary>
        public static Projectile SpawnProjectileForTower(TowerType towerType, Projectile projectile, GameObject parent, float size)
        {
            int poolId = ObjectPools.GetProjectilePoolIndex(towerType);
            Projectile projectileObject = ObjectPool<Projectile>.RequestResource(poolId, () => {return Instantiate(projectile);});
            projectileObject.gameObject.SetActive(true);
            projectileObject.transform.parent = parent.transform;
            projectileObject.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            projectileObject.EnemyColliders = new List<Collider>();

            if(towerType == TowerType.Archer)
            {
                projectileObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            } else {
                // Calculate the projectile scale
                float projectileSize = size * towerDownScaleCounter;
                projectileObject.transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);
            }
            Debug.Log("Projectile was just scaled down.");
            return projectileObject;
        }
    }
}