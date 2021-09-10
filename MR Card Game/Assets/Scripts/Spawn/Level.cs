using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static i5.Toolkit.Core.Examples.Spawners.SpawnEnemy;
using UnityEngine.EventSystems;
using TMPro;

static class LevelInfo
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

    // The flag that states that a new level was started and the level information need to be reset
    public static bool newLevelStarted = false;
}

public class Level : MonoBehaviour
{
    // The number of additional enemies per wave
    [SerializeField]
    private int numberOfAdditionalEnemiesPerWave;

    // The start next wave button
    [SerializeField]
    private Button startNextWave;

    // The current enemy index that should be spawned together
    private string enemyType = "";

    // The current number of the enemies that should be spawned at the same time that remain
    private int enemySpawnNumber = 0;

    // The current weakness of the enemies
    private string weakness = "";

    // The current resistance of the enemies;
    private string resistance = "";

    // The number of enemy categories that are non empty at the moment
    private int numberOfCategoriesNotEmpty = 0;

    // The time between spawning enemies
    [SerializeField]
    private float timeBetweenSpawns;

    // The current time since the last spawn
    private float spawnTimer;

    // The boolean variable stating if a unit can spawn or not
    private bool canSpawn = true;

    // Instantiate random number generator.  
    private readonly System.Random _random = new System.Random();  
    
    // Generates a random number within a range.      
    public int RandomNumber(int min, int max)  
    {  
        return _random.Next(min, max);  
    }  

