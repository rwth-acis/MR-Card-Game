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

        // Define the canvas of the build tower menu
        [SerializeField]
        private GameObject buildTowerCanvas;

        // Define the build trap window
        [SerializeField]
        private GameObject buildTrapWindow;

        // Define the build tower window
        [SerializeField]
        private GameObject buildTowerWindow;

        // Define the answer questions menu
        [SerializeField]
        private GameObject answerQuestions;

        // Define the currency display button
        [SerializeField]
        private Button currencyDisplay;

        // Define the wave display button
        [SerializeField]
        private Button waveDisplay;

        [SerializeField]
        private Button enemyDisplay;

        [SerializeField]
        private Button toggleGameboard;
        // Define the start next wave button
        [SerializeField]
        private Button startNextWave;
        // Define the build archer tower button
        [SerializeField]
        private Button buildArcherTower;
        // Define the build fire tower button
        [SerializeField]
        private Button buildFireTower;
        // Define the build earth tower button
        [SerializeField]
        private Button buildEarthTower;
        // Define the build Lightning tower button
        [SerializeField]
        private Button buildLightningTower;
        // Define the build wind tower button
        [SerializeField]
        private Button buildWindTower;
        // Define the build wind tower button
        [SerializeField]
        private Button buildHole;
        // Define the build wind tower button
        [SerializeField]
        private Button buildSwamp;

        private bool GameboardLocked;

        // The method used to access to the archer tower cost integer as a static object
        public static int ArcherTowerCost
        {
            get { return instance.archerTowerCost; }
        }
        // The method used to access to the fire tower cost integer as a static object
        public static int FireTowerCost
        {
            get { return instance.fireTowerCost; }
        }
        // The method used to access to the earth tower cost integer as a static object
        public static int EarthTowerCost
        {
            get { return instance.earthTowerCost; }
        }
        // The method used to access to the lightning tower cost integer as a static object
        public static int LightningTowerCost
        {
            get { return instance.lightningTowerCost; }
        }
        // The method used to access to the wind tower cost integer as a static object
        public static int WindTowerCost
        {
            get { return instance.windTowerCost; }
        }
        // The method used to access to the hole cost integer as a static object
        public static int HoleCost
        {
            get { return instance.holeCost; }
        }
        // The method used to access to the swamp cost integer as a static object
        public static int SwampCost
        {
            get { return instance.swampCost; }
        }
        // The method used to access to the build tower canvas as a static object
        public static GameObject BuildTowerCanvas
        {
            get { return instance.buildTowerCanvas; }
        }
        // The method used to access to the build trap window as a static object
        public static GameObject BuildTrapWindow
        {
            get { return instance.buildTrapWindow; }
        }
        // The method used to access to the build tower window as a static object
        public static GameObject BuildTowerWindow
        {
            get { return instance.buildTowerWindow; }
        }
        // The method used to access to the currency display button as a static object
        public static Button CurrencyDisplay
        {
            get { return instance.currencyDisplay; }
        }
        // The method used to access to the wave display button as a static object
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
        // The method used to access to the start next wave button as a static object
        public static Button StartNextWave
        {
            get { return instance.startNextWave; }
        }
        // The method used to access to the build archer tower button as a static object
        public static Button BuildArcherTowerButton
        {
            get { return instance.buildArcherTower; }
        }
        // The method used to access to the build fire tower button as a static object
        public static Button BuildFireTowerButton
        {
            get { return instance.buildFireTower; }
        }
        // The method used to access to the build earth tower button as a static object
        public static Button BuildEarthTowerButton
        {
            get { return instance.buildEarthTower; }
        }
        // The method used to access to the build lightning tower button as a static object
        public static Button BuildLightningTowerButton
        {
            get { return instance.buildLightningTower; }
        }
        // The method used to access to the build wind tower button as a static object
        public static Button BuildWindTowerButton
        {
            get { return instance.buildWindTower; }
        }
        // The method used to access to the build hole button as a static object
        public static Button BuildHoleButton
        {
            get { return instance.buildHole; }
        }
        // The method used to access to the build swamp button as a static object
        public static Button BuildSwampButton
        {
            get { return instance.buildSwamp; }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set the instance to this script
            instance = this;

            buildArcherTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{archerTowerCost}";
            buildEarthTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{earthTowerCost}";
            buildFireTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{fireTowerCost}";
            buildLightningTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{lightningTowerCost}";
            buildWindTower.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{windTowerCost}";
            buildHole.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{holeCost}";
            buildSwamp.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"Cost:{swampCost}";
        }
        // Method that activates the components of the game overlay
        public static void ActivateGameOverlay()
        {
            // Activate the currency display button
            CurrencyDisplay.gameObject.SetActive(true);

            // Activate the wave display button
            WaveDisplay.gameObject.SetActive(true);

            // Check if the wave is currently ongoing
            if(LevelInfo.waveOngoing == false)
            {
                // If it is not the case, activate the start next wave button
                StartNextWave.gameObject.SetActive(true);
            }

            ToogleGameboard.gameObject.SetActive(true);

            EnemyDisplay.gameObject.SetActive(true);
        }

        // Method that deactivates the components of the game overlay
        public static void DeactivateGameOverlay()
        {
            // Deactivate the currency display button
            CurrencyDisplay.gameObject.SetActive(false);

            // Deactivate the wave display button
            WaveDisplay.gameObject.SetActive(false);

            // Deactivate the start next wave button
            StartNextWave.gameObject.SetActive(false);

            ToogleGameboard.gameObject.SetActive(false);

            EnemyDisplay.gameObject.SetActive(false);
        }

        // The method that opens the build tower menu
        public static void OpenBuildTowerMenu()
        {
            Debug.Log("Opening build tower menu");

            // Pause the game
            GameAdvancement.gamePaused = true;

            // Set the canvas as active
            BuildTowerCanvas.SetActive(true);

            // Set the build tower menu as active
            BuildTowerWindow.SetActive(true);

            // Make sure the build trap menu is inactive
            BuildTrapWindow.SetActive(false);

            // Disable the tower buttons that cannot be bought, and enable the tower buttons that can be bought

            // Check if the player has enough coin to build an archer tower
            if(GameAdvancement.currencyPoints >= ArcherTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the archer tower
                BuildArcherTowerButton.interactable = true;

            } else {

                // Disable the archer tower
                BuildArcherTowerButton.interactable = false;
            }

            // Check if the player has enough coin to build a fire tower
            if(GameAdvancement.currencyPoints >= FireTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the fire tower
                BuildFireTowerButton.interactable = true;

            } else {

                // Disable the fire tower
                BuildFireTowerButton.interactable = false;
            }

            // Check if the player has enough coin to build an earth tower
            if(GameAdvancement.currencyPoints >= EarthTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the earth tower
                BuildEarthTowerButton.interactable = true;

            } else {

                // Disable the earth tower
                BuildEarthTowerButton.interactable = false;
            }

            // Check if the player has enough coin to build a lightning tower
            if(GameAdvancement.currencyPoints >= LightningTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the lightning tower
                BuildLightningTowerButton.interactable = true;

            } else {

                // Disable the lightning tower
                BuildLightningTowerButton.interactable = false;
            }

            // Check if the player has enough coin to build a wind tower
            if(GameAdvancement.currencyPoints >= WindTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the wind tower
                BuildWindTowerButton.interactable = true;

            } else {

                // Disable the wind tower
                BuildWindTowerButton.interactable = false;
            }
        }

        // Method used to close the build menu
        public void CloseBuildMenu()
        {
            // Pause the game
            GameAdvancement.gamePaused = false;

            // Make sure the build tower menu is inactive
            buildTowerWindow.SetActive(false);

            // Make sure the build trap menu is inactive
            buildTrapWindow.SetActive(false);

            // Set the canvas as inactive
            buildTowerCanvas.SetActive(false);
        }

        // The method that opens the build tower menu
        public static void OpenBuildTrapMenu()
        {
            // Pause the game
            GameAdvancement.gamePaused = true;

            // Set the canvas as active
            BuildTowerCanvas.SetActive(true);

            // Set the build trap menu as active
            BuildTrapWindow.SetActive(true);

            // Make sure the build tower menu is inactive
            BuildTowerWindow.SetActive(false);

            // Disable the trap buttons that cannot be bought, and enable the trap buttons that can be bought

            // Check if the player has enough coin to build a hole
            if(GameAdvancement.currencyPoints >= HoleCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the hole button
                BuildHoleButton.interactable = true;

            } else {

                // Disable the hole button
                BuildHoleButton.interactable = false;
            }

            // Check if the player has enough coin to build a swamp
            if(GameAdvancement.currencyPoints >= SwampCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the swamp button
                BuildSwampButton.interactable = true;

            } else {

                // Disable the swamp button
                BuildSwampButton.interactable = false;
            }
        }

        // The method that activates when the player wants to build an archer tower by pressing on the archer tower button in the build menu
        public void InitiateArcherTowerBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildArcherTower());
        }

        // The method that activates when the player wants to build a fire tower by pressing on the fire tower button in the build menu
        public void InitiateFireTowerBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildFireTower());
        }

        // The method that activates when the player wants to build an earth tower by pressing on the earth tower button in the build menu
        public void InitiateEarthTowerBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildEarthTower());
        }

        // The method that activates when the player wants to build a lightning tower by pressing on the lightning tower button in the build menu
        public void InitiateLightningTowerBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildLightningTower());
        }

        // The method that activates when the player wants to build a wind tower by pressing on the wind tower button in the build menu
        public void InitiateWindTowerBuild()
        {
            PrepareForBuild();
            StartCoroutine(BuildWindTower());
        }

        // The method that activates when the player wants to build a hole by pressing on the archer tower button in the build menu
        public void InitiateHoleBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildHole());
        }

        // The method that activates when the player wants to build a swamp by pressing on the swamp button in the build menu
        public void InitiateSwampBuild()
        {
            PrepareForBuild();
            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildSwamp());
        }

        private void PrepareForBuild()
        {
            // Set the question requesting image target correctly
            Questions.questionRequestingImageTarget = TowerImageTarget.currentImageTarget;

            // Disable the game overlay
            DeactivateGameOverlay();

            // Enable the answer question menu
            answerQuestions.SetActive(true);

            // Set the number of questions that are needed to answer to 1
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);

            // Close the menu
            // Disable the build canvas
            BuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            BuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            BuildTowerWindow.SetActive(false);
        }

        // Function that is used to test when all questions that were needed to be answered were answered correctly
        private bool NoMoreQuestionsNeeded()
        {
            return Questions.numberOfQuestionsNeededToAnswer == 0;
        }

        // The method that builds an archer tower over the image target
        IEnumerator BuildArcherTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the archer tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Archer);
            // Ground the building
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(ArcherTowerCost);
        }


        // The method that builds a fire tower over the image target
        IEnumerator BuildFireTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the fire tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Fire);
            // Ground the building
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(FireTowerCost);
        }

        // The method that builds a earth tower over the image target
        IEnumerator BuildEarthTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the earth tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Earth);
            // Ground the building
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(EarthTowerCost);
        }

        // The method that builds a lightning tower over the image target
        IEnumerator BuildLightningTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the lightning tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Lightning);
            // Ground the building
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(LightningTowerCost);
        }

        // The method that builds a wind tower over the image target
        IEnumerator BuildWindTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the wind tower or extract it from the object pool
            GameObject tower = SpawnTowerFromPool(TowerImageTarget.currentImageTarget, TowerType.Wind);
            // Ground the building
            GroundBuilding(tower, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(WindTowerCost);
        }

        // The method that builds a hole over the image target
        IEnumerator BuildHole()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the archer tower or extract it from the object pool
            Trap trap = SpawnTrap.SpawnHole(TowerImageTarget.currentImageTarget);
            // Ground the building
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(HoleCost);
        }

        // The method that builds a swamp over the image target
        IEnumerator BuildSwamp()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);
            // Spawn the archer tower or extract it from the object pool
            Trap trap = SpawnTrap.SpawnSwamp(TowerImageTarget.currentImageTarget);
            // Ground the building
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);
            UpdateGameAdvancementAfterBuilding(SwampCost);
        }

        private void UpdateGameAdvancementAfterBuilding(int cost)
        {
            // Enable the game overlay
            ActivateGameOverlay();

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints -= cost;

            // Actualize the currency display
            GameSetup.UpdateCurrencyDisplay();

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt++;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method used to ground buildings
        public void GroundBuilding(GameObject building, GameObject imageTarget)
        {
            // Set the building as child of the buildings storage object that is a child of the game board
            building.transform.parent = Board.buildingStorage.transform;

            // Set the position of the building to the position of the image target
            building.transform.localPosition = TowerEnhancer.buildPosition;

            // Set the rotation of the tower to the same as the rotation of the game board
            building.transform.rotation = Board.gameBoard.transform.rotation;

            building.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            // Make sure the y position of the tower is at 0.1

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