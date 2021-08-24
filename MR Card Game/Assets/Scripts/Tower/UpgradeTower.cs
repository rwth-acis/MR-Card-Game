using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTower : MonoBehaviour
{
    // The instance of this script
    public static UpgradeTower instance;

    [SerializeField]
    private Tower archerTowerPrefab;

    [SerializeField]
    private Tower fireTowerPrefab;

    [SerializeField]
    private Tower earthTowerPrefab;

    [SerializeField]
    private Tower lightningTowerPrefab;

    [SerializeField]
    private Tower windTowerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //--------------------------------------------------------------------------------------
    // Deleting towers
    //--------------------------------------------------------------------------------------

    // Method used to delete towers
    public void DeleteTower()
    {
        // // Close the upgrade tower menu
        // upgradeTowerMenu.SetActive(false);
        CloseUpgradeTowerMenu();

        // Initialize the tower prefab
        Tower towerprefab = archerTowerPrefab;

        // Get the right prefab for this tower and refund a part of the price
        switch(TowerEnhancer.currentlyEnhancedTower.getTowerType)
        {
            case "Archer Tower":
                towerprefab = archerTowerPrefab;
                GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getArcherTowerCost;
            break;

            case "Fire Tower":
                towerprefab = fireTowerPrefab;
                GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getFireTowerCost;
            break;

            case "Earth Tower":
                towerprefab = earthTowerPrefab;
                GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getEarthTowerCost;
            break;

            case "Lightning Tower":
                towerprefab = lightningTowerPrefab;
                GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getLightningTowerCost;
            break;

            case "Wind Tower":
                towerprefab = windTowerPrefab;
                GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getWindTowerCost;
            break;
        }

        // Reset the tower statistics
        TowerEnhancer.currentlyEnhancedTower.ResetTowerStatistics(towerprefab);

        // Release the tower
        ObjectPools.ReleaseTower(TowerEnhancer.currentlyEnhancedTower.transform.parent.gameObject);

        // Actualize the currency display
        GameSetup.ActualizeCurrencyDisplay();
    }

    // Define the delete trap menu
    [SerializeField]
    private GameObject deleteTrapMenu;

    // The method used to access to the delete trap menu as a static object
    public static GameObject getDeleteTrapMenu
    {
        get { return instance.deleteTrapMenu; }
    }

    // Method used to delete traps
    public void DeleteTrap()
    {
        // Close the upgrade tower menu
        CloseDeleteTrapMenu();

        // Release the trap
        ObjectPools.ReleaseTrap(TrapDeleter.currentlyOpenedTrapWindow);

        // Check the type of the trap and refund the correct amount of currency
        if(TrapDeleter.currentlyOpenedTrapWindow.getTrapType == "Hole")
        {
            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getHoleCost;

        } else {

            GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + build.BuildTowerMenu.getSwampCost;
        }

        // Actualize the currency display
        GameSetup.ActualizeCurrencyDisplay();
    }

    // Method used to open the delete trap menu
    public static void OpenDeleteTrapMenu(Trap trap)
    {
        // Set the right game object as the one that opened the menu
        TrapDeleter.currentlyOpenedTrapWindow = trap;

        // Set the upgrade tower menu as active
        getDeleteTrapMenu.SetActive(true);

        // Pause the game
        GameAdvancement.gamePaused = true;
    }

    // Method used to close the delete trap menu
    public void CloseDeleteTrapMenu()
    {
        // Set the upgrade tower menu as inactive
        deleteTrapMenu.SetActive(false);

        // Un-pause the game
        GameAdvancement.gamePaused = false;
    }

    //--------------------------------------------------------------------------------------
    // Upgrading towers
    //--------------------------------------------------------------------------------------
    
    // Define the upgrade tower menu
    [SerializeField]
    private GameObject upgradeTowerMenu;

    // The method used to access to the upgrade tower menu as a static object
    public static GameObject getUpgradeTowerMenu
    {
        get { return instance.upgradeTowerMenu; }
    }

    // Define all the text fields of the build tower menu
    [SerializeField]
    private TMP_Text towerTypeField;

    // The method used to access to the tower type field as a static object
    public static TMP_Text getTowerTypeField
    {
        get { return instance.towerTypeField; }
    }

    [SerializeField]
    private TMP_Text towerDamageField;

    // The method used to access to the tower damage field as a static object
    public static TMP_Text getTowerDamageField
    {
        get { return instance.towerDamageField; }
    }

    [SerializeField]
    private TMP_Text towerAttackCooldownField;

    // The method used to access to the tower attack cooldown field as a static object
    public static TMP_Text getTowerAttackCooldownField
    {
        get { return instance.towerAttackCooldownField; }
    }

    [SerializeField]
    private TMP_Text towerRangeField;

    // The method used to access to the tower range field as a static object
    public static TMP_Text getTowerRangeField
    {
        get { return instance.towerRangeField; }
    }

    [SerializeField]
    private TMP_Text additionalField1;

    // The method used to access to the additional field 1 as a static object
    public static TMP_Text getAdditionalField1
    {
        get { return instance.additionalField1; }
    }

    [SerializeField]
    private TMP_Text additionalField2;

    // The method used to access to the additional field 2 as a static object
    public static TMP_Text getAdditionalField2
    {
        get { return instance.additionalField2; }
    }

    [SerializeField]
    private TMP_Text upgradeCostField;

    // The method used to access to the upgrade cost field as a static object
    public static TMP_Text getUpgradeCostField
    {
        get { return instance.upgradeCostField; }
    }

    // The upgrade tower button
    [SerializeField]
    private Button upgradeTowerButton;

    // The method used to access to the upgrade tower button as a static object
    public static Button getUpgradeTowerButton
    {
        get { return instance.upgradeTowerButton; }
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

    // The level up multiplicators
    private static float lightningDamageEnhancer = 1.1f;
    private static float lightningRangeEnhancer = 1.2f;
    private static float lightningJumpRangeEnhancer = 1.4f;
    private static float earthDamageEnhancer = 1.6f;
    private static float earthSizeEnhancer = 1.2f;
    private static float windAttackCooldownEnhancer = 0.9f;
    private static float windDropBackEnhancer = 1.2f;
    private static float arrowDamageEnhancer = 1.2f;
    private static float arrowAttackCooldownEnhancer = 0.85f;
    private static float arrowRangeEnhancer = 1.1f;
    private static float fireDamageEnhancer = 1.4f;
    private static float fireAttackCooldownEnhancer = 0.85f;

    // The basic cost for tower upgrade
    private static float archerTowerUpgradeBaseCost = 70f;
    private static float fireTowerUpgradeBaseCost = 90f;
    private static float earthTowerUpgradeBaseCost = 120f;
    private static float lightningTowerUpgradeBaseCost = 105f;
    private static float windTowerUpgradeBaseCost = 65f;

    // The upgrade cost multiplicator for upgrading towers
    private static float archerTowerUpgradeCostMultiplicator = 1.5f;
    private static float fireTowerUpgradeCostMultiplicator = 1.7f;
    private static float earthTowerUpgradeCostMultiplicator = 2f;
    private static float lightningTowerUpgradeCostMultiplicator = 1.85f;
    private static float windTowerUpgradeCostMultiplicator = 1.4f;

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


    // Method used to open the upgrade menu
    public static void OpenUpgradeTowerMenu(Tower tower)
    {
        // Check that nothing is beeing build or upgraded
        if(GameAdvancement.gamePaused == false)
        {
            // // Set the flag that states that the player is currently building or upgrading something
            // GameAdvancement.currentlyBuildingOrUpgrading = true;

            // Save the tower that opened the upgrade menu
            TowerEnhancer.currentlyEnhancedTower = tower;

            // Pause the game
            GameAdvancement.gamePaused = true;

            // Set the upgrade tower menu as active
            getUpgradeTowerMenu.SetActive(true);

            // Write the right tower type as heading
            getTowerTypeField.text = tower.getTowerType;

            // Initialize the upgrade cost integer
            int upgradeCost = 0;

            // Check the tower type and fill the stats window accordingly
            switch(tower.getTowerType)
            {
                case "Lightning Tower":
                    // Enable the two additional fields for the lightning tower
                    getAdditionalField1.gameObject.SetActive(true);
                    getAdditionalField2.gameObject.SetActive(true);

                    // Actualize the standard fields
                    getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage * lightningDamageEnhancer);
                    getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown;
                    getTowerRangeField.text = "Range " + tower.getAttackRange + " > " + (tower.getDamage * lightningRangeEnhancer);

                    // Fill the aditional fields
                    getAdditionalField1.text = "Number of jumps: " + tower.getNumberOfEffect + " > " + (tower.getNumberOfEffect + 1);
                    getAdditionalField2.text = "Jump range: " + tower.getEffectRange + " > " + (tower.getEffectRange * lightningJumpRangeEnhancer);
                    
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(lightningTowerUpgradeBaseCost * Mathf.Pow(lightningTowerUpgradeCostMultiplicator, tower.getLevel - 1));

                    // Write this cost in the upgrade cost field
                    getUpgradeCostField.text = "Upgrade cost: " + upgradeCost;

                    // Disable or enable the upgrade button depending on if the player has enough currency for it
                    if(GameAdvancement.currencyPoints >= upgradeCost)
                    {
                        // Can upgrade, so enable the button
                        getUpgradeTowerButton.interactable = true;
                        
                    } else {
                        
                        // Cannot upgrade, so disable the button
                        getUpgradeTowerButton.interactable = false;
                    }
                
                break;

                case "Earth Tower":
                    // Enable one of the additional fields for the earth tower
                    getAdditionalField1.gameObject.SetActive(true);
                    getAdditionalField2.gameObject.SetActive(false);

                    // Actualize the standard fields
                    getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage * earthDamageEnhancer);
                    getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown;
                    getTowerRangeField.text = "Range " + tower.getAttackRange;

                    // Fill the aditional field
                    getAdditionalField1.text = "Projectile size: " + tower.getEffectRange + " > " + (tower.getEffectRange * earthSizeEnhancer);
                
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(earthTowerUpgradeBaseCost * Mathf.Pow(earthTowerUpgradeCostMultiplicator, tower.getLevel - 1));

                    // Write this cost in the upgrade cost field
                    getUpgradeCostField.text = "Upgrade cost: " + upgradeCost;

                    // Disable or enable the upgrade button depending on if the player has enough currency for it
                    if(GameAdvancement.currencyPoints >= upgradeCost)
                    {
                        // Can upgrade, so enable the button
                        getUpgradeTowerButton.interactable = true;
                        
                    } else {
                        
                        // Cannot upgrade, so disable the button
                        getUpgradeTowerButton.interactable = false;
                    }
                
                break;

                case "Wind Tower":
                    // Enable one of the additional fields for the wind tower
                    getAdditionalField1.gameObject.SetActive(true);
                    getAdditionalField2.gameObject.SetActive(false);

                    // Actualize the standard fields
                    getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown * windAttackCooldownEnhancer);
                    getTowerDamageField.text = "Damage " + tower.getDamage;
                    getTowerRangeField.text = "Range " + tower.getAttackRange;

                    // Fill the aditional fields
                    getAdditionalField1.text = "Drop back distance: " + tower.getEffectRange + " > " + (tower.getEffectRange * windDropBackEnhancer);
                    
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(windTowerUpgradeBaseCost * Mathf.Pow(windTowerUpgradeCostMultiplicator, tower.getLevel - 1));

                    // Write this cost in the upgrade cost field
                    getUpgradeCostField.text = "Upgrade cost: " + upgradeCost;

                    // Disable or enable the upgrade button depending on if the player has enough currency for it
                    if(GameAdvancement.currencyPoints >= upgradeCost)
                    {
                        // Can upgrade, so enable the button
                        getUpgradeTowerButton.interactable = true;
                        
                    } else {
                        
                        // Cannot upgrade, so disable the button
                        getUpgradeTowerButton.interactable = false;
                    }
                
                break;

                case "Archer Tower":
                    // Disable the two additinal fields for the archer tower
                    getAdditionalField1.gameObject.SetActive(false);
                    getAdditionalField2.gameObject.SetActive(false);

                    // Actualize all standard fields
                    getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage * arrowDamageEnhancer);
                    getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown * arrowAttackCooldownEnhancer);
                    getTowerRangeField.text = "Range " + tower.getAttackRange + " > " + (tower.getAttackRange * arrowRangeEnhancer);
                
                    Debug.Log("The damage on level 1 is multiplied with: " + (Mathf.Pow(arrowDamageEnhancer, tower.getLevel)));

                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(archerTowerUpgradeBaseCost * Mathf.Pow(archerTowerUpgradeCostMultiplicator, tower.getLevel - 1));

                    // Write this cost in the upgrade cost field
                    getUpgradeCostField.text = "Upgrade cost: " + upgradeCost;

                    // Disable or enable the upgrade button depending on if the player has enough currency for it
                    if(GameAdvancement.currencyPoints >= upgradeCost)
                    {
                        // Can upgrade, so enable the button
                        getUpgradeTowerButton.interactable = true;
                        
                    } else {
                        
                        // Cannot upgrade, so disable the button
                        getUpgradeTowerButton.interactable = false;
                    }

                break;

                case "Fire Tower":
                    // Disable the two additinal fields for the fire tower
                    getAdditionalField1.gameObject.SetActive(false);
                    getAdditionalField2.gameObject.SetActive(false);

                    // Actualize all standard fields
                    getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage * fireDamageEnhancer);
                    getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown * fireAttackCooldownEnhancer);
                    getTowerRangeField.text = "Range " + tower.getAttackRange;

                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(fireTowerUpgradeBaseCost * Mathf.Pow(fireTowerUpgradeCostMultiplicator, tower.getLevel - 1));

                    // Write this cost in the upgrade cost field
                    getUpgradeCostField.text = "Upgrade cost: " + upgradeCost;

                    // Disable or enable the upgrade button depending on if the player has enough currency for it
                    if(GameAdvancement.currencyPoints >= upgradeCost)
                    {
                        // Can upgrade, so enable the button
                        getUpgradeTowerButton.gameObject.SetActive(true);
                        
                    } else {
                        
                        // Cannot upgrade, so disable the button
                        getUpgradeTowerButton.gameObject.SetActive(false);
                    }

                break;
            }
        }
    }

    // Method used to close the upgrade tower menu
    public void CloseUpgradeTowerMenu()
    {
        // // Lower the flag that states that the player is currently upgrading or building something
        // GameAdvancement.currentlyBuildingOrUpgrading = false;

        // Set the upgrade tower menu as inactive
        getUpgradeTowerMenu.SetActive(false);

        // Unpause the game
        GameAdvancement.gamePaused = false;
    }

    // Method called when the user clicks on the upgrade tower button
    public void InitiateTowerUpgrade()
    {
        // Disable the game overlay
        DeactivateGameOverlay();

        ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered((int)(TowerEnhancer.currentlyEnhancedTower.getLevel + 2 / 2));
        Debug.Log("The number of questions that need to be answered that was added was: " + (int)(TowerEnhancer.currentlyEnhancedTower.getLevel / 2));

        // ActivateQuestions.ActualizeNumberOfQuestionsThatNeedToBeAnsweredDisplay();

        // // Enable the answer question menu
        // answerQuestions.SetActive(true);

        // // Disable the game overlay
        // gameOverlay.SetActive(false);

        Debug.Log("The game overlay was deactivated");
        Debug.Log("Last question index: " + Questions.lastQuestionIndex);
        Debug.Log("Number of questions: " + Questions.numberOfQuestionsNeededToAnswer);

        // Set the number of questions that are needed to answer to the level of the tower divided by 2
        // Questions.numberOfQuestionsNeededToAnswer = Questions.numberOfQuestionsNeededToAnswer + (int)(TowerEnhancer.currentlyEnhancedTower.getLevel / 2);
        // ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered((int)(1));

        // Save the content of the get tower type field
        string towerTypeText = getTowerTypeField.text;

        // Close the menu
        upgradeTowerMenu.SetActive(false);

        // Initialize the upgrade cost integer
        int upgradeCost = 0;

        // Start the coroutine that upgrades the tower
        StartCoroutine(UpgradeTowerMethod(towerTypeText));

        // Enable the answer question menu
        answerQuestions.SetActive(true);
    }

    // Function that is used to test when all questions that were needed to be answered were answered correctly
    private bool NoMoreQuestionsNeeded()
    {
        return Questions.numberOfQuestionsNeededToAnswer == 0;
    }

    // Method used to upgrade a tower
    IEnumerator UpgradeTowerMethod(string type)
    {
        // Initialize the upgrade cost integer
        int upgradeCost = 0;

        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Check what is written in the tower type field and call the right upgrade function
        switch(type)
        {
            case "Archer Tower":

                // Calculate the cost of upgrading this tower
                upgradeCost = (int)(archerTowerUpgradeBaseCost * Mathf.Pow(archerTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.getLevel - 1));

                // Upgrade the tower
                TowerEnhancer.currentlyEnhancedTower.UpgradeArcherTower(arrowDamageEnhancer, arrowAttackCooldownEnhancer, arrowRangeEnhancer);

            break;

            case "Fire Tower":

                // Calculate the cost of upgrading this tower
                upgradeCost = (int)(fireTowerUpgradeBaseCost * Mathf.Pow(fireTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.getLevel - 1));

                // Upgrade the tower
                TowerEnhancer.currentlyEnhancedTower.UpgradeFireTower(fireDamageEnhancer, fireAttackCooldownEnhancer);

            break;

            case "Earth Tower":

                // Calculate the cost of upgrading this tower
                upgradeCost = (int)(earthTowerUpgradeBaseCost * Mathf.Pow(earthTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.getLevel - 1));

                // Upgrade the tower
                TowerEnhancer.currentlyEnhancedTower.UpgradeEarthTower(earthDamageEnhancer, earthSizeEnhancer);

            break;

            case "Lightning Tower":

                // Calculate the cost of upgrading this tower
                upgradeCost = (int)(lightningTowerUpgradeBaseCost * Mathf.Pow(lightningTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.getLevel - 1));

                // Upgrade the tower
                TowerEnhancer.currentlyEnhancedTower.UpgradeLightningTower(lightningDamageEnhancer, lightningJumpRangeEnhancer, lightningRangeEnhancer);

            break;

            case "Wind Tower":

                // Calculate the cost of upgrading this tower
                upgradeCost = (int)(windTowerUpgradeBaseCost * Mathf.Pow(windTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.getLevel - 1));

                // Upgrade the tower
                TowerEnhancer.currentlyEnhancedTower.UpgradeWindTower(windAttackCooldownEnhancer, windDropBackEnhancer);

            break;
        }

        // Remove the upgrade cost from the currency points of the player
        GameAdvancement.currencyPoints = GameAdvancement.currencyPoints - upgradeCost;

        // Actualize the currency display
        GameSetup.ActualizeCurrencyDisplay();

        // Set the upgrade tower menu as inactive
        getUpgradeTowerMenu.SetActive(false);

        // Unpause the game
        GameAdvancement.gamePaused = false;

        // Enable the game overlay
        ActivateGameOverlay();
    }
}
