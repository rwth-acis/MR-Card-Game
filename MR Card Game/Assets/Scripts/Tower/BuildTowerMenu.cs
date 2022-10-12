using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static i5.Toolkit.Core.Examples.Spawners.SpawnTower;
namespace build
{
    public class BuildTowerMenu : MonoBehaviour
    {
        // The instance object used to access the static prefabs / objects
        public static BuildTowerMenu instance;

        [Header("Costs")]

        [SerializeField]
        private int archerTowerCost;

        [SerializeField]
        private int fireTowerCost;

        [SerializeField]
        private int earthTowerCost;

        [SerializeField]
        private int lightningTowerCost;

        [SerializeField]
        private int windTowerCost;

        [SerializeField]
        private int holeCost;

        [SerializeField]
        private int swampCost;

        [Header("UI Elements")]

        [SerializeField]
        private GameObject buildTowerCanvas;

        [SerializeField]
        private GameObject buildTrapWindow;

        [SerializeField]
        private GameObject buildTowerWindow;

        [SerializeField]
        private GameObject answerQuestions;

        [SerializeField]
        private Button currencyDisplay;

        [SerializeField]
        private Button waveDisplay;

        [SerializeField]
        private Button enemyDisplay;

        [SerializeField]
        private Button toggleGameboard;

        [SerializeField]
        private Button startNextWave;

        [SerializeField]
        private Button buildArcherTower;

        [SerializeField]
        private Button buildFireTower;

        [SerializeField]
        private Button buildEarthTower;

        [SerializeField]
        private Button buildLightningTower;

        [SerializeField]
        private Button buildWindTower;

        [SerializeField]
        private Button buildHole;

        [SerializeField]
        private Button buildSwamp;

        public static int ArcherTowerCost
        {
            get { return instance.archerTowerCost; }
        }

        public static int FireTowerCost
        {
            get { return instance.fireTowerCost; }
        }

        public static int EarthTowerCost
        {
            get { return instance.earthTowerCost; }
        }

        public static int LightningTowerCost
        {
            get { return instance.lightningTowerCost; }
        }

        public static int WindTowerCost
        {
            get { return instance.windTowerCost; }
        }

        public static int HoleCost
        {
            get { return instance.holeCost; }
        }

        public static int SwampCost
        {
            get { return instance.swampCost; }
        }

        public static GameObject BuildTowerCanvas
        {
            get { return instance.buildTowerCanvas; }
        }

        public static GameObject BuildTrapWindow
        {
            get { return instance.buildTrapWindow; }
        }

        public static GameObject BuildTowerWindow
        {
            get { return instance.buildTowerWindow; }
        }

        public static Button CurrencyDisplay
        {
            get { return instance.currencyDisplay; }
        }

        public static Button WaveDisplay
        {
            get { return instance.waveDisplay; }
        }

        public static Button EnemyDisplay
        {
            get => instance.enemyDisplay;
        }

        public static Button ToogleGameboard
        {
            get { return instance.toggleGameboard; }
        }

        public static Button StartNextWave
        {
            get { return instance.startNextWave; }
        }

        public static Button BuildArcherTowerButton
        {
            get { return instance.buildArcherTower; }
        }

        public static Button BuildFireTowerButton
        {
            get { return instance.buildFireTower; }
        }

        public static Button BuildEarthTowerButton
        {
            get { return instance.buildEarthTower; }
        }

        public static Button BuildLightningTowerButton
        {
            get { return instance.buildLightningTower; }
        }

        public static Button BuildWindTowerButton
        {
            get { return instance.buildWindTower; }
        }

        public static Button BuildHoleButton
        {
            get { return instance.buildHole; }
        }

        public static Button BuildSwampButton
        {
            get { return instance.buildSwamp; }
        }

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            //update cost display
            buildArcherTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{archerTowerCost}";
            buildEarthTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{earthTowerCost}";
            buildFireTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{fireTowerCost}";
            buildLightningTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{lightningTowerCost}";
            buildWindTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{windTowerCost}";
            buildHole.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{holeCost}";
            buildSwamp.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{swampCost}";
        }

        public static void OpenBuildTowerMenu()
        {
            Debug.Log("Opening build tower menu");
            GameAdvancement.gamePaused = true;
            BuildTowerCanvas.SetActive(true);
            BuildTowerWindow.SetActive(true);
            BuildTrapWindow.SetActive(false);

            // Disable the tower buttons that cannot be bought, and enable the tower buttons that can be bought
            // Check if the player has enough money to build an archer tower
            if(GameAdvancement.currencyPoints >= ArcherTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildArcherTowerButton.interactable = true;

            } else {
                BuildArcherTowerButton.interactable = false;
            }

            if(GameAdvancement.currencyPoints >= FireTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildFireTowerButton.interactable = true;

            } else {
                BuildFireTowerButton.interactable = false;
            }

            if(GameAdvancement.currencyPoints >= EarthTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildEarthTowerButton.interactable = true;

            } else {
                BuildEarthTowerButton.interactable = false;
            }

            if(GameAdvancement.currencyPoints >= LightningTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildLightningTowerButton.interactable = true;

            } else {
                BuildLightningTowerButton.interactable = false;
            }

            if(GameAdvancement.currencyPoints >= WindTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildWindTowerButton.interactable = true;

            } else {
                BuildWindTowerButton.interactable = false;
            }
        }

