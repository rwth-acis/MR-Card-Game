using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static i5.Toolkit.Core.Examples.Spawners.SpawnTower;
namespace build
{
    public class BuildTowerMenu : MonoBehaviour
    {
        // The instance object used to access the static prefabs / objects
        public static BuildTowerMenu instance;

        [SerializeField]
        private int archerTowerCost;

        // The method used to access to the archer tower cost integer as a static object
        public static int getArcherTowerCost
        {
            get { return instance.archerTowerCost; }
        }

        [SerializeField]
        private int fireTowerCost;

        // The method used to access to the fire tower cost integer as a static object
        public static int getFireTowerCost
        {
            get { return instance.fireTowerCost; }
        }

        [SerializeField]
        private int earthTowerCost;

        // The method used to access to the earth tower cost integer as a static object
        public static int getEarthTowerCost
        {
            get { return instance.earthTowerCost; }
        }

        [SerializeField]
        private int lightningTowerCost;

        // The method used to access to the lightning tower cost integer as a static object
        public static int getLightningTowerCost
        {
            get { return instance.lightningTowerCost; }
        }

        [SerializeField]
        private int windTowerCost;

        // The method used to access to the wind tower cost integer as a static object
        public static int getWindTowerCost
        {
            get { return instance.windTowerCost; }
        }

        [SerializeField]
        private int holeCost;

        // The method used to access to the hole cost integer as a static object
        public static int getHoleCost
        {
            get { return instance.holeCost; }
        }

        [SerializeField]
        private int swampCost;

        // The method used to access to the swamp cost integer as a static object
        public static int getSwampCost
        {
            get { return instance.swampCost; }
        }

        // Define the canvas of the build tower menu
        [SerializeField]
        private GameObject buildTowerCanvas;

        // The method used to access to the build tower canvas as a static object
        public static GameObject getBuildTowerCanvas
        {
            get { return instance.buildTowerCanvas; }
        }

        // Define the build trap window
        [SerializeField]
        private GameObject buildTrapWindow;

        // The method used to access to the build trap window as a static object
        public static GameObject getBuildTrapWindow
        {
            get { return instance.buildTrapWindow; }
        }

        // Define the build tower window
        [SerializeField]
        private GameObject buildTowerWindow;

        // The method used to access to the build tower window as a static object
        public static GameObject getBuildTowerWindow
        {
            get { return instance.buildTowerWindow; }
        }

        // Define the answer questions menu
        [SerializeField]
        private GameObject answerQuestions;

        // Define the currency display button
        [SerializeField]
        private Button currencyDisplay;

        // The method used to access to the currency display button as a static object
        public static Button getCurrencyDisplay
        {
            get { return instance.currencyDisplay; }
        }

        // Define the wave display button
        [SerializeField]
        private Button waveDisplay;

        // The method used to access to the wave display button as a static object
        public static Button getWaveDisplay
        {
            get { return instance.waveDisplay; }
        }

        // Define the start next wave button
        [SerializeField]
        private Button startNextWave;

        // The method used to access to the start next wave button as a static object
        public static Button getStartNextWave
        {
            get { return instance.startNextWave; }
        }

        // Define the build archer tower button
        [SerializeField]
        private Button buildArcherTower;

        // The method used to access to the build archer tower button as a static object
        public static Button getBuildArcherTower
        {
            get { return instance.buildArcherTower; }
        }

        // Define the build fire tower button
        [SerializeField]
        private Button buildFireTower;

        // The method used to access to the build fire tower button as a static object
        public static Button getBuildFireTower
        {
            get { return instance.buildFireTower; }
        }

        // Define the build earth tower button
        [SerializeField]
        private Button buildEarthTower;

        // The method used to access to the build earth tower button as a static object
        public static Button getBuildEarthTower
        {
            get { return instance.buildEarthTower; }
        }

        // Define the build Lightning tower button
        [SerializeField]
        private Button buildLightningTower;

        // The method used to access to the build lightning tower button as a static object
        public static Button getBuildLightningTower
        {
            get { return instance.buildLightningTower; }
        }

        // Define the build wind tower button
        [SerializeField]
        private Button buildWindTower;

        // The method used to access to the build wind tower button as a static object
        public static Button getBuildWindTower
        {
            get { return instance.buildWindTower; }
        }

        // Define the build wind tower button
        [SerializeField]
        private Button buildHole;

        // The method used to access to the build hole button as a static object
        public static Button getBuildHole
        {
            get { return instance.buildHole; }
        }

        // Define the build wind tower button
        [SerializeField]
        private Button buildSwamp;

