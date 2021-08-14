// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // List of spell cards:     Status:
// // - Meteor                 Done (animation missing)
// // - Arrow rain             Done (animation missing)
// // - Thunder strike         Done (animation missing)
// // - Armor                  Done (armor display missing)
// // - Heal                   Done
// // - Plague                 Done (not in enemy)
// // - Obliteration           Done
// // - Draw                   Done
// // - Telport                Done
// // - Space distortion       Done
// // - Slow time              Done
// // - stop time              Done
// // - rain                   Done

// public class SpellEffect : MonoBehaviour
// {
//     // The instance of this class to access the static value of certain variables
//     public static SpellEffect instance;

//     // The meteor radius
//     [SerializeField]
//     private float meteorRadius;

//     // The method used to access to the meteor radius as a static variable
//     public static float getMeteorRadius
//     {
//         get { return instance.meteorRadius; }
//     }

//     // The meteor damage
//     [SerializeField]
//     private int meteorDamage;

//     // The method used to access to the meteor damage as a static variable
//     public static int getMeteorDamage
//     {
//         get { return instance.meteorDamage; }
//     }

//     // The arrow rain radius
//     [SerializeField]
//     private float arrowRainRadius;

//     // The method used to access to the arrow rain radius as a static variable
//     public static float getArrowRainRadius
//     {
//         get { return instance.arrowRainRadius; }
//     }

//     // The arrow rain damage
//     [SerializeField]
//     private int arrowRainDamage;

//     // The method used to access to the arrow rain damage as a static variable
//     public static int getArrowRainDamage
//     {
//         get { return instance.arrowRainDamage; }
//     }

//     // The teleport radius
//     [SerializeField]
//     private float teleportRadius;

//     // The method used to access to the teleport radius as a static variable
//     public static float getTeleportRadius
//     {
//         get { return instance.teleportRadius; }
//     }

//     // The space distortion radius
//     [SerializeField]
//     private float spaceDistortionRadius;

//     // The method used to access to the space distortion radius as a static variable
//     public static float getSpaceDistortionRadius
//     {
//         get { return instance.spaceDistortionRadius; }
//     }

//     // The space distortion radius
//     [SerializeField]
//     private float spaceDistortionDuration;

//     // The method used to access to the space distortion duration as a static variable
//     public static float getSpaceDistortionDuration
//     {
//         get { return instance.spaceDistortionDuration; }
//     }

//     // The space distortion factor
//     [SerializeField]
//     private float spaceDistortionFactor;

//     // The method used to access to the space distortion factor as a static variable
//     public static float getSpaceDistortionFactor
//     {
//         get { return instance.spaceDistortionFactor; }
//     }

//     // The duration the time should be stopped when the stop time card is played
//     [SerializeField]
//     private float stopTimeDuration;

//     // The static variable used by other classes to access the stop time duration
//     public static float getStopTimeDuration
//     {
//         get { return instance.stopTimeDuration; }
//     }


//     // The duration the time should be slowed when the slow time card is played
//     [SerializeField]
//     private float slowTimeDuration;

//     // The method used to access to the slow time duration as a static variable
//     public static float getSlowTimeDuration
//     {
//         get { return instance.slowTimeDuration; }
//     }

//     // The factor by which the time should be slowed when the slow time card is played
//     [SerializeField]
//     private float slowTimeFactor;

//     // The method used to access to the slow time factor as a static variable
//     public static float getSlowTimeFactor
//     {
//         get { return instance.slowTimeFactor; }
//     }

//     // The duration the rain should last when the rain card is played
//     [SerializeField]
//     private float rainDuration;

//     // The method used to access to the rain duration as a static variable
//     public static float getRainDuration
//     {
//         get { return instance.rainDuration; }
//     }

//     // The factor by which the time should be slowed when the rain card is played
//     [SerializeField]
//     private float rainFactor;

//     // The method used to access to the rain factor as a static variable
//     public static float getRainFactor
//     {
//         get { return instance.rainFactor; }
//     }

//     // Instantiate random number generator.  
//     private readonly System.Random _random = new System.Random();  
    
