using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class LevelInfo
{
    // The number of waves of the level
    public static int numberOfWaves;

    // The number of normal enemies that come each wave
    public static int[] normalEnemies;

    // The number of fast enemies that come each wave
    public static int[] fastEnemies;

    // The number of fast enemies that come each wave
    public static int[] superFastEnemies;

    // The number of flying enemies that come each wave
    public static int[] flyingEnemies;

    // The number of tank enemies that come each wave
    public static int[] tankyEnemies;

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

    // Start is called before the first frame update
    void Start()
    {
        // Set the current wave to wave 1
        currentWave = 1;

        // Create level information given the number of question
        CreateLevelInformation();

        // Spawn the first level
        SpawnLevel(LevelInfo.normalEnemies[0], LevelInfo.fastEnemies[0], LevelInfo.superFastEnemies[0], LevelInfo.flyingEnemies[0], LevelInfo.tankyEnemies[0], LevelInfo.slowEnemies[0], LevelInfo.berzerkerEnemies[0], LevelInfo.berzerkerFlyingEnemies[0], LevelInfo.berzerkerTankEnemies[0]);
    }

    // Update is called once per frame
    void Update()
    {
        // If the next wave flag is set, then the next wave should be spawned
        if(LevelInfo.nextWave == true && currentWave < LevelInfo.numberOfWaves)
        {
            // Spawn the level of current wave index 
            SpawnLevel(LevelInfo.normalEnemies[currentWave], LevelInfo.fastEnemies[currentWave], LevelInfo.superFastEnemies[currentWave], LevelInfo.flyingEnemies[currentWave], LevelInfo.tankyEnemies[currentWave], LevelInfo.slowEnemies[currentWave], LevelInfo.berzerkerEnemies[currentWave], LevelInfo.berzerkerFlyingEnemies[currentWave], LevelInfo.berzerkerTankEnemies[currentWave]);

            // Increase the current wave counter
            currentWave = currentWave + 1;

            // Set the flag down
            LevelInfo.nextWave = false;
        }
    }

    // Method that creates the level information
    public void CreateLevelInformation()
    {

    }

    // Method that spawns the level, the given counters are the number of enemies of each type that should be spawned
    public void SpawnLevel(int CounterE1, int CounterE2, int CounterE3, int CounterE4, int CounterE5, int CounterE6, int CounterE7, int CounterE8, int CounterE9)
    {

    }
}
