using i5.Toolkit.Core.Spawners;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnProjectile : MonoBehaviour
    {

        public static float towerDownScaleCounter = 5;
        // // The instance, needed to access the static versions of the projectile prefabs
        // public static SpawnProjectile instance;

        // // The prefab for the archer tower
        // [SerializeField]
        // private Projectile projectile;

        // // The method used to access to the archer tower prefab as a static object
        // public static Projectile aProjectile
        // {
        //     get { return instance.projectile; }
        // }

        // void Start()
        // {
        //     instance = this;
        // }

        // // The prefab for the archer tower
        // [SerializeField]
        // private GameObject arrow;

        // // The method used to access to the archer tower prefab as a static object
        // public static GameObject anArrow
        // {
        //     get { return instance.arrow; }
        // }

        // // The prefab for the fire tower
        // [SerializeField]
        // private GameObject fireBall;

        // // The method used to access to the fire towerprefab as a static object
        // public static GameObject aFireBall
        // {
        //     get { return instance.fireBall; }
        // }

        // // The prefab for the earth tower
        // [SerializeField]
        // private GameObject stone;

        // // The method used to access to the earth tower prefab as a static object
        // public static GameObject aStone
        // {
        //     get { return instance.stone; }
        // }

        // // Define the "spawner" object that is the location where the projectile should appear
        // public GameObject spawner;

        // // Method used to spawn the projectile for the tower
        // public static GameObject SpawnProjectileForTower(Spawner spawner)
        // {
        //     // Spawn the projectile
        //     spawner.Spawn();

        //     // Set the projectile active
        //     spawner.MostRecentlySpawnedObject.SetActive(true);

        //     // Scale the projectile down with the board scale
        //     spawner.MostRecentlySpawnedObject.transform.localScale = spawner.MostRecentlySpawnedObject.transform.localScale * Board.greatestBoardDimension;

        //     Debug.Log("Projectile spawned");

        //     // Return the projectile game object
        //     return spawner.MostRecentlySpawnedObject;
        // }

        // // Define the "spawner" object that is the location where the projectile should appear
        // private GameObject spawner;

        // public static GameObject aSpawner
        // {
        //     get { return instance.spawner; }
        // }

        // Method used to spawn the projectile for the tower
        public static Projectile SpawnProjectileForTower(TowerType type, Projectile projectile, GameObject parent, float size)
        {
            // Initialize the pool index for the object pool of the projectile
            int poolId = ObjectPools.GetProjectilePoolIndex(type);

            // // Depending on the type of the tower, find the right projectile pool index
            // switch(type)
            // {
            //     case "Archer Tower":
            //         poolId = ObjectPools.GetProjectilePoolIndex("Arrow");
            //     break;

            //     case "Fire Tower":
            //         poolId = ObjectPools.GetProjectilePoolIndex("Fire Ball");
            //     break;

            //     case "Earth Tower":
            //         poolId = ObjectPools.GetProjectilePoolIndex("Stone");
            //     break;
            // }

            // Get a new projectile from the object pool of the projectile type
            Projectile projectileObject = ObjectPool<Projectile>.RequestResource(poolId, () => {return Instantiate(projectile);});

            // Set the tower as active
            projectileObject.gameObject.SetActive(true);

            // Set the parent of the projectile as the projectile spawner
            projectileObject.transform.parent = parent.transform;

            // Reset the position of the projectile game object
            projectileObject.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            // Reset the colliders list of the projectile
            projectileObject.colliders = new List<Collider>();

            if(type == TowerType.Archer)
            {
                // Scale the projectile correctly
                projectileObject.transform.localScale = new Vector3(1, 1, 1);

            } else {

                // Calculate the projectile scale
                float projectileSize = size * Board.greatestBoardDimension * towerDownScaleCounter;

                // Scale the projectile correctly
                projectileObject.transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);

            }

            Debug.Log("Projectile was just scaled down.");

            // Return the projectile game object
            return projectileObject;
        }
    }
}