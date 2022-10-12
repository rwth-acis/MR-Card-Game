using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{
    public class SpawnTower : MonoBehaviour
    {


        [Header("Tower Prefabs")]
        [SerializeField]
        private GameObject archerTower;
        [SerializeField]
        private GameObject fireTower;
        [SerializeField]
        private GameObject earthTower;
        [SerializeField]
        private GameObject lightningTower;
        [SerializeField]
        private GameObject windTower;

        /// <summary>
        /// The instance, needed to access the static versions of the tower prefabs
        /// </summary>
        public static SpawnTower instance;
        public static GameObject ArcherTower
        {
            get { return instance.archerTower; }
        }
        public static GameObject FireTower
        {
            get { return instance.fireTower; }
        }
        public static GameObject EarthTower
        {
            get { return instance.earthTower; }
        }
        public static GameObject LightningTower
        {
            get { return instance.lightningTower; }
        }
        public static GameObject WindTower
        {
            get { return instance.windTower; }
        }

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        public static GameObject GetTowerPrefabOfType(TowerType type)
        {
            return type switch
            {
                TowerType.Archer => ArcherTower,
                TowerType.Fire => FireTower,
                TowerType.Wind => WindTower,
                TowerType.Lightning => LightningTower,
                TowerType.Earth => EarthTower,
                _ => ArcherTower,
            };
        }

        /// <summary>
        /// Get a tower from the pool with a parent and the tower type
        /// </summary>
        public static GameObject SpawnTowerFromPool(GameObject parent, TowerType type)
        {
            int poolIndex = ObjectPools.GetTowerPoolIndex(type);
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(GetTowerPrefabOfType(type)); });
            tower.SetActive(true);
            tower.transform.parent = parent.transform;
            return tower;
        }
    }
}