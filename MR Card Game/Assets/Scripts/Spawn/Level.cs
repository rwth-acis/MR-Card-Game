using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static i5.Toolkit.Core.Examples.Spawners.SpawnEnemy;
using UnityEngine.EventSystems;

static class LevelInfo
{
    // The number of waves of the level
    public static int numberOfWaves;

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
}

public class Level : MonoBehaviour
{
    // The current wave counter
    private int currentWave;

    // The boolean that indicates if the wave is currently spawning
    private bool waveBegan = false;

    // The current enemy index that should be spawned together
    private string enemyType = "";

    // The current number of the enemies that should be spawned at the same time that remain
    private int enemySpawnNumber = 0;

    // The number of enemy categories that are non empty at the moment
    private int numberOfCategoriesNotEmpty = 7;

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
        // Set the current wave to wave 1
        currentWave = 1;

        // Initilaize the arrays
        LevelInfo.numberOfEnemies = new int[currentWave];
        LevelInfo.normalEnemies = new int[currentWave];
        LevelInfo.fastEnemies = new int[currentWave];
        LevelInfo.superFastEnemies = new int[currentWave];
        LevelInfo.flyingEnemies = new int[currentWave];
        LevelInfo.tankEnemies = new int[currentWave];
        LevelInfo.slowEnemies = new int[currentWave];
        LevelInfo.berzerkerEnemies = new int[currentWave];
        LevelInfo.berzerkerFlyingEnemies = new int[currentWave];
        LevelInfo.berzerkerTankEnemies = new int[currentWave];

        // Create level information given the number of question
        CreateLevelInformation();

        // Set the type of enemy to spawn to normal enemy
        enemyType = "Normal Enemy";

        // Set the number of enemies to spawn of that type to 5
        enemySpawnNumber = 5;

        // // Spawn the first level
        // SpawnLevel(LevelInfo.normalEnemies[0], LevelInfo.fastEnemies[0], LevelInfo.superFastEnemies[0], LevelInfo.flyingEnemies[0], LevelInfo.tankEnemies[0], LevelInfo.slowEnemies[0], LevelInfo.berzerkerEnemies[0], LevelInfo.berzerkerFlyingEnemies[0], LevelInfo.berzerkerTankEnemies[0]);
        // StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        // If the next wave flag is set, then the next wave should be spawned
        if(LevelInfo.nextWave == true && currentWave < LevelInfo.numberOfWaves)
        {
            // Increase the current wave counter
            currentWave = currentWave + 1;

            // Set the flag down
            LevelInfo.nextWave = false;

            // Set the boolean that the new wave began to false
            waveBegan = false;
        }