//     // Generates a random number within a range.      
//     public int RandomNumber(int min, int max)  
//     {  
//         return _random.Next(min, max);  
//     }  

//     // Start is called before the first frame update
//     void Start()
//     {
//         instance = this;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     // The method used to make the armor spell card take effect
//     private void PlayArmor()
//     {
//         // Add three armor points to the castle armor
//         GameAdvancement.castleCurrentAP = GameAdvancement.castleCurrentAP + 3;

//         // Actualize the castle health points (and armor points)
//         GameSetup.ActualizeCastleHealthPoints();
//     }

//     // The method used to make the heal spell card take effect
//     private void PlayHeal()
//     {
//         // Check if the plus five health points heal would exceed the castle's maximum health points
//         if(GameAdvancement.castlecurrentHP + 5 > GameAdvancement.castleMaxHP)
//         {
//             // If yes, set the castle current health points to the castle max health points
//             GameAdvancement.castlecurrentHP = GameAdvancement.castleMaxHP;

//         } else {

//             // If not, add five health points to the castle current health points
//             GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP + 5;
//         }

//         // Actualize the castle health points
//         GameSetup.ActualizeCastleHealthPoints();
//     }

//     // The method used to make the thunder strike spell card take effect
//     private void PlayThunderStrike(GameObject spellCard)
//     {
//         // Initialize and fill the enemies array
//         GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

//         // Check that there are enemies on the board
//         if(enemies != null)
//         {
//             // Initialize the closest distance variable
//             float closestDistance = 1000;

//             // Initialize the distance variable
//             float distance = 0;

//             // Initialize the closest enemy variable
//             GameObject closestEnemy = null;

//             // Find the enemy that is the closest to the spell card
//             foreach(GameObject enemy in enemies)
//             {
//                 // Calculate the distance between the spell cards location and this enemy
//                 distance = Vector3.Distance(spellCard.transform.position, enemy.transform.position);

//                 // Check if this distance is smaller than the current smallest enemy
//                 if(distance < closestDistance)
//                 {
//                     // Set the closest distance to this distance
//                     closestDistance = distance;

//                     // Set the closest enemy to this enemy
//                     closestEnemy = enemy;
                    
//                 }
//             }

//             // Kill this enemy by making it take more damage than the maximum number of health points that exist
//             closestEnemy.GetComponent<Enemy>().TakeDamage(1000);
//         }
//     }

//     // The method used to make the meteor spell card take effect
//     private void PlayMeteor(GameObject spellCard)
//     {
//         // Make the damage in radius take effect in the meteor radius and with the meteor damage
//         PlayDamageInRadiusSpell(spellCard, getMeteorRadius, getMeteorDamage);
//     }

//     // The method used to make the arrow rain spell card take effect
//     private void PlayArrowRain(GameObject spellCard)
//     {
//         // Make the damage in radius take effect in the meteor radius and with the meteor damage
//         PlayDamageInRadiusSpell(spellCard, getArrowRainRadius, getArrowRainDamage);
//     }

//     // The method used to make the damage in radius effect take place
//     private void PlayDamageInRadiusSpell(GameObject spellCard, float radius, int damage)
//     {
//         // Initialize the maximum distance where the meteor damage should still happen
//         float closestDistance = radius * Board.greatestBoardDimension;

//         // Initialize the distance variable
//         float distance = 0;

//         // Initialize the closest enemy variable
//         List<GameObject> enemiesInRange = EnemiesInRange(spellCard, closestDistance);

//         // Check that the enemies in range list is not empty
//         if(enemiesInRange != null)
//         {
//             // Go through all enemies in the list
//             foreach(GameObject enemy in enemiesInRange)
//             {
//                 // Damage this enemy by the meteor damage
//                 enemy.GetComponent<Enemy>().TakeDamage(damage);
//             }
//         }
//     }

//     // The method used to make the obliteration spell card take effect
//     private void PlayObliteration(GameObject spellCard)
//     {
//         // Initialize and fill the enemies array
//         GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

//         // Check that there are enemies on the board
//         if(enemies != null)
//         {
//             // Go over all enemies
//             foreach(GameObject enemy in enemies)
//             {
//                 // Kill this enemy by making it take more damage than the maximum number of health points that exist
//                 enemy.GetComponent<Enemy>().TakeDamage(1000);
//             }
//         }

