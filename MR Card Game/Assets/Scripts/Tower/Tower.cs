using i5.Toolkit.Core.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using i5.Toolkit.Core.Utilities;
using static i5.Toolkit.Core.Examples.Spawners.SpawnProjectile;

// The class where the tower that needs to be enhanced is saved
public static class TowerEnhancer
{
    public static Tower currentlyEnhancedTower;

    // The position where the tower / trap building was initialized
    public static Vector3 buildPosition;
}

public class Tower : MonoBehaviour
{
    [SerializeField]
    private GameObject projectileSpawner;

    [SerializeField]
    private TowerType towerType;

    [Tooltip("The projectile prefab of the tower")]
    [SerializeField]
    private Projectile towerProjectile;

    [SerializeField]
    private float cost;

    // The attack range of the tower
    [SerializeField]
    private float attackRange;

    // The damage of the tower
    [SerializeField]
    private int damage;

    // The attack cooldown between the attacks of the tower
    [SerializeField]
    private float attackCooldown;

    // The projectile speed of the projectiles of this tower
    [SerializeField]
    private float projectileSpeed;

    [Tooltip("The effect range of the projectiles of this tower")] 
    [SerializeField]
    private float effectRange;

    [Tooltip("The effect number of the projectiles of this tower")]
    [SerializeField]
    private int numberOfEffect;

    [Tooltip("The weakness multiplier of the tower type")]
    [SerializeField]
    private float weaknessMultiplier;

    // The level of the tower
    private int level;

    // The flag that states if the tower can attack right now or not
    private bool canAttack;

    // The attack timer that is counted up after an attack
    private float attackTimer;

    // The current target of the tower
    private Collider target;

    // The list of colliders that enter the range of the tower
    private List<Collider> colliders = new List<Collider>();

    // The list of enemies that can still be targeted by the lightning tower
    private List<Collider> enemies = new List<Collider>();

    // Method used in the projectile class to read the tower type
    public TowerType TowerType
    {
        get { return towerType; }
    }

    // Method used to access the current level of the tower
    public float Level
    {
        get { return level; }
    }

    // The variable used to access the value of the weakness multiplier of projectiles from the projectile class
    public float Cost
    {
        get { return cost; }
    }

    // The method used to access the attack range value
    public float AttackRange
    {
        get { return attackRange; }
    }

    // The method used to access the damage value
    public int Damage
    {
        get { return damage; }
    }

    // The method used to access the attack cooldown value
    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    // The variable used to access the value of the projectile speed from the projectile class
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    // The variable used to access the value of the effect range of projectiles from the projectile class
    public float EffectRange
    {
        get { return effectRange; }
    }

    // The variable used to access the value of the effect number of projectiles from the projectile class
    public int NumberOfEffect
    {
        get { return numberOfEffect; }
    }

    // The variable used to access the value of the weakness multiplier of projectiles from the projectile class
    public float WeaknessMultiplier
    {
        get { return weaknessMultiplier; }
    }
    // The variable used to access the value of the target from the projectile class
    public Collider Target
    {
        get { return target; }
    }
    // The method used to access the list of colliders
    public List<Collider> Colliders
    {
        get => colliders;
    }
    // The method used to access the list of enemies that can be targeted by the lightning tower
    public List<Collider> Enemies
    {
        get => enemies;
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
        // If the game is not paused, make the tower attack
        if(GameAdvancement.gamePaused == false)
        {
            // Attack if there are enemies in range
            Attack();
        }
    }


