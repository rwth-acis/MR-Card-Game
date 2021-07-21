using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using static map.GameMap;

// // The class of the castle game object
// static class Castle
// {
//     // The maximum and current health point of the castle
//     public static int maximumHP;
//     public static int currentHP;

//     // The current armor points of the castle
//     public static int currentAP;
// }

public class Enemy : MonoBehaviour
{
    // Waypoints placed on the path that enemies have to travel
    private Transform[] waypoints;

    // The last waypoint the enemy passed
    public Vector3 lastWaypoint;

    // The maximum and current health point of the enemy unit
    public int maximumHP;
    public int currentHP;

    // The size of the enemy unit
    public float size;

    // The movement speed of the enemy unit
    public float moveSpeed;

    // The damage that the enemy unit deals to the castle if it is reached
    public int damage;

    // The currency points won when defeating the enemy
    public int enemyValue;

    // The height of fly, if zero then the unit cannot fly
    public float flying;

    // Initialize the flight height variable
    private float flightHeight;

    // The UI for the health bar
    public GameObject healthBarUI;

    // The health bar slider
    public Slider healthBar;

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

    private int waypointIndex = 0;

    public bool isAlive = true;

    // The gameboard game object
    public GameObject gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        // Set the waypoints that apply to this map
        waypoints = Waypoints.mapWaypoints;

        // Set the enemy as child of the gameboard
        transform.parent = gameBoard.transform;

        // When spawning, set the current health points to the maximum health points
        currentHP = maximumHP;

        // Set the flight height and standing size
        flightHeight = flying * Board.greatestBoardDimension * (float)0.6  + (float)0.1 * size * Board.greatestBoardDimension;

        // Deactivate the health bar since it is full
        healthBarUI.SetActive(false);

        // Set the health bar correctly
        healthBar.value = CalculateHealth();

        // Scale the enemy down to have the right size
        transform.localScale = new Vector3((float)0.2 * size, (float)0.2 * size, (float)0.4 * size);

        // Set it to the position of the first waypoint on spawn
        transform.position = (waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // If the current health of the unit is not at its maximum, activate the health bar
        if(currentHP < maximumHP)
        {
            healthBarUI.SetActive(true);
        }

        // Kill the enemy if it its health points reach zero
        if(currentHP <= 0)
        {
            // Set the enemy as dead
            isAlive = false;

            // Destroy the game enemy
            // Destroy(gameObject); // Disabled so the tower can know that the enemy is dead

            // Make the player win the currency points
            WinPoints();
        }

        // Move enemy
        Move();

        // Set the health value correctly
        healthBar.value = CalculateHealth();
    }

    // Method that make the enemy walk
    private void Move()
    {
        // If the last waypoint was not reached, move the enemy
        if(waypointIndex <= waypoints.Length - 1)
        {
            // Move the enemy toward the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0), moveSpeed * Time.deltaTime * gameBoard.transform.localScale.x);

            // If the enemy reached the position of a waypoint, increase the waypoint index by one
            if(transform.position == (waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0)))
            {
                // Set the passed waypoint as last waypoint
                lastWaypoint = waypoints[waypointIndex].transform.position;

                // Increase the waypoint index by one
                waypointIndex = waypointIndex + 1;
            }
        }

        // Check if the enemy reached the castle
        if(waypointIndex == waypoints.Length)
        {
            // Destroy the enemy
            Destroy(gameObject);

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
        GameSetup.ActualizeCurrencyDisplay(GameAdvancement.currencyDisplay);
    }

    // Method that reduces the health points of the castle if an enemy reaches it
    public void ReduceCastleHealth()
    {
        // Reduce the castle health by the amount of damage the unit does
        GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP - damage;

        // Display the lost health points on the castle
        GameSetup.ActualizeCastleHealthPoints(GameAdvancement.castleHealthBar, GameAdvancement.castleHealthCounter, GameAdvancement.castlecurrentHP, GameAdvancement.castleMaxHP);
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
}
