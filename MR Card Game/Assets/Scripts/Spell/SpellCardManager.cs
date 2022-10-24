using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vuforia;
using System;

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

    [Tooltip("number of jumps of thunder strike")]
    [SerializeField]
    private int thunderStrikeJumps;

    [Tooltip("searching radius in meter when thunder strike is jumping in meter")]
    [SerializeField] 
    private float thunderStrikeRadius;

    [Tooltip("the multiplying factor of the radius when it is raining")]
    [SerializeField]
    private float thunderStrikeRadiusRainingMultiplier;

    [Tooltip("the initial damage of thunderStrike")]
    [SerializeField]
    private float thunderStrikeDamage;

    [Tooltip("the multiplying factor of the damage when it is raining")]
    [SerializeField]
    private float thunderStrikeDamageRainingMultiplier;

    [Tooltip("the percentage of damage that is reduced after a jump")]
    [SerializeField]
    private float thunderStrikeDamageReducingFactor;

    [Tooltip("radius in meter")]
    [SerializeField]
    private float meteorRadius;

    [SerializeField]
    private int meteorDamage;

    [Tooltip("The arrow rain radius in meter")] 
    [SerializeField]
    private float arrowRainRadius;

    [SerializeField]
    private int arrowRainDamage;

    [Tooltip("The teleport radius in meter")] 
    [SerializeField]
    private float teleportRadius;

    [Tooltip("The space distortion radius in meter")]
    [SerializeField]
    private float spaceDistortionRadius;

    [Tooltip("the duration in second")]
    [SerializeField]
    private float spaceDistortionDuration;

    [Tooltip("The space distortion factor for slowing down")]
    [SerializeField]
    private float spaceDistortionFactor;

    [Tooltip("The duration the time should be stopped in second")] 
    [SerializeField]
    private float stopTimeDuration;

    [Tooltip("The duration the time should be slowed in second")] 
    [SerializeField]
    private float slowTimeDuration;

    [Tooltip("The factor by which the time should be slowed when the slow time card is played")] 
    [SerializeField]
    private float slowTimeFactor;

    [Tooltip("The duration the rain should last in second")] 
    [SerializeField]
    private float rainDuration;

    [Tooltip("The factor by which the time should be slowed when the rain card is played")]
    [SerializeField]
    private float rainSlowingFactor;

    [Tooltip("The armor point to add for one armor spell")]
    [SerializeField]
    private int armorPointAmount;

    [Tooltip("The HP to add for one healing spell")]
    [SerializeField]
    private int healingAmount;

    [Tooltip("The free draw amount to add for one draw spell")]
    [SerializeField]
    private int drawAmount;

    [Header("UI Components")]
    [SerializeField]
    private GameObject answerQuestions;

    [SerializeField]
    private GameObject groundPlane;

    [SerializeField]
    private GameObject gameBoard;

    [SerializeField]
    private Button currencyDisplay;

    [SerializeField]
    private Button waveDisplay;

    [SerializeField]
    private Button startNextWaveButton;

    [Tooltip("The spell range indicator which has a size of (1,1,1), i.e. 1 meter, originally, needs to be scaled")]
    [SerializeField]
    private GameObject spellRangeIndicator;
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

    /// <summary>
    /// A sphere whose size is 1m*1m*1m
    /// </summary>
    public static GameObject SpellRangeIndicator
    {
        get => Instance.spellRangeIndicator;
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeCardDeck();
    }

    //---------------------------------------------------------------------------------------------------------------
    // Initializing the deck of cards
    //---------------------------------------------------------------------------------------------------------------

    // Put the right amount of cards in the deck of cards
    private void InitializeCardDeck()
    {
        cardDeck = new Dictionary<SpellType, int>
        {
            { SpellType.Meteor, 5 },
            { SpellType.ArrowRain, 5 },
            { SpellType.ThunderStrike, 4 },
            { SpellType.Armor, 4 },
            { SpellType.Healing, 4 },
            { SpellType.Obliteration, 1 },
            { SpellType.Draw, 1 },
            { SpellType.Teleport, 3 },
            { SpellType.SpaceDistortion, 3 },
            { SpellType.SlowTime, 2 },
            { SpellType.StopTime, 1 },
            { SpellType.Rain, 2 }
        };
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

                case SpellType.Healing:
                    PlayHealing();
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
            DrawnSpellsOnBoard--;

            Debug.Log("The number of drawn card spells that are on the board is: " + DrawnSpellsOnBoard);

            // Check if the number of drawn spell cards that are on the board is 0
            if (DrawnSpellsOnBoard <= 0)
            {
                Debug.Log("The game is being unpaused after a spell card has been played");
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

    /// <summary>
    /// Check how many spell cards are currently drawn
    /// </summary>
    public int NumberDrawnSpellCards()
    {
        // Get all spell cards with tag
        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("Spell Card");
        int count = 0;
        foreach(GameObject card in spellcards)
        {
            if(card.GetComponent<SpellCardController>().CardDrawn == true)
            {
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
        GameObject spellEffect = SpawnSpellEffect.SpawnSpellFromPool(spellType);
        spellEffect.transform.position = position;
        float timeWaited = 0;
        // Wait until the effective timer is reached
        while (timeWaited <= effectiveTimer)
        {
            yield return new WaitForSeconds(0.1f);
            if (GameAdvancement.gamePaused == false)
            {
                timeWaited += 0.1f;
            }
        }
        ObjectPools.ReleaseSpellEffect(spellEffect, spellType);     
    }

    // Play the meteor effect
    private void PlayMeteor(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlaySpellDamageInRadius(spellPosition, meteorRadius, meteorDamage);
        StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.Meteor, spellPosition, 1f));
    }

    // Play the arrow rain effect
    private void PlayArrowRain(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlaySpellDamageInRadius(spellPosition, arrowRainRadius, arrowRainDamage);
        StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.ArrowRain, spellPosition, 1f));
    }

    private void PlayThunderStrike(Vector3 spellPosition)
    {
        // Initialize and fill the enemies array
        List<GameObject> enemies = new(GameObject.FindGameObjectsWithTag("Enemy"));
        // Check that there are enemies on the board
        if(enemies.Count > 0)
        {
            float radius = GameAdvancement.raining ? thunderStrikeRadiusRainingMultiplier * thunderStrikeRadius : thunderStrikeRadius;
            float initialDamage = GameAdvancement.raining ? thunderStrikeDamageRainingMultiplier * thunderStrikeDamage : thunderStrikeDamage;
            Vector3 thunderStrikePosition = spellPosition;
            // Find the enemy that is the closest to the thunder strike position
            for (int i = 0; i < thunderStrikeJumps; i++)
            {
                // initialize the closestEnemy to null to check whether there is an enemy in the range later.
                GameObject closestEnemy = null;
                float closestDistance = radius;
                foreach (GameObject enemy in enemies)
                {
                    float distance = Vector3.Distance(thunderStrikePosition, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
                Debug.Log(closestEnemy);    
                if(closestEnemy != null)
                {
                    thunderStrikePosition = closestEnemy.transform.position;
                    closestEnemy.GetComponent<Enemy>().TakeDamage((int)(initialDamage * Mathf.Pow(thunderStrikeDamageReducingFactor, i)));
                    StartCoroutine(ActivateSpellAnimationAndEffect(SpellType.ThunderStrike, closestEnemy.transform.position, 0.5f));
                    enemies.Remove(closestEnemy);
                }
                else
                {
                    return;
                }
            }       
        }
    }

    private void PlayArmor()
    {
        GameAdvancement.castleCurrentAP += armorPointAmount;
        GameSetup.UpdateCastleHealthPoints();
    }

    private void PlayHealing()
    {
        // Check if the plus five health points heal would exceed the castle's maximum health points
        if(GameAdvancement.castleCurrentHP + healingAmount > GameAdvancement.castleMaxHP)
        {
            GameAdvancement.castleCurrentHP = GameAdvancement.castleMaxHP;

        } else {

            GameAdvancement.castleCurrentHP += healingAmount;
        }
        GameSetup.UpdateCastleHealthPoints();
    }

    private void PlayObliteration()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Check that there are enemies on the board
        if(enemies != null)
        {
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
            GameAdvancement.castleCurrentHP = GameAdvancement.castleMaxHP;
        }
        GameSetup.UpdateCastleHealthPoints();
    }

    private void PlayDraw()
    {
        FreeDraws += drawAmount;
    }

    // Teleport the enemies to the spawn point
    private void PlayTeleport(Vector3 spellPosition)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float greatestDistance = teleportRadius;
        List<GameObject> enemiesInRange = EnemiesInRange(spellPosition, greatestDistance);
        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            foreach(GameObject enemy in enemiesInRange)
            {
                enemy.transform.position = Waypoints.enemySpawn.transform.position;
                enemy.GetComponent<Enemy>().WaypointIndex = 0;
            }
        }
    }

    private void PlaySpaceDistortion(Vector3 spellPosition)
    {
        StartCoroutine(SpaceDistortion(spellPosition));
    }

    // Slow enemies in a certain area
    private IEnumerator SpaceDistortion(Vector3 spellPosition)
    {
        float timer = 0f;
        float range = spaceDistortionRadius;
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

    private void PlaySlowTime()
    {
        StartCoroutine(SlowTime());
    }

    // Coroutine that makes the slow time spell card take effect
    private IEnumerator SlowTime()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = slowTimeFactor;
        float timeWaited = 0;
        while(timeWaited <= slowTimeDuration)
        {
            yield return new WaitForSeconds(0.1f);
            if(GameAdvancement.gamePaused == false)
            {
                timeWaited += 0.1f;
            }
        }
        // Remove the slow time effect
        GameAdvancement.globalSlow = 1;
    }

    private void PlayStopTime()
    {
        StartCoroutine(StopTime());
    }

    // Coroutine that makes the stop time spell card take effect
    IEnumerator StopTime()
    {
        // Stop time
        GameAdvancement.timeStopped = true;
        float timeWaited = 0;
        while(timeWaited <= stopTimeDuration)
        {
            yield return new WaitForSeconds(0.1f);
            if(GameAdvancement.gamePaused == false)
            {
                timeWaited += 0.1f;
            }
        }
        // Remove the stop time
        GameAdvancement.timeStopped = false;
    }

    private void PlayRain()
    {
        StartCoroutine(Rain());
    }

    private IEnumerator Rain()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = rainSlowingFactor;
        GameAdvancement.raining = true;
        float timeWaited = 0;
        while(timeWaited <= rainDuration)
        {
            yield return new WaitForSeconds(0.1f);
            if(GameAdvancement.gamePaused == false)
            {
                timeWaited += 0.1f;
            }
        }

        // Remove the slow effect
        GameAdvancement.globalSlow = 1;
        GameAdvancement.raining = false;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell card helper methods
    //---------------------------------------------------------------------------------------------------------------

    // Get the list of objects in a certain range of a game object
    private List<GameObject> EnemiesInRange(Vector3 position, float radius)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Check that there are enemies on the board
        if(enemies != null)
        {
            List<GameObject> enemiesInRange = new List<GameObject>();
            // Find the enemies in the given range
            foreach(GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                if(distance < radius)
                {
                    enemiesInRange.Add(enemy);              
                }
            }
            return enemiesInRange;

        } else {
            return null;
        }
    }

    // Do damage in a certain area with the given radius and center
    private void PlaySpellDamageInRadius(Vector3 center, float radius, int damage)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float closestDistance = radius;
        List<GameObject> enemiesInRange = EnemiesInRange(center, closestDistance);
        if(enemiesInRange != null)
        {
            foreach(GameObject enemy in enemiesInRange)
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// Reset the spell card deck
    /// </summary>
    public static void ResetSpellCardDeck()
    {
        FreeDraws = 0;
        DrawnSpellsOnBoard = 0;
        Instance.InitializeCardDeck();
    }

    /// <summary>
    /// Activated when the spell image target enters the camera field
    /// </summary>
    /// <param name="imageTargetBehaviour">the ImageTargetBehaviour script added in inspector</param>
    public void SpellCardEnteredCameraField(ImageTargetBehaviour imageTargetBehaviour)
    {
        SpellCardController controller = imageTargetBehaviour.gameObject.GetComponent<SpellCardController>();
        controller.SpellCardEnteredCameraField(imageTargetBehaviour);
    }

    /// <summary>
    /// Activated when the spell image target leaves the camera field
    /// </summary>
    /// <param name="imageTargetBehaviour">the ImageTargetBehaviour script added in inspector</param>
    public void SpellCardLeftCameraField(ImageTargetBehaviour imageTargetBehaviour)
    {
        SpellCardController controller = imageTargetBehaviour.gameObject.GetComponent<SpellCardController>();
        controller.SpellCardLeftCameraField();
    }

    /// <summary>
    /// Get the radius of the spell with the given type
    /// If the given type doesn't have a radius property, return 0.
    /// </summary>
    public static float GetSpellRadiusWithType(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.Meteor:
                return Instance.meteorRadius;
            case SpellType.ArrowRain:
                return Instance.arrowRainRadius;
            case SpellType.SpaceDistortion:
                return Instance.spaceDistortionRadius;
            case SpellType.Teleport:
                return Instance.teleportRadius;
            case SpellType.ThunderStrike:
                return GameAdvancement.raining ? Instance.thunderStrikeRadius * Instance.thunderStrikeRadiusRainingMultiplier : Instance.thunderStrikeRadius;
            default:
                Debug.LogWarning("The given spell type doesn't have a radius property.");
                return 0;
        }
    }
}
