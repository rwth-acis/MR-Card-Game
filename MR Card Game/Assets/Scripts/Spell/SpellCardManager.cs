using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vuforia;

// List of spell cards:     Status:     Comments:               Tested:
// - Meteor                 Done        animation done!         working
// - Arrow rain             Done        animation done!         working
// - Thunder strike         Done        animation done!         working
// - Armor                  Done        display is there        working
// - Heal                   Done                                working
// - Obliteration           Done                                working
// - Draw                   Done                                working
// - Telport                Done                                working
// - Space distortion       Done                                working
// - Slow time              Done                                working
// - stop time              Done                                working
// - rain                   Done        double damage working   working

public class SpellCardManager : MonoBehaviour
{

    #region Serializable Fields
    [Header("Spell Properties")]
    [SerializeField]
    private float timeBeforeSpellLaunch;

    // The meteor radius
    [SerializeField]
    private float meteorRadius;

    // The meteor damage
    [SerializeField]
    private int meteorDamage;

    // The arrow rain radius
    [SerializeField]
    private float arrowRainRadius;

    // The arrow rain damage
    [SerializeField]
    private int arrowRainDamage;

    // The teleport radius
    [SerializeField]
    private float teleportRadius;

    // The space distortion radius
    [SerializeField]
    private float spaceDistortionRadius;

    // The space distortion radius
    [SerializeField]
    private float spaceDistortionDuration;

    // The space distortion factor
    [SerializeField]
    private float spaceDistortionFactor;

    // The duration the time should be stopped when the stop time card is played
    [SerializeField]
    private float stopTimeDuration;

    // The duration the time should be slowed when the slow time card is played
    [SerializeField]
    private float slowTimeDuration;

    // The factor by which the time should be slowed when the slow time card is played
    [SerializeField]
    private float slowTimeFactor;

    // The duration the rain should last when the rain card is played
    [SerializeField]
    private float rainDuration;

    // The factor by which the time should be slowed when the rain card is played
    [SerializeField]
    private float rainFactor;


    [Header("UI Components")]
    // The answer question overlay object
    [SerializeField]
    private GameObject answerQuestions;

    [SerializeField]
    private GameObject groundPlane;

    [SerializeField]
    private GameObject gameBoard;

    // Define the currency display button
    [SerializeField]
    private Button currencyDisplay;

    // Define the wave display button
    [SerializeField]
    private Button waveDisplay;

    // Define the start next wave button
    [SerializeField]
    private Button startNextWaveButton;

    #endregion

    #region Non-Serializable Fields
    // The instance of this class to access the static value of certain variables
    public static SpellCardManager Instance;

    private int drawnSpellsOnBoard;

    private Dictionary<SpellType, int> cardDeck;

    // The number of free cards that can be drawn without answering questions
    private int freeDraws = 0;
    #endregion

    #region Properties

    public static int DrawnSpellsOnBoard
    {
        get => Instance.drawnSpellsOnBoard;
        set => Instance.drawnSpellsOnBoard = value;
    }

    public static int FreeDraws
    {
        get => Instance.freeDraws;
        set => Instance.freeDraws = value;
    }

    // The static variable used by other classes to access the stop time duration
    public static float StopTimeDuration
    {
        get { return Instance.stopTimeDuration; }
    }

    // The method used to access to the currency display button as a static object
    public static Button CurrencyDisplay
    {
        get { return Instance.currencyDisplay; }
    }

    // The method used to access to the wave display button as a static object
    public static Button WaveDisplay
    {
        get { return Instance.waveDisplay; }
    }

    // The method used to access to the start next wave button as a static object
    public static Button StartNextWaveButton
    {
        get { return Instance.startNextWaveButton; }
    }

    public static GameObject GroundPlane
    {
        get => Instance.groundPlane;
    }
    public static GameObject GameBoard
    {
        get => Instance.gameBoard;
    }
    public static GameObject AnswerQuestions
    {
        get => Instance.answerQuestions;
    }

    public static Dictionary<SpellType, int> CardDeck
    {
        get => Instance.cardDeck;
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Fill the card deck
        InitializeCardDeck();
    }

    //---------------------------------------------------------------------------------------------------------------
    // Initializing the deck of cards
    //---------------------------------------------------------------------------------------------------------------

