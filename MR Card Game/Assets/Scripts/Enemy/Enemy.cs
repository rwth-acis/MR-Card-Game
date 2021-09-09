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
    public string resistance;

    // Method used to get the restistance of the enemy
    public string GetEnemyResistance
    {
        get { return resistance; }
    }

    // What the enemy is weak to
    [SerializeField]
    public string weakness;

    // Method used to get the weakness of the enemy
    public string GetEnemyWeakness
    {
        get { return weakness; }
    }

    // The gameboard game object
    private GameObject gameBoard;

    // The current waypoint index so that enemies go from waypoint to waypoint
    public int waypointIndex = 0;

    // The flag that tells if the enemy is currently alive or not
    public bool isAlive = true;

    // The personal slow factor of the enemy
    public float personalSlowFactor = 1;

    public int numberOfTrapsIn = 0;

    // The flag that states if an enemy is wet or not
    public bool isWet = false;

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

        // // Set the enemy as child of the gameboard
        // transform.parent = gameBoard.transform;

        // Set the flight height and standing size
        flightHeight = flying * Board.greatestBoardDimension * (float)0.6  + (float)0.2 * size * Board.greatestBoardDimension;

        // Deactivate the health bar since it is full
        healthBarUI.SetActive(false);

        // Set the health bar correctly
        healthBar.value = CalculateHealth();

        // Scale the enemy down to have the right size
        transform.localScale = new Vector3((float)0.3 * size, (float)0.3 * size, (float)0.3 * size);

        // Set it to the position of the first waypoint on spawn
        transform.position = (waypoints[waypointIndex].transform.position + this.transform.up * flightHeight);

        // When spawning, set the current health points to the maximum health points
        ReviveEnemy();

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

                // Reset the current waypoint index so that enemies walk toward the first waypoint upon respawn
                waypointIndex = 0;

                // Increase the number of enemies defeated by one
                LevelInfo.numberOfEnemiesDefeated = LevelInfo.numberOfEnemiesDefeated + 1;

                // Make the enemy die
                StartCoroutine(Die());
            }

            // Check if the game is stopped
            if(GameAdvancement.timeStopped == false)
            {
                // Make the enemy mode
                Move();
            }

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
            // Get the current goal that is the position of the next waypoint, added with a height
            Vector3 currentGoal = waypoints[waypointIndex].transform.position + this.transform.up * flightHeight;

            // // Move the enemy toward the next waypoint
            // transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0), moveSpeed * GameAdvancement.globalSlow * personalSlowFactor * Time.deltaTime * gameBoard.transform.localScale.x);

            // Move the enemy toward the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentGoal, moveSpeed * GameAdvancement.globalSlow * personalSlowFactor * Time.deltaTime * gameBoard.transform.localScale.x);

            // // Make the enemy face the direction it is moving
            // transform.LookAt(waypoints[waypointIndex].transform.position);

            // If the enemy reached the position of a waypoint, increase the waypoint index by one
            if(transform.position == currentGoal)
            // if(transform.position.x == waypoints[waypointIndex].transform.position.x && transform.position.z == waypoints[waypointIndex].transform.position.z)
            {
                // // Set the passed waypoint as last waypoint
                // lastWaypoint = waypoints[waypointIndex].transform.position;

                // Increase the waypoint index by one
                waypointIndex = waypointIndex + 1;

                if(waypointIndex <= waypoints.Length - 1)
                {
                    // // Make the enemy face the direction it is moving
                    // transform.LookAt(waypoints[waypointIndex].transform.position + this.transform.up * flightHeight);
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

            // Increase the number of enemies that reached the castle by one point
            LevelInfo.numberOfEnemiesThatReachedTheCastle = LevelInfo.numberOfEnemiesThatReachedTheCastle + 1;

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
        // Reduce the number of undefeated enemies of the wave by one
        LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfUndefeatedEnemies - 1;

        // Check if the castle has armor points
        if(GameAdvancement.castleCurrentAP != 0) 
        {
            // Check if the armor points of the castle exceed the damage of the enemy
            if(GameAdvancement.castleCurrentAP >= damage)
            {
                // Reduce the castle armor points by the enemy damage
                GameAdvancement.castleCurrentAP = GameAdvancement.castleCurrentAP - damage;

            } else {

                // Initialize the additional damage variable
                int additionalDamage = 0;

                // Set the additional damage to the damage reduced by the castle armor points
                additionalDamage = damage - GameAdvancement.castleCurrentAP;

                // Set the armor points of the castle to 0
                GameAdvancement.castleCurrentAP = 0;

                // Reduce the castle health by the additional amount of damage the unit does
                GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP - additionalDamage;
            }
            
        } else {

            // Reduce the castle health by the amount of damage the unit does
            GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP - damage;
        }

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

    // Method used to return the enemy to the right object pool uppn death
    public void ReturnEnemyToObjectPool()
    {
        // Call the release enemy of the object pool class
        ObjectPools.ReleaseEnemy(this);
    }

    // Method used to set the health points of the enemy correctly upon respawn
    public void ReviveEnemy()
    {
        // Set the waypoint index to 0
        waypointIndex = 0;

        // Make sure the enemy is alive
        isAlive = true;

        // Set the health points to max hp
        currentHP = GetMaximumHP;
    }

    // The coroutine that spawns an oponent and waits for a time before the next spawn
    IEnumerator Die()
    {
        // Reduce the number of undefeated enemies of the wave by one
        LevelInfo.numberOfUndefeatedEnemies = LevelInfo.numberOfUndefeatedEnemies - 1;

        // Wait for 0.5 second
        yield return new WaitForSeconds((float)0.5);

        // Return the enemy to the object pool
        ReturnEnemyToObjectPool();
    }


    // // Method that sets the resistance of this enemy
    // public static void SetResistance(string type)
    // {
    //     // Set the resistance to type
    //     resistance = type;
    // }

    // // Method that sets the weakness of this enemy
    // public static void SetWeakness(string type)
    // {
    //     // Set the weakness to type
    //     weakness = type;
    // }
}
