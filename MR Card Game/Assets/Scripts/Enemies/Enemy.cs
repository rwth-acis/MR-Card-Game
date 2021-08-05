using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    // Waypoints placed on the path that enemies have to travel
    private Transform[] waypoints;

    // The type of the enemy
    [SerializeField]
    private string enemyType;

    // Method used to get the type of the enemy
    public string GetEnemyType
    {
        get { return enemyType; }
    }

    // The maximum and current health point of the enemy unit
    [SerializeField]
    private int maximumHP;

    // Method used to get the type of the enemy
    public int GetMaximumHP
    {
        get { return maximumHP; }
    }

    [SerializeField]
    private int currentHP;

    // Method used to get the type of the enemy
    public int GetCurrentHP
    {
        get { return currentHP; }
    }

    // The size of the enemy unit
    [SerializeField]
    private float size;

    // The movement speed of the enemy unit
    [SerializeField]
    private float moveSpeed;

    // The damage that the enemy unit deals to the castle if it is reached
    [SerializeField]
    private int damage;

    // The currency points won when defeating the enemy
    [SerializeField]
    private int enemyValue;

    // The height of fly, if zero then the unit cannot fly
    [SerializeField]
    private float flying;

    // Initialize the flight height variable
    private float flightHeight;

    // Method used to get the flight height of the enemy
    public float GetFlightHeight
    {
        get { return flightHeight; }
    }

    // The UI for the health bar
    [SerializeField]
    private GameObject healthBarUI;

    // The health bar slider
    [SerializeField]
    private Slider healthBar;

    // What the enemy is resistant to
    [SerializeField]
    private string resistance;

    // Method used to get the restistance of the enemy
    public string GetEnemyResistance
    {
        get { return resistance; }
    }

    // What the enemy is weak to
    [SerializeField]
    private string weakness;

    // Method used to get the weakness of the enemy
    public string GetEnemyWeakness
    {
        get { return weakness; }
    }

    // The gameboard game object
    private GameObject gameBoard;

    // The current waypoint index so that enemies go from waypoint to waypoint
    private int waypointIndex = 0;

    // The flag that tells if the enemy is currently alive or not
    public bool isAlive = true;

    // // Method used to get the weakness of the enemy
    // public bool isAlive
    // {
    //     get { return alive; }
    // }

    // Start is called before the first frame update
    void Start()
    {
        // Set the gameboard object
        gameBoard = Board.gameBoard;
        
        // Set the waypoints that apply to this map
        waypoints = Waypoints.mapWaypoints;

        // Set the enemy as child of the gameboard
        transform.parent = gameBoard.transform;

        // When spawning, set the current health points to the maximum health points
        ReviveEnemy();

        // Set the flight height and standing size
        flightHeight = flying * Board.greatestBoardDimension * (float)0.6  + (float)0.2 * size * Board.greatestBoardDimension;

        // Deactivate the health bar since it is full
        healthBarUI.SetActive(false);

        // Set the health bar correctly
        healthBar.value = CalculateHealth();

        // Scale the enemy down to have the right size
        transform.localScale = new Vector3((float)0.3 * size, (float)0.3 * size, (float)0.3 * size);

        // Set it to the position of the first waypoint on spawn
        transform.position = (waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0));

        // Set the game board correctly
        gameBoard = Board.gameBoard;
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is not paused, update
        if(GameAdvancement.gamePaused == false && isAlive == true)
        {
            // If the current health of the unit is not at its maximum, activate the health bar
            if(currentHP < maximumHP)
            {
                healthBarUI.SetActive(true);
            }

            // Kill the enemy if it its health points reach zero
            if(currentHP <= 0 && isAlive == true)
            {
                // Set the enemy as dead
                isAlive = false;

                // Make the player win the currency points
                WinPoints();

                // Reduce the number of undefeated enemies of the wave by one
                LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfUndefeatedEnemies - 1;

                // Make the enemy die
                StartCoroutine(Die());
            }

            // Make the enemy mode
            Move();

            // Set the health value correctly
            healthBar.value = CalculateHealth();
        }
    }

    // Method that make the enemy walk
    private void Move()
    {
        // If the last waypoint was not reached, move the enemy
        if(waypointIndex <= waypoints.Length - 1)
        {
            // Move the enemy toward the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0), moveSpeed * Time.deltaTime * gameBoard.transform.localScale.x);

            // // Make the enemy face the direction it is moving
            // transform.LookAt(waypoints[waypointIndex].transform.position);

            // If the enemy reached the position of a waypoint, increase the waypoint index by one
            if(transform.position == (waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0)))
            // if(transform.position.x == waypoints[waypointIndex].transform.position.x && transform.position.z == waypoints[waypointIndex].transform.position.z)
            {
                // // Set the passed waypoint as last waypoint
                // lastWaypoint = waypoints[waypointIndex].transform.position;

                // Increase the waypoint index by one
                waypointIndex = waypointIndex + 1;

                if(waypointIndex <= waypoints.Length - 1)
                {
                    // Make the enemy face the direction it is moving
                    transform.LookAt(waypoints[waypointIndex].transform.position);

                    // Change the scaling of the enemy
                }
            }
        }

        // Check if the enemy reached the castle
        if(waypointIndex == waypoints.Length)
        {
            // Set the enemy as dead
            isAlive = false;

            // Return the enemy to the object pool
            ReturnEnemyToObjectPool();

            // Reduce the health of the castle
            ReduceCastleHealth();
        }
    }

    // Method that calculates the health value
    public float CalculateHealth()
    {
        return (float)currentHP / (float)maximumHP;
    }

    // Method that rewards the player with currency points if an enemy is defeated
    public void WinPoints()
    {
        // Add the enemy value to the currency points of the player
        GameAdvancement.currencyPoints = GameAdvancement.currencyPoints + enemyValue;

        // Actualize the currency display so that the player can see that he won currency points
        GameSetup.ActualizeCurrencyDisplay();
    }

    // Method that reduces the health points of the castle if an enemy reaches it
    public void ReduceCastleHealth()
    {
        // Reduce the castle health by the amount of damage the unit does
        GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP - damage;

        // Display the lost health points on the castle
        GameSetup.ActualizeCastleHealthPoints();
    }

    // Method that makes enemies take damage
    public void TakeDamage(int damage)
    {
        // Reduce the current health points of the monster by the damage
        if(currentHP - damage >= 0)
        {
            currentHP = currentHP - damage;
        } else {
            currentHP = 0;
        }
    }

    // Method used to return the enemy to the right object pool uppon death
    public void ReturnEnemyToObjectPool()
    {
        // Call the release enemy of the object pool class
        ObjectPools.ReleaseEnemy(this);
    }

    // Method used to set the health points of the enemy correclty uppon respawn
    public void ReviveEnemy()
    {
        // Make sure the enemy is alive
        isAlive = true;

        // Set the health points to max hp
        currentHP = GetMaximumHP;
    }

    // The coroutine that spawns an oponent and waits for a time before the next spawn
    IEnumerator Die()
    {
        Debug.Log("Enemy is dying.");

        // Wait for 0.5 second
        yield return new WaitForSeconds((float)0.5);

        Debug.Log("Enemy died and is being returned to the object pool.");
        // Return the enemy to the object pool
        ReturnEnemyToObjectPool();
    }
}