    // Method used to attack
    public void Attack()
    {
        // Create a new list that contains the same colliders as the colliders list
        List<Collider> listOfDead = new List<Collider>();

        // Go over all enemies in the colliders list
        foreach(Collider coll in colliders)
        {
            // Check if there are any dead enemies in the colliders list
            if(coll.GetComponent<Enemy>().isAlive == false)
            {
                // Add theses enemies to the list of dead enemies
                listOfDead.Add(coll);
            }
        }

        // Check if there were any dead enemies in the colliders list
        if(listOfDead != null)
        {
            // Remove all dead enemies of the colliders list
            foreach(Collider coll2 in listOfDead)
            {
                colliders.Remove(coll2);
            }
        }

        // Check if the can attack flag is true or false
        if(!canAttack)
        {
            // If it is false, increase the timer by the time passed
            attackTimer += Time.deltaTime;

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

        // // Check if the enemy was just respawned, tagged as dead but has full health points
        // if(target != null && target.GetComponent<Enemy>().isAlive == false && target.GetComponent<Enemy>().GetCurrentHP == target.GetComponent<Enemy>().GetMaximumHP)
        // {
        //     // Set the target as alive again
        //     target.GetComponent<Enemy>().isAlive = true;

        //     Debug.Log("An enemy was reset in the tower range of: " + this.gameObject.name);
        // }

        // // If the Target isn't null and dead, remove it from the colliders list
        // if(target != null && target.GetComponent<Enemy>().isAlive == false)
        // {
        //     // Remove the target from the list
        //     colliders.Remove(target);

        //     Debug.Log("A dead enemy was removed form the collider list of the tower: " + this.gameObject.name);

        //     // // Destroy the game enemy
        //     // Destroy(target.gameObject);
        // }

        // Check if the sphere colider around the tower still contains the target enemy
        if(Colliders.Contains(target) == false)
        {
            // If the target left the range, set it to null
            target = null;
            // Debug.Log("The target was not contained anymore in the range and was set to null");

            // Debug.Log("The target has left the attack range of the tower: " + this.gameObject.name);
        }

        // Check if the tower needs a new target and if there are any enemies in range
        if(target == null && colliders.Count > 0)
        {
            // If a new target is needed, and there are enemies in range, set the oldest enemy as target
            target = Colliders[0];

            // Debug.Log("The new target was set to " + target);
        }

        // Check if there is a current target
        if(target != null && target.GetComponent<Enemy>().isAlive == true)
        {
            // Check if the can attack flag is set to true
            if(canAttack == true)
            {
                // Attack the target
                Shoot();
                // Debug.Log("The current enemy that is targeted is named: " + target.name);

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

        if(TowerType == TowerType.Lightning)
        {
            // // Set the list of targets to the list of colliders
            // enemies = GetColliders();
            enemies = new List<Collider>(colliders);

            // Remove the current target form the list
            enemies.Remove(target);

            // Apply the effect to the enemy
            LightningStrikeEffect(NumberOfEffect, target.gameObject.GetComponent<Enemy>());

            // Apply a visual effect on all enemies hit

        } else if(TowerType == TowerType.Wind)
        {
            // Apply the effect to the enemy
            WindGustEffect();

        } else {

            // Spawn the projectile
            // Projectile spawnedProjectile = SpawnProjectileForTower(towerType).GetComponent<Projectile>();
            Projectile spawnedProjectile = SpawnProjectileForTower(towerType, towerProjectile, projectileSpawner, EffectRange);

            // Initialize the projectile object, so that it knows what his parent is
            spawnedProjectile.Initialize(this);

            // // Set the projectile as child of the object where the projectile spawner
            // spawnedProjectile.gameObject.transform.parent = projectileSpawner.transform;

            // // Reset the position of the projectile game object
            // spawnedProjectile.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            // // Set the position of the projectile game object to the position of the projectile spawner
            // spawnedProjectile.gameObject.transform.position = projectileSpawner.transform.position;

            // Make sure the projectile is active
            // spawnedProjectile.gameObject.SetActive(true);

            // // Set the position of the projectile to the position of the tower
            // spawnedProjectile.transform.position = transform.position;

            // // Initialize the projectile object, so that it knows what his parent is
            // spawnedProjectile.Initialize(this);

            // // Resize the projectile
            // Vector3 scale = spawnedProjectile.gameObject.transform.localScale;
            // float scaleX = scale.x;
            // float scaleY = scale.y;
            // float scaleZ = scale.z;

            // spawnedProjectile.gameObject.transform.localScale = new Vector3(scaleX * (float)0.1, scaleY * (float)0.1, scaleZ * (float)0.1);
        }
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Make sure it is a game object with enemy tag
        if(other.gameObject.CompareTag("Enemy") && !colliders.Contains(other) )
        {
            colliders.Add(other);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            colliders.Remove(other);
        }
    }

    //-----------------------------------------------------------------------------------------------------------
    // The effect of towers that do not use projectiles that need to get spawned
    //-----------------------------------------------------------------------------------------------------------

    // The method that produces the effect of a lightning strike arriving at destination
    private void LightningStrikeEffect(int numberOfStrikes, Enemy targetEnemy)
    {
        // // Create a new list that contains the same colliders as the colliders list
        // List<Collider> enemiesList = new List<Collider>();

        // enemiesList = 

        // Calculate the damage
        int damage = Projectile.CalculateDamage(Damage, WeaknessMultiplier, TowerType, targetEnemy);

        // Make the enemy take damage
        targetEnemy.TakeDamage(damage);

        // Check if the lightning strike should jump
        if(numberOfStrikes > 0)
        {
            // Initialize the raycast hit
            //RaycastHit hit;

            // // Initialize the nearest enemy game object
            // GameObject nearestEnemy;

            // Calculate the radius of the effect
            // float radius = getEffectRange * targetEnemy.gameBoard.transform.localScale.x;

            // Check if there is another enemy in the range of the tower
            if(Enemies.Count >= 1)
            {
                // Initialize the nearest enemy
                Collider nearestEnemy = null;

                // Initialize the shortest distance
                float shortestDistance = EffectRange * Board.greatestBoardDimension;

                // Go through all other enemies, so skip the first index of the array
                for(int counter = 0; counter < Enemies.Count; counter = counter + 1)
                {
                    // Get the distance between the current target and the current candidate
                    float distance = Vector3.Distance(targetEnemy.transform.position, Enemies[counter].GetComponent<Enemy>().transform.position);

                    // Check if this distance is shorter than the current shortest distance
                    if(distance <= shortestDistance)
                    {
                        // Set this enemy as nearest enemy
                        nearestEnemy = Enemies[counter];

                        // Set the shortest distance to the distance between those two
                        shortestDistance = distance;
                    }
                }

                if(nearestEnemy != null)
                {
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
        int damage = Projectile.CalculateDamage(Damage, WeaknessMultiplier, TowerType, targetEnemy);

        // Make the enemy take damage
        targetEnemy.TakeDamage(damage);

        // // Calculate the direction in which the enemy should be pushed
        // Vector3 targetPosition = transform.position + (transform.position - targetEnemy.lastWaypoint).normalized * getProjectileSpeed * GetEffectTime * getLevel * targetEnemy.gameBoard.transform.localScale.x; // TODO

        // Push the enemy back by the distance scaled down to the board size * the level in the direction of the last waypoint
        // target.transform.position = Vector3.MoveTowards(transform.position, target.waypoints[target.waypointIndex - 1].transform.position + new Vector3(0, target.flightHeight, 0), parent.GetProjectileSpeed * parent.effectTime * parent.level * target.gameBoard.transform.localScale.x);    
        // target.transform.position = transform.Translate(targetPosition);
        // targetEnemy.transform.position = Vector3.MoveTowards(transform.position, targetPosition, getProjectileSpeed * GetEffectTime * getLevel * targetEnemy.gameBoard.transform.localScale.x);
        targetEnemy.transform.position = targetEnemy.transform.position - targetEnemy.transform.forward * EffectRange * Board.greatestBoardDimension;
    }

    // The method activated when clicking on the hidden button on towers to upgrade them
    public void TryOpeningUpgradeTowerMenu()
    {
        // Debug.Log("Upgrade tower button was pressed!");
        // Debug.Log("The number of questions that need to be answered is: " + Questions.numberOfQuestionsNeededToAnswer);
        // Debug.Log("The value of the game paused variable is: " + GameAdvancement.gamePaused);

        // Check that nothing is beeing build or upgraded
        if(GameAdvancement.gamePaused == false)
        {
            // Open the upgrade tower menu with the method of another script
            UpgradeTower.OpenUpgradeTowerMenu(this);
        }
    }

    //-----------------------------------------------------------------------------------------------
    // Upgrade tower methods
    //-----------------------------------------------------------------------------------------------

    // Method used to upgrade an archer tower
    public void UpgradeArcherTower(float damageMultiplicator, float attackCooldownMultiplicator, float attackRangeMultiplicator)
    {
        // Increase the tower level by one
        level = level + 1;

        // Increase the damage by the multiplicator
        damage = (int)(damage * damageMultiplicator);

        // Increase the attack cooldown by the multiplicator
        attackCooldown = attackCooldown * attackCooldownMultiplicator;

        // Increase the range cooldown by the multiplicator
        attackRange = attackRange * attackRangeMultiplicator;

        // Adjust the attack range
        AdjustTowerRange();
    }

    // Method used to upgrade a fire tower
    public void UpgradeFireTower(float damageMultiplicator, float attackCooldownMultiplicator)
    {
        // Increase the tower level by one
        level = level + 1;

        // Increase the damage by the multiplicator
        damage = (int)(damage * damageMultiplicator);

        // Increase the attack cooldown by the multiplicator
        attackCooldown = attackCooldown * attackCooldownMultiplicator;
    }

    // Method used to upgrade an earth tower
    public void UpgradeEarthTower(float damageMultiplicator, float sizeMultiplicator)
    {
        // Increase the tower level by one
        level = level + 1;

        // Increase the damage by the multiplicator
        damage = (int)(damage * damageMultiplicator);

        // Increase the effect range by the multiplicator
        effectRange = effectRange * sizeMultiplicator;
    }

    // Method used to upgrade a lightning tower
    public void UpgradeLightningTower(float damageMultiplicator, float jumpRangeMultiplicator, float attackRangeMultiplicator)
    {
        // Increase the tower level by one
        level = level + 1;

        // Increase the damage by the multiplicator
        damage = (int)(damage * damageMultiplicator);

        // Increase the effect range by the multiplicator
        effectRange = effectRange * jumpRangeMultiplicator;

        // Increase the attack range of the lightning tower
        attackRange = attackRange * attackRangeMultiplicator;

        // Adjust the attack range
        AdjustTowerRange();
    }

    // Method used to upgrade a wind tower
    public void UpgradeWindTower(float attackCooldownMultiplicator, float dropBackRangeMultiplicator)
    {
        // Increase the tower level by one
        level = level + 1;

        // Increase the attack cooldown by the multiplicator
        attackCooldown = attackCooldown * attackCooldownMultiplicator;

        // Increase the effect range by the multiplicator
        effectRange = effectRange * dropBackRangeMultiplicator;
    }

    // Method used to adjust the tower range (size of the light blue collider)
    private void AdjustTowerRange()
    {
        // Set the radius of the sphere collider on the tower range component with the tower script to the attack range
        this.gameObject.transform.localScale = new Vector3(AttackRange, AttackRange, AttackRange);
    }

    public void ResetTowerStatistics(Tower towerprefab)
    {
        Debug.Log("The tower statistics were reset");
        level = 1;
        cost = towerprefab.Cost;
        attackRange = towerprefab.AttackRange;
        damage = towerprefab.Damage;
        attackCooldown = towerprefab.AttackCooldown;
        projectileSpeed = towerprefab.ProjectileSpeed;
        effectRange = towerprefab.EffectRange;
        numberOfEffect = towerprefab.NumberOfEffect;
        weaknessMultiplier = towerprefab.WeaknessMultiplier;
    }
}