    // Method used to put the right amount of cards in the deck of cards
    private void InitializeCardDeck()
    {
        cardDeck = new Dictionary<SpellType, int>
        {
            { SpellType.Meteor, 5 },
            { SpellType.ArrowRain, 5 },
            { SpellType.ThunderStrike, 4 },
            { SpellType.Armor, 4 },
            { SpellType.Heal, 4 },
            { SpellType.Obliteration, 1 },
            { SpellType.Draw, 1 },
            { SpellType.Teleport, 3 },
            { SpellType.SpaceDistortion, 3 },
            { SpellType.SlowTime, 2 },
            { SpellType.StopTime, 1 },
            { SpellType.Rain, 2 }
        };
    }

    // Initialize random number generator
    private static readonly System.Random random = new System.Random();

    // Method that shuffles the card deck
    public static void ShuffleCardDeck(SpellType[] array)
    {
        // Get the length of the question array
        int length = array.Length;

        // Initialize the swap index
        int swapIndex = 0;

        // Initialize the loop index
        int index = length - 1;

        // Shuffle the card deck
        while(index >= 0)
        {
            // Get a random number
            swapIndex = random.Next(0, index);

            // Copy entry at swapIndex to the entry index of the array
            SpellType value = array[swapIndex];
            array[swapIndex] = array[index];
            array[index] = value;

            // Reduce the index by one
            index--;
        }
    }

    /// <summary>
    /// Try to play spell with the given type. If there is no such spell cards, return false and do nothing, else return true and play the spell.
    /// </summary>
    /// <param name="spellType"> type of the spell to play</param>
    /// <param name="position"> the position (center) of the spell effect</param>
    /// <returns>If there is at least one card with the given type in the deck</returns>
    public bool TryPlaySpell(SpellType spellType, Vector3 position)
    {
        if (cardDeck[spellType] > 0)
        {
            cardDeck[spellType]--;
            // Depending on the type of card that is next in the card deck, make the right overlay appear and set the spell type variable to the right type
            switch (spellType)
            {
                case SpellType.Meteor:
                    PlayMeteor(position);
                    break;

                case SpellType.ArrowRain:
                    PlayArrowRain(position);
                    break;

                case SpellType.ThunderStrike:
                    PlayThunderStrike(position);
                    break;

                case SpellType.Armor:
                    PlayArmor();
                    break;

                case SpellType.Heal:
                    PlayHeal();
                    break;

                case SpellType.Obliteration:
                    PlayObliteration();
                    break;

                case SpellType.Draw:
                    PlayDraw();
                    break;

                case SpellType.Teleport:
                    PlayTeleport(position);
                    break;

                case SpellType.SpaceDistortion:
                    PlaySpaceDistortion(position);
                    break;

                case SpellType.SlowTime:
                    PlaySlowTime();
                    break;

                case SpellType.StopTime:
                    PlayStopTime();
                    break;

                case SpellType.Rain:
                    PlayRain();
                    break;
            }
            // Decrease the number of drawn spells that are on the board by one
            DrawnSpellsOnBoard--;

            Debug.Log("The number of drawn card spells that are on the board is: " + DrawnSpellsOnBoard);

            // Check if the number of drawn spell cards that are on the board is 0
            if (DrawnSpellsOnBoard <= 0)
            {
                Debug.Log("The game is being unpaused after a spell card has been played");

                // Un-pause the game
                GameAdvancement.gamePaused = false;
            }

            // Check if no spell card is drawn
            if (NumberDrawnSpellCards() == 0)
            {
                GameAdvancement.gamePaused = false;
            }
            return true;
        }
        else
        {
            DrawnSpellsOnBoard--;

            // Check if the number of drawn spell cards that are on the board is 0
            if (DrawnSpellsOnBoard <= 0)
            {
                Debug.Log("The game is being unpaused after a spell card has been played");

                // Un-pause the game
                GameAdvancement.gamePaused = false;
            }

            // Check if no spell card is drawn
            if (NumberDrawnSpellCards() == 0)
            {
                GameAdvancement.gamePaused = false;
            }
            return false;
        }
    }

    // Method used to check how many spell cards are currently drawn
    public int NumberDrawnSpellCards()
    {
        // Get all spell cards with tag
        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("Spell Card");

        // Initialize the count
        int count = 0;

        foreach(GameObject card in spellcards)
        {
            if(card.GetComponent<SpellController>().CardDrawn == true)
            {
                // Count one up
                count++;
            }
        }

        return count;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell cards effect
    //---------------------------------------------------------------------------------------------------------------

    private IEnumerator ActivateSpellAnimationAndEffect(SpellType spellType, Vector3 position, float effectiveTimer)
    {
        GameObject spellEffect = SpawnSpellEffect.SpawnSpell(spellType);

        spellEffect.transform.position = position;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of one second
        while (timeWaited <= effectiveTimer)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if (GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited += 0.1f;
            }
        }
        ObjectPools.ReleaseSpellEffect(spellEffect, spellType);     
    }

