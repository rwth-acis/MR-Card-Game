using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static i5.Toolkit.Core.Examples.Spawners.SpawnEnemy;
using UnityEngine.EventSystems;
using TMPro;

public static class LevelInfo
{
    // The number of waves of the level
    public static int numberOfWaves;

    // The number of additional enemies per wave
    public static int numberOfAdditionalEnemiesPerWave;

    // The number of enemies that were not defeated for now in the wave
    public static int numberOfUndefeatedEnemies = 0;

    // The number of enemies per wave
    public static int[] numberOfEnemies;

    // The number of normal enemies that come each wave
    public static int[] normalEnemies;

    // The number of fast enemies that come each wave
    public static int[] fastEnemies;

    // The number of fast enemies that come each wave
    public static int[] superFastEnemies;

    // The number of flying enemies that come each wave
    public static int[] flyingEnemies;

    // The number of tank enemies that come each wave
    public static int[] tankEnemies;

    // The number of slow and extremely tanky enemies that come each wave
    public static int[] slowEnemies;

    // The number of enemies that deal increased damage to the castle that come each wave
    public static int[] berzerkerEnemies;

    // The number of enemies that fly and deal increased damage to the castle that come each wave
    public static int[] berzerkerFlyingEnemies;

    // The number of enemies that have a lot of health points and deal increased damage to the castle that come each wave
    public static int[] berzerkerTankEnemies;

    // The flag that triggers the next wave
    public static bool nextWave = false;

    // The flag that states if the wave is ongoing
    public static bool waveOngoing = false;

    // The number of enemies that reached and damaged the castle
    public static int numberOfEnemiesThatReachedTheCastle = 0;

    // The number of enemies that reached and damaged the castle
    public static int numberOfEnemiesDefeated = 0;

    public static int numberOfEnemiesDefeatedOrReachedCastleCurrentWave = 0;

    // The flag that states that a new level was started and the level information need to be reset
    public static bool newLevelStarted = false;
}

public class Level : MonoBehaviour
{
    // The start next wave button
    [SerializeField]
    private Button startNextWave;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private TextMeshProUGUI enemyNumberDisplay;

    [SerializeField]
    private int numberOfAdditionalEnemiesPerWave;

    [SerializeField]
    private GameObject victoryScreen;

    [SerializeField]
    private TMP_Text enemiesDefeatedVictory;

    [SerializeField]
    private TMP_Text enemiesMissedVictory;

    [SerializeField]
    private TMP_Text castleHealthCounter;

    [SerializeField]
    private GameObject defeatScreen;

    [SerializeField]
    private TMP_Text enemiesDefeatedDefeat;

    [SerializeField]
    private TMP_Text enemiesMissedDefeat;

    [SerializeField]
    private GameObject returnMainMenuWindow;

    [Tooltip("The time between spawning enemies")] 
    [SerializeField]
    private float timeBetweenSpawns;

    // The flag that states that a round was won previously and the next level needs to be set up.
    private bool oneRoundAlreadyWon = false;

    // The current enemy index that should be spawned together
    private EnemyType enemyType;

    // The current number of the enemies that should be spawned at the same time that remain
    private int enemySpawnNumber = 0;

    // The current weakness of the enemies
    private ResistenceAndWeaknessType weakness = ResistenceAndWeaknessType.None;

    // The current resistance of the enemies;
    private ResistenceAndWeaknessType resistance = ResistenceAndWeaknessType.None;

    // The number of enemy categories that are non empty at the moment
    private int numberOfCategoriesNotEmpty = 0;

    private readonly System.Random _random = new System.Random();

    public static Level Instance;

    /// <summary>
    /// Get a random number in the given range
    /// </summary>
    public int RandomNumber(int min, int max)  
    {  
        return _random.Next(min, max);  
    }  