        // Spawn the level
        if(waveBegan == true)
        {
            // Start the new wave
            StartWave();

            // Set the wave began flag to false
            waveBegan = false;
        }
    }

    // Method that starts the first wave
    public void StartFirstWave()
    {
        // Set the wave began flag to true
        waveBegan = true;
    }

    // Method that creates the level information
    public void CreateLevelInformation()
    {
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
    }

    // Method that spawns the level, the given counters are the number of enemies of each type that should be spawned
    public void SpawnLevel(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        // Check if the can spawn flag is true or false
        if(!canSpawn)
        {
            // If it is false, increase the timer by the time passed
            spawnTimer = spawnTimer + Time.deltaTime;

            // Check if the attack timer has reached the attack cooldown
            if(spawnTimer >= timeBetweenSpawns)
            {
                // If it is the case, set the can attack flag to true
                canSpawn = true;

                // Reset the attack timer
                spawnTimer = 0;
            }
        }
        if(canSpawn == true)
        {
            // Check if currently there is no group of enemy that should be spawned
            if(enemySpawnNumber == 0)
            {
                // Choose a new type of enemy that should be spawned
                SetNextEnemyType(CounterE1, CounterE2, CounterE3, CounterE4, CounterE5, CounterE6, CounterE7, CounterE8, CounterE9);
            }

            // Check if currently a group of enemy should be spawned
            if(enemySpawnNumber > 0)
            {
                // Spawn enemy of the type given
                SpawnAnEnemy(enemyType);

                // Reduce the number of enemies that should spawn as a group
                enemySpawnNumber = enemySpawnNumber - 1;

                // Reset the flag that an enemy can spawn
                canSpawn = false;
            }
        }
    }

    // The method that starts a wave
    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    // The coroutine that spawns an oponent and waits for a time before the next spawn
    IEnumerator SpawnWave()
    {
        for(int counter = LevelInfo.numberOfEnemies[currentWave - 1]; counter > 0; counter = counter - 1)
        {
            // Check if currently there is no group of enemy that should be spawned
            if(enemySpawnNumber == 0)
            {
                // Choose a new type of enemy that should be spawned
                SetNextEnemyType(LevelInfo.normalEnemies[currentWave - 1], LevelInfo.fastEnemies[currentWave - 1], LevelInfo.superFastEnemies[currentWave - 1], LevelInfo.flyingEnemies[currentWave - 1], LevelInfo.tankEnemies[currentWave - 1], LevelInfo.slowEnemies[currentWave - 1], LevelInfo.berzerkerEnemies[currentWave - 1], LevelInfo.berzerkerFlyingEnemies[currentWave - 1], LevelInfo.berzerkerTankEnemies[currentWave - 1]);
            }

            // Check if currently a group of enemy should be spawned
            if(enemySpawnNumber > 0)
            {
                // Spawn enemy of the type given
                SpawnAnEnemy(enemyType);

                // Reduce the number of enemies that should spawn as a group
                enemySpawnNumber = enemySpawnNumber - 1;

                // // Reset the flag that an enemy can spawn
                // canSpawn = false;
            }

            yield return new WaitForSeconds(timeBetweenSpawns);

            Debug.Log("The waiting time just finished");
        }
        
    }

    // Method that determines the next type of enemies that should be spawned
    public void SetNextEnemyType(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {
        // Get a random number
        int newCategoryIndex = RandomNumber(0, numberOfCategoriesNotEmpty);

        if(CounterE1 != 0)
        {
            if(newCategoryIndex == 0)
            {
                enemyType = "Normal Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE2 != 0)
        {
            if(newCategoryIndex == 1)
            {
                enemyType = "Fast Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE3 != 0)
        {
            if(newCategoryIndex == 2)
            {
                enemyType = "Super Fast Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE4 != 0)
        {
            if(newCategoryIndex == 3)
            {
                enemyType = "Flying Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE5 != 0)
        {
            if(newCategoryIndex == 4)
            {
                enemyType = "Tank Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE6 != 0)
        {
            if(newCategoryIndex == 5)
            {
                enemyType = "Slow Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE6 != 0)
        {
            if(newCategoryIndex == 5)
            {
                enemyType = "Slow Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE7 != 0)
        {
            if(newCategoryIndex == 6)
            {
                enemyType = "Berzerker Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE8 != 7)
        {
            if(newCategoryIndex == 4)
            {
                enemyType = "Berzerker Flying Enemy";
            }
        } else {
            newCategoryIndex = newCategoryIndex + 1;
        }

        if(CounterE9 != 0)
        {
            if(newCategoryIndex == 8)
            {
                enemyType = "Berzerker Tank Enemy";
            }
        }

        // Get another random number and set the enemy spawn number to it
        enemySpawnNumber = RandomNumber(1, 3) + (currentWave - 1);
    }

    // // Method that waits a certain time before a next action can take place
    // public void WaitTime()
    // {
    //     // If it is false, increase the timer by the time passed
    //     spawnTimer = spawnTimer + Time.deltaTime;

    //     // Check if the attack timer has reached the attack cooldown
    //     if(spawnTimer >= timeBetweenSpawns)
    //     {
    //         // If it is the case, set the can attack flag to true
    //         canSpawn = true;

    //         // Reset the attack timer
    //         spawnTimer = 0;
    //     }
    // }
}
