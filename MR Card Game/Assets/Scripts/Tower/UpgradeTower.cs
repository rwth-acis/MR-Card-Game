using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using build;

public class UpgradeTower : MonoBehaviour
{

    [Header("Tower Prefabs")]
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

    [SerializeField]
    private GameObject deleteTrapMenu;

    [Header("UI Components")]
    [SerializeField]
    private GameObject upgradeTowerMenu;

    [SerializeField]
    private TMP_Text towerTypeField;

    [SerializeField]
    private TMP_Text towerLevelField;

    [SerializeField]
    private TMP_Text towerDamageField;

    [SerializeField]
    private TMP_Text towerAttackCooldownField;

    [SerializeField]
    private TMP_Text towerRangeField;

    [SerializeField]
    private TMP_Text additionalField1;

    [SerializeField]
    private TMP_Text additionalField2;

    [SerializeField]
    private TMP_Text upgradeCostField;

    [SerializeField]
    private Button upgradeTowerButton;

    [SerializeField]
    private GameObject answerQuestions;

    [SerializeField]
    private Button currencyDisplay;

    [SerializeField]
    private Button waveDisplay;

    [SerializeField]
    private Button startNextWave;

    public static UpgradeTower Instance;

    // The level up multiplicators
    private static float lightningDamageEnhancer = 1.1f;
    private static float lightningRangeEnhancer = 1.2f;
    private static float lightningJumpRangeEnhancer = 1.4f;
    private static float earthDamageEnhancer = 1.6f;
    private static float earthSizeEnhancer = 1.2f;
    private static float windAttackCooldownEnhancer = 0.9f;
    private static float windDropBackEnhancer = 1.2f;
    private static float archerDamageEnhancer = 1.2f;
    private static float archerAttackCooldownEnhancer = 0.85f;
    private static float archerRangeEnhancer = 1.1f;
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

    public static GameObject DeleteTrapMenu
    {
        get { return Instance.deleteTrapMenu; }
    }

    public static GameObject UpgradeTowerMenu
    {
        get { return Instance.upgradeTowerMenu; }
    }


    public static TMP_Text TowerTypeField
    {
        get { return Instance.towerTypeField; }
    }

    public static TMP_Text TowerLevelField
    {
        get => Instance.towerLevelField;
    }

    public static TMP_Text TowerDamageField
    {
        get { return Instance.towerDamageField; }
    }

    public static TMP_Text TowerAttackCooldownField
    {
        get { return Instance.towerAttackCooldownField; }
    }

    public static TMP_Text TowerRangeField
    {
        get { return Instance.towerRangeField; }
    }

