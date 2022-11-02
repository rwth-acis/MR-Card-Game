using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{

    #region Serializable Fields
    [Header("Properties")]
    // The type of the enemy
    [SerializeField]
    private EnemyType enemyType;

    [Tooltip("What the enemy is resistant to")]
    [SerializeField]
    private ResistenceAndWeaknessType resistance;

    [Tooltip("What the enemy is weak to")]
    [SerializeField]
    private ResistenceAndWeaknessType weakness;

    [SerializeField]
    private int maximumHP;

    [SerializeField]
    private int currentHP;

    [Tooltip("The size of the enemy unit")] 
    [SerializeField]
    private float size;

    [Tooltip("The initial speed of the enemy in m/s")]
    [SerializeField]
    private float movingSpeed;

    [Tooltip("The damage that the enemy unit deals to the castle if it is reached.")] 
    [SerializeField]
    private int damage;

    [Tooltip("The currency points won when defeating the enemy")] 
    [SerializeField]
    private int currencyPoints;

    [Tooltip("The height of fly in meter, if zero then the unit cannot fly")]
    [SerializeField]
    private float flyingHeight;

    [Tooltip("The personal slow factor of the enemy")]
    public float enemySlowFactor = 1;

    [Header("Control")]
    [SerializeField]
    private GameObject healthBarUI;

    [Tooltip("The health bar slider")] 
    [SerializeField]
    private Slider healthBar;

    [Tooltip("The current waypoint index so that enemies go from waypoint to waypoint")] 
    [SerializeField] private int waypointIndex = 0;

    // if the enemy is currently alive or not
    [SerializeField] private bool isAlive = true;

    [Tooltip("if an enemy is wet or not")] 
    [SerializeField] private bool isWet = false;

    [Tooltip(" The flag that states if the enemy need its position to be reset or not")]
    [SerializeField] private int firstLife = 0;

    public int numberOfTrapsIn = 0;

    #endregion

    #region Non-Serializable Fields

    // Waypoints placed on the path that enemies have to travel
    private Transform[] waypoints;
    #endregion

    #region Properties
    // Method used to get the type of the enemy
    public EnemyType EnemyType
    {
        get { return enemyType; }
    }

    public int MaximumHP
    {
        get { return maximumHP; }
    }

    public int CurrentHP
    {
        get { return currentHP; }
    }

    public float FlyingHeight
    {
        get => flyingHeight;
    }

    public ResistenceAndWeaknessType Resistance
    {
        get => resistance;
        set => resistance = value;
    }

    public ResistenceAndWeaknessType Weakness
    {
        get => weakness;
        set => weakness = value;
    }

    public int WaypointIndex
    {
        get => waypointIndex;
        set => waypointIndex = value;
    }

    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }

    public bool IsWet
    {
        get => isWet;
        set => isWet = value;
    }

    public int FirstLife
    {
        get => firstLife;
        set => firstLife = value;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Initialize();

    }

    // Update is called once per frame
    void Update()
    {

        if (IsAlive)
        {
            // If the current health of the unit is not at its maximum, activate the health bar
            if (currentHP < maximumHP)
            {
                healthBarUI.SetActive(true);
            }
            // If the game is not paused, update
            if (GameAdvancement.gamePaused == false)
            {
                // Kill the enemy if it its health points reach zero
                if (currentHP <= 0 && IsAlive == true)
                {
                    IsAlive = false;
                    GetCurrencyPoints();
                    // Reset the current waypoint index so that enemies walk toward the first waypoint upon respawn
                    WaypointIndex = 0;
                    LevelInfo.numberOfEnemiesDefeated++;
                    StartCoroutine(Die());
                }

                if (GameAdvancement.timeStopped == false)
                {
                    Move();
                }
            }
        }

        
    }

    private void Move()
    {
        // If the last waypoint was not reached, move the enemy
        if(WaypointIndex <= waypoints.Length - 1)
        {
            // Get the current goal that is the position of the next waypoint, added with a height
            Vector3 currentGoal = waypoints[WaypointIndex].transform.position + transform.up * flyingHeight;

            // Move the enemy toward the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentGoal, movingSpeed * GameAdvancement.globalSlow * enemySlowFactor * Time.deltaTime);
            transform.LookAt(currentGoal);

            // If the enemy reached the position of a waypoint, increase the waypoint index by one
            if(ReachPosition(currentGoal))
            {
                WaypointIndex++;
            }
        }

        // if the enemy reached the castle
        if(WaypointIndex == waypoints.Length)
        {
            IsAlive = false;
            ReturnEnemyToObjectPool();
            LevelInfo.numberOfEnemiesThatReachedTheCastle++;
            ReduceCastleHealth();
        }
    }

    private bool ReachPosition(Vector3 position)
    {
        if(Vector3.Magnitude(transform.position - position) <= 0.005f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float UpdateHealth()
    {
        return (float)currentHP / maximumHP;
    }

    /// <summary>
    /// Rewards the player with currency points if an enemy is defeated
    /// </summary>
    public void GetCurrencyPoints()
    {
        // Add the enemy value to the currency points of the player
        GameAdvancement.currencyPoints += currencyPoints;
        GameSetup.UpdateCurrencyDisplay();
    }

    /// <summary>
    /// reduces the health points of the castle if an enemy reaches it
    /// </summary>
    public void ReduceCastleHealth()
    {
        // Reduce the number of undefeated enemies of the wave by one
        LevelInfo.numberOfUndefeatedEnemies--;

        LevelInfo.numberOfEnemiesDefeatedOrReachedCastleCurrentWave++;

        Level.Instance.UpdateEnemyNumberDisplay();

        // Check if the castle has armor points
        if(GameAdvancement.castleCurrentAP != 0) 
        {
            // Check if the armor points of the castle exceed the damage of the enemy
            if(GameAdvancement.castleCurrentAP >= damage)
            {
                // Reduce the castle armor points by the enemy damage
                GameAdvancement.castleCurrentAP -= damage;

            } else {
                int additionalDamage = damage - GameAdvancement.castleCurrentAP;
                GameAdvancement.castleCurrentAP = 0;
                GameAdvancement.castleCurrentHP -= additionalDamage;
            }
            
        } else {
            GameAdvancement.castleCurrentHP -= damage;
        }
        GameSetup.UpdateCastleHealthPoints();
    }

    /// <summary>
    /// Enemy takes damage
    /// </summary>
    public void TakeDamage(int damage)
    {
        if(currentHP - damage >= 0)
        {
            currentHP -= damage;
        } else {
            currentHP = 0;
        }
        healthBar.value = UpdateHealth();
    }

    public void ReturnEnemyToObjectPool()
    {
        ObjectPools.ReleaseEnemy(this);
    }

    /// <summary>
    /// Set the health points of the enemy correctly upon respawn
    /// </summary>
    public void Initialize()
    {
        waypoints = Waypoints.mapWaypoints;
        WaypointIndex = 0;

        // Deactivate the health bar since it is full
        healthBarUI.SetActive(false);

        // Scale the enemy down to have the right size
        transform.localScale = new Vector3(0.25f * size, 0.25f * size, 0.25f * size);

        // Set it to the position of the first waypoint on spawn
        transform.position = (waypoints[WaypointIndex].transform.position + transform.up * flyingHeight);
        IsAlive = true;
        currentHP = MaximumHP;

        healthBar.value = UpdateHealth();
    }

    IEnumerator Die()
    {
        LevelInfo.numberOfUndefeatedEnemies--;
        LevelInfo.numberOfEnemiesDefeatedOrReachedCastleCurrentWave++;
        Level.Instance.UpdateEnemyNumberDisplay();
        yield return new WaitForSeconds(0.5f);
        ReturnEnemyToObjectPool();
    }
}
