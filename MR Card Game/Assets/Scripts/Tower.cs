using i5.Toolkit.Core.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using i5.Toolkit.Core.Utilities;
using static i5.Toolkit.Core.Examples.Spawners.SpawnProjectile;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Spawner projectileSpawner;

    // The type of the tower
    [SerializeField]
    private string towerType;

    // Method used in the projectile class to read the tower type
    public string GetTowerType
    {
        get { return towerType; }
    }

    // The level of the tower
    private int level;

    // Method used to access the current level of the tower
    public float GetLevel
    {
        get { return level; }
    }

    // The weakness multiplier of the tower type
    [SerializeField]
    private float cost;

    // The variable used to access the value of the weakness multiplier of projectiles from the projectile class
    public float getCost
    {
        get { return cost; }
    }

    // The attack range of the tower
    [SerializeField]
    private float attackRange;

    // The damage of the tower
    [SerializeField]
    private int damage;

    // The method used to access the damage value
    public int GetDamage
    {
        get { return damage; }
    }

    // The flag that states if the tower can attack right now or not
    private bool canAttack;

    // The attack timer that is counted up after an attack
    private float attackTimer;

    // The attack cooldown between the attacks of the tower
    [SerializeField]
    private float attackCooldown;

    // The projectile speed of the projectiles of this tower
    [SerializeField]
    private float projectileSpeed;

    // The variable used to access the value of the projectile speed from the projectile class
    public float GetProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    // // The effect time of the projectiles of this tower
    // [SerializeField]
    // private float effectTime;

    // // The variable used to access the value of the effect time of projectiles from the projectile class
    // public float GetEffectTime
    // {
    //     get { return effectTime; }
    // }

    // The effect range of the projectiles of this tower
    [SerializeField]
    private float effectRange;

    // The variable used to access the value of the effect range of projectiles from the projectile class
    public float GetEffectRange
    {
        get { return effectRange; }
    }

    // The effect number of the projectiles of this tower
    [SerializeField]
    private int numberOfEffect;

    // The variable used to access the value of the effect number of projectiles from the projectile class
    public int GetNumberOfEffect
    {
        get { return numberOfEffect; }
    }

    // The weakness multiplier of the tower type
    [SerializeField]
    private float weaknessMultiplier;

    // The variable used to access the value of the weakness multiplier of projectiles from the projectile class
    public float GetWeaknessMultiplier
    {
        get { return weaknessMultiplier; }
    }


    // The current target of the tower
    private Collider target;

    // The variable used to access the value of the target from the projectile class
    public Collider Target
    {
        get { return target; }
    }

    // The list of coliders that enter the range of the tower
    private List<Collider> colliders = new List<Collider>();

    // The method used to access the list of colliders
    public List<Collider> GetColliders()
    {
        return colliders;
    }

    // The list of enemies that can still be targeted by the lightning tower
    private List<Collider> enemies = new List<Collider>();

    // The method used to access the list of enemies that can be targeted by the lightning tower
    public List<Collider> GetEnemies()
    {
        return enemies;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set the can attack flag to true
        canAttack = true;

        // From the attack speed, extract the actual attack cooldown
        // // Get the vector scale of the parent object
        // vector =  transform.parent.gameObject.transform.localScale;
        // float scaleX = vector.x;
        // float scaleY = vector.y;
        // float scaleZ = vector.z;

        // // Set the scale of the parent gameObject correctly
        // transform.parent.gameObject.transform.localScale =
    }

    // Update is called once per frame
    void Update()
    {
        // Attack if there are enemies in range
        Attack();
    }


    // Method used to attack
    public void Attack()
    {
        // Check if the can attack flag is true or false
        if(!canAttack)
        {
            // If it is false, increase the timer by the time passed
            attackTimer = attackTimer + Time.deltaTime;

            // Check if the attack timer has reached the attack cooldown
            if(attackTimer >= attackCooldown)
            {
                // If it is the case, set the can attack flag to true
                canAttack = true;

                // Reset the attack timer
                attackTimer = 0;
            }
        }

        // // Check that the enemy did not die already
        // // if(target != null && !IsValid(target.GetComponent<Enemies>().gameObject))
        // if(target == null && !IsValid(target.GetComponent<Enemy>().gameObject))
        // {
        //     // If the target is already dead, remove the target from the colliders array
        //     colliders.Remove(target);
        //     target = GetColliders()[0];
        //     Debug.Log("The target was dead and was removed");
        // }

        // Check if the enemy was just respawned, tagged as dead but has full health points
        if(target != null && target.GetComponent<Enemy>().isAlive == false && target.GetComponent<Enemy>().GetCurrentHP == target.GetComponent<Enemy>().GetMaximumHP)
        {
            // Set the target as alive again
            target.GetComponent<Enemy>().isAlive = true;
        }

        // If the Target isn't null and dead, remove it from the colliders list
        if(target != null && target.GetComponent<Enemy>().isAlive == false)
        {
            // Remove the target from the list
            colliders.Remove(target);

            // // Destroy the game enemy
            // Destroy(target.gameObject);
        }

        // Check if the sphere colider around the tower still contains the target enemy
        if(GetColliders().Contains(target) == false)
        {
            // If the target left the range, set it to null
            target = null;
            Debug.Log("The target was not contained anymore in the range and was set to null");
        }

        // Check if the tower needs a new target and if there are any enemies in range
        if(target == null && colliders.Count > 0)
        {
            // If a new target is needed, and there are enemies in range, set the oldest enemy as target
            target = GetColliders()[0];
            Debug.Log("The new target was set to " + target);
        }

        // Check if there is a current target
        if(target != null && target.GetComponent<Enemy>().isAlive == true)
        {
            // Check if the can attack flag is set to true
            if(canAttack == true)
            {
                // Attack the target
                Shoot();
                Debug.Log("The current enemey that is targeted is named: " + target.name);

                // Set the can attack flag to false
                canAttack = false;
            }
        }
    }

    // // Method that checks if a game object still exists
    // public static bool IsValid(GameObject enemy)
    // {
    //     // Try to access it
    //     try
    //     {
    //         // If it is null, return false
    //         if (enemy.gameObject == null) return false;

    //     }
    //     // Check if you catch an exception
    //     catch(Exception)
    //     {
    //         // If an exception was thrown, then the object was destroyed
    //         return false;
    //     }
    //     return true;
    // }

    // Method that shoots projectiles at enemies
    private void Shoot()
    {
        // // Find the spawner object
        // Spawner spawner = GameObject.Find("ProjSpawn").GetComponent<Spawner>();

        // Get a projectile form the object pool
        // Projectile spawnedProjectile = ObjectPool<Projectile>.RequestResource(() => {return new Projectile();});

        if(GetTowerType == "Lightning Tower")
        {
            // Set the list of targets to the list of colliders
            enemies = GetColliders();

            // Remove the current target form the list
            enemies.Remove(target);

            // Apply the effect to the enemy
            LightningStrikeEffect(GetNumberOfEffect, target.gameObject.GetComponent<Enemy>());

            // Apply a visual effect on all enemies hit

        } else if(GetTowerType == "Wind Tower")
        {
            // Apply the effect to the enemy
            WindGustEffect();

        } else {

            // Spawn the projectile
            Projectile spawnedProjectile = SpawnProjectileForTower(projectileSpawner).GetComponent<Projectile>();

            // Make sure the projectile is active
            // spawnedProjectile.gameObject.SetActive(true);

            // Set the position of the projectile to the position of the tower
            spawnedProjectile.transform.position = transform.position;
            Debug.Log("A projectile was shot.");

            // Initialize the projectile object, so that it knows what his parent is
            spawnedProjectile.Initialize(this);

            // // Resize the projectile
            // Vector3 scale = spawnedProjectile.gameObject.transform.localScale;
            // float scaleX = scale.x;
            // float scaleY = scale.y;
            // float scaleZ = scale.z;

            // spawnedProjectile.gameObject.transform.localScale = new Vector3(scaleX * (float)0.1, scaleY * (float)0.5, scaleZ * (float)0.1);
        }
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other) {
        if(!colliders.Contains(other))
        {
            colliders.Add(other);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        colliders.Remove(other);
    }

    //-----------------------------------------------------------------------------------------------------------
    // The effect of towers that do not use projectiles that need to get spawned
    //-----------------------------------------------------------------------------------------------------------

    // The method that produces the effect of a lightning strike arriving at destination
    private void LightningStrikeEffect(int numberOfStrikes, Enemy targetEnemy)
    {
        // Calculate the damage
        int damage = Projectile.CalculateDamage(GetDamage, GetWeaknessMultiplier, GetTowerType, targetEnemy);

        // Make the enemy take damage
        targetEnemy.TakeDamage(damage);

        Debug.Log("The number of strikes is: " + numberOfStrikes);

        // Check if the lightning strike should jump
        if(numberOfStrikes > 0)
        {
            // Initialise the raycast hit
            RaycastHit hit;

            // // Initialise the nearest enemy game object
            // GameObject nearestEnemy;

            // Calculate the radius of the effect
            // float radius = GetEffectRange * targetEnemy.gameBoard.transform.localScale.x;

            Debug.Log("The collider count is: " + GetColliders().Count);

            // Check if there is another enemy in the range of the tower
            if(GetEnemies().Count > 1)
            {
                // Initialise the nearest enemy
                Collider nearestEnemy = null;

                // Initialize the shortest distance
                float shortestDistance = GetEffectRange;

                // Go through all other enemies, so skip the first index of the array
                for(int counter = 1; counter < GetEnemies().Count; counter = counter + 1)
                {
                    // Get the distance between the current target and the current candidate
                    float distance = Vector3.Distance(targetEnemy.transform.position, GetEnemies()[counter].GetComponent<Enemy>().transform.position);

                    Debug.Log("The distance to the enemy number " + counter + " is: " + distance);

                    // Check if this distance is shorter than the current shortest distance
                    if(distance <= shortestDistance)
                    {
                        // Set this enemy as nearest enemy
                        nearestEnemy = GetEnemies()[counter];

                        // Set the shortest distance to the distance between thoses two
                        shortestDistance = distance;

                        Debug.Log("The shortest distance was set to " + distance);
                    }
                }

                if(nearestEnemy != null)
                {
                    Debug.Log("Lightning is jumping!");
                    // Remove the nearest enemy from the list
                    enemies.Remove(nearestEnemy);
                    
                    // Cast Lightning strike again with one less jump
                    LightningStrikeEffect((numberOfStrikes - 1), nearestEnemy.GetComponent<Enemy>());
                }
            }

            // // Find the nearest enemy with a Physics.SphereCast
            // if(Physics.SphereCast(targetEnemy.transform.position, radius, transform.forward, out hit, 1) == true)
            // {
            //     // Get the nearest enemy
            //     nearestEnemy = hit.transform.gameObject;
            //     Debug.Log("Nearest enemy: " + nearestEnemy.name);

            //     // Cast Lightning strike effect on it with number of strikes - 1
            //     LightningStrikeEffect(numberOfStrikes - 1, nearestEnemy.GetComponent<Enemy>());

            // } else {
            //     // No enemy near enough, finished
            // }
        }
    }

    // The method that produces the effect of a gust of wind arriving at destination
    private void WindGustEffect()
    {
        // Get the enemy component of the target collinder
        Enemy targetEnemy = target.GetComponent<Enemy>();
        // Calculate the damage
        int damage = Projectile.CalculateDamage(GetDamage, GetWeaknessMultiplier, GetTowerType, targetEnemy);

        // Make the enemy take damage
        targetEnemy.TakeDamage(damage);

        // // Calculate the direction in which the enemy should be pushed
        // Vector3 targetPosition = transform.position + (transform.position - targetEnemy.lastWaypoint).normalized * GetProjectileSpeed * GetEffectTime * GetLevel * targetEnemy.gameBoard.transform.localScale.x; // TODO

        // Push the enemy back by the distance scaled down to the board size * the level in the direction of the last waypoint
        // target.transform.position = Vector3.MoveTowards(transform.position, target.waypoints[target.waypointIndex - 1].transform.position + new Vector3(0, target.flightHeight, 0), parent.GetProjectileSpeed * parent.effectTime * parent.level * target.gameBoard.transform.localScale.x);    
        // target.transform.position = transform.Translate(targetPosition);
        // targetEnemy.transform.position = Vector3.MoveTowards(transform.position, targetPosition, GetProjectileSpeed * GetEffectTime * GetLevel * targetEnemy.gameBoard.transform.localScale.x);
        targetEnemy.transform.position = targetEnemy.transform.position - targetEnemy.transform.forward * GetEffectRange;
    }
}