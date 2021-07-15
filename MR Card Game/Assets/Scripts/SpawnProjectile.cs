using i5.Toolkit.Core.Spawners;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnProjectile : MonoBehaviour
    {
        // Define the spawner object that contains the object to spawn
        // public Spawner spawner;

        // Method used to spawn the projectile for the tower
        public static GameObject SpawnProjectileForTower(Spawner spawner)
        {
            // Spawn the projectile
            spawner.Spawn();

            // Set the projectile active
            spawner.MostRecentlySpawnedObject.SetActive(true);

            // Return the projectile game object
            return spawner.MostRecentlySpawnedObject;
        }
    }
}