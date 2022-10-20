using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// The settings and controls of the game
/// </summary>
public static class GameAdvancement
{
    /// <summary>
    /// The maximum and current health point of the castle
    /// </summary>
    public static int castleMaxHP;
    public static int castleCurrentHP;

    /// <summary>
    /// The current armor points of the castle
    /// </summary>
    public static int castleCurrentAP;

    /// <summary>
    /// The amount of currency the player has right now
    /// </summary>
    public static int currencyPoints;

    /// <summary>
    /// The current wave
    /// </summary>
    public static int currentWave;

    /// <summary>
    /// The currency display button for the player
    /// </summary>
    public static Button currencyDisplay;

    /// <summary>
    /// The health bar slider of the castle
    /// </summary>
    public static Slider castleHealthBar;

    /// <summary>
    /// The armor bar slider of the castle
    /// </summary>
    public static Slider castleArmorBar;
    
    /// <summary>
    /// The health counter C / M displayed on the health bar
    /// </summary>
    public static TMP_Text castleHealthCounter;

    /// <summary>
    /// The wave display button;
    /// </summary>
    public static Button waveDisplay;

    /// <summary>
    /// If the game is paused
    /// </summary>
    public static bool gamePaused = false;

    /// <summary>
    /// If the time was stopped for the enemies
    /// </summary>
    public static bool timeStopped = false;

    /// <summary>
    /// The global enemy slow factor
    /// </summary>
    public static float globalSlow = 1;

    /// <summary>
    /// If it is raining
    /// </summary>
    public static bool raining = false;

    /// <summary>
    /// The flag that states if the game setup needs to be reset or not
    /// </summary>
    public static bool needToReset = false;

    /// <summary>
    /// Number of buildings built
    /// </summary>
    public static int numberOfBuildingsBuilt = 0;

    /// <summary>
    /// The maximum number of buildings
    /// </summary>
    public static int maxNumberOfBuildings = 0;
}

public class GameSetup : MonoBehaviour
{
    /// <summary>
    /// The instance of this class so that some variables can be accessed in a static way
    /// </summary>
    public static GameSetup Instance;

    [Tooltip("Max number of buildings in the first wave.")]
    [SerializeField]
    private int numberOfBuildingsFirstWave = 3;

    [Tooltip("Max number of buildings in the second wave.")]
    [SerializeField]
    private int numberOfBuildingsSecondWave = 6;

    [Tooltip("Max number of buildings after the third wave.")]
    [SerializeField]
    private int numberrOfBuildingsMax = 10;

    [Tooltip("The button, not interactable, on which the current wave is displayed")] 
    [SerializeField]
    private GameObject gameOverlay;

    [Tooltip("The health points the castle has.")]
    [SerializeField]
    private int castleHP;

    [Tooltip("The currency the player should have at the beginning.")]
    [SerializeField]
    private int beginingCurrency;

    [Tooltip("The button, not interactable, on which the current wave is displayed")]
    [SerializeField]
    private Button waveDisplay;

    [Tooltip("The button, not interactable, on which the current currency is displayed")]
    [SerializeField]
    private Button currencyDisplay;

    [Tooltip("The health bar slider of the castle")] 
    [SerializeField]
    private Slider castleHealthBar;

    [Tooltip("The health bar slider of the castle")]
    [SerializeField]
    private TMP_Text castleHealthCounter;

    [SerializeField]
    private Slider castleArmorBar;

    /// <summary>
    /// The game overlay object
    /// </summary>
    public static GameObject GameOverlay
    {
        get { return Instance.gameOverlay; }
    }

    /// <summary>
    /// The castle health points
    /// </summary>
    public static int CastleHP
    {
        get { return Instance.castleHP; }
    }

    /// <summary>
    /// The begining currecy
    /// </summary>
    public static int BeginingCurrency
    {
        get { return Instance.beginingCurrency; }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameAdvancement.waveDisplay = waveDisplay;
        GameAdvancement.currencyDisplay = currencyDisplay;
        GameAdvancement.castleHealthBar = castleHealthBar;
        GameAdvancement.castleArmorBar = castleArmorBar;
        GameAdvancement.castleHealthCounter = castleHealthCounter;
        ResetGameSetup();
    }


    /// <summary>
    /// Update the current currency display of the player
    /// </summary>
    public static void UpdateCurrencyDisplay()
    {
        GameAdvancement.currencyDisplay.GetComponentInChildren<TMP_Text>().text = "Currency: " + GameAdvancement.currencyPoints;
    }

    /// <summary>
    /// Update the current health points of the castle
    /// </summary>
    public static void UpdateCastleHealthPoints()
    {
        GameAdvancement.castleHealthBar.value = (float)((float)GameAdvancement.castleCurrentHP / (float)GameAdvancement.castleMaxHP);
        GameAdvancement.castleHealthCounter.text = GameAdvancement.castleCurrentHP + " / " + GameAdvancement.castleMaxHP;
        GameAdvancement.castleArmorBar.value = (float)((float)GameAdvancement.castleCurrentAP / (float)GameAdvancement.castleMaxHP);
    }

    /// <summary>
    /// Update the current wave
    /// </summary>
    public static void UpdateWaveDisplay()
    {
        GameAdvancement.waveDisplay.GetComponentInChildren<TMP_Text>().text = "Wave: " + GameAdvancement.currentWave;

    }

    /// <summary>
    /// Reset the game setup after a level was finished
    /// </summary>
    public static void ResetGameSetup()
    {
        // Check the number of waves, and set the castle health points accordingly
        if(LevelInfo.numberOfWaves == 1)
        {
            GameAdvancement.castleMaxHP = 10;

        } else if(LevelInfo.numberOfWaves == 2)
        {
            GameAdvancement.castleMaxHP = 20;

        } else {
            GameAdvancement.castleMaxHP = CastleHP;
        }
        GameAdvancement.castleCurrentHP = GameAdvancement.castleMaxHP;
        GameAdvancement.castleCurrentAP = 0;
        GameAdvancement.currencyPoints = BeginingCurrency;
        GameAdvancement.currentWave = 0;
        GameAdvancement.numberOfBuildingsBuilt = 0;
        SetMaxNumberOfBuildings();
        GameAdvancement.gamePaused = false;
        GameAdvancement.timeStopped = false;
        GameAdvancement.globalSlow = 1;
        GameAdvancement.raining = false;
        UpdateWaveDisplay();
        UpdateCurrencyDisplay();
        UpdateCastleHealthPoints();
    }

    /// <summary>
    /// Set the max number of buildings correctly
    /// </summary>
    public static void SetMaxNumberOfBuildings()
    {
        // Check the number of waves
        if(LevelInfo.numberOfWaves == 1)
        {
            GameAdvancement.maxNumberOfBuildings = Instance.numberOfBuildingsFirstWave;

        } else if(LevelInfo.numberOfWaves == 2)
        {
            GameAdvancement.maxNumberOfBuildings = Instance.numberOfBuildingsSecondWave;

        } else {
            GameAdvancement.maxNumberOfBuildings = Instance.numberrOfBuildingsMax;
        }
    }
}