        // The method used to access to the build swamp button as a static object
        public static Button getBuildSwamp
        {
            get { return instance.buildSwamp; }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set the instance to this script
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Method that activates the components of the game overlay
        public static void ActivateGameOverlay()
        {
            // Activate the currency display button
            getCurrencyDisplay.gameObject.SetActive(true);

            // Activate the wave display button
            getWaveDisplay.gameObject.SetActive(true);

            // Check if the wave is currently ongoing
            if(LevelInfo.waveOngoing == false)
            {
                // If it is not the case, activate the start next wave button
                getStartNextWave.gameObject.SetActive(true);
            }
        }

        // Method that deactivates the components of the game overlay
        public static void DeactivateGameOverlay()
        {
            // Deactivate the currency display button
            getCurrencyDisplay.gameObject.SetActive(false);

            // Deactivate the wave display button
            getWaveDisplay.gameObject.SetActive(false);

            // Deactivate the start next wave button
            getStartNextWave.gameObject.SetActive(false);
        }

        // The method that opens the build tower menu
        public static void OpenBuildTowerMenu()
        {
            Debug.Log("Opening build tower menu");

            // Pause the game
            GameAdvancement.gamePaused = true;

            // Set the canvas as active
            getBuildTowerCanvas.SetActive(true);

            // Set the build tower menu as active
            getBuildTowerWindow.SetActive(true);

            // Make sure the build trap menu is inactive
            getBuildTrapWindow.SetActive(false);

            // Disable the tower buttons that cannot be bought, and enable the tower buttons that can be bought

            // Check if the player has enough coin to build an archer tower
            if(GameAdvancement.currencyPoints >= getArcherTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the archer tower
                getBuildArcherTower.interactable = true;

            } else {

                // Disable the archer tower
                getBuildArcherTower.interactable = false;
            }

            // Check if the player has enough coin to build a fire tower
            if(GameAdvancement.currencyPoints >= getFireTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the fire tower
                getBuildFireTower.interactable = true;

            } else {

                // Disable the fire tower
                getBuildFireTower.interactable = false;
            }

            // Check if the player has enough coin to build an earth tower
            if(GameAdvancement.currencyPoints >= getEarthTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the earth tower
                getBuildEarthTower.interactable = true;

            } else {

                // Disable the earth tower
                getBuildEarthTower.interactable = false;
            }

            // Check if the player has enough coin to build a lightning tower
            if(GameAdvancement.currencyPoints >= getLightningTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the lightning tower
                getBuildLightningTower.interactable = true;

            } else {

                // Disable the lightning tower
                getBuildLightningTower.interactable = false;
            }

            // Check if the player has enough coin to build a wind tower
            if(GameAdvancement.currencyPoints >= getWindTowerCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the wind tower
                getBuildWindTower.interactable = true;

            } else {

                // Disable the wind tower
                getBuildWindTower.interactable = false;
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
            getBuildTowerCanvas.SetActive(true);

            // Set the build trap menu as active
            getBuildTrapWindow.SetActive(true);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Disable the trap buttons that cannot be bought, and enable the trap buttons that can be bought

            // Check if the player has enough coin to build a hole
            if(GameAdvancement.currencyPoints >= getHoleCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the hole button
                getBuildHole.interactable = true;

            } else {

                // Disable the hole button
                getBuildHole.interactable = false;
            }

            // Check if the player has enough coin to build a swamp
            if(GameAdvancement.currencyPoints >= getSwampCost && GameAdvancement.numberOfBuildingsBuilt < GameAdvancement.maxNumberOfBuildings)
            {
                // Enable the swamp button
                getBuildSwamp.interactable = true;

            } else {

                // Disable the swamp button
                getBuildSwamp.interactable = false;
            }
        }

        // The method that activates when the player wants to build an archer tower by pressing on the archer tower button in the build menu
        public void InitiateArcherTowerBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildArcherTower());
        }

        // The method that activates when the player wants to build a fire tower by pressing on the fire tower button in the build menu
        public void InitiateFireTowerBuild()
        {
            // Disable the game overlay
            DeactivateGameOverlay();

            // Enable the answer question menu
            answerQuestions.SetActive(true);

            // Set the number of questions that are needed to answer to 1
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);

