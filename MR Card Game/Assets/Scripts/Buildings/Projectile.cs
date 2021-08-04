using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // A projectile instance needed to access non static variables in a static way
    public static Projectile instance;

    // The target of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

    // Method used to get the type of the tower
    public static Tower GetParent
    {
        get { return instance.parent; }
    }

    // The parent tower of the projectile
    private string projectileType;

    // Method used to get the type of the tower
    public string GetProjectileType
    {
        get { return instance.projectileType; }
    }

    // The last position of the target is saved so that the projectile can continue travelling after the enemy is dead
    private Vector3 lastPosition;

    // The list of coliders that enter the range of the tower
    private List<Collider> colliders = new List<Collider>();

    // The method used to access the list of colliders
    public List<Collider> GetColliders()
    {
        return colliders;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialie the instance
        instance = this;

        // Get the right type
        switch(GetParent.GetTowerType)
        {
            case "Archer Tower":
                projectileType = "Arrow";
            break;

            case "Fire Tower":
                projectileType = "Fire Ball";
            break;

            case "Earth Tower":
                projectileType = "Stone";
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is not paused, update and make the projectile move
        if(GameAdvancement.gamePaused == false)
        {
            // If a projectile was created, it should move to the target
            MoveToTarget();
        }
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

            // Check if the projectile is an arrow
            if(parent.GetTowerType == "Archer Tower")
            {
                // Make the arrow face his target
                transform.LookAt(target.transform.position);
            }

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
                    // case "Lightning Tower":
                    //     LightningStrikeEffect(parent.GetNumberOfEffect, target);
                    // break;
                    case "Earth Tower":
                        ProjectileThatCanHitMultipleEnemiesEffect();
                    break;
                    // case "Wind Tower":
                    //     WindGustEffect();
                    // break;
                }

                // Delete the projectile
                // Destroy(gameObject);
                ObjectPools.ReleaseProjectile(this);
            }

        } else {

            // Check if it is not a wind tower or lightning tower that do not have a projectile
            if(parent.GetTowerType != "Wind Tower" || parent.GetTowerType != "Lightning Tower")
            {
            //     // // Destroy the projectile since there is no target anymore
            //     // Destroy(gameObject);

            // } else {

                // Make the projectile move to the last position
                transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * parent.GetProjectileSpeed);

                // Check if the projectile reached the destination
                if(transform.position == target.transform.position)
                {
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

                    // // Delete the projectile
                    // Destroy(gameObject);
                    ObjectPools.ReleaseProjectile(this);
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
        foreach(var targetEnemy in GetColliders())
        {
            // Calculate the damage
            damage = CalculateDamage(parent.GetDamage, parent.GetWeaknessMultiplier, parent.GetTowerType, targetEnemy.GetComponent<Enemy>());

            // Make the enemy take damage
            targetEnemy.GetComponent<Enemy>().TakeDamage(damage);
        }

    }

    // // The method that produces the effect of a lightning strike arriving at destination
    // private void LightningStrikeEffect(int numberOfStrikes, Enemy targetEnemy)
    // {
    //     // Calculate the damage
    //     int damage = CalculateDamage(parent.GetDamage, parent.GetWeaknessMultiplier, parent.GetTowerType, targetEnemy);

    //     // Make the enemy take damage
    //     targetEnemy.TakeDamage(damage);

    //     // Check if the lightning strike should jump
    //     if(numberOfStrikes < 0)
    //     {
    //         // Initialise the raycast hit
    //         RaycastHit hit;

    //         // Initialise the nearest enemy game object
    //         GameObject nearestEnemy;

    //         // Calculate the radius of the effect
    //         float radius = parent.GetEffectRange * targetEnemy.gameBoard.transform.localScale.x;

    //         // Find the nearest enemy with a Physics.SphereCast
    //         if(Physics.SphereCast(transform.position, radius, transform.forward, out hit, 0) == true)
    //         {
    //             // Get the nearest enemy
    //             nearestEnemy = hit.transform.gameObject;
    //             Debug.Log("Nearest enemy: " + nearestEnemy.name);

    //             // Cast Lightning strike effect on it with number of strikes - 1
    //             LightningStrikeEffect(numberOfStrikes - 1, nearestEnemy.GetComponent<Enemy>());

    //         } else {
    //             // No enemy near enough, finished
    //         }
    //     }
    // }

    // // The method that produces the effect of a gust of wind arriving at destination
    // private void WindGustEffect()
    // {
    //     // Calculate the damage
    //     int damage = CalculateDamage(parent.GetDamage, parent.GetWeaknessMultiplier, parent.GetTowerType, target);

    //     // Make the enemy take damage
    //     target.TakeDamage(damage);

    //     // Calculate the direction in which the enemy should be pushed
    //     Vector3 targetPosition = transform.position + (transform.position - target.lastWaypoint).normalized * parent.GetProjectileSpeed * parent.GetEffectTime * parent.GetLevel * target.gameBoard.transform.localScale.x; // TODO
    //     // Vector3 targetPosition = new Vector3(0, 0, 0);

    //     // Push the enemy back by the distance scaled down to the board size * the level in the direction of the last waypoint
    //     // target.transform.position = Vector3.MoveTowards(transform.position, target.waypoints[target.waypointIndex - 1].transform.position + new Vector3(0, target.flightHeight, 0), parent.GetProjectileSpeed * parent.effectTime * parent.level * target.gameBoard.transform.localScale.x);    
    //     // target.transform.position = transform.Translate(targetPosition);
    //     target.transform.position = Vector3.MoveTowards(transform.position, targetPosition, parent.GetProjectileSpeed * parent.GetEffectTime * parent.GetLevel * target.gameBoard.transform.localScale.x);
    // }

    // The method that calculates the damage a unit should take depending on the enemy, tower and tower attack type
    public static int CalculateDamage(int damage, float weaknessMultiplier, string towerType, Enemy target)
    {
        // Initialize the additinal damage multiplier
        int additianlDamageMultiplier = 0;

        switch(towerType)
        {
            case "Archer Tower":

                if(target.GetEnemyWeakness == "Piercing")
                {
                    additianlDamageMultiplier = 1;
                } else if(target.GetEnemyResistance == "Piercing")
                {
                    additianlDamageMultiplier = -1;
                }

            break;
            case "Fire Tower":
                if(target.GetEnemyWeakness == "Fire")
                {
                    additianlDamageMultiplier = 1;
                } else if(target.GetEnemyResistance == "Fire")
                {
                    additianlDamageMultiplier = -1;
                }
            break;
            case "Lightning Tower":
                if(target.GetEnemyWeakness == "Lightning")
                {
                    additianlDamageMultiplier = 1;
                } else if(target.GetEnemyResistance == "Lightning")
                {
                    additianlDamageMultiplier = -1;
                }
            break;
            case "Earth Tower":
                if(target.GetEnemyWeakness == "Earth")
                {
                    additianlDamageMultiplier = 1;
                } else if(target.GetEnemyResistance == "Earth")
                {
                    additianlDamageMultiplier = -1;
                }
            break;
            case "Wind Tower":
                if(target.GetEnemyWeakness == "Wind")
                {
                    additianlDamageMultiplier = 1;
                } else if(target.GetEnemyResistance == "Wind")
                {
                    additianlDamageMultiplier = -1;
                }
            break;
        }

        // Return the damage with a bonus, a malus or flat depending on if a weakness or resistance was found
        return (int) (damage + additianlDamageMultiplier * damage * weaknessMultiplier);
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is already in the list or not
        if(!colliders.Contains(other))
        {
            // Add colliders that enter the projectile collider
            colliders.Add(other);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        // Remove colliders that leave the collider of the projectile collider
        colliders.Remove(other);
    }

    // Method used to return the enemy to the right object pool uppon death
    public void ReturnProjectileoObjectPool()
    {
        // Call the release enemy of the object pool class
        ObjectPools.ReleaseProjectile(this);
    }
}
