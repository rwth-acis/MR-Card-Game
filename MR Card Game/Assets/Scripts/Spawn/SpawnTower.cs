using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities;

namespace i5.Toolkit.Core.Examples.Spawners
{
    public class SpawnTower : MonoBehaviour
    {
        // The instance, needed to access the static versions of the tower prefabs
        public static SpawnTower instance;

        // The prefab for the archer tower
        [SerializeField]
        private GameObject archerTower;

        // The prefab for the fire tower
        [SerializeField]
        private GameObject fireTower;

        // The prefab for the earth tower
        [SerializeField]
        private GameObject earthTower;

        // The prefab for the lightning tower
        [SerializeField]
        private GameObject lightningTower;

        // The prefab for the wind tower
        [SerializeField]
        private GameObject windTower;

        // The method used to access to the archer tower prefab as a static object
        public static GameObject ArcherTower
        {
            get { return instance.archerTower; }
        }
        // The method used to access to the fire towerprefab as a static object
        public static GameObject FireTower
        {
            get { return instance.fireTower; }
        }
        // The method used to access to the earth tower prefab as a static object
        public static GameObject EarthTower
        {
            get { return instance.earthTower; }
        }
        // The method used to access to the lightning tower prefab as a static object
        public static GameObject LightningTower
        {
            get { return instance.lightningTower; }
        }
        // The method used to access to the wind tower prefab as a static object
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
            switch (type)
            {
                case TowerType.Archer:
                    return ArcherTower;
                case TowerType.Fire:
                    return FireTower;
                case TowerType.Wind:
                    return WindTower;
                case TowerType.Lightning:
                    return LightningTower;
                case TowerType.Earth:
                    return EarthTower;
                default:
                    return ArcherTower;
            }
        }

        public static GameObject SpawnTowerFromPool(GameObject parent, TowerType type)
        {
            // Get the right object pool index for the tower type
            int poolIndex = ObjectPools.GetTowerPoolIndex(type);

            // Get a new tower from the object pool of the archer tower
            GameObject tower = ObjectPool<GameObject>.RequestResource(poolIndex, () => { return Instantiate(GetTowerPrefabOfType(type)); });

            // Set the tower as active
            tower.SetActive(true);

            // Set them as children of the parent that was passed
            tower.transform.parent = parent.transform;

/*            // Scale the tower down
            tower.transform.localScale = new Vector3(Board.greatestBoardDimension * 3, Board.greatestBoardDimension * 3, Board.greatestBoardDimension * 3) * 0.2f;

            // Vector3 newPosition =  new Vector3(0, towerOverhead * Board.greatestBoardDimension, 0);

            // Reset the position of the tower and add the necessary overhead so that the tower is over the ground
            tower.transform.localPosition = new Vector3(0, towerOverhead * Board.greatestBoardDimension, 0);*/

            // Return the tower object
            return tower;

            // // Add the reference to this building to the Buildings class so that it can be accessed
            // AddBuildingReference(tower, parent);
        }        

        // // The method that adds the buildings reference to the buildings class
        // public static void AddBuildingReference(GameObject tower, GameObject parent)
        // {
        //     // Save the tower game object in the right game object of the buildings glass
        //     switch(Buildings.numberOfBuildings)
        //     {
        //         case 0:
        //             Buildings.firstBuilding = tower;
        //         break;

        //         case 1:
        //             Buildings.secondBuilding = tower;
        //         break;

        //         case 2:
        //             Buildings.thirdBuilding = tower;
        //         break;

        //         case 3:
        //             Buildings.fourthBuilding = tower;
        //         break;

        //         case 4:
        //             Buildings.fifthBuilding = tower;
        //         break;

        //         case 5:
        //             Buildings.sixthBuilding = tower;
        //         break;

        //         case 6:
        //             Buildings.seventhBuilding = tower;
        //         break;

        //         case 7:
        //             Buildings.eighthBuilding = tower;
        //         break;

        //         case 8:
        //             Buildings.ninthBuilding = tower;
        //         break;

        //         case 9:
        //             Buildings.tenthBuilding = tower;
        //         break;
        //     }

        //     Buildings.imageTargetToBuilding[Buildings.numberOfBuildings] = parent;

        //     // Increase the number of buildings by one
        //     Buildings.numberOfBuildings = Buildings.numberOfBuildings + 1;

        //     // // Initialize the array index
        //     // int arrayIndex = 0;
    
        //     // // Add the reference to this building to the right image target
        //     // switch(parent.name)
        //     // {
        //     //     case "ImageTargetTower1":
        //     //         arrayIndex = 1;
        //     //     break;

        //     //     case "ImageTargetTower2":
        //     //         arrayIndex = 2;
        //     //     break;

        //     //     case "ImageTargetTower3":
        //     //         arrayIndex = 3;
        //     //     break;

        //     //     case "ImageTargetTower4":
        //     //         arrayIndex = 4;
        //     //     break;

        //     //     case "ImageTargetTower5":
        //     //         arrayIndex = 5;
        //     //     break;

        //     //     case "ImageTargetTower6":
        //     //         arrayIndex = 6;
        //     //     break;

        //     //     case "ImageTargetTower7":
        //     //         arrayIndex = 7;
        //     //     break;

        //     //     case "ImageTargetTower8":
        //     //         arrayIndex = 8;
        //     //     break;

        //     //     case "ImageTargetTower9":
        //     //         arrayIndex = 9;
        //     //     break;

        //     //     case "ImageTargetTower10":
        //     //         arrayIndex = 10;
        //     //     break;
        //     // }

        //     // // Set the right reference
        //     // Buildings.imageTargetToBuilding[arrayIndex] = Buildings.numberOfBuildings;
        // }
    }
}