        public void CloseBuildMenu()
        {
            GameAdvancement.gamePaused = false;
            //Set UI inactive
            buildTowerWindow.SetActive(false);
            buildTrapWindow.SetActive(false);
            buildTowerCanvas.SetActive(false);
        }

        public static void OpenBuildTrapMenu()
        {
            GameAdvancement.gamePaused = true;
            BuildTowerCanvas.SetActive(true);
            BuildTrapWindow.SetActive(true);
            BuildTowerWindow.SetActive(false);

            // Disable the trap buttons that cannot be bought, and enable the trap buttons that can be bought
            // Check if the player has enough coin to build a hole
            if(GameAdvancement.currencyPoints >= HoleCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildHoleButton.interactable = true;

            } else {
                BuildHoleButton.interactable = false;
            }

            // Check if the player has enough coin to build a swamp
            if(GameAdvancement.currencyPoints >= SwampCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                BuildSwampButton.interactable = true;

            } else {
                BuildSwampButton.interactable = false;
            }
        }

        /// <summary>
        /// Activates when the player wants to build an archer tower by pressing on the archer tower button in the build menu
        /// </summary>
        public void InitiateArcherTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildArcherTower());
        }

        /// <summary>
        /// Activates when the player wants to build a fire tower by pressing on the fire tower button in the build menu
        /// </summary>
        public void InitiateFireTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildFireTower());
        }

        /// <summary>
        /// Activates when the player wants to build an earth tower by pressing on the earth tower button in the build menu
        /// </summary>
        public void InitiateEarthTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildEarthTower());
        }

        /// <summary>
        /// Activates when the player wants to build a lightning tower by pressing on the lightning tower button in the build menu
        /// </summary>
        public void InitiateLightningTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildLightningTower());
        }

        /// <summary>
        /// Activates when the player wants to build a wind tower by pressing on the wind tower button in the build menu
        /// </summary>
        public void InitiateWindTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildWindTower());
        }

        /// <summary>
        /// Activates when the player wants to build a hole by pressing on the archer tower button in the build menu
        /// </summary>
        public void InitiateHoleBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildHole());
        }

        /// <summary>
        /// Activates when the player wants to build a swamp by pressing on the swamp button in the build menu
        /// </summary>
        public void InitiateSwampBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildSwamp());
        }

        // set required properties and activate and deactivate UIs accordingly
        private void PrepareForBuild()
        {
            Questions.questionRequestingImageTarget = TowerImageTarget.currentImageTarget;
            GameSceneManager.DeactivateGameOverlay();
            answerQuestions.SetActive(true);
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);
            BuildTowerCanvas.SetActive(false);
            BuildTrapWindow.SetActive(false);
            BuildTowerWindow.SetActive(false);
        }

        private IEnumerator BuildArcherTower()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            // Spawn the archer tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Archer);
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(ArcherTowerCost);
        }

        private IEnumerator BuildFireTower()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Fire);
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(FireTowerCost);
        }
        private IEnumerator BuildEarthTower()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Earth);
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(EarthTowerCost);
        }

        private IEnumerator BuildLightningTower()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Lightning);
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(LightningTowerCost);
        }

        private IEnumerator BuildWindTower()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Wind);
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(WindTowerCost);
        }

        private IEnumerator BuildHole()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            Trap trap = SpawnTrap.SpawnTrapFromPool(TrapType.Hole, TowerImageTarget.currentImageTarget);
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(HoleCost);
        }

        private IEnumerator BuildSwamp()
        {
            yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
            Trap trap = SpawnTrap.SpawnTrapFromPool(TrapType.Swamp, TowerImageTarget.currentImageTarget);
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(SwampCost);
        }


        // Update properties and activate or deactivate UIs accordingly
        private void UpdateGameAdvancementAfterBuilding(int cost)
        {
            GameSceneManager.ActivateGameOverlay();
            GameAdvancement.currencyPoints -= cost;
            GameSetup.UpdateCurrencyDisplay();
            GameAdvancement.numberOfBuildingsBuilt++;
            GameAdvancement.gamePaused = false;
        }

        // Ground buildings on the game board
        public void GroundBuilding(GameObject building, GameObject imageTarget)
        {
            building.transform.parent = Board.buildingStorage.transform;
            // Set the position of the building to the position of the image target/building position, the rotation, and scale
            building.transform.localPosition = TowerEnhancer.buildPosition;
            building.transform.rotation = Board.gameBoard.transform.rotation;
            building.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            // Make sure the y position of the tower is at 0.1 for a safety offset
            Vector3 newPosition = building.transform.localPosition;
            if(building.transform.CompareTag("Trap"))
            {
                newPosition.y = -0.07f;
            } else {
                newPosition.y = 0.1f;
            }
            building.transform.localPosition = newPosition;        
        }
    }
}