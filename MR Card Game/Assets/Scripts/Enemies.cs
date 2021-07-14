using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// // The class containing all spawned enemies
// static class Enemies
// {
//     // The array of existing enemies
//     public static Enemy[] enemies;
// }

// // The class of the enemy game object
// static class Enemy
// {
//     // The maximum and current health point of the enemy unit
//     public static int maximumHP;
//     public static int currentHP;

//     // The size of the enemy unit
//     public static int size;

//     // The movement speed of the enemy unit
//     public static int movementSpeed;

//     // The damage that the enemy unit deals to the castle if it is reached
//     public static int damage;

//     // The color of the enemy unit
//     public static Color color;
// }

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

    [SerializeField]
    public float moveSpeed = 2f;

    private int waypointIndex = 0;

    // The gameboard game object
    public GameObject gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move enemy
        Move();
    }

    // Method that make the enemy walk
    private void Move()
    {
        // If the last waypoint was not reached, move the enemy
        if(waypointIndex <= waypoints.Length - 1)
        {
            // Move the enemy toward the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime * gameBoard.transform.localScale.x);

            // If the enemy reached the position of a waypoint, increase the waypoint index by one
            if(transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex = waypointIndex + 1;
            }
        }
    }
}
