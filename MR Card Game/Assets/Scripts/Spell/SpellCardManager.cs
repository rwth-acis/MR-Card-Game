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
    private float thunderStrikeDamageAttenuationFactor;

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

    [Header("Spell Costs")]

    [SerializeField]
    private int spellDeckRefillCost;

    [SerializeField]
    private int meteorCost;

    [SerializeField]
    private int arrowRainCost;

    [SerializeField]
    private int thunderStrikeCost;

    [SerializeField]
    private int teleportCost;

    [SerializeField]
    private int armorCost;

    [SerializeField]
    private int drawCost;

    [SerializeField]
    private int obliterationCost;

    [SerializeField]
    private int stopTimeCost;

    [SerializeField]
    private int slowTimeCost;

    [SerializeField]
    private int rainCost;

    [SerializeField]
    private int spaceDistortionCost;

    [SerializeField]
    private int healingCost;

    [Header("UI Components")]
    [SerializeField]
    private GameObject answerQuestions;

    [SerializeField]
    private GameObject groundPlane;

    [SerializeField]
    private GameObject gameBoard;

    [SerializeField]
    private DurationDisplay rainDurationDisplay;

    [SerializeField]
    private DurationDisplay stopTimeDurationDisplay;

    [SerializeField]
    private DurationDisplay slowTimeDurationDisplay;

    [SerializeField]
    private DurationDisplay spaceDistortionDurationDisplay;

    [SerializeField]
    private Button currencyDisplay;

    [SerializeField]
    private Button waveDisplay;

    [SerializeField]
    private Button startNextWaveButton;

    [SerializeField]
    private GameObject refillSpellDeckWindow;

    [SerializeField]
    private TMP_Text refillSpellDeckWindowText;

    [SerializeField]
    private GameObject refillSpellDeckFailedWindow;

    [Tooltip("The spell range indicator which has a size of (1,1,1), i.e. 1 meter, originally, needs to be scaled")]
    [SerializeField]
    private GameObject spellRangeIndicator;

    [SerializeField]
    private GameObject healingEffect;
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
        InitializeSpellDeck();
    }

    //---------------------------------------------------------------------------------------------------------------
    // Initializing the deck of cards
    //---------------------------------------------------------------------------------------------------------------

    // Put the right amount of cards in the deck of cards
    private void InitializeSpellDeck()
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
    /// Called when the "Yes" button of the refill spell deck window is clicked.
    /// </summary>
    public void RefillSpellDeck()
    {
        if(GameAdvancement.currencyPoints >= spellDeckRefillCost)
        {
            GameAdvancement.currencyPoints -= spellDeckRefillCost;
            GameSetup.UpdateCurrencyDisplay();
            InitializeSpellDeck();
        }
        else
        {
            refillSpellDeckFailedWindow.SetActive(true);
        }

    }

    public void OpenRefillSpellDeckWindow()
    {
        refillSpellDeckWindow.SetActive(true);
        refillSpellDeckWindowText.text = $"Do you want to pay {spellDeckRefillCost} currency to refill the spell deck? You will not get additional spell cards even if you still have some in the deck.";
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
            DrawnSpellsOnBoard--;
            Debug.Log("The number of drawn card spells that are on the board is: " + DrawnSpellsOnBoard);
            // Check if no spell card is drawn
            if (NumberDrawnSpellCards() == 0)
            {
                GameAdvancement.gamePaused = false;
            }

            // Check if the number of drawn spell cards that are on the board is 0
            if (DrawnSpellsOnBoard <= 0)
            {
                GameAdvancement.gamePaused = false;
            }
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
            return true;
        }
        else
        {
            DrawnSpellsOnBoard--;
            // Check if the number of drawn spell cards that are on the board is 0
            if (DrawnSpellsOnBoard <= 0)
            {
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

    // Get an spell effect object from the pool, and return it to the pool after the effectiveTimer.
    // Do not do damage.
    private IEnumerator SpwanSpellAnimationEffect(SpellType spellType, Vector3 position, float effectiveTimer, bool pauseGame)
    {
        GameAdvancement.gamePaused = pauseGame;
        GameObject spellEffect = SpawnSpellEffect.SpawnSpellFromPool(spellType);
        spellEffect.transform.position = position;
        // Wait for the animation to play, if any.
        yield return new WaitForSeconds(effectiveTimer);
        ObjectPools.ReleaseSpellEffect(spellEffect, spellType);
        GameAdvancement.gamePaused = false;
    }

    // Play the meteor effect
    private void PlayMeteor(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        StartCoroutine(PlaySpellDamageInRadiusDelayed(spellPosition, meteorRadius, meteorDamage, SpawnSpellEffect.MeteorAnimationDuration));
        StartCoroutine(SpwanSpellAnimationEffect(SpellType.Meteor, spellPosition, SpawnSpellEffect.MeteorAnimationDuration + 1f, true));
    }

    // Play the arrow rain effect
    private void PlayArrowRain(Vector3 spellPosition)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        StartCoroutine(PlaySpellDamageInRadiusDelayed(spellPosition, arrowRainRadius, arrowRainDamage, SpawnSpellEffect.ArrowRainAnimationDuration));
        StartCoroutine(SpwanSpellAnimationEffect(SpellType.ArrowRain, spellPosition, SpawnSpellEffect.ArrowRainAnimationDuration + 0.5f, true));
    }

    private void PlayThunderStrike(Vector3 spellPosition)
    {
        StartCoroutine(PlayThunderStrikeCoro(spellPosition));
    }

    private IEnumerator PlayThunderStrikeCoro(Vector3 spellPosition)
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
                if(closestEnemy != null)
                {
                    thunderStrikePosition = closestEnemy.transform.position;
                    int finalDamage = (int)(initialDamage * Mathf.Pow(thunderStrikeDamageAttenuationFactor, i));
                    if(GameAdvancement.raining == false && closestEnemy.GetComponent<Enemy>().IsWet)
                    {
                        finalDamage = (int) (finalDamage * thunderStrikeDamageRainingMultiplier);                       
                    }
                    closestEnemy.GetComponent<Enemy>().TakeDamage(finalDamage);
                    // Let the thunder strike prefab show 0.5 seconds.
                    StartCoroutine(SpwanSpellAnimationEffect(SpellType.ThunderStrike, closestEnemy.transform.position, 0.5f, true));
                    enemies.Remove(closestEnemy);
                    // Play the thunder strike effect separately, so that players can better see the jump effect.
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    yield return null;
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
        StartCoroutine(ActivateHealingEffectOnCastle(1f));
    }

    private IEnumerator ActivateHealingEffectOnCastle(float duration)
    {
        healingEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        healingEffect.SetActive(false);
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
        spaceDistortionDurationDisplay.StartCountDown(spaceDistortionDuration);
    }

    // Slow enemies in a certain area
    private IEnumerator SpaceDistortion(Vector3 spellPosition)
    {
        float timer = 0f;
        float range = spaceDistortionRadius;
        StartCoroutine(SpwanSpellAnimationEffect(SpellType.SpaceDistortion, spellPosition, spaceDistortionDuration, false));
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
        slowTimeDurationDisplay.StartCountDown(slowTimeDuration);
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
        stopTimeDurationDisplay.StartCountDown(stopTimeDuration);
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
        rainDurationDisplay.StartCountDown(rainDuration);
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

    // Play spell damage with a time delay, used especially for spells that have an animation.
    private IEnumerator PlaySpellDamageInRadiusDelayed(Vector3 center, float radius, int damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySpellDamageInRadius(center, radius, damage);
    }

    /// <summary>
    /// Reset the spell card deck
    /// </summary>
    public static void ResetSpellCardDeck()
    {
        FreeDraws = 0;
        DrawnSpellsOnBoard = 0;
        Instance.InitializeSpellDeck();
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

    public static int GetSpellCost(SpellType spellType)
    {
        return spellType switch
        {
            SpellType.Armor => Instance.armorCost,
            SpellType.Draw => Instance.drawCost,
            SpellType.Rain => Instance.rainCost,
            SpellType.Healing => Instance.healingCost,
            SpellType.ArrowRain => Instance.arrowRainCost,
            SpellType.SpaceDistortion => Instance.spaceDistortionCost,
            SpellType.Teleport => Instance.teleportCost,
            SpellType.ThunderStrike => Instance.teleportCost,
            SpellType.StopTime => Instance.stopTimeCost,
            SpellType.SlowTime => Instance.slowTimeCost,
            SpellType.Meteor => Instance.meteorCost,
            SpellType.Obliteration => Instance.obliterationCost,
            _ => 0
        };
    }
}