    // Start is called before the first frame update
    void Start()
    {
        ResetLevelInfo();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the health points of the castle reach 0 health points
        if(GameAdvancement.castleCurrentHP <= 0 && LevelInfo.newLevelStarted == false)
        {
            ActivateDefeatScreen();
        }

        // Check if the new level started flag is true and the round already won flag false
        if(LevelInfo.newLevelStarted == true && oneRoundAlreadyWon == false)
        {
            // Reset the new level started flag
            LevelInfo.newLevelStarted = false;
        }

        // Check if a round was already won
        if(oneRoundAlreadyWon == true && LevelInfo.newLevelStarted == true)
        {
            ResetLevelInfo();
            oneRoundAlreadyWon = false;
            LevelInfo.newLevelStarted = false;
        } else {
            if(LevelInfo.numberOfUndefeatedEnemies == 0)
            {
                if(GameAdvancement.currentWave < LevelInfo.numberOfWaves)
                {
                    SetupNextWave();

                } else {
                    ActivateVictoryScreen();
                }
            }
        }
    }

    /// <summary>
    /// Setup to make sure the next wave can spawn
    /// </summary>
    public void SetupNextWave()
    {
        LevelInfo.waveOngoing = false;
        Debug.Log("The wave is not ongoing anymore");
        if(GameAdvancement.currentWave != 0)
        {
            startNextWave.gameObject.SetActive(true);
        }
        LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfEnemies[GameAdvancement.currentWave];
    }

    public void StartWave()
    {
        LevelInfo.numberOfEnemiesDefeatedOrReachedCastleCurrentWave = 0;
        GameAdvancement.currentWave++;
        GameSetup.UpdateWaveDisplay();
        LevelInfo.waveOngoing = true;
        Debug.Log("The wave is now ongoing");
        StartCoroutine(SpawnWave());
    }

    /// <summary>
    /// Set the enemy number display to Enemy: numberOfEnemiesDefeatedOrReachedCastleCurrentWave / numberOfEnemyInCurrentWave
    /// </summary>
    public void UpdateEnemyNumberDisplay()
    {
        enemyNumberDisplay.text = $"Enemy {LevelInfo.numberOfEnemiesDefeatedOrReachedCastleCurrentWave}/{LevelInfo.numberOfEnemies[GameAdvancement.currentWave - 1]}";
    }

