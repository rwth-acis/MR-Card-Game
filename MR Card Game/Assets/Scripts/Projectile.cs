using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // The target of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

    // The list of the enemy colliders inside the projectile collider
    private List<Enemy> enemies = new List<Enemy>();

    // The method used to get the list
    public List<Enemy> GetEnemies()
    {
        return enemies;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If a projectile was created, it should move to the target
        MoveToTarget();
    }

    // The initialize method used to set the parent and target objects
    public void Initialize(Tower parent)
    {
        // Set the parent tower of the projectile as the parent of the projectile
        this.target = parent.Target.gameObject.GetComponent<Enemy>();
        this.parent = parent;

        // Set the game object as child under the tower
        transform.parent = parent.transform;
    }

    // Method that makes a projectile move to a target
    private void MoveToTarget()
    {
        // Check that the target is not null
        if(target != null && target.isAlive == true)
        {
            // If the target is not null and alive, move the projectile in the direction of the target
            transform.position = Vector3.MoveTowards(transform.position, target.gameObject.transform.position, Time.deltaTime * parent.ProjectileSpeed);

            // Check if the projectile reached the destination
            if(transform.position == target.transform.position)
            {
                // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                // Delete the projectile
                Destroy(gameObject);

                // Make the enemy take damage
                target.TakeDamage(parent.Damage);
            }
        } else {

            // Delete the projectile if the enemy is null or not alive anymore
            Destroy(gameObject);
        }
    }

    // The method that produces the effect of an arrow arriving at destination
    private void ArrowEffect()
    {
        // Calculate the damage the enemy should take form the damage type and basic amount
        int damage = CalculateDamage(parent.Damage, parent.TowerType, target);

        // Make the enemy take damage
        target.TakeDamage(damage);
    }

    // The method that produces the effect of a fire ball arriving at destination
    private void FireBallEffect()
    {
        // Initialize the damage integer
        int damage = 0;

        // For each enemy in the collider, calculate the damage they should take
        foreach(var targetEnemy in GetEnemies())
        {
            // Calculate the damage
            damage = CalculateDamage(parent.Damage, parent.TowerType, targetEnemy);

            // Make the enemy take damage
            target.TakeDamage(damage);
        }

    }

    // The method that produces the effect of a lightning strike arriving at destination
    private void LightningStrikeEffect()
    {
        
    }

    // The method that produces the effect of a rock arriving at destination
    private void RockEffect()
    {
        
    }

    // The method that produces the effect of a gust of wind arriving at destination
    private void WindGustEffect()
    {
        // Calculate the damage the enemy should take form the damage type and basic amount
        int damage = CalculateDamage(parent.Damage, parent.TowerType, target);

        // Make the enemy take damage
        target.TakeDamage(damage);

        // Calculate the direction in which the enemy should be pushed
        Vector3 direction = transform.position - target.lastWaypoint; // TODO

        // Push the enemy back by the distance scaled down to the board size * the level in the direction of the last waypoint
        // target.transform.position = Vector3.MoveTowards(transform.position, target.waypoints[target.waypointIndex - 1].transform.position + new Vector3(0, target.flightHeight, 0), parent.projectileSpeed * parent.effectTime * parent.level * target.gameBoard.transform.localScale.x);    
    }

    // The method that calculates the damage a unit should take depending on the enemy, tower and tower attack type
    private int CalculateDamage(int damage, string towerType, Enemy target)
    {
        // For now return the damage of the tower
        return parent.Damage; // TODO
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Access the enemy object from the collider
        Enemy enemy = other.GetComponent<Enemy>();

        // If the enemy is not already contained, add him to the list
        if(!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        // If an enemy leaves the collider, remove it from the list
        enemies.Remove(other.GetComponent<Enemy>());
    }
}
