using i5.Toolkit.Core.Spawners;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnProjectile : MonoBehaviour
    {
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
        public static Projectile SpawnProjectileForTower(string type, Projectile projectile)
        {
            // Initialize the pool index for the object pool of the projectile
            int poolId = ObjectPools.GetProjectilePoolIndex("Arrow");

            // Depending on the type of the tower, find the right projectile pool index
            switch(type)
            {
                case "Archer Tower":
                    poolId = ObjectPools.GetProjectilePoolIndex("Arrow");
                break;

                case "Fire Tower":
                    poolId = ObjectPools.GetProjectilePoolIndex("Fire Ball");
                break;

                case "Earth Tower":
                    poolId = ObjectPools.GetProjectilePoolIndex("Stone");
                break;
            }

            // Debug.Log("The current object pool index is: " + poolId);

            // Get a new projectile from the object pool of the projectile type
            Projectile projectileObject = ObjectPool<Projectile>.RequestResource(poolId, () => {return Instantiate(projectile);});

            // Set the tower as active
            projectileObject.gameObject.SetActive(true);

            // // Resize the projectile
            // if(type == "Archer Tower")
            // {
            //     projectileObject.gameObject.transform.localScale = new Vector3(1, 1, 1) * Board.greatestBoardDimension * 5;
            // }

            // Scale the projectile down
            projectileObject.transform.localScale = new Vector3(1, 1, 1);

            // Return the projectile game object
            return projectileObject;
        }

        // // Method that spawns an arrow
        // public static GameObject SpawnArrow(GameObject spawner)
        // {
        //     // Get the right object pool index for the tower type
        //     int poolIndex = ObjectPools.GetTowerPoolIndex("Arrow");

        //     // Get a new tower from the object pool of the archer tower
        //     GameObject projectile = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(anArrow);});

        //     // Set the tower as active
        //     projectile.gameObject.SetActive(true);

        //     // Set them as children of the parent that was passed
        //     projectile.transform.parent = spawner.transform;

        //     // Reset the position of the projectile
        //     projectile.transform.position = new Vector3(0, 0, 0);

        //     // Return the projectile
        //     return projectile;
        // }

        // // Method that spawns a fire ball
        // public static GameObject SpawnFireBall(GameObject spawner)
        // {
        //     // Get the right object pool index for the tower type
        //     int poolIndex = ObjectPools.GetTowerPoolIndex("Fire Ball");

        //     // Get a new tower from the object pool of the archer tower
        //     GameObject projectile = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(aFireBall);});

        //     // Set the tower as active
        //     projectile.gameObject.SetActive(true);

        //     // Set them as children of the parent that was passed
        //     projectile.transform.parent = spawner.transform;

        //     // Reset the position of the projectile
        //     projectile.transform.position = new Vector3(0, 0, 0);

        //     // Return the projectile
        //     return projectile;
        // }

        // // Method that spawns a stone
        // public static GameObject SpawnStone(GameObject spawner)
        // {
        //     // Get the right object pool index for the tower type
        //     int poolIndex = ObjectPools.GetTowerPoolIndex("Stone");

        //     // Get a new tower from the object pool of the archer tower
        //     GameObject projectile = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(aStone);});

        //     // Set the tower as active
        //     projectile.gameObject.SetActive(true);

        //     // Set them as children of the parent that was passed
        //     projectile.transform.parent = spawner.transform;

        //     // Reset the position of the projectile
        //     projectile.transform.position = new Vector3(0, 0, 0);

        //     // Return the projectile
        //     return projectile;
        // }
    }
}