    // Spawns an opponent and waits for a time before the next spawn
    private IEnumerator SpawnWave()
    {
        // Set the right number of categories not empty
        SetNumberOfCategoriesNotEmpty(LevelInfo.normalEnemies[GameAdvancement.currentWave - 1], LevelInfo.fastEnemies[GameAdvancement.currentWave - 1], LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1], LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.tankEnemies[GameAdvancement.currentWave - 1], LevelInfo.slowEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1]);
        
        Debug.Log("Spawning wave number: " + GameAdvancement.currentWave);

        Debug.Log("The number of enemies in the wave is: " + LevelInfo.numberOfEnemies[GameAdvancement.currentWave - 1]);

        Debug.Log("The number of normal enemies in the wave is: " + LevelInfo.normalEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of fast enemies in the wave is: " + LevelInfo.fastEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of super fast enemies in the wave is: " + LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of flying enemies in the wave is: " + LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of tank enemies in the wave is: " + LevelInfo.tankEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of slow enemies in the wave is: " + LevelInfo.slowEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of berzerker enemies in the wave is: " + LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of berzerker flying enemies in the wave is: " + LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1]);
        Debug.Log("The number of berzerker tank enemies in the wave is: " + LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1]);

        UpdateEnemyNumberDisplay();

        // Spawn the whole wave
        for (int counter = LevelInfo.numberOfEnemies[GameAdvancement.currentWave - 1]; counter > 0; counter--)
        {
            // Check if currently there is no group of enemy that should be spawned
            if(enemySpawnNumber == 0)
            {
                // Choose a new type of enemy that should be spawned
                int spawnNumber = SetNextEnemyType(LevelInfo.normalEnemies[GameAdvancement.currentWave - 1], LevelInfo.fastEnemies[GameAdvancement.currentWave - 1], LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1], LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.tankEnemies[GameAdvancement.currentWave - 1], LevelInfo.slowEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1]);
                enemySpawnNumber = spawnNumber;
                Debug.Log("The new enemy's type was set to: " + enemyType);
            }
            // Check if currently a group of enemy should be spawned
            if(enemySpawnNumber > 0)
            {
                Enemy enemy = SpawnEnemyType(enemyType);

                enemy.Resistance = resistance;
                enemy.Weakness = weakness;

                // Reduce the number of enemies that should spawn as a group
                enemySpawnNumber--;
                enemy.transform.parent = Waypoints.enemySpawn.transform;
                enemy.transform.rotation = Board.gameBoard.transform.rotation;
                enemy.transform.position = Waypoints.mapWaypoints[0].transform.position + enemy.transform.up * enemy.FlyingHeight;
            }

            float timeWaited = 0;

            // Wait until the time waited equals the duration of the slow time card
            while(timeWaited <= timeBetweenSpawns)
            {
                yield return new WaitForSeconds(0.1f);

                // Check that the game is not paused, the time not stopped, and the board visible
                if(GameAdvancement.gamePaused == false && GameAdvancement.timeStopped == false && Board.boardVisible)
                {
                    timeWaited += 0.1f;
                }
            }
        }
    }

    // Sets the number of categories not empty variable
    private void SetNumberOfCategoriesNotEmpty(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        int number = 0;
        // Check if the categories are not empty

        if(CounterE1 > 0)
        {
            number++;
        }
        if(CounterE2 > 0)
        {
            number++;
        }
        if(CounterE3 > 0)
        {
            number++;
        }
        if(CounterE4 > 0)
        {
            number++;
        }
        if(CounterE5 > 0)
        {
            number++;
        }
        if(CounterE6 > 0)
        {
            number++;
        }
        if(CounterE7 > 0)
        {
            number++;
        }
        if(CounterE8 > 0)
        {
            number++;
        }
        if(CounterE9 > 0)
        {
            number++;
        }

        // Set the number of categories that is not empty
        numberOfCategoriesNotEmpty = number;
        Debug.Log("*****************The number of categories not empty is: " + number);
    }

    /// <summary>
    /// Determines the next type of enemies that should be spawned
    /// </summary>
    public int SetNextEnemyType(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        // Get a random number
        int newCategoryIndex = RandomNumber(0, numberOfCategoriesNotEmpty);

        // Get another random number and set the enemy spawn number to it
        int newEnemySpawnNumber = RandomNumber(1, 3) + (GameAdvancement.currentWave - 1);

        // Check that there are enough enemies in the wave
        if(newEnemySpawnNumber > LevelInfo.numberOfUndefeatedEnemies)
        {
            newEnemySpawnNumber = LevelInfo.numberOfUndefeatedEnemies;
        }
        
        // Get the right enemy type, and reduce the counter and number of enemies accordingly
        if(CounterE1 != 0)
        {
            if(newCategoryIndex == 0)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Normal;

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE1 > newEnemySpawnNumber)
                {
                    LevelInfo.normalEnemies[GameAdvancement.currentWave - 1] = CounterE1 - newEnemySpawnNumber;
                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE1;
                    LevelInfo.normalEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }

        if(CounterE2 != 0)
        {
            if(newCategoryIndex == 1)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Fast;
                Debug.Log("The counter of the fast enemies is: " + CounterE2 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE2 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.fastEnemies[GameAdvancement.currentWave - 1] = CounterE2 - newEnemySpawnNumber;
                } else {
                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE2;
                    LevelInfo.fastEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }
        if(CounterE3 != 0)
        {
            if(newCategoryIndex == 2)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.SuperFast;
                Debug.Log("The counter of the super fast enemies is: " + CounterE3 + " and will be reduced by at most: " + newEnemySpawnNumber);
                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE3 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1] = CounterE3 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE3;
                    LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }
        if(CounterE4 != 0)
        {
            if(newCategoryIndex == 3)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Flying;

                Debug.Log("The counter of the flying enemies is: " + CounterE4 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE4 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1] = CounterE4 - newEnemySpawnNumber;
                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE4;
                    LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }
        if(CounterE5 != 0)
        {
            if(newCategoryIndex == 4)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Tank;

                Debug.Log("The counter of the tank enemies is: " + CounterE5 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE5 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.tankEnemies[GameAdvancement.currentWave - 1] = CounterE5 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE5;
                    LevelInfo.tankEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }

        if(CounterE6 != 0)
        {
            if(newCategoryIndex == 5)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Slow;

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE6 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.slowEnemies[GameAdvancement.currentWave - 1] = CounterE6 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE6;
                    LevelInfo.slowEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }

        if(CounterE7 != 0)
        {
            if(newCategoryIndex == 6)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Berzerker;

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE7 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1] = CounterE7 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE7;
                    LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex++;
        }

        if(CounterE8 != 0)
        {
            if(newCategoryIndex == 7)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.Flying;

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE8 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1] = CounterE8 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE8;
                    LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        } else {
            newCategoryIndex++;
        }

        if(CounterE9 != 0)
        {
            if(newCategoryIndex == 8)
            {
                // Set the enemy type correctly
                enemyType = EnemyType.BerzerkerTank;

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE9 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1] = CounterE9 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE9;
                    LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1] = 0;
                    numberOfCategoriesNotEmpty--;
                }
            }
        }
        // Initialize the two maximal numbers that state if the enemies should have a weakness and resistance or not
        int weaknessRandom = 6;
        int resistanceRandom = 8;

        // Depending on the current wave, choose what maximal random it could be
        switch(GameAdvancement.currentWave)
        {
            case 1:
            break;

            case 2:
                weaknessRandom = 7;
                resistanceRandom = 9;
            break;

            case 3:
                weaknessRandom = 7;
                resistanceRandom = 9;
            break;

            case 4:
                weaknessRandom = 8;
                resistanceRandom = 10;
            break;

            case 5:
                weaknessRandom = 8;
                resistanceRandom = 10;
            break;

            case 6:
                weaknessRandom = 9;
                resistanceRandom = 11;
            break;

            case 7:
                weaknessRandom = 9;
                resistanceRandom = 11;
            break;

            case 8:
                weaknessRandom = 10;
                resistanceRandom = 12;
            break;

            case 9:
                weaknessRandom = 10;
                resistanceRandom = 12;
            break;

            case 10:
                weaknessRandom = 10;
                resistanceRandom = 12;
            break;

            case 11:
                weaknessRandom = 10;
                resistanceRandom = 12;
            break;
        }

        // Get a random number to decide if the enemies should have a resistance or not
        int resistanceOrNot = RandomNumber(0, resistanceRandom);

        // If the number is greater than 6, give a resistance and a weakness
        if(resistanceOrNot > 6)
        {
            // Initialize the random number for the resitance
            int randomNumberResistance = RandomNumber(0, 4);
            resistance = GetRandomAttackType(randomNumberResistance);
            // Initialize the random number for the weakness
            int randomNumberWeakness = RandomNumber(0, 3);

            if(randomNumberResistance == randomNumberWeakness)
            {
                randomNumberWeakness++;
            }
            weakness = GetRandomAttackType(randomNumberWeakness);

        } else {

            resistance = ResistenceAndWeaknessType.None;

            // Get a random number to decide if the enemies should have a weakness or not
            int weaknessOrNot = enemySpawnNumber = RandomNumber(0, weaknessRandom);

            // If the number is greater than 5, give a weakness
            if(weaknessOrNot > 5)
            {
                // Initialize the random number for the resitance
                int randomNumberWeakness = RandomNumber(0, 4);
                weakness = GetRandomAttackType(randomNumberWeakness);

            } else {
                weakness = ResistenceAndWeaknessType.None;
            }
        }

        return newEnemySpawnNumber;
    }


    /// <summary>
    /// Get a damage type with a given number
    /// </summary>
    public ResistenceAndWeaknessType GetRandomAttackType(int randomNumber)
    {
        // Depending on that number, return a type
        switch(randomNumber)
        {
            case 0:
                return ResistenceAndWeaknessType.Archer;
            case 1:
                return ResistenceAndWeaknessType.Fire;
            case 2:
                return ResistenceAndWeaknessType.Earth;
            case 3:
                return ResistenceAndWeaknessType.Lighting;
            case 4:
                return ResistenceAndWeaknessType.Wind;
            default:
                return ResistenceAndWeaknessType.Archer;
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------
    // Creating all the wave informations
    //---------------------------------------------------------------------------------------------------------------------------------

    public void CreateLevelInformation()
    {
        // -------------------------------------------------------------------------------------------------------
        // Generate the first wave
        // -------------------------------------------------------------------------------------------------------

        // Get a random number between 10 and 20 and set the number of enemies to this number
        int numberOfEnemiesInTheWave = RandomNumber(10, 20);
        LevelInfo.numberOfEnemies[0] = numberOfEnemiesInTheWave;

        // Save the number of enemies that do not have a category
        int enemiesWithoutCategory = numberOfEnemiesInTheWave;

        // A high number of enemies in the first wave should be normal enemies
        int normalEnemies = RandomNumber(10, numberOfEnemiesInTheWave);
        LevelInfo.normalEnemies[0] = normalEnemies;

        // Set the number of enemies without category correctly
        enemiesWithoutCategory -= normalEnemies;

        int enemyIndex;
        int numberOfEnemiesInNewCategory;

        // For as long as there are enemies, choose what enemy should be added and the number
        while(enemiesWithoutCategory > 0)
        {
            // Generate a random number between 0 and 3
            enemyIndex = RandomNumber(0, 3);

            // Generate a random number between 1 and the number of enemies without category
            numberOfEnemiesInNewCategory = RandomNumber(1, enemiesWithoutCategory);
            switch(enemyIndex)
            {
                case 0:
                    LevelInfo.normalEnemies[0] = LevelInfo.normalEnemies[0] + numberOfEnemiesInNewCategory;
                break;

                case 1:
                    LevelInfo.fastEnemies[0] = LevelInfo.fastEnemies[0] + numberOfEnemiesInNewCategory;
                break;

                case 2:
                    LevelInfo.flyingEnemies[0] = LevelInfo.flyingEnemies[0] + numberOfEnemiesInNewCategory;
                break;

                case 3:
                    LevelInfo.tankEnemies[0] = LevelInfo.tankEnemies[0] + numberOfEnemiesInNewCategory;
                break;
            }

            // Reduce the number of enemies without category by the number of enemies that were but in a category
            enemiesWithoutCategory -= numberOfEnemiesInNewCategory;
        }

        // -------------------------------------------------------------------------------------------------------
        // Generate the other waves
        // -------------------------------------------------------------------------------------------------------

        // Generate the number of waves - 1 other waves
        for(int index = 1; index < LevelInfo.numberOfWaves; index++)
        {
            // Set the number of enemies in the wave
            numberOfEnemiesInTheWave = RandomNumber(10, 20) + index * numberOfAdditionalEnemiesPerWave;
            LevelInfo.numberOfEnemies[index] = numberOfEnemiesInTheWave;

            // Save the number of enemies that do not have a category
            enemiesWithoutCategory = numberOfEnemiesInTheWave;

            // For as long as there are enemies, choose what enemy should be added and the number
            while(enemiesWithoutCategory > 0)
            {
                // Not all enemies should be added at the second wave, only standard berzerker enemy
                if(index == 1)
                {
                    enemyIndex = RandomNumber(0, 9);

                } else {
                    enemyIndex = RandomNumber(0, 11);
                }

                // Generate a random number between 1 and the number of enemies without category
                numberOfEnemiesInNewCategory = RandomNumber(1, enemiesWithoutCategory);

                // Add this number of enemies in the right category. The probability it is a normal enemy is higher than the rest
                switch(enemyIndex)
                {
                    case 0:
                        LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 1:
                        LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 2:
                        LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 3:
                        LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 4:
                        LevelInfo.fastEnemies[index] = LevelInfo.fastEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 5:
                        LevelInfo.superFastEnemies[index] = LevelInfo.superFastEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 6:
                        LevelInfo.flyingEnemies[index] = LevelInfo.flyingEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 7:
                        LevelInfo.tankEnemies[index] = LevelInfo.tankEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 8:
                        LevelInfo.slowEnemies[index] = LevelInfo.slowEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 9:
                        LevelInfo.berzerkerEnemies[index] = LevelInfo.berzerkerEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 10:
                        LevelInfo.berzerkerFlyingEnemies[index] = LevelInfo.berzerkerFlyingEnemies[index] + numberOfEnemiesInNewCategory;
                    break;

                    case 11:
                        LevelInfo.berzerkerTankEnemies[index] = LevelInfo.berzerkerTankEnemies[index] + numberOfEnemiesInNewCategory;
                    break;
                }

                // Reduce the number of enemies without category by the number of enemies that were but in a category
                enemiesWithoutCategory -= numberOfEnemiesInNewCategory;
            }
            
        }
    }

    private void SetNumberOfWaves()
    {
        // Check how many questions there are
        if(Questions.lastQuestionIndex + 1 <= 7)
        {
            LevelInfo.numberOfWaves = 1;

        } else if(Questions.lastQuestionIndex + 1 <= 14) 
        {
            LevelInfo.numberOfWaves = 2;

        } else if(Questions.lastQuestionIndex + 1 <= 21) 
        {
            LevelInfo.numberOfWaves = 3;

        } else if(Questions.lastQuestionIndex + 1 <= 28) 
        {
            LevelInfo.numberOfWaves = 4;

        } else if(Questions.lastQuestionIndex + 1 <= 35) 
        {
            LevelInfo.numberOfWaves = 5;

        } else if(Questions.lastQuestionIndex + 1 <= 42) 
        {
            LevelInfo.numberOfWaves = 6;

        } else if(Questions.lastQuestionIndex + 1 <= 49) 
        {
            LevelInfo.numberOfWaves = 7;

        } else if(Questions.lastQuestionIndex + 1 <= 56) 
        {
            LevelInfo.numberOfWaves = 8;

        } else if(Questions.lastQuestionIndex + 1 <= 63) 
        {
            LevelInfo.numberOfWaves = 9;

        } else if(Questions.lastQuestionIndex + 1 <= 70) 
        {
            LevelInfo.numberOfWaves = 10;

        } else if(Questions.lastQuestionIndex + 1 > 70) 
        {
            LevelInfo.numberOfWaves = 11;
        }
    }

    // -------------------------------------------------------------------------------------------------------
    // Level finished methods
    // -------------------------------------------------------------------------------------------------------

    private void ActivateVictoryScreen()
    {
        LevelInfo.waveOngoing = false;
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag ("Tower");
 
        // Disable all towers
        foreach(GameObject tower in towerArray)
        {
            if(tower.activeSelf == true)
            {
                ObjectPools.ReleaseTower(tower);
            }
        }
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag ("Trap");

        // Disable all traps
        foreach(GameObject trap in trapArray)
        {
            if(trap.activeSelf == true)
            {
                ObjectPools.ReleaseTrap(trap.GetComponent<Trap>());
            }
        }

        // Reset all spell cards so that they are not drawn
        GameObject[] spellArray = GameObject.FindGameObjectsWithTag("Spell Card");
        foreach(GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCardController>().ResetSpellCard();
        }

        ActivateQuestions.ResetQuestionMenuWindows();
        SpellCardManager.ResetSpellCardDeck();
        victoryScreen.SetActive(true);

        // Check if the castle had any armor points and update the text of the counter
        if(GameAdvancement.castleCurrentAP > 0)
        {
            castleHealthCounter.text = "Castle health bar:  " + GameAdvancement.castleCurrentHP + " / " + GameAdvancement.castleMaxHP + " + " + GameAdvancement.castleCurrentAP + " armor points";
        
        } else {
            castleHealthCounter.text = "Castle health bar:  " + GameAdvancement.castleCurrentHP + " / " + GameAdvancement.castleMaxHP;
        }
        enemiesDefeatedVictory.text = "Number of enemies defeated: " + (LevelInfo.numberOfEnemiesDefeated);
        enemiesMissedVictory.text = "Number of enemies missed: " + LevelInfo.numberOfEnemiesThatReachedTheCastle;
        oneRoundAlreadyWon = true;
    }

    public void ActivateDefeatScreen()
    {
        LevelInfo.waveOngoing = false;

        // Disable all towers
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towerArray)
        {
            if (tower.activeSelf == true)
            {
                ObjectPools.ReleaseTower(tower);
            }
        }

        // Disable all Enemies
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyArray)
        {
            if (enemy.activeSelf == true)
            {
                ObjectPools.ReleaseEnemy(enemy.GetComponent<Enemy>());
            }
        }

        // Reset all spell cards so that they are not drawn
        GameObject[] spellArray = GameObject.FindGameObjectsWithTag("Spell Card");
        foreach (GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCardController>().ResetSpellCard();
        }

        ActivateQuestions.ResetQuestionMenuWindows();
        SpellCardManager.ResetSpellCardDeck();
        defeatScreen.SetActive(true);

        enemiesDefeatedDefeat.text = "Number of enemies defeated: " + (LevelInfo.numberOfEnemiesDefeated);
        enemiesMissedDefeat.text = "Number of enemies missed: " + LevelInfo.numberOfEnemiesThatReachedTheCastle;

        oneRoundAlreadyWon = true;
        Board.gameBoard.SetActive(false);
    }

    // Get the total number of enemies defeated in all waves
    private int CalculateNumberOfEnemies()
    {
        int counter = 0;

        // Go over all waves
        for(int index = 0; index < LevelInfo.numberOfWaves; index = index + 1)
        {
            counter += LevelInfo.numberOfEnemies[index];
        }
        return counter;
    }

    private void ResetLevelInfo()
    {

        // Reset all spell cards so that they are not drawn
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");
        foreach(GameObject enemy in enemyArray)
        {
            ObjectPools.ReleaseEnemy(enemy.GetComponent<Enemy>());
        }

        SetNumberOfWaves();
        GameAdvancement.currentWave = 0;
        GameAdvancement.gamePaused = false;
        LevelInfo.waveOngoing = false;

        // Initialize the arrays
        LevelInfo.numberOfEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.normalEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.fastEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.superFastEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.flyingEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.tankEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.slowEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.berzerkerEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.berzerkerFlyingEnemies = new int[LevelInfo.numberOfWaves];
        LevelInfo.berzerkerTankEnemies = new int[LevelInfo.numberOfWaves];

        // Set the right additional number of enemies per wave
        LevelInfo.numberOfAdditionalEnemiesPerWave = numberOfAdditionalEnemiesPerWave;

        // Create level information given the number of question
        CreateLevelInformation();

        enemyType = EnemyType.Normal;
        enemySpawnNumber = 5;

        // Reduce the counter of normal enemies in the first wave by five
        LevelInfo.normalEnemies[0] = LevelInfo.normalEnemies[0] - 5;
        LevelInfo.numberOfEnemiesThatReachedTheCastle = 0;
        LevelInfo.numberOfEnemiesDefeated = 0;
        LevelInfo.numberOfEnemiesDefeatedOrReachedCastleCurrentWave = 0;
    }

    /// <summary>
    /// Open the return to main menu window when wanting to abandon the level
    /// </summary>
    public void OpenReturnMainMenuScreen()
    {
        returnMainMenuWindow.SetActive(true);
        GameAdvancement.gamePaused = true;
    }

    /// <summary>
    /// Close (cancel) the return to main menu window
    /// </summary>
    public void CloseReturnMainMenuScreen()
    {
        returnMainMenuWindow.SetActive(false);
        GameAdvancement.gamePaused = false;
    }
    
    public void PauseGame()
    {
        GameAdvancement.gamePaused = true;
        pauseButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        GameAdvancement.gamePaused = false;
        pauseButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    
}