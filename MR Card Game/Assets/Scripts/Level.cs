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
}

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that creates the level information
    public void CreateLevelInformation()
    {

    }
}