//         // Check if the castle maximum health points is over 10
//         if(GameAdvancement.castleMaxHP <= 10)
//         {
//             // Set the castle maximum health points to 0
//             GameAdvancement.castleMaxHP = 0;

//         } else {

//             // Make the castle maximum health points lose 5 points
//             GameAdvancement.castleMaxHP = GameAdvancement.castleMaxHP - 10;
//         }

//         // Check that the castle current health points are not exceeding the castle maximum health points
//         if(GameAdvancement.castlecurrentHP > GameAdvancement.castleMaxHP)
//         {
//             // Set the castle current health points to the castle maximun health points
//             GameAdvancement.castlecurrentHP = GameAdvancement.castleMaxHP;
//         }

//         // Actualize the castle health points
//         GameSetup.ActualizeCastleHealthPoints();
//     }

//     // The method used to make the teleport spell card take effect
//     private void PlayTeleport(GameObject spellCard)
//     {
//         // Initialize the maximum distance where the meteor damage should still happen
//         float greatestDistance = getTeleportRadius * Board.greatestBoardDimension;

//         // Initialize the distance variable
//         float distance = 0;

//         // Initialize the closest enemy variable
//         List<GameObject> enemiesInRange = EnemiesInRange(spellCard, greatestDistance);

//         // Check that the enemies in range list is not empty
//         if(enemiesInRange != null)
//         {
//             // Go through all enemies in the list
//             foreach(GameObject enemy in enemiesInRange)
//             {
//                 // Set the position of the enemy to the position of the enemy spawn
//                 enemy.transform.position = Waypoints.enemySpawn.transform.position;

//                 // Set the waypoint index of the enemy to 0
//                 enemy.GetComponent<Enemy>().waypointIndex = 0;
//             }
//         }
//     }

//     // The method used to get the list of objects in a certain range of a game object
//     private List<GameObject> EnemiesInRange(GameObject spellCard, float radius)
//     {
//         // Initialize and fill the enemies array
//         GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

//         // Check that there are enemies on the board
//         if(enemies != null)
//         {
//             // Initialize the maximum distance where the meteor damage should still happen
//             float closestDistance = getTeleportRadius * Board.greatestBoardDimension;

//             // Initialize the distance variable
//             float distance = 0;

//             // Initialize the closest enemy variable
//             List<GameObject> enemiesInRange = new List<GameObject>();

//             // Find the enemy that is the closest to the spell card
//             foreach(GameObject enemy in enemies)
//             {
//                 // Calculate the distance between the spell cards location and this enemy
//                 distance = Vector3.Distance(spellCard.transform.position, enemy.transform.position);

//                 // Check if this distance is smaller than the current smallest enemy
//                 if(distance < closestDistance)
//                 {
//                     // Add this enemy to the enemies in range array
//                     enemiesInRange.Add(enemy);              
//                 }
//             }

//             // Return the list of the enemies in range
//             return enemiesInRange;

//         } else {

//             // Return nul if there are no enemies in range
//             return null;
//         }
//     }

//     // The method used to make the stop time spell card take effect
//     private void PlayStopTime()
//     {
//         StartCoroutine(StopTime());
//     }

//     // Coroutine that makes the stop time spell card take effect
//     IEnumerator StopTime()
//     {
//         // Stop time
//         GameAdvancement.timeStopped = true;

//         // Wait for the duration of the stop time
//         yield return new WaitForSeconds(getStopTimeDuration);

//         // Remove the stop time
//         GameAdvancement.timeStopped = false;
//     }

//     // The method used to make the slow time spell card take effect
//     private void PlaySlowTime()
//     {
//         StartCoroutine(SlowTime());
//     }

//     // Coroutine that makes the slow time spell card take effect
//     IEnumerator SlowTime()
//     {
//         // Slow enemies down
//         GameAdvancement.globalSlow = getSlowTimeFactor;

//         // Wait for the duration of the slow time card
//         yield return new WaitForSeconds(getSlowTimeDuration);

