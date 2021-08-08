using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static i5.Toolkit.Core.Examples.Spawners.SpawnEnemy;
using UnityEngine.EventSystems;

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
        // Set the number of waves
        SetNumberOfWaves();

        // Set the current wave to wave 0
        GameAdvancement.currentWave = 0;

        // Initilaize the arrays
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

        // // Spawn the first level
        // SpawnLevel(LevelInfo.normalEnemies[0], LevelInfo.fastEnemies[0], LevelInfo.superFastEnemies[0], LevelInfo.flyingEnemies[0], LevelInfo.tankEnemies[0], LevelInfo.slowEnemies[0], LevelInfo.berzerkerEnemies[0], LevelInfo.berzerkerFlyingEnemies[0], LevelInfo.berzerkerTankEnemies[0]);
        // StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current wave is smaller than the number of waves and if the number of undefeated enemies is 0
        if(LevelInfo.numberOfUndefeatedEnemies == 0 && GameAdvancement.currentWave < LevelInfo.numberOfWaves)
        {
            // Make the next wave setup
            MakeNextWaveSetup();
        }
    }

    // The method that does the setup to make sure the next wave can spawn
    public void MakeNextWaveSetup()
    {
        // Activate the start next wave button
        startNextWave.gameObject.SetActive(true);

        // Set the number of undefeated enemies to the number of enemies in the wave
        LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfEnemies[GameAdvancement.currentWave];

        // Unground all the buildings now that the wave is finished
        UngroundAllBuildings();
    }

    // The method that starts a wave
    public void StartWave()
    {
        // Ground all buildings when the wave begins
        GroundAllBuildings();

        // Increase the ucrrent wave number
        GameAdvancement.currentWave = GameAdvancement.currentWave + 1;

        // Actualize the wave display
        GameSetup.ActualizeWaveDisplay();

        // Start the coroutine that spawns all the wave
        StartCoroutine(SpawnWave());
    }

    // The coroutine that spawns an oponent and waits for a time before the next spawn
    IEnumerator SpawnWave()
    {
        for(int counter = LevelInfo.numberOfEnemies[GameAdvancement.currentWave - 1]; counter > 0; counter = counter - 1)
        {
            // Check if currently there is no group of enemy that should be spawned
            if(enemySpawnNumber == 0)
            {
                // Choose a new type of enemy that should be spawned
                SetNextEnemyType(LevelInfo.normalEnemies[GameAdvancement.currentWave - 1], LevelInfo.fastEnemies[GameAdvancement.currentWave - 1], LevelInfo.superFastEnemies[GameAdvancement.currentWave - 1], LevelInfo.flyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.tankEnemies[GameAdvancement.currentWave - 1], LevelInfo.slowEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerFlyingEnemies[GameAdvancement.currentWave - 1], LevelInfo.berzerkerTankEnemies[GameAdvancement.currentWave - 1]);
            }

            // Check if currently a group of enemy should be spawned
            if(enemySpawnNumber > 0)
            {
                // Spawn enemy of the type given
                Enemy enemy = SpawnAnEnemy(enemyType);

                // // Set the resistance of the enemy
                // enemy.SetResistance(resistance);

                enemy.resistance = resistance;

                // // Set the weakness of the enemy
                // enemy.SetWeakness(weakness);

                enemy.weakness = weakness;

                // Reduce the number of enemies that should spawn as a group
                enemySpawnNumber = enemySpawnNumber - 1;

                // // Reset the flag that an enemy can spawn
                // canSpawn = false;
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
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
        enemySpawnNumber = RandomNumber(1, 3) + (GameAdvancement.currentWave - 1);

        // Check that there are enough enemies in the wave
        if(enemySpawnNumber > LevelInfo.numberOfUndefeatedEnemies)
        {
            // Set the enemy spawn number to the number of undefeated enemies
            enemySpawnNumber = LevelInfo.numberOfUndefeatedEnemies;
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
            int normalEnemies = RandomNumber(10, 20);
            LevelInfo.normalEnemies[0] = normalEnemies;

            // Set the number of enemies without category correctly
            enemiesWithoutCategory = enemiesWithoutCategory - normalEnemies;

            // Initialize the enemy index and the number of enemies in the new category
            int enemyIndex = 0;
            int numberOfEnmiesInNewCategory = 0;

            // For as long as there are enemies, choose what enemy should be added and the number
            while(enemiesWithoutCategory > 0)
            {
                // Generate a random number between 0 and 3
                enemyIndex = RandomNumber(0, 3);

                // Generate a random number between 1 and the number of enemies without category
                numberOfEnmiesInNewCategory = RandomNumber(1, enemiesWithoutCategory);

                //
                switch(enemyIndex)
                {
                    case 0:
                        LevelInfo.normalEnemies[0] = LevelInfo.normalEnemies[0] + numberOfEnmiesInNewCategory;
                    break;

                    case 1:
                        LevelInfo.fastEnemies[0] = LevelInfo.fastEnemies[0] + numberOfEnmiesInNewCategory;
                    break;

                    case 2:
                        LevelInfo.flyingEnemies[0] = LevelInfo.flyingEnemies[0] + numberOfEnmiesInNewCategory;
                    break;

                    case 3:
                        LevelInfo.tankEnemies[0] = LevelInfo.tankEnemies[0] + numberOfEnmiesInNewCategory;
                    break;
                }

                // Reduce the number of enemies without category by the number of enemies that were but in a category
                enemiesWithoutCategory = enemiesWithoutCategory - numberOfEnmiesInNewCategory;
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
                    numberOfEnmiesInNewCategory = RandomNumber(1, enemiesWithoutCategory);

                    // Add this number of enemies in the right category. The probability it is a normal enemy is higher than the rest
                    switch(enemyIndex)
                    {
                        case 0:
                            LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 1:
                            LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 2:
                            LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 3:
                            LevelInfo.normalEnemies[index] = LevelInfo.normalEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 4:
                            LevelInfo.fastEnemies[index] = LevelInfo.fastEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 5:
                            LevelInfo.superFastEnemies[index] = LevelInfo.superFastEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 6:
                            LevelInfo.flyingEnemies[index] = LevelInfo.flyingEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 7:
                            LevelInfo.tankEnemies[index] = LevelInfo.tankEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 8:
                            LevelInfo.slowEnemies[index] = LevelInfo.slowEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 9:
                            LevelInfo.berzerkerEnemies[index] = LevelInfo.berzerkerEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 10:
                            LevelInfo.berzerkerFlyingEnemies[index] = LevelInfo.berzerkerFlyingEnemies[index] + numberOfEnmiesInNewCategory;
                        break;

                        case 11:
                            LevelInfo.berzerkerTankEnemies[index] = LevelInfo.berzerkerTankEnemies[index] + numberOfEnmiesInNewCategory;
                        break;
                    }

                    // Reduce the number of enemies without category by the number of enemies that were but in a category
                    enemiesWithoutCategory = enemiesWithoutCategory - numberOfEnmiesInNewCategory;
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

    // Method that grounds the buildings on the game board when the wave starts
    public void GroundAllBuildings()
    {
        // Ground the first building if it exists
        if(Buildings.numberOfBuildings > 0)
        {
            // Ground the first building
            GroundBuilding(Buildings.firstBuilding, 0);

            // Ground the second building if it exists
            if(Buildings.numberOfBuildings > 1)
            {
                GroundBuilding(Buildings.secondBuilding, 1);

                // Ground the third building if it exists
                if(Buildings.numberOfBuildings > 2)
                {
                    GroundBuilding(Buildings.thirdBuilding, 2);

                    // Ground the fourth building if it exists
                    if(Buildings.numberOfBuildings > 3)
                    {
                        GroundBuilding(Buildings.fourthBuilding, 3);

                        // Ground the fifth building if it exists
                        if(Buildings.numberOfBuildings > 4)
                        {
                            GroundBuilding(Buildings.fifthBuilding, 4);

                            // Ground the sixth building if it exists
                            if(Buildings.numberOfBuildings > 5)
                            {
                                GroundBuilding(Buildings.sixthBuilding, 5);

                                // Ground the seventh building if it exists
                                if(Buildings.numberOfBuildings > 6)
                                {
                                    GroundBuilding(Buildings.seventhBuilding, 6);

                                    // Ground the eighth building if it exists
                                    if(Buildings.numberOfBuildings > 7)
                                    {
                                        GroundBuilding(Buildings.eighthBuilding, 7);

                                        // Ground the ninth building if it exists
                                        if(Buildings.numberOfBuildings > 8)
                                        {
                                            GroundBuilding(Buildings.ninthBuilding, 8);

                                            // Ground the tenth building if it exists
                                            if(Buildings.numberOfBuildings > 9)
                                            {
                                                GroundBuilding(Buildings.tenthBuilding, 9);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private float additionalOffset = (float)0.2;

    // The method used to ground buildings
    public void GroundBuilding(GameObject building, int index)
    {
        // Set the position of the building to the position of the image target
        building.transform.position = Buildings.imageTargetToBuilding[index].transform.position;

        // Set the building as child of the buildings storage object that is a child of the game board
        building.transform.parent = Board.buildingStorage.transform;

        // Give an aditional offset to the tower building
        building.transform.localPosition = building.transform.localPosition + new Vector3(0, additionalOffset * Board.greatestBoardDimension, 0);
    }

    // Method that ungrounds all the buildings on the game board when the wave starts
    public void UngroundAllBuildings()
    {
        // Ground the first building if it exists
        if(Buildings.numberOfBuildings > 0)
        {
            // Ground the first building
            UngroundBuilding(Buildings.firstBuilding, 0);

            // Ground the second building if it exists
            if(Buildings.numberOfBuildings > 1)
            {
                UngroundBuilding(Buildings.secondBuilding, 1);

                // Ground the third building if it exists
                if(Buildings.numberOfBuildings > 2)
                {
                    UngroundBuilding(Buildings.thirdBuilding, 2);

                    // Ground the fourth building if it exists
                    if(Buildings.numberOfBuildings > 3)
                    {
                        UngroundBuilding(Buildings.fourthBuilding, 3);

                        // Ground the fifth building if it exists
                        if(Buildings.numberOfBuildings > 4)
                        {
                            UngroundBuilding(Buildings.fifthBuilding, 4);

                            // Ground the sixth building if it exists
                            if(Buildings.numberOfBuildings > 5)
                            {
                                UngroundBuilding(Buildings.sixthBuilding, 5);

                                // Ground the seventh building if it exists
                                if(Buildings.numberOfBuildings > 6)
                                {
                                    UngroundBuilding(Buildings.seventhBuilding, 6);

                                    // Ground the eighth building if it exists
                                    if(Buildings.numberOfBuildings > 7)
                                    {
                                        UngroundBuilding(Buildings.eighthBuilding, 7);

                                        // Ground the ninth building if it exists
                                        if(Buildings.numberOfBuildings > 8)
                                        {
                                            UngroundBuilding(Buildings.ninthBuilding, 8);

                                            // Ground the tenth building if it exists
                                            if(Buildings.numberOfBuildings > 9)
                                            {
                                                UngroundBuilding(Buildings.tenthBuilding, 9);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // The method used to unground buildings
    public void UngroundBuilding(GameObject building, int index)
    {
        // Reset the position of the building 
        building.transform.position = new Vector3(0, 0, 0);

        // Set the building as child of the image target
        building.transform.parent = Buildings.imageTargetToBuilding[index].transform;
    }
}