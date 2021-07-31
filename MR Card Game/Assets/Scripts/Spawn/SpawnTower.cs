using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{
    public class SpawnTower : MonoBehaviour
    {
        //
        public static SpawnTower instance;

        private static float towerOverhead = (float)0.22;

        // The prefab for the archer tower
        [SerializeField]
        private GameObject archerTower;

        // The method used to access to the archer tower prefab as a static object
        public static GameObject anArcherTower
        {
            get { return instance.archerTower; }
        }

        // The prefab for the fire tower
        [SerializeField]
        private GameObject fireTower;

        // The method used to access to the fire towerprefab as a static object
        public static GameObject aFireTower
        {
            get { return instance.fireTower; }
        }

        // The prefab for the earth tower
        [SerializeField]
        private GameObject earthTower;

        // The method used to access to the earth tower prefab as a static object
        public static GameObject anEarthTower
        {
            get { return instance.earthTower; }
        }

        // The prefab for the lightning tower
        [SerializeField]
        private GameObject lightningTower;

        // The method used to access to the lightning tower prefab as a static object
        public static GameObject aLightningTower
        {
            get { return instance.lightningTower; }
        }

        // The prefab for the wind tower
        [SerializeField]
        private GameObject windTower;

        // The method used to access to the wind tower prefab as a static object
        public static GameObject aWindTower
        {
            get { return instance.windTower; }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Method that spawns an archer tower
        public static void SpawnArcherTower(GameObject parent)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex("Archer Tower");

            // Get a new tower from the object pool of the archer tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(anArcherTower);});

            // Set the tower as active
            tower.gameObject.SetActive(true);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.position = new Vector3(0, towerOverhead, 0);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;
        }

        // Method that spawns a fire tower
        public static void SpawnFireTower(GameObject parent)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex("Fire Tower");

            // Get a new tower from the object pool of the fire tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(aFireTower);});

            // Set the tower as active
            tower.gameObject.SetActive(true);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.position = new Vector3(0, towerOverhead, 0);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;
        }

        // Method that spawns an earth tower
        public static void SpawnEarthTower(GameObject parent)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex("Earth Tower");

            // Get a new tower from the object pool of the earth tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(anEarthTower);});

            // Set the tower as active
            tower.gameObject.SetActive(true);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.position = new Vector3(0, towerOverhead, 0);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;
        }

        // Method that spawns an lightning tower
        public static void SpawnLightningTower(GameObject parent)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex("Lightning Tower");

            // Get a new tower from the object pool of the lightning tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(aLightningTower);});

            // Set the tower as active
            tower.gameObject.SetActive(true);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.position = new Vector3(0, towerOverhead, 0);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;
        }

        // Method that spawns a wind tower
        public static void SpawnWindTower(GameObject parent)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex("Wind Tower");

            // Get a new tower from the object pool of the wind tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => {return Instantiate(aWindTower);});

            // Set the tower as active
            tower.gameObject.SetActive(true);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.position = new Vector3(0, towerOverhead, 0);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;
        }
    }
}
