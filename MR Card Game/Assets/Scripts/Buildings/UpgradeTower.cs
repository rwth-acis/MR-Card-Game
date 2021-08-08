using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTower : MonoBehaviour
{
    // The instance of this script
    public static UpgradeTower instance;

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
    // Upgrading towers
    //--------------------------------------------------------------------------------------

    // Define the updrage tower cavas
    [SerializeField]
    private GameObject upgradeTowerCanvas;

    // The method used to access to the upgrade tower canvas as a static object
    public static GameObject getUpgradeTowerCanvas
    {
        get { return instance.upgradeTowerCanvas; }
    }
    
    // Define the updrage tower menu
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

    // Method used to open the upgrade menu
    public static void OpenUpgradeTowerMenu(Tower tower)
    {
        // Pause the game
        GameAdvancement.gamePaused = true;

        // Set the upgrade tower menu as active
        getUpgradeTowerMenu.SetActive(true);

        // Check the tower type and fill the stats window accordingly
        switch(tower.getTowerType)
        {
            case "Lightning Tower":
                // Enable the two additional fields for the lightning tower
                getAdditionalField1.gameObject.SetActive(true);
                getAdditionalField2.gameObject.SetActive(true);

                // Actualize the standard fields
                getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage + 15);
                getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown;
                getTowerRangeField.text = "Range " + tower.getAttackRange;

                // Fill the aditional fields
                getAdditionalField1.text = "Number of jumps: " + tower.getNumberOfEffect + " > " + (tower.getNumberOfEffect + 1);
                getAdditionalField2.text = "Jump range: " + tower.getEffectRange + " > " + (tower.getEffectRange + (float)0.1);
            break;

            case "Earth Tower":
                // Enable one of the additional fields for the earth tower
                getAdditionalField1.gameObject.SetActive(true);
                getAdditionalField2.gameObject.SetActive(false);

                // Actualize the standard fields
                getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage + 50);
                getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown;
                getTowerRangeField.text = "Range " + tower.getAttackRange;

                // Fill the aditional field
                getAdditionalField1.text = "Projectile size: " + tower.getEffectRange + " > " + (tower.getEffectRange + (float)0.2);
            break;

            case "Wind Tower":
                // Enable one of the additional fields for the wind tower
                getAdditionalField1.gameObject.SetActive(true);
                getAdditionalField2.gameObject.SetActive(false);

                // Actualize the standard fields
                getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown - (float)0.15);
                getTowerDamageField.text = "Damage " + tower.getDamage;
                getTowerRangeField.text = "Range " + tower.getAttackRange;

                // Fill the aditional fields
                getAdditionalField1.text = "Drop back distance: " + tower.getEffectRange + " > " + (tower.getEffectRange + (float)0.1);
            break;

            case "Archer Tower":
                // Disable the two additinal fields for the archer tower
                getAdditionalField1.gameObject.SetActive(false);
                getAdditionalField2.gameObject.SetActive(false);

                // Actualize all standard fields
                getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage + 20);
                getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown - (float)0.15);
                getTowerRangeField.text = "Range " + tower.getAttackRange + " > " + (tower.getAttackRange + (float)0.1);
            break;

            case "Fire Tower":
                // Disable the two additinal fields for the fire tower
                getAdditionalField1.gameObject.SetActive(false);
                getAdditionalField2.gameObject.SetActive(false);

                // Actualize all standard fields
                getTowerDamageField.text = "Damage " + tower.getDamage + " > " + (tower.getDamage + 40);
                getTowerAttackCooldownField.text = "Attack cooldown " + tower.getAttackCooldown + " > " + (tower.getAttackCooldown - (float)0.10);
                getTowerRangeField.text = "Range " + tower.getAttackRange;
            break;
        }

        // // For three towers, additional fields are needed as well as individual buffs
        // if(tower.towerType == "Lightning Tower") 
        // {
        //     // Enable the two additional fields for the lightning tower
        //     additionalField1.gameObject.SetActive(true);
        //     additionalField2.gameObject.SetActive(true);

        //     // Actualize the standard fields
        //     towerDamageField.text = "Damage " + tower.damage + " > " + (tower.damage + 15);
        //     towerAttackCooldownField.text = "Attack cooldown " + tower.attackCooldown;
        //     towerRangeField.text = "Range " + tower.attackRange;

        //     // Fill the aditional fields
        //     additionalField1.text = "Number of jumps: " + tower.numberOfEffect + " > " + (tower.numberOfEffect + 1);
        //     additionalField2.text = "Jump range: " + tower.effectRange + " > " + (tower.effectRange + (float)0.1);

        // } else if(tower.towerType == "Earth Tower") 
        // {
        //     // Enable one of the additional fields for the earth tower
        //     additionalField1.gameObject.SetActive(true);
        //     additionalField2.gameObject.SetActive(false);

        //     // Actualize the standard fields
        //     towerDamageField.text = "Damage " + tower.damage + " > " + (tower.damage + 50);
        //     towerAttackCooldownField.text = "Attack cooldown " + tower.attackCooldown;
        //     towerRangeField.text = "Range " + tower.attackRange;

        //     // Fill the aditional field
        //     additionalField1.text = "Projectile size: " + tower.effectRange + " > " + (tower.effectRange + (float)0.2);

        // } else if(towerType == "Wind Tower") 
        // {
        //     // Enable one of the additional fields for the wind tower
        //     additionalField1.gameObject.SetActive(true);
        //     additionalField2.gameObject.SetActive(false);

        //     // Actualize the standard fields
        //     towerAttackCooldownField.text = "Attack cooldown " + attackCooldown + " > " + (attackCooldown - (float)0.15);
        //     towerDamageField.text = "Damage " + damage;
        //     towerRangeField.text = "Range " + attackRange;

        //     // Fill the aditional fields
        //     additionalField1.text = "Drop back distance: " + effectRange + " > " + (effectRange + (float)0.1);

        // } else {

        //     // Disable the two additinal fields for the other towers
        //     additionalField1.gameObject.SetActive(false);
        //     additionalField2.gameObject.SetActive(false);

        //     // Make the individual inprovements to the other towers
        //     if(towerType == "Archer Tower")
        //     {
        //         // Actualize all standard fields
        //         towerDamageField.text = "Damage " + damage + " > " + (damage + 20);
        //         towerAttackCooldownField.text = "Attack cooldown " + attackCooldown + " > " + (attackCooldown - (float)0.15);
        //         towerRangeField.text = "Range " + attackRange + " > " + (attackRange + (float)0.1);

        //     } else if(towerType == "Fire Tower")
        //     {
        //         // Actualize all standard fields
        //         towerDamageField.text = "Damage " + damage + " > " + (damage + 40);
        //         towerAttackCooldownField.text = "Attack cooldown " + attackCooldown + " > " + (attackCooldown - (float)0.10);
        //         towerRangeField.text = "Range " + attackRange;
        //     }
        // }
    }
}