//         // Remove the slow time effect
//         GameAdvancement.globalSlow = 1;
//     }

//     // The method used to make the rain card take effect
//     private void PlayRain()
//     {
//         StartCoroutine(Rain());
//     }

//     // Coroutine that makes the rain spell card take effect
//     IEnumerator Rain()
//     {
//         // slow enemies down
//         GameAdvancement.globalSlow = getRainFactor;

//         // Set the raining flag to true
//         GameAdvancement.raining = true;

//         // Wait for the duration of the stop time
//         yield return new WaitForSeconds(getRainDuration);

//         // Remove the slow effect
//         GameAdvancement.globalSlow = 1;

//         // Set the raining flag to false
//         GameAdvancement.raining = false;
//     }

//     // The method used to make the draw card take effect
//     private void PlayDraw()
//     {
//         // Increase the number of free card draws by three
//         Cards.freeDraws = Cards.freeDraws + 3;
//     }

//     // The method used to make the plague card take effect
//     private void PlayPlaque()
//     {
//         // Get a random number
//         int newCategoryIndex = instance.RandomNumber(0, 8);

//         // Depending on the random number, set a category of enemy as plagued
//         switch(newCategoryIndex)
//         {
//             case 0:
//                 // Set the type of plagued enemy to normal enemy
//                 GameAdvancement.plaguedEnemyType = "Normal Enemy";
//             break;

//             case 1:
//                 // Set the type of plagued enemy to fast enemy
//                 GameAdvancement.plaguedEnemyType = "Fast Enemy";
//             break;

//             case 2:
//                 // Set the type of plagued enemy to super fast enemy
//                 GameAdvancement.plaguedEnemyType = "Super Fast Enemy";
//             break;

//             case 3:
//                 // Set the type of plagued enemy to flying enemy
//                 GameAdvancement.plaguedEnemyType = "Flying Enemy";
//             break;

//             case 4:
//                 // Set the type of plagued enemy to tank enemy
//                 GameAdvancement.plaguedEnemyType = "Tank Enemy";
//             break;

//             case 5:
//                 // Set the type of plagued enemy to slow enemy
//                 GameAdvancement.plaguedEnemyType = "Slow Enemy";
//             break;

//             case 6:
//                 // Set the type of plagued enemy to berzerker enemy
//                 GameAdvancement.plaguedEnemyType = "Berzerker Enemy";
//             break;

//             case 7:
//                 // Set the type of plagued enemy to berzerkerflying enemy
//                 GameAdvancement.plaguedEnemyType = "Berzerker Flying Enemy";
//             break;

//             case 8:
//                 // Set the type of plagued enemy to berzerker tank enemy
//                 GameAdvancement.plaguedEnemyType = "Berzerker Tank Enemy";
//             break;
//         }
//     }

//     // The method used to make the space distortion card take effect
//     private void PlaySpaceDistortion(GameObject spellCard)
//     {
//         // Initialize the maximum distance where the meteor damage should still happen
//         float closestDistance = getSpaceDistortionRadius * Board.greatestBoardDimension;

//         // Initialize the distance variable
//         float distance = 0;

//         // Initialize the closest enemy variable
//         List<GameObject> enemiesInRange = EnemiesInRange(spellCard, closestDistance);


//         // Check that the enemies in range list is not empty
//         if(enemiesInRange != null)
//         {
//             StartCoroutine(SlowEnemies(enemiesInRange));
//         }
//     }

//     // Coroutine that makes the space distortion spell card take effect
//     IEnumerator SlowEnemies(List<GameObject> enemies)
//     {
//         // Go through all enemies in the list
//         foreach(GameObject enemy in enemies)
//         {
//             // Set the personal slow factor of the enemies to the space distortion factor
//             enemy.GetComponent<Enemy>().personalSlowFactor = spaceDistortionFactor;
//         }

//         // Wait for the duration of the space distortion
//         yield return new WaitForSeconds(spaceDistortionDuration);

//         // Go through all enemies in the list
//         foreach(GameObject enemy in enemies)
//         {
//             // Reset the personal slow factor of the enemies
//             enemy.GetComponent<Enemy>().personalSlowFactor = 1;
//         }
//     }
// }