    // Start is called before the first frame update
    void Start()
    {
        // Reset the level info
        ResetLevelInfo();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the health points of the castle reach 0 health points
        if(GameAdvancement.castlecurrentHP <= 0 && LevelInfo.newLevelStarted == false)
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
            // Reset the level info
            ResetLevelInfo();

            // Set the one round already won flag to false
            oneRoundAlreadyWon = false;

            LevelInfo.newLevelStarted = false;

        } else {
            // Check if the current wave is smaller than the number of waves and if the number of undefeated enemies is 0
            if(LevelInfo.numberOfUndefeatedEnemies == 0)
            {
                if(GameAdvancement.currentWave < LevelInfo.numberOfWaves)
                {
                    // Make the next wave setup
                    MakeNextWaveSetup();

                } else {
                    ActivateVictoryScreen();
                }
            }
        }
    }

    // The method that does the setup to make sure the next wave can spawn
    public void MakeNextWaveSetup()
    {
        // Set the wave ongoing flag to false
        LevelInfo.waveOngoing = false;

        Debug.Log("The wave is not ongoing anymore");

        // Check that it is not the first wave
        if(GameAdvancement.currentWave != 0)
        {
            // Activate the start next wave button
            startNextWave.gameObject.SetActive(true);
        }

        // Set the number of undefeated enemies to the number of enemies in the wave
        LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfEnemies[GameAdvancement.currentWave];

        // // Unground all the buildings now that the wave is finished
        // UngroundAllBuildings();
    }

    // The method that starts a wave
    public void StartWave()
    {
        // // Ground all buildings when the wave begins
        // GroundAllBuildings();

        // Increase the current wave number
        GameAdvancement.currentWave = GameAdvancement.currentWave + 1;

        // Actualize the wave display
        GameSetup.ActualizeWaveDisplay();

        // Set the wave ongoing flag to true
        LevelInfo.waveOngoing = true;

        Debug.Log("The wave is now ongoing");

        // Start the coroutine that spawns all the wave
        StartCoroutine(SpawnWave());
    }

    // // Function that is used to test when the game is not paused anymore
    // private bool GameNotPaused()
    // {
    //     return GameAdvancement.gamePaused == false;
    // }

    // // Function that is used to test when the game is not paused anymore
    // private bool TimeNotStopped()
    // {
    //     return GameAdvancement.timeStopped == false;
    // }

    // The coroutine that spawns an opponent and waits for a time before the next spawn
    IEnumerator SpawnWave()
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

        // Spawn the whole wave
        for(int counter = LevelInfo.numberOfEnemies[GameAdvancement.currentWave - 1]; counter > 0; counter = counter - 1)
        {
            // Check if currently there is no group of enemy that should be spawned
            if(enemySpawnNumber == 0)
            {
                // Choose a new type of enemy that should be spawned
                int spawnNumber = SetNextEnemyType(LevelInfo.normalEnemies[GameAdvancement.currentWave - 1], LevelInfo.fastEnemies[GameAdvancement.currentWave - 1], LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1], LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.tankEnemies[GameAdvancement.currentWave - 1], LevelInfo.slowEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1]);
                
                // Set the enemy spawn number to the number returned by the set next enemy type method
                enemySpawnNumber = spawnNumber;

                Debug.Log("The enemy spawn number was set to: " + spawnNumber);

                Debug.Log("The new enemy type was set to: " + enemyType);
            }

            Debug.Log("The enemy spawn number is: " + enemySpawnNumber);

            // Check if currently a group of enemy should be spawned
            if(enemySpawnNumber > 0)
            {
                // Spawn enemy of the type given
                Enemy enemy = SpawnAnEnemy(enemyType);

                // Set the resistance of the enemy
                enemy.resistance = resistance;

                // Set the weakness of the enemy
                enemy.weakness = weakness;

                // Reduce the number of enemies that should spawn as a group
                enemySpawnNumber = enemySpawnNumber - 1;

                // // Reset the flag that an enemy can spawn
                // canSpawn = false;

                // Set them as children of the game board
                enemy.transform.parent = Waypoints.enemySpawn.transform;

                // Rotate the enemies like the game board
                enemy.transform.rotation = Board.gameBoard.transform.rotation;

                // Set the position of the child to the position of the parent object
                enemy.transform.position = (Waypoints.mapWaypoints[0].transform.position + enemy.transform.up * enemy.GetFlightHeight);;
            }

            // // Check that the stop time card is not taking effect
            // if(GameAdvancement.timeStopped == true)
            // {
            //     // // Make sure that no enemy is spawned in the duration of this stop time card
            //     // yield return new WaitForSeconds(SpellCard.getStopTimeDuration);

            //     // Initialize the time waited variable
            //     float timeWaited = 0;

            //     // Wait until the time waited equals the duration of the stop time card
            //     while(timeWaited != SpellCard.getStopTimeDuration)
            //     {
            //         // Wait for 0.1 seconds
            //         yield return new WaitForSeconds(0.1f);

            //         // Check that the game is not paused
            //         if(GameNotPaused() == true)
            //         {
            //             // Increase the time waited by 0.1 seconds
            //             timeWaited = timeWaited + 0.1f;
            //         }
            //     }
            // }

            // Initialize the time waited variable
            float timeWaited = 0;

            // Wait until the time waited equals the duration of the slow time card
            while(timeWaited <= timeBetweenSpawns)
            {
                // Wait for 0.1 seconds
                yield return new WaitForSeconds(0.1f);

                // Check that the game is not paused, the time not stopped, and the board visible
                if(GameAdvancement.gamePaused == false && GameAdvancement.timeStopped == false && Board.boardVisible == true)
                {
                    // Increase the time waited by 0.1 seconds
                    timeWaited = timeWaited + 0.1f;
                }
            }

            // // Wait for the game not being paused
            // yield return new WaitUntil(GameNotPaused);

            // // Wait for the time between spawns
            // yield return new WaitForSeconds(timeBetweenSpawns);

            // // Wait until the time stopped card lost effect
            // yield return new WaitUntil(TimeNotStopped);
        }
    }

    // The method that sets the number of categories not empty variable
    private void SetNumberOfCategoriesNotEmpty(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        // Initialize the number
        int number = 0;

        // Check if the first category is non empty
        if(CounterE1 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the second category is non empty
        if(CounterE2 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the third category is non empty
        if(CounterE3 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the fourth category is non empty
        if(CounterE4 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the fifth category is non empty
        if(CounterE5 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the sixth category is non empty
        if(CounterE6 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the seventh category is non empty
        if(CounterE7 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the eighth category is non empty
        if(CounterE8 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Check if the ninth category is non empty
        if(CounterE9 > 0)
        {
            // Increase the number by one
            number = number + 1;
        }

        // Set the number of categories that is not empty
        numberOfCategoriesNotEmpty = number;

        Debug.Log("*****************The number of categories not empty is: " + number);
    }

    // Method that determines the next type of enemies that should be spawned
    public int SetNextEnemyType(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        // Get a random number
        int newCategoryIndex = RandomNumber(0, numberOfCategoriesNotEmpty);

        // Get another random number and set the enemy spawn number to it
        int newEnemySpawnNumber = RandomNumber(1, 3) + (GameAdvancement.currentWave - 1);

        // Check that there are enough enemies in the wave
        if(newEnemySpawnNumber > LevelInfo.numberOfUndefeatedEnemies)
        {
            // Set the enemy spawn number to the number of undefeated enemies
            newEnemySpawnNumber = LevelInfo.numberOfUndefeatedEnemies;
        }
        
        // Get the right enemy type, and reduce the counter and number of enemies accordingly
        if(CounterE1 != 0)
        {
            if(newCategoryIndex == 0)
            {
                // Set the enemy type correctly
                enemyType = "Normal Enemy";

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE1 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.normalEnemies[GameAdvancement.currentWave - 1] = CounterE1 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE1;

                    // Set the counter to 0
                    LevelInfo.normalEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE2 != 0)
        {
            if(newCategoryIndex == 1)
            {
                // Set the enemy type correctly
                enemyType = "Fast Enemy";
                Debug.Log("The counter of the fast enemies is: " + CounterE2 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE2 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.fastEnemies[GameAdvancement.currentWave - 1] = CounterE2 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE2;

                    // Set the counter to 0
                    LevelInfo.fastEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE3 != 0)
        {
            if(newCategoryIndex == 2)
            {
                // Set the enemy type correctly
                enemyType = "Super Fast Enemy";

                Debug.Log("The counter of the super fast enemies is: " + CounterE3 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE3 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1] = CounterE3 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE3;

                    // Set the counter to 0
                    LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE4 != 0)
        {
            if(newCategoryIndex == 3)
            {
                // Set the enemy type correctly
                enemyType = "Flying Enemy";

                Debug.Log("The counter of the flying enemies is: " + CounterE4 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE4 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1] = CounterE4 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE4;

                    // Set the counter to 0
                    LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE5 != 0)
        {
            if(newCategoryIndex == 4)
            {
                // Set the enemy type correctly
                enemyType = "Tank Enemy";

                Debug.Log("The counter of the tank enemies is: " + CounterE5 + " and will be reduced by at most: " + newEnemySpawnNumber);

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE5 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.tankEnemies[GameAdvancement.currentWave - 1] = CounterE5 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE5;

                    // Set the counter to 0
                    LevelInfo.tankEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE6 != 0)
        {
            if(newCategoryIndex == 5)
            {
                // Set the enemy type correctly
                enemyType = "Slow Enemy";

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE6 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.slowEnemies[GameAdvancement.currentWave - 1] = CounterE6 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE6;

                    // Set the counter to 0
                    LevelInfo.slowEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE7 != 0)
        {
            if(newCategoryIndex == 6)
            {
                // Set the enemy type correctly
                enemyType = "Berzerker Enemy";

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE7 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1] = CounterE7 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE7;

                    // Set the counter to 0
                    LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE8 != 0)
        {
            if(newCategoryIndex == 7)
            {
                // Set the enemy type correctly
                enemyType = "Berzerker Flying Enemy";

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE8 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1] = CounterE8 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE8;

                    // Set the counter to 0
                    LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE9 != 0)
        {
            if(newCategoryIndex == 8)
            {
                // Set the enemy type correctly
                enemyType = "Berzerker Tank Enemy";

                // Check if the counter of remaining enemies is higher than the enemy spawn number that was drawn before
                if(CounterE9 > newEnemySpawnNumber)
                {
                    // Reduce the counter by the enemy spawn number
                    LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1] = CounterE9 - newEnemySpawnNumber;

                } else {

                    // Set the enemy spawn number to the counter
                    newEnemySpawnNumber = CounterE9;

                    // Set the counter to 0
                    LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1] = 0;

                    // Reduce the number of categories not empty by one
                    numberOfCategoriesNotEmpty = numberOfCategoriesNotEmpty - 1;
                }
            }
        }

        // Reduce the number of enemies in the category


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

            // Give resistance
            resistance = GetRandomType(randomNumberResistance);

            // Initialize the random number for the weakness
            int randomNumberWeakness = RandomNumber(0, 3);

            // If the random number for the resistance is the same as the one for the weakness, add one
            if(randomNumberResistance == randomNumberWeakness)
            {
                randomNumberWeakness = randomNumberWeakness + 1;
            }

            // Give weakness
            weakness = GetRandomType(randomNumberWeakness);

        } else {

            // Delete the resistance
            resistance = "";

            // Get a random number to decide if the enemies should have a weakness or not
            int weaknessOrNot = enemySpawnNumber = RandomNumber(0, weaknessRandom);

            // If the number is greater than 5, give a weakness
            if(weaknessOrNot > 5)
            {
                // Initialize the random number for the resitance
                int randomNumberWeakness = RandomNumber(0, 4);

                // Give resistance
                weakness = GetRandomType(randomNumberWeakness);

            } else {

                // Delete the weakness
                weakness = "";
            }
        }

        return newEnemySpawnNumber;
    }

    // // Method that attributes a resistance to the currently spawned enemies
    // public void GiveResistance()
    // {
    //     // Get a random type and set the resistance to it
    //     resistance = GetRandomType();
    // }

    // // Method that attributes a weakness to the currently spawned enemies
    // public void GiveWeakness()
    // {
    //     // Get a random type and set the weakness to it
    //     weakness = GetRandomType();
    // }

    // Method that returns a random damage type
    public string GetRandomType(int randomNumber)
    {
        // Depending on that number, return a type
        switch(randomNumber)
        {
            case 0:
                return "Piercing";
            break;

            case 1:
                return "Fire";
            break;

            case 2:
                return "Earth";
            break;

            case 3:
                return "Lightning";
            break;

            case 4:
                return "Wind";
            break;
        }

        return "Piercing";
    }

    //---------------------------------------------------------------------------------------------------------------------------------
    // Creating all the wave informations
    //---------------------------------------------------------------------------------------------------------------------------------


    // Method that creates the level information
    public void CreateLevelInformation()
    {
        if(false)
        {
            // For testing
            LevelInfo.numberOfWaves = 1;
            //
            LevelInfo.numberOfEnemies[0] = 29;
            LevelInfo.normalEnemies[0] = 5;
            LevelInfo.fastEnemies[0] = 3;
            LevelInfo.superFastEnemies[0] = 3;
            LevelInfo.flyingEnemies[0] = 3;
            LevelInfo.tankEnemies[0] = 3;
            LevelInfo.slowEnemies[0] = 3;
            LevelInfo.berzerkerEnemies[0] = 3;
            LevelInfo.berzerkerFlyingEnemies[0] = 3;
            LevelInfo.berzerkerTankEnemies[0] = 3;

        } else {
            
            // The real method
            
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
            enemiesWithoutCategory = enemiesWithoutCategory - normalEnemies;

            // Initialize the enemy index and the number of enemies in the new category
            int enemyIndex = 0;
            int numberOfEnemiesInNewCategory = 0;

            // For as long as there are enemies, choose what enemy should be added and the number
            while(enemiesWithoutCategory > 0)
            {
                // Generate a random number between 0 and 3
                enemyIndex = RandomNumber(0, 3);

                // Generate a random number between 1 and the number of enemies without category
                numberOfEnemiesInNewCategory = RandomNumber(1, enemiesWithoutCategory);

                //
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
                enemiesWithoutCategory = enemiesWithoutCategory - numberOfEnemiesInNewCategory;
            }

            // -------------------------------------------------------------------------------------------------------
            // Generate the other waves
            // -------------------------------------------------------------------------------------------------------

            // Generate the number of waves - 1 other waves
            for(int index = 1; index < LevelInfo.numberOfWaves; index = index + 1)
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
                        // Generate a random number between 0 and 10
                        enemyIndex = RandomNumber(0, 9);

                    } else {

                        // Generate a random number between 0 and 12
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
                    enemiesWithoutCategory = enemiesWithoutCategory - numberOfEnemiesInNewCategory;
                }
            }
        }
    }

    private void SetNumberOfWaves()
    {
        // Check how many questions there are
        if(Questions.lastQuestionIndex + 1 <= 7)
        {
            // Set the number of waves to 1
            LevelInfo.numberOfWaves = 1;

        } else if(Questions.lastQuestionIndex + 1 <= 14) 
        {
            // Set the number of waves to 2
            LevelInfo.numberOfWaves = 2;

        } else if(Questions.lastQuestionIndex + 1 <= 21) 
        {
            // Set the number of waves to 3
            LevelInfo.numberOfWaves = 3;

        } else if(Questions.lastQuestionIndex + 1 <= 28) 
        {
            // Set the number of waves to 4
            LevelInfo.numberOfWaves = 4;

        } else if(Questions.lastQuestionIndex + 1 <= 35) 
        {
            // Set the number of waves to 5
            LevelInfo.numberOfWaves = 5;

        } else if(Questions.lastQuestionIndex + 1 <= 42) 
        {
            // Set the number of waves to 6
            LevelInfo.numberOfWaves = 6;

        } else if(Questions.lastQuestionIndex + 1 <= 49) 
        {
            // Set the number of waves to 7
            LevelInfo.numberOfWaves = 7;

        } else if(Questions.lastQuestionIndex + 1 <= 56) 
        {
            // Set the number of waves to 8
            LevelInfo.numberOfWaves = 8;

        } else if(Questions.lastQuestionIndex + 1 <= 63) 
        {
            // Set the number of waves to 9
            LevelInfo.numberOfWaves = 9;

        } else if(Questions.lastQuestionIndex + 1 <= 70) 
        {
            // Set the number of waves to 10
            LevelInfo.numberOfWaves = 10;

        } else if(Questions.lastQuestionIndex + 1 > 70) 
        {
            // Set the number of waves to 11
            LevelInfo.numberOfWaves = 11;

        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------
    // Grounding and ungrounding towers
    //---------------------------------------------------------------------------------------------------------------------------------

    // // Method that grounds the buildings on the game board when the wave starts
    // public void GroundAllBuildings()
    // {
    //     // Ground the first building if it exists
    //     if(Buildings.numberOfBuildings > 0)
    //     {
    //         // Ground the first building
    //         GroundBuilding(Buildings.firstBuilding, 0);

    //         // Ground the second building if it exists
    //         if(Buildings.numberOfBuildings > 1)
    //         {
    //             GroundBuilding(Buildings.secondBuilding, 1);

    //             // Ground the third building if it exists
    //             if(Buildings.numberOfBuildings > 2)
    //             {
    //                 GroundBuilding(Buildings.thirdBuilding, 2);

    //                 // Ground the fourth building if it exists
    //                 if(Buildings.numberOfBuildings > 3)
    //                 {
    //                     GroundBuilding(Buildings.fourthBuilding, 3);

    //                     // Ground the fifth building if it exists
    //                     if(Buildings.numberOfBuildings > 4)
    //                     {
    //                         GroundBuilding(Buildings.fifthBuilding, 4);

    //                         // Ground the sixth building if it exists
    //                         if(Buildings.numberOfBuildings > 5)
    //                         {
    //                             GroundBuilding(Buildings.sixthBuilding, 5);

    //                             // Ground the seventh building if it exists
    //                             if(Buildings.numberOfBuildings > 6)
    //                             {
    //                                 GroundBuilding(Buildings.seventhBuilding, 6);

    //                                 // Ground the eighth building if it exists
    //                                 if(Buildings.numberOfBuildings > 7)
    //                                 {
    //                                     GroundBuilding(Buildings.eighthBuilding, 7);

    //                                     // Ground the ninth building if it exists
    //                                     if(Buildings.numberOfBuildings > 8)
    //                                     {
    //                                         GroundBuilding(Buildings.ninthBuilding, 8);

    //                                         // Ground the tenth building if it exists
    //                                         if(Buildings.numberOfBuildings > 9)
    //                                         {
    //                                             GroundBuilding(Buildings.tenthBuilding, 9);
    //                                         }
    //                                     }
    //                                 }
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    private float additionalOffset = (float)1.5;

    // // The method used to ground buildings
    // public void GroundBuilding(GameObject building, int index)
    // {
    //     // Set the position of the building to the position of the image target
    //     building.transform.position = Buildings.imageTargetToBuilding[index].transform.position;

    //     // Set the building as child of the buildings storage object that is a child of the game board
    //     building.transform.parent = Board.buildingStorage.transform;

    //     Vector3 buildingPosition = building.transform.position;

    //     buildingPosition = new Vector3(buildingPosition.x, Board.castle.transform.position.y, buildingPosition.z);

    //     building.transform.position = buildingPosition;

    //     // // Make sure the tower is on the same height as the castle
    //     // building.transform.position.y = Board.castle.transform.position.y;
    // }

    // // Method that un-grounds all the buildings on the game board when the wave starts
    // public void UngroundAllBuildings()
    // {
    //     // Ground the first building if it exists
    //     if(Buildings.numberOfBuildings > 0)
    //     {
    //         // Ground the first building
    //         UngroundBuilding(Buildings.firstBuilding, 0);

    //         // Ground the second building if it exists
    //         if(Buildings.numberOfBuildings > 1)
    //         {
    //             UngroundBuilding(Buildings.secondBuilding, 1);

    //             // Ground the third building if it exists
    //             if(Buildings.numberOfBuildings > 2)
    //             {
    //                 UngroundBuilding(Buildings.thirdBuilding, 2);

    //                 // Ground the fourth building if it exists
    //                 if(Buildings.numberOfBuildings > 3)
    //                 {
    //                     UngroundBuilding(Buildings.fourthBuilding, 3);

    //                     // Ground the fifth building if it exists
    //                     if(Buildings.numberOfBuildings > 4)
    //                     {
    //                         UngroundBuilding(Buildings.fifthBuilding, 4);

    //                         // Ground the sixth building if it exists
    //                         if(Buildings.numberOfBuildings > 5)
    //                         {
    //                             UngroundBuilding(Buildings.sixthBuilding, 5);

    //                             // Ground the seventh building if it exists
    //                             if(Buildings.numberOfBuildings > 6)
    //                             {
    //                                 UngroundBuilding(Buildings.seventhBuilding, 6);

    //                                 // Ground the eighth building if it exists
    //                                 if(Buildings.numberOfBuildings > 7)
    //                                 {
    //                                     UngroundBuilding(Buildings.eighthBuilding, 7);

    //                                     // Ground the ninth building if it exists
    //                                     if(Buildings.numberOfBuildings > 8)
    //                                     {
    //                                         UngroundBuilding(Buildings.ninthBuilding, 8);

    //                                         // Ground the tenth building if it exists
    //                                         if(Buildings.numberOfBuildings > 9)
    //                                         {
    //                                             UngroundBuilding(Buildings.tenthBuilding, 9);
    //                                         }
    //                                     }
    //                                 }
    //                             }
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    // // The method used to unground buildings
    // public void UngroundBuilding(GameObject building, int index)
    // {
    //     // Reset the position of the building 
    //     building.transform.position = new Vector3(0, 0, 0);

    //     // Set the building as child of the image target
    //     building.transform.parent = Buildings.imageTargetToBuilding[index].transform;
    // }

    // -------------------------------------------------------------------------------------------------------
    // Level finished methods
    // -------------------------------------------------------------------------------------------------------

    // The victory screen game object
    [SerializeField]
    private GameObject victoryScreen;

    // The victory screen game object
    [SerializeField]
    private TMP_Text enemiesDefeatedVictory;

    // The victory screen game object
    [SerializeField]
    private TMP_Text enemiesMissedVictory;

    // The victory screen game object
    [SerializeField]
    private TMP_Text castleHealthCounter;

    // The flag that states that a round was won previously and the next level needs to be set up.
    private bool oneRoundAlreadyWon = false;

    // The method that activates the win screen
    private void ActivateVictoryScreen()
    {
        // Set the wave ongoing flag to false
        LevelInfo.waveOngoing = false;
        
        // Get the array of all tower objects
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag ("Tower");
 
        // Disable all towers
        foreach(GameObject tower in towerArray)
        {
            // Check if the tower is active
            if(tower.activeSelf == true)
            {
                // Release the tower object
                ObjectPools.ReleaseTower(tower);
            }
        }

        // Get the array of all trap objects
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag ("Trap");
 
        // Disable all traps
        foreach(GameObject trap in trapArray)
        {
            // Check if the trap is active
            if(trap.activeSelf == true)
            {
                // Release the trap object
                ObjectPools.ReleaseTrap(trap.GetComponent<Trap>());
            }
        }

        // Reset all spell cards so that they are not drawn
         GameObject[] spellArray = GameObject.FindGameObjectsWithTag ("Spell Card");

        foreach(GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCard>().ResetSpellCard();
        }

        // Reset the activate question class to disable the question menus and reset the number of questions that need to be answered
        ActivateQuestions.ResetQuestionMenuWindows();

        // Reset the spell card deck
        SpellCard.ResetSpellCardDeck();

        // Activate the victory screen
        victoryScreen.SetActive(true);

        // Check if the castle had any armor points
        if(GameAdvancement.castleCurrentAP > 0)
        {
            // Write how many health points and armor points the castle had at the end
            castleHealthCounter.text = "Castle health bar:  " + GameAdvancement.castlecurrentHP + " / " + GameAdvancement.castleMaxHP + " + " + GameAdvancement.castleCurrentAP + " armor points";
        
        } else {

            // Write how many health points the castle had at the end
            castleHealthCounter.text = "Castle health bar:  " + GameAdvancement.castlecurrentHP + " / " + GameAdvancement.castleMaxHP;
        }

        // Get the number of enemies in all waves
        int number = CalculateNumberOfEnemies();

        // Write how many enemies were defeated in all waves
        enemiesDefeatedVictory.text = "Number of enemies defeated: " + (LevelInfo.numberOfEnemiesDefeated);

        // Write how many enemies were missed in all waves
        enemiesMissedVictory.text = "Number of enemies missed: " + LevelInfo.numberOfEnemiesThatReachedTheCastle;

        // Set the flag that a level was already won
        oneRoundAlreadyWon = true;
    }

    // The method used to get the total number of enemies defeated in all waves
    private int CalculateNumberOfEnemies()
    {
        // Initialize the enemy counter variable
        int counter = 0;

        // Go over all waves
        for(int index = 0; index < LevelInfo.numberOfWaves; index = index + 1)
        {
            // Add the number of enemies in that wave
            counter = counter + LevelInfo.numberOfEnemies[index];
        }

        // Return the number of enemies in all waves
        return counter;
    }

    // The method used to reset the level info
    private void ResetLevelInfo()
    {
        // Set the number of waves
        SetNumberOfWaves();

        // Set the current wave to wave 0
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

        // Set the type of enemy to spawn to normal enemy
        enemyType = "Normal Enemy";

        // Set the number of enemies to spawn of that type to 5
        enemySpawnNumber = 5;

        // Reduce the counter of normal enemies in the first wave by five
        LevelInfo.normalEnemies[0] = LevelInfo.normalEnemies[0] - 5;

        // Reset the number of enemies that reach the castle
        LevelInfo.numberOfEnemiesThatReachedTheCastle = 0;

        // Reset the number of enemies that were defeated
        LevelInfo.numberOfEnemiesDefeated = 0;

        // LevelInfo.needToReset
    }

    // The victory screen game object
    [SerializeField]
    private GameObject defeatScreen;

    // The victory screen game object
    [SerializeField]
    private TMP_Text enemiesDefeatedDefeat;

    // The victory screen game object
    [SerializeField]
    private TMP_Text enemiesMissedDefeat;

    // The are you sure you want to return to the main menu window
    [SerializeField]
    private GameObject returnMainMenuWindow;

    // Method used to open the return to main menu window when wanting to abandon the level
    public void OpenReturnMainMenuScreen()
    {
        // Activate the window
        returnMainMenuWindow.gameObject.SetActive(true);

        // Pause the game
        GameAdvancement.gamePaused = true;
    }

    // Method used to close (cancel) the return to main menu window
    public void CloseReturnMainMenuScreen()
    {
        // Close the window
        returnMainMenuWindow.gameObject.SetActive(false);

        // Un-pause the game
        GameAdvancement.gamePaused = false;
    }

    // The method that activates the win screen
    public void ActivateDefeatScreen()
    {
        // Set the wave ongoing flag to false
        LevelInfo.waveOngoing = false;

        // Disable all towers
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag ("Tower");
 
        foreach(GameObject tower in towerArray)
        {
            // Check if the tower is active
            if(tower.activeSelf == true)
            {
                // Release the tower object
                ObjectPools.ReleaseTower(tower);
            }
        }

        // Disable all towers
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");
 
        foreach(GameObject enemy in enemyArray)
        {
            // Check if the tower is active
            if(enemy.activeSelf == true)
            {
                // Release the tower object
                ObjectPools.ReleaseEnemy(enemy.GetComponent<Enemy>());
            }
        }

        // Reset all spell cards so that they are not drawn
         GameObject[] spellArray = GameObject.FindGameObjectsWithTag ("Spell Card");

        foreach(GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCard>().ResetSpellCard();
        }

        // Reset the activate question class to disable the question menus and reset the number of questions that need to be answered
        ActivateQuestions.ResetQuestionMenuWindows();

        // Reset the spell card deck
        SpellCard.ResetSpellCardDeck();

        // Activate the victory screen
        defeatScreen.SetActive(true);

        // Get the number of enemies in all waves
        int number = CalculateNumberOfEnemies();

        // Write how many enemies were defeated in all waves
        enemiesDefeatedDefeat.text = "Number of enemies defeated: " + (LevelInfo.numberOfEnemiesDefeated);

        // Write how many enemies were missed in all waves
        enemiesMissedDefeat.text = "Number of enemies missed: " + LevelInfo.numberOfEnemiesThatReachedTheCastle;

        // Set the flag that a level was already won
        oneRoundAlreadyWon = true;

        // Deactivate the game board
        Board.gameBoard.SetActive(false);
    }
}