using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // The target of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

    // 
    private Vector3 lastPosition;

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
            // Actualise the target last position
            lastPosition = target.transform.position;

            // If the target is not null and alive, move the projectile in the direction of the target
            transform.position = Vector3.MoveTowards(transform.position, target.gameObject.transform.position, Time.deltaTime * parent.GetProjectileSpeed);

            // Check if the projectile reached the destination
            if(transform.position == target.transform.position)
            {
                // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                switch(parent.GetTowerType)
                {
                    case "Archer Tower":
                        ProjectileThatCanHitMultipleEnemiesEffect();
                    break;
                    case "Fire Tower":
                        ProjectileThatCanHitMultipleEnemiesEffect();
                    break;
                    case "Lightning Tower":
                        LightningStrikeEffect(parent.GetNumberOfEffect, target);
                    break;
                    case "Earth Tower":
                        ProjectileThatCanHitMultipleEnemiesEffect();
                    break;
                    case "Wind Tower":
                        WindGustEffect();
                    break;
                }

                // Delete the projectile
                Destroy(gameObject);

                // // Make the enemy take damage
                // target.TakeDamage(parent.GetDamage);
            }
        } else {

            // Check if it is not a wind projectile
            if(parent.GetTowerType == "Wind Tower" || parent.GetTowerType == "Lightning Tower")
            {
                // Destroy the projectile since there is no target anymore
                Destroy(gameObject);

            } else {

                // Make the projectile move to the last position
                transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * parent.GetProjectileSpeed);

                // Check if the projectile reached the destination
                if(transform.position == target.transform.position)
                {
                    // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                    int damage = CalculateDamage(parent.GetDamage, parent.GetTowerType, target);

                    // Check what is the type of the parent tower
                    switch(parent.GetTowerType)
                    {
                        case "Archer Tower":
                            ProjectileThatCanHitMultipleEnemiesEffect();
                        break;
                        case "Fire Tower":
                            ProjectileThatCanHitMultipleEnemiesEffect();
                        break;
                        case "Earth Tower":
                            ProjectileThatCanHitMultipleEnemiesEffect();
                        break;
                    }

                    // Delete the projectile
                    Destroy(gameObject);
                }
            }
        }
    }

    // // Method that makes a stone projectile move to a target location
    // private void MakeStoneFallOnEnemy()
    // {
    //     // Calculate the position the enemy will be at at the moment the stone falls on the ground
    //     Vector3 targetPosition = target.position + 
    // }

    // // The method that produces the effect of an arrow arriving at destination
    // private void ArrowEffect(int damage)
    // {
    //     // Make the enemy take damage
    //     target.TakeDamage(damage);
    // }

    // The method that produces the effect of a fire ball and rock arriving at destination
    private void ProjectileThatCanHitMultipleEnemiesEffect()
    {
        // Initialize the damage variable
        int damage = 0;

        // For each enemy in the collider, calculate the damage they should take
        foreach(var targetEnemy in GetEnemies())
        {
            // Calculate the damage
            damage = CalculateDamage(parent.GetDamage, parent.GetTowerType, targetEnemy);

            // Make the enemy take damage
            target.TakeDamage(damage);
        }

    }

    // The method that produces the effect of a lightning strike arriving at destination
    private void LightningStrikeEffect(int numberOfStrikes, Enemy targetEnemy)
    {
        // Calculate the damage
        int damage = CalculateDamage(parent.GetDamage, parent.GetTowerType, targetEnemy);

        // Make the enemy take damage
        targetEnemy.TakeDamage(damage);

        // Check if the lightning strike should jump
        if(numberOfStrikes < 0)
        {
            // Initialise the raycast hit
            RaycastHit hit;

            // Initialise the nearest enemy game object
            GameObject nearestEnemy;

            // Calculate the radius of the effect
            float radius = parent.GetEffectRange * targetEnemy.gameBoard.transform.localScale.x;

            // Find the nearest enemy with a Physics.SphereCast
            if(Physics.SphereCast(transform.position, radius, transform.forward, out hit, 0) == true)
            {
                // Get the nearest enemy
                nearestEnemy = hit.transform.gameObject;
                Debug.Log("Nearest enemy: " + nearestEnemy.name);

                // Cast Lightning strike effect on it with number of strikes - 1
                LightningStrikeEffect(numberOfStrikes - 1, nearestEnemy.GetComponent<Enemy>());

            } else {
                // No enemy near enough, finished
            }
        }
    }

    // The method that produces the effect of a gust of wind arriving at destination
    private void WindGustEffect()
    {
        // Calculate the damage
        int damage = CalculateDamage(parent.GetDamage, parent.GetTowerType, target);

        // Make the enemy take damage
        target.TakeDamage(damage);

        // Calculate the direction in which the enemy should be pushed
        Vector3 targetPosition = transform.position + (transform.position - target.lastWaypoint).normalized * parent.GetProjectileSpeed * parent.GetEffectTime * parent.GetLevel * target.gameBoard.transform.localScale.x; // TODO
        // Vector3 targetPosition = new Vector3(0, 0, 0);

        // Push the enemy back by the distance scaled down to the board size * the level in the direction of the last waypoint
        // target.transform.position = Vector3.MoveTowards(transform.position, target.waypoints[target.waypointIndex - 1].transform.position + new Vector3(0, target.flightHeight, 0), parent.GetProjectileSpeed * parent.effectTime * parent.level * target.gameBoard.transform.localScale.x);    
        // target.transform.position = transform.Translate(targetPosition);
        target.transform.position = Vector3.MoveTowards(transform.position, targetPosition, parent.GetProjectileSpeed * parent.GetEffectTime * parent.GetLevel * target.gameBoard.transform.localScale.x);
    }

    // The method that calculates the damage a unit should take depending on the enemy, tower and tower attack type
    private int CalculateDamage(int damage, string towerType, Enemy target)
    {
        // For now return the damage of the tower
        return parent.GetDamage; // TODO
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

            Debug.Log("Enemy: " + enemy.gameObject.name + " has entered the collider of the projectile");
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit(Collider other)
    {
        // If an enemy leaves the collider, remove it from the list
        enemies.Remove(other.GetComponent<Enemy>());

        Debug.Log("Enemy: " + other.GetComponent<Enemy>().gameObject.name + " has left the collider of the projectile");
    }
}