    public static TMP_Text AdditionalField1
    {
        get { return Instance.additionalField1; }
    }
    public static TMP_Text AdditionalField2
    {
        get { return Instance.additionalField2; }
    }
    public static TMP_Text UpgradeCostField
    {
        get { return Instance.upgradeCostField; }
    }
    public static Button UpgradeTowerButton
    {
        get { return Instance.upgradeTowerButton; }
    }
    public static Button CurrencyDisplay
    {
        get { return Instance.currencyDisplay; }
    }
    public static Button WaveDisplay
    {
        get { return Instance.waveDisplay; }
    }
    public static Button StartNextWave
    {
        get { return Instance.startNextWave; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    //--------------------------------------------------------------------------------------
    // Deleting towers
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// delete towers
    /// </summary>
    public void DeleteTower()
    {
        CloseUpgradeTowerMenu();

        Tower towerprefab = archerTowerPrefab;

        // Get the right prefab for this tower and refund a part of the price
        switch(TowerEnhancer.currentlyEnhancedTower.TowerType)
        {
            case TowerType.Archer:
                towerprefab = archerTowerPrefab;
                GameAdvancement.currencyPoints += build.BuildTowerMenu.ArcherTowerCost;
            break;

            case TowerType.Fire:
                towerprefab = fireTowerPrefab;
                GameAdvancement.currencyPoints += build.BuildTowerMenu.FireTowerCost;
            break;

            case TowerType.Earth:
                towerprefab = earthTowerPrefab;
                GameAdvancement.currencyPoints += build.BuildTowerMenu.EarthTowerCost;
            break;

            case TowerType.Lightning:
                towerprefab = lightningTowerPrefab;
                GameAdvancement.currencyPoints += build.BuildTowerMenu.LightningTowerCost;
            break;

            case TowerType.Wind:
                towerprefab = windTowerPrefab;
                GameAdvancement.currencyPoints += build.BuildTowerMenu.WindTowerCost;
            break;
        }
        TowerEnhancer.currentlyEnhancedTower.ResetTowerProperties(towerprefab);
        ObjectPools.ReleaseTower(TowerEnhancer.currentlyEnhancedTower.transform.parent.gameObject);
        GameSetup.UpdateCurrencyDisplay();
    }

    /// <summary>
    /// Delete traps
    /// </summary>
    public void DeleteTrap()
    {
        CloseDeleteTrapMenu();
        ObjectPools.ReleaseTrap(TrapDeleter.currentlyOpenedTrapWindow);
        // refund according to the trap type
        GameAdvancement.currencyPoints += TrapDeleter.currentlyOpenedTrapWindow.TrapType switch
        {
            TrapType.Hole => build.BuildTowerMenu.HoleCost,
            TrapType.Swamp => build.BuildTowerMenu.SwampCost,
            _ => build.BuildTowerMenu.HoleCost
        };
        GameSetup.UpdateCurrencyDisplay();
    }

    public static void OpenDeleteTrapMenu(Trap trap)
    {
        // Set the right game object as the one that opened the menu
        TrapDeleter.currentlyOpenedTrapWindow = trap;
        DeleteTrapMenu.SetActive(true);
        GameAdvancement.gamePaused = true;
    }

    public void CloseDeleteTrapMenu()
    {
        deleteTrapMenu.SetActive(false);
        GameAdvancement.gamePaused = false;
    }

    //--------------------------------------------------------------------------------------
    // Upgrading towers
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// Open the upgrade menu
    /// </summary>
    public static void OpenUpgradeTowerMenu(Tower tower)
    {
        // Check that nothing is beeing build or upgraded
        if(GameAdvancement.gamePaused == false)
        {
            // Save the tower that opened the upgrade menu
            TowerEnhancer.currentlyEnhancedTower = tower;
            GameAdvancement.gamePaused = true;
            UpgradeTowerMenu.SetActive(true);
            TowerTypeField.text = tower.TowerType.ToString();

            int upgradeCost = 0;

            // Check the tower type and fill the stats window accordingly
            switch(tower.TowerType)
            {
                case TowerType.Lightning:
                    // Enable the two additional fields for the lightning tower
                    AdditionalField1.gameObject.SetActive(true);
                    AdditionalField2.gameObject.SetActive(true);
                    // Fill the aditional fields
                    AdditionalField1.text = "Number of jumps: " + tower.NumberOfEffect + " > " + (tower.NumberOfEffect + 1);
                    AdditionalField2.text = "Jump range: " + tower.EffectRange + " > " + (tower.EffectRange * lightningJumpRangeEnhancer);
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(lightningTowerUpgradeBaseCost * Mathf.Pow(lightningTowerUpgradeCostMultiplicator, tower.Level - 1));          
                break;
                case TowerType.Earth:
                    // Enable one of the additional fields for the earth tower
                    AdditionalField1.gameObject.SetActive(true);
                    AdditionalField2.gameObject.SetActive(false);
                    // Fill the aditional field
                    AdditionalField1.text = "Projectile size: " + tower.EffectRange + " > " + (tower.EffectRange * earthSizeEnhancer);
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(earthTowerUpgradeBaseCost * Mathf.Pow(earthTowerUpgradeCostMultiplicator, tower.Level - 1));
                break;
                case TowerType.Wind:
                    // Enable one of the additional fields for the wind tower
                    AdditionalField1.gameObject.SetActive(true);
                    AdditionalField2.gameObject.SetActive(false);
                    // Fill the aditional fields
                    AdditionalField1.text = "Drop back distance: " + tower.EffectRange + " > " + (tower.EffectRange * windDropBackEnhancer);
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(windTowerUpgradeBaseCost * Mathf.Pow(windTowerUpgradeCostMultiplicator, tower.Level - 1));        
                break;
                case TowerType.Archer:
                    // Disable the two additinal fields for the archer tower
                    AdditionalField1.gameObject.SetActive(false);
                    AdditionalField2.gameObject.SetActive(false);
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(archerTowerUpgradeBaseCost * Mathf.Pow(archerTowerUpgradeCostMultiplicator, tower.Level - 1));
                break;
                case TowerType.Fire:
                    // Disable the two additinal fields for the fire tower
                    AdditionalField1.gameObject.SetActive(false);
                    AdditionalField2.gameObject.SetActive(false);
                    // Calculate the cost of upgrading this tower
                    upgradeCost = (int)(fireTowerUpgradeBaseCost * Mathf.Pow(fireTowerUpgradeCostMultiplicator, tower.Level - 1));
                break;
            }
            DisplayUpgradeInfo(tower, upgradeCost);
        }
    }

    private static void DisplayUpgradeInfo(Tower tower, int upgradeCost)
    {
        // Update all standard fields
        float damageEnhancer = GetDamageEnhancer(tower.TowerType);
        float cooldownEnhancer = GetCooldownEnhancer(tower.TowerType);
        float rangeEnhancer = GetRangeEnhancer(tower.TowerType);
        TowerDamageField.text = "Damage " + tower.Damage + " > " + (tower.Damage * damageEnhancer);
        TowerAttackCooldownField.text = "Attack cooldown " + tower.AttackCooldown + " > " + (tower.AttackCooldown * cooldownEnhancer);
        TowerRangeField.text = "Range " + tower.AttackRange + " > " + (tower.AttackRange * rangeEnhancer);

        // Write this cost in the upgrade cost field
        UpgradeCostField.text = "Upgrade cost: " + upgradeCost;

        TowerLevelField.text = $"Lv{(int)tower.Level}";

        // Disable or enable the upgrade button depending on if the player has enough currency for it
        if (GameAdvancement.currencyPoints >= upgradeCost)
        {
            UpgradeTowerButton.interactable = true;
        }
        else
        {
            UpgradeTowerButton.interactable = false;
        }
    }

    public static float GetCooldownEnhancer(TowerType type)
    {
        switch (type)
        {
            case TowerType.Archer:
                return archerAttackCooldownEnhancer;
            case TowerType.Fire:
                return fireAttackCooldownEnhancer;
            case TowerType.Wind:
                return windAttackCooldownEnhancer;
            default:
                return 1f;
        }
    }

    public static float GetDamageEnhancer(TowerType type)
    {
        switch (type)
        {
            case TowerType.Archer:
                return archerDamageEnhancer;
            case TowerType.Fire:
                return fireDamageEnhancer;
            case TowerType.Lightning:
                return lightningDamageEnhancer;
            case TowerType.Earth:
                return earthDamageEnhancer;
            default:
                return 1f;
        }
    }

    public static float GetRangeEnhancer(TowerType type)
    {
        switch (type)
        {
            case TowerType.Archer:
                return archerRangeEnhancer;
            case TowerType.Lightning:
                return lightningRangeEnhancer;
            default:
                return 1f;
        }
    }

    /// <summary>
    /// Close the upgrade tower menu
    /// </summary>
    public void CloseUpgradeTowerMenu()
    {
        UpgradeTowerMenu.SetActive(false);
        GameAdvancement.gamePaused = false;
    }

    /// <summary>
    /// called when the user clicks on the upgrade tower button
    /// </summary>
    public void InitiateTowerUpgrade()
    {
        // Set the question requesting image target correctly
        Questions.questionRequestingImageTarget = TowerImageTarget.currentImageTarget;
        GameSceneManager.DeactivateGameOverlay();
        Debug.Log("Current Tower Level: " + TowerEnhancer.currentlyEnhancedTower.Level);
        ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered((int)(TowerEnhancer.currentlyEnhancedTower.Level + 2 / 2));
        Debug.Log("The number of questions that need to be answered that was added was: " + (int)(TowerEnhancer.currentlyEnhancedTower.Level / 2));
        Debug.Log("The game overlay was deactivated");
        Debug.Log("Last question index: " + Questions.lastQuestionIndex);
        Debug.Log("Number of questions: " + Questions.numberOfQuestionsNeededToAnswer);

        // Save the content of the get tower type field
        string towerTypeText = TowerTypeField.text;

        upgradeTowerMenu.SetActive(false);
        // Start the coroutine that upgrades the tower
        StartCoroutine(UpgradeTowerMethod(towerTypeText));

        answerQuestions.SetActive(true);
    }

    /// <summary>
    /// Upgrade a tower with the given type in string because the type is retrieved from UI text.
    /// </summary>
    /// <param name="type">string of the tower type</param>
    private IEnumerator UpgradeTowerMethod(string type)
    {
        int upgradeCost = 0;
        yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);

        switch(type)
        {
            case "Archer":
                upgradeCost = (int)(archerTowerUpgradeBaseCost * Mathf.Pow(archerTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.Level - 1));
                TowerEnhancer.currentlyEnhancedTower.UpgradeArcherTower(archerDamageEnhancer, archerAttackCooldownEnhancer, archerRangeEnhancer);

            break;
            case "Fire":
                upgradeCost = (int)(fireTowerUpgradeBaseCost * Mathf.Pow(fireTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.Level - 1));
                TowerEnhancer.currentlyEnhancedTower.UpgradeFireTower(fireDamageEnhancer, fireAttackCooldownEnhancer);

            break;
            case "Earth":
                upgradeCost = (int)(earthTowerUpgradeBaseCost * Mathf.Pow(earthTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.Level - 1));
                TowerEnhancer.currentlyEnhancedTower.UpgradeEarthTower(earthDamageEnhancer, earthSizeEnhancer);

            break;
            case "Lightning":
                upgradeCost = (int)(lightningTowerUpgradeBaseCost * Mathf.Pow(lightningTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.Level - 1));
                TowerEnhancer.currentlyEnhancedTower.UpgradeLightningTower(lightningDamageEnhancer, lightningJumpRangeEnhancer, lightningRangeEnhancer);

            break;
            case "Wind":
                upgradeCost = (int)(windTowerUpgradeBaseCost * Mathf.Pow(windTowerUpgradeCostMultiplicator, TowerEnhancer.currentlyEnhancedTower.Level - 1));
                TowerEnhancer.currentlyEnhancedTower.UpgradeWindTower(windAttackCooldownEnhancer, windDropBackEnhancer);
            break;
        }

        GameAdvancement.currencyPoints -= upgradeCost;
        GameSetup.UpdateCurrencyDisplay();
        UpgradeTowerMenu.SetActive(false);
        GameAdvancement.gamePaused = false;
        GameSceneManager.ActivateGameOverlay();
    }
}
