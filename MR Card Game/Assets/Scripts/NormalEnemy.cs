using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{

    public Enemy myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        maximumHP = myPrefab.maximumHP;
        currentHP = myPrefab.currentHP;

        // The size of the enemy unit
        size = myPrefab.size;

        // The movement speed of the enemy unit
        moveSpeed = myPrefab.moveSpeed;

        // The damage that the enemy unit deals to the castle if it is reached
        damage = myPrefab.damage;

        // The currency points won when defeating the enemy
        enemyValue = myPrefab.enemyValue;

        // The height of fly, if zero then the unit cannot fly
        flying = myPrefab.flying;
        // The UI for the health bar
        healthBarUI = myPrefab.healthBarUI;

        // The health bar slider
        healthBar = myPrefab.healthBar;

        // The gameboard game object
        gameBoard = myPrefab.gameBoard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
