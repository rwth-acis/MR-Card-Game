using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// The class of the castle game object
static class GameAdvancement
{
    // The maximum and current health point of the castle
    public static int castleMaxHP;
    public static int castlecurrentHP;

    // The current armor points of the castle
    public static int castleCurrentAP;

    // The amount of currency the player has right now
    public static int currencyPoints;

    // The current wave
    public static int currentWave;

    // The currency display button for the player
    public static Button currencyDisplay;

    // The health bar slider of the castle
    public static Slider castleHealthBar;
    
    // The health counter C / M displayed on the health bar
    public static TMP_Text castleHealthCounter;

    // The wave display button;
    public static Button waveDisplay;

    // The status of the game, if it should be paused or not
    public static bool gamePaused = false;

    // The variable that states if the time was stopped for the enemies
    public static bool timeStopped = false;

    // The variable that states what the global enemy slow factor is currently
    public static float globalSlow = 1;

    // The flag that states if it is raining
    public static bool raining = false;

    // The type of enemy that is plagued. Plagued enemy take damage over time and are slowed
    public static string plaguedEnemyType = "";

    // // The global flag that states if the player is currently building or upgrading something
    // public static bool currentlyBuildingOrUpgrading = false;
}

// The class of the castle game object
static class Buildings
{
    // The number of buildings buit in this round
    public static int numberOfBuildings;

    // The array that binds the image target to the building
    public static GameObject[] imageTargetToBuilding;

    // Define the first building
    public static GameObject firstBuilding;

    // Define the second building
    public static GameObject secondBuilding;
    
    // Define the third building
    public static GameObject thirdBuilding;
    
    // Define the fourth building
    public static GameObject fourthBuilding;
    
    // Define the fifth building
    public static GameObject fifthBuilding;
    
    // Define the sixth building
    public static GameObject sixthBuilding;
    
    // Define the seventh building
    public static GameObject seventhBuilding;
    
    // Define the eighth building
    public static GameObject eighthBuilding;
    
    // Define the ninth building
    public static GameObject ninthBuilding;
    
    // Define the tenth building
    public static GameObject tenthBuilding;
}
   
public class GameSetup : MonoBehaviour
{
    // The health points the castle has. Can be changed in the inspector.
    [SerializeField]
    private int castleHP;

    // The currency the player should have at the beginning. Can be changed in the inspector.
    [SerializeField]
    private int beginingCurrency;

    // The button, not interactable, on which the current wave is displayed
    [SerializeField]
    private Button waveDisplay;

    // The button, not interactable, on which the current currency is displayed
    [SerializeField]
    private Button currencyDisplay;

    // The health bar slider of the castle
    [SerializeField]
    private Slider castleHealthBar;

    // The health bar slider of the castle
    [SerializeField]
    private TMP_Text castleHealthCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Set the number of buildings built in this round to zero
        Buildings.numberOfBuildings = 0;

        // Initialize the array of image targets
        Buildings.imageTargetToBuilding = new GameObject[10];

        // Set the castle max and current hp to the hp given in the inspector
        GameAdvancement.castleMaxHP = castleHP;
        GameAdvancement.castlecurrentHP = castleHP;

        // Set the currency points to the amount the player should have in the begining
        GameAdvancement.currencyPoints = beginingCurrency;

        // Set the wave counter to wave 1
        GameAdvancement.currentWave = 0;

        GameAdvancement.waveDisplay = waveDisplay;

        // Set the currency display button
        GameAdvancement.currencyDisplay = currencyDisplay;

        // Set the castle health bar slider
        GameAdvancement.castleHealthBar = castleHealthBar;

        // Set the castle health counter
        GameAdvancement.castleHealthCounter = castleHealthCounter;

        // Actualize the wave display
        ActualizeWaveDisplay();

        // Actualize the currency display
        ActualizeCurrencyDisplay();

        // Actualize the castle health points
        ActualizeCastleHealthPoints();
    }

    // Update is called once per frame
    void Update()
    {
        // Display currency points
    }

    // Method used to actualize the current currency display of the player
    public static void ActualizeCurrencyDisplay()
    {
        // Actualize the currency currently owned
        GameAdvancement.currencyDisplay.GetComponentInChildren<TMP_Text>().text = "Currency: " + GameAdvancement.currencyPoints;
    }

    // Method used to actualize the current health points of the castle
    public static void ActualizeCastleHealthPoints()
    {
        // TODO add armor points

        // Actualize the value of the castle health bar
        GameAdvancement.castleHealthBar.value = (float)(GameAdvancement.castlecurrentHP / GameAdvancement.castleMaxHP);

        // Change the text field that displayed current HP / max HP
        GameAdvancement.castleHealthCounter.text = GameAdvancement.castlecurrentHP + " / " + GameAdvancement.castleMaxHP;
    }

    // Method used to actualize the current wave
    public static void  ActualizeWaveDisplay()
    {
        // Change the button display
        GameAdvancement.waveDisplay.GetComponentInChildren<TMP_Text>().text = "Wave: " + GameAdvancement.currentWave;
    }
}