    // The method used to make the meteor spell card take effect
    private void PlayMeteor(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlaySpellDamageInRadius(spellPosition, meteorRadius, meteorDamage);

        StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.Meteor, spellPosition, 1f));
    }

    // The method used to make the arrow rain spell card take effect
    private void PlayArrowRain(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlaySpellDamageInRadius(spellPosition, arrowRainRadius, arrowRainDamage);
        StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.ArrowRain, spellPosition, 1f));
    }

    // The method used to make the thunder strike spell card take effect
    private void PlayThunderStrike(Vector3 spellPosition)
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Initialize the closest distance variable
            float closestDistance = 1000;
            // Initialize the closest enemy variable
            GameObject closestEnemy = null;
            // Find the enemy that is the closest to the spell card
            foreach(GameObject enemy in enemies)
            {
                // Calculate the distance between the spell cards location and this enemy
                float distance = Vector3.Distance(spellPosition, enemy.transform.position);

                // Check if this distance is smaller than the current smallest enemy
                if(distance < closestDistance)
                {
                    // Set the closest distance to this distance
                    closestDistance = distance;

                    // Set the closest enemy to this enemy
                    closestEnemy = enemy;
                    
                }
            }
            // Kill this enemy by making it take more damage than the maximum number of health points that exist
            closestEnemy.GetComponent<Enemy>().TakeDamage(1000);
            StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.ThunderStrike, closestEnemy.transform.position, 0.5f));
        }
    }

    // The method used to make the armor spell card take effect
    private void PlayArmor()
    {
        // Add three armor points to the castle armor
        GameAdvancement.castleCurrentAP += 3;

        // Actualize the castle health points (and armor points)
        GameSetup.UpdateCastleHealthPoints();
    }

    // The method used to make the heal spell card take effect
    private void PlayHeal()
    {
        // Check if the plus five health points heal would exceed the castle's maximum health points
        if(GameAdvancement.castleCurrentHP + 5 > GameAdvancement.castleMaxHP)
        {
            // If yes, set the castle current health points to the castle max health points
            GameAdvancement.castleCurrentHP = GameAdvancement.castleMaxHP;

        } else {

            // If not, add five health points to the castle current health points
            GameAdvancement.castleCurrentHP += 5;
        }

        // Actualize the castle health points
        GameSetup.UpdateCastleHealthPoints();
    }

    // The method used to make the obliteration spell card take effect
    private void PlayObliteration()
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Go over all enemies
            foreach(GameObject enemy in enemies)
            {
                // Kill this enemy by making it take more damage than the maximum number of health points that exist
                enemy.GetComponent<Enemy>().TakeDamage(1000);
            }
        }

        // Reduce the castle maximum health points by 30%
        GameAdvancement.castleMaxHP = (int)(GameAdvancement.castleMaxHP * 0.7f);

        // Check that the castle current health points are not exceeding the castle maximum health points
        if(GameAdvancement.castleCurrentHP > GameAdvancement.castleMaxHP)
        {
            // Set the castle current health points to the castle maximun health points
            GameAdvancement.castleCurrentHP = GameAdvancement.castleMaxHP;
        }

        // Actualize the castle health points
        GameSetup.UpdateCastleHealthPoints();
    }

    // The method used to make the draw card take effect
    private void PlayDraw()
    {
        // Increase the number of free card draws by three
        FreeDraws += 3;
    }

    // The method used to make the teleport spell card take effect
    private void PlayTeleport(Vector3 spellPosition)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float greatestDistance = teleportRadius * Board.greatestBoardDimension;

        // Initialize the distance variable
        //float distance = 0;

        // Initialize the closest enemy variable
        List<GameObject> enemiesInRange = EnemiesInRange(spellPosition, greatestDistance);

        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            // Go through all enemies in the list
            foreach(GameObject enemy in enemiesInRange)
            {
                // Set the position of the enemy to the position of the enemy spawn
                enemy.transform.position = Waypoints.enemySpawn.transform.position;

                // Set the waypoint index of the enemy to 0
                enemy.GetComponent<Enemy>().waypointIndex = 0;
            }
        }
    }

    private void PlaySpaceDistortion(Vector3 spellPosition)
    {
        StartCoroutine(SpaceDistortion(spellPosition));
    }

    // The method used to make the space distortion card take effect
    private IEnumerator SpaceDistortion(Vector3 spellPosition)
    {
        float timer = 0f;
        float range = spaceDistortionRadius * Board.greatestBoardDimension;
        StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.SpaceDistortion, spellPosition, spaceDistortionDuration));
        while (timer < spaceDistortionDuration)
        {
            List<GameObject> enemiesInRange = EnemiesInRange(spellPosition, range);
            if (enemiesInRange != null)
            {
                foreach (GameObject enemy in enemiesInRange)
                {
                    // Set the personal slow factor of the enemies to the space distortion factor
                    enemy.GetComponent<Enemy>().enemySlowFactor = spaceDistortionFactor;
                }
            }
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
            foreach (GameObject enemy in enemiesInRange)
            {
                // Reset the personal slow factor of the enemies
                enemy.GetComponent<Enemy>().enemySlowFactor = 1;
            }
        }
    }

    // Coroutine that makes the space distortion spell card take effect
    private IEnumerator SlowEnemies(List<GameObject> enemies, float timer)
    {
        // Go through all enemies in the list
        foreach(GameObject enemy in enemies)
        {
            // Set the personal slow factor of the enemies to the space distortion factor
            enemy.GetComponent<Enemy>().enemySlowFactor = spaceDistortionFactor;
        }

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= spaceDistortionDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(timer);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited += 0.1f;
            }
        }

        // Go through all enemies in the list
        foreach(GameObject enemy in enemies)
        {
            // Reset the personal slow factor of the enemies
            enemy.GetComponent<Enemy>().enemySlowFactor = 1;
        }
    }

    // The method used to make the slow time spell card take effect
    private void PlaySlowTime()
    {
        StartCoroutine(SlowTime());
    }

    // Coroutine that makes the slow time spell card take effect
    private IEnumerator SlowTime()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = slowTimeFactor;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= slowTimeDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited += 0.1f;
            }
        }

        // Wait for the duration of the slow time card
        yield return new WaitForSeconds(slowTimeDuration);

        // Remove the slow time effect
        GameAdvancement.globalSlow = 1;
    }

    // The method used to make the stop time spell card take effect
    private void PlayStopTime()
    {
        StartCoroutine(StopTime());
    }

    // Coroutine that makes the stop time spell card take effect
    IEnumerator StopTime()
    {
        // Stop time
        GameAdvancement.timeStopped = true;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= stopTimeDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited += 0.1f;
            }
        }

        // Remove the stop time
        GameAdvancement.timeStopped = false;
    }

    // The method used to make the rain card take effect
    private void PlayRain()
    {
        StartCoroutine(Rain());
    }

    // Coroutine that makes the rain spell card take effect
    private IEnumerator Rain()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = rainFactor;

        // Set the raining flag to true
        GameAdvancement.raining = true;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= rainDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited += 0.1f;
            }
        }

        // Remove the slow effect
        GameAdvancement.globalSlow = 1;

        // Set the raining flag to false
        GameAdvancement.raining = false;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell card helper methods
    //---------------------------------------------------------------------------------------------------------------

    // The method used to get the list of objects in a certain range of a game object
    private List<GameObject> EnemiesInRange(Vector3 position, float radius)
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Initialize the maximum distance where the meteor damage should still happen

            // Initialize the closest enemy variable
            List<GameObject> enemiesInRange = new List<GameObject>();

            // Find the enemy that is the closest to the spell card
            foreach(GameObject enemy in enemies)
            {
                // Calculate the distance between the spell cards location and this enemy
                float distance = Vector3.Distance(position, enemy.transform.position);
                // Check if this distance is smaller than the current smallest enemy
                if(distance < radius)
                {
                    // Add this enemy to the enemies in range array
                    enemiesInRange.Add(enemy);              
                }
            }

            // Return the list of the enemies in range
            return enemiesInRange;

        } else {

            // Return nul if there are no enemies in range
            return null;
        }
    }

    // The method used to make the damage in radius effect take place
    private void PlaySpellDamageInRadius(Vector3 center, float radius, int damage)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float closestDistance = radius * Board.greatestBoardDimension;

        // Initialize the distance variable
        // float distance = 0;

        // Initialize the closest enemy variable
        List<GameObject> enemiesInRange = EnemiesInRange(center, closestDistance);

        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            // Go through all enemies in the list
            foreach(GameObject enemy in enemiesInRange)
            {
                // Damage this enemy by the meteor damage
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    // The method used to reset the spell card deck
    public static void ResetSpellCardDeck()
    {
        // Reset the number of free draws
        FreeDraws = 0;

        // Set the number of drawn spells on the board to 0
        DrawnSpellsOnBoard = 0;
    }
}
