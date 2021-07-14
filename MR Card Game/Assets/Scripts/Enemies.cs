using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// // The class of the castle game object
// static class Castle
// {
//     // The maximum and current health point of the castle
//     public static int maximumHP;
//     public static int currentHP;

//     // The current armor points of the castle
//     public static int currentAP;
// }

public class Enemies : MonoBehaviour
{
    // Array of waypoints to walk from one to the next
    [SerializeField]
    private Transform[] waypoints;

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

    private int waypointIndex = 0;

    // The gameboard game object
    public GameObject gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        // When spawning, set the current health points to the maximum health points
        currentHP = maximumHP;

        // Set the flight height
        flightHeight = flying * Board.greatestBoardDimension * (float)0.6;

        // Deactivate the health bar since it is full
        healthBarUI.SetActive(false);

        // Set the health bar correctly
        healthBar.value = CalculateHealth();

        // Scale the enemy down to have the right size on the board
        transform.localScale = new Vector3(Board.greatestBoardDimension * (float)0.2 * size, Board.greatestBoardDimension * (float)0.2 * size, Board.greatestBoardDimension * (float)0.2 * size);

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
            // Destroy the game enemy
            Destroy(gameObject);

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
                waypointIndex = waypointIndex + 1;
            }
        }

        // Check if the enemy reached the castle
        if(waypointIndex == waypoints.Length - 1 && transform.position == (waypoints[waypointIndex].transform.position + new Vector3(0, flightHeight, 0)))
        {
            // Destroy the enemy
            Destroy(gameObject);

            // Reduce the health of the castle
            ReduceCastleHealth(damage);
        }
    }

    // Method that calculates the health value
    public float CalculateHealth()
    {
        return currentHP / maximumHP;
    }

    // Method that rewards the player with currency points if an enemy is defeated
    public void WinPoints()
    {
        // TODO
    }

    // Method that reduces the health points of the castle if an enemy reaches it
    public void ReduceCastleHealth(int damage)
    {
        // TODO
    }
}