            // Close the menu
            // Disable the build canvas
            getBuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildFireTower());
        }

        // The method that activates when the player wants to build an earth tower by pressing on the earth tower button in the build menu
        public void InitiateEarthTowerBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildEarthTower());
        }

        // The method that activates when the player wants to build a lightning tower by pressing on the lightning tower button in the build menu
        public void InitiateLightningTowerBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildLightningTower());
        }

        // The method that activates when the player wants to build a wind tower by pressing on the wind tower button in the build menu
        public void InitiateWindTowerBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Set the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            StartCoroutine(BuildWindTower());
        }

        // The method that activates when the player wants to build a hole by pressing on the archer tower button in the build menu
        public void InitiateHoleBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Make sure the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildHole());
        }

        // The method that activates when the player wants to build a swamp by pressing on the swamp button in the build menu
        public void InitiateSwampBuild()
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
            getBuildTowerCanvas.SetActive(false);

            // Make sure the build trap menu as inactive
            getBuildTrapWindow.SetActive(false);

            // Make sure the build tower menu is inactive
            getBuildTowerWindow.SetActive(false);

            // Start the routine that waits for the questions to be answered
            StartCoroutine(BuildSwamp());
        }

        // Function that is used to test when all questions that were needed to be answered were answered correctly
        private bool NoMoreQuestionsNeeded()
        {
            return Questions.numberOfQuestionsNeededToAnswer == 0;
        }

        // // Function that is used to test when the game overlay is enabled correctly
        // private bool GameOverlayEnabled()
        // {
        //     return gameOverlay.activeSelf == true;
        // }

        // The method that builds an archer tower over the image target
        IEnumerator BuildArcherTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the archer tower or extract it from the object pool
            GameObject tower = SpawnArcherTower(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getArcherTowerCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(tower.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }


        // The method that builds a fire tower over the image target
        IEnumerator BuildFireTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the fire tower or extract it from the object pool
            GameObject tower = SpawnFireTower(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getFireTowerCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(tower.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;
            
            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method that builds a earth tower over the image target
        IEnumerator BuildEarthTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the earth tower or extract it from the object pool
            GameObject tower = SpawnEarthTower(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getEarthTowerCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(tower.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method that builds a lightning tower over the image target
        IEnumerator BuildLightningTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the lightning tower or extract it from the object pool
            GameObject tower = SpawnLightningTower(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getLightningTowerCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(tower.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method that builds a wind tower over the image target
        IEnumerator BuildWindTower()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the wind tower or extract it from the object pool
            GameObject tower = SpawnWindTower(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getWindTowerCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(tower.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method that builds a hole over the image target
        IEnumerator BuildHole()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the archer tower or extract it from the object pool
            Trap trap = SpawnTrap.SpawnHole(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getHoleCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method that builds a swamp over the image target
        IEnumerator BuildSwamp()
        {
            // Wait until the number of questions that need to be answered is 0
            yield return new WaitUntil(NoMoreQuestionsNeeded);

            // Enable the game overlay
            ActivateGameOverlay();

            // // Wait until the game overlay is activated
            // yield return new WaitUntil(GameOverlayEnabled);

            // Spawn the archer tower or extract it from the object pool
            Trap trap = SpawnTrap.SpawnSwamp(TowerImageTarget.currentImageTarget);

            // Reduce the current currency by the cost of the tower
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - getSwampCost;

            // Actualize the currency display
            GameSetup.ActualizeCurrencyDisplay();

            // // Set the flag that this image target was used to build a tower on it
            // TowerImageTarget.currentImageTarget.GetComponent<BuildTower>().towerBuiltCorrectly = true;

            // Ground the building
            GroundBuilding(trap.gameObject, TowerImageTarget.currentImageTarget);

            // Increase the number of buildings built by one
            GameAdvancement.numberOfBuildingsBuilt = GameAdvancement.numberOfBuildingsBuilt + 1;

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // The method used to ground buildings
        public void GroundBuilding(GameObject building, GameObject imageTarget)
        {
            // Set the position of the building to the position of the image target
            building.transform.position = imageTarget.transform.position;

            // Set the building as child of the buildings storage object that is a child of the game board
            building.transform.parent = Board.buildingStorage.transform;

            // Initialize the build position
            Vector3 buildingPosition = building.transform.position;

            // // Set the building position at the same height as the castle (so as the game board)
            // buildingPosition = new Vector3(buildingPosition.x, Board.castle.transform.position.y, buildingPosition.z);

            // Set the building position at the same height as the first waypoint (on the ground of the game board)
            buildingPosition = new Vector3(buildingPosition.x, Waypoints.mapWaypoints[0].position.y, buildingPosition.z);

            // Give this position to the building
            building.transform.position = buildingPosition;

            // // Scale the trap down
            // building.transform.localScale = new Vector3(1, 1, 1);

            // // Make sure the tower is on the same height as the castle
            // building.transform.position.y = Board.castle.transform.position.y;
        }
    }
}