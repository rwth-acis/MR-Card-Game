using i5.Toolkit.Core.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using i5.Toolkit.Core.Utilities;
using static i5.Toolkit.Core.Examples.Spawners.SpawnProjectile;

/// <summary>
/// Save the tower that needs to be enhanced
/// </summary>
public static class TowerEnhancer
{
    public static Tower currentlyEnhancedTower;

    /// <summary>
    /// The position where the tower / trap building was initialized
    /// </summary>
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

    [Tooltip("The attack range (radius) of the tower in meter")]
    [SerializeField]
    private float attackRange;

    [Tooltip("The original range (radius) of the tower prefab in meter, assume the tower prefab has a scale of (1,1,1)")]
    [SerializeField]
    private float originalAttackRange = 7.5f;

    [SerializeField]
    private int damage;

    [Tooltip("The attack CD in second.")]
    [SerializeField]
    private float attackCooldown;

    [Tooltip("The fyling speed of the projectiles of this tower in m/s. 0 means no projectile (projectile flys infinitely fast)")]
    [SerializeField]
    private float projectileSpeed;

    [Tooltip("The effect range of the projectiles of this tower in meter. Exact meaning is depending on tower type." +
        "\n Fire and Earth tower: range of the projectiles." +
        "\n Lightning tower: the range for jumping of the attack between enemies." +
        "\n Wind tower: the distance that enemies are blowed away")] 
    [SerializeField]
    private float effectRange;

    [Tooltip("The effect number of the projectiles of this tower")]
    [SerializeField]
    private int numberOfEffect;

    [Tooltip("The weakness multiplier of the tower type")]
    [SerializeField]
    private float weaknessMultiplier;

    [Tooltip("The damage multiplier when it is raining or isWet (in the swamp trap), especially for lightning tower and fire tower")]
    [SerializeField]
    private float damageWetMultiplier = 1f;

    [Tooltip("The damage reducing factor when the tower can attack more than one enemy at the same time")]
    [SerializeField]
    private float damageReducingFactor = 1f;


    private int level;
    private bool canAttack;
    // The attack timer that is counted up after an attack
    private float attackTimer;
    private Collider target;

    // The list of enemy colliders that enter the range of the tower
    private List<Collider> enemyColliders = new List<Collider>();

    public TowerType TowerType
    {
        get { return towerType; }
    }

    public float OriginalAttackRange
    {
        get => originalAttackRange;
    }

    public float Level
    {
        get { return level; }
    }

    public float Cost
    {
        get { return cost; }
    }

    public float AttackRange
    {
        get { return attackRange; }
    }

    public int Damage
    {
        get { return damage; }
    }

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    /// <summary>
    /// Projectile speed in m/s
    /// </summary>
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    public float EffectRange
    {
        get { return effectRange; }
    }

    public int NumberOfEffect
    {
        get { return numberOfEffect; }
    }

    public float WeaknessMultiplier
    {
        get { return weaknessMultiplier; }
    }

    public float DamageRainingMultiplier
    {
        get => damageWetMultiplier;
    }

    public Collider Target
    {
        get { return target; }
    }
    /// <summary>
    /// List of enmey colliders
    /// </summary>
    public List<Collider> Colliders
    {
        get => enemyColliders;
    }

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is not paused, make the tower attack
        if(GameAdvancement.gamePaused == false)
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Create a new list that contains the same colliders as the colliders list
        List<Collider> listOfDeadEnemies = new List<Collider>();
        foreach(Collider coll in enemyColliders)
        {
            if(coll.GetComponent<Enemy>().IsAlive == false)
            {
                listOfDeadEnemies.Add(coll);
            }
        }

        // Check if there were any dead enemies in the colliders list
        if(listOfDeadEnemies != null)
        {
            foreach(Collider coll in listOfDeadEnemies)
            {
                enemyColliders.Remove(coll);
            }
        }

        // check the timer if the tower cannot attack
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        // Check if the sphere colider around the tower still contains the target enemy
        if(Colliders.Contains(target) == false)
        {
            // If the target left the range, set it to null
            target = null;
        }

        // Check if the tower needs a new target and if there are any enemies in range
        if(target == null && enemyColliders.Count > 0)
        {
            target = Colliders[0];
        }

        // Check if there is a current target
        if(target != null && target.GetComponent<Enemy>().IsAlive == true)
        {
            if(canAttack == true)
            {
                Shoot();
                canAttack = false;
            }
        }
    }


    // Shoots projectiles at enemies
    private void Shoot()
    {
        if(TowerType == TowerType.Lightning)
        {
            LightningStrikeEffect(NumberOfEffect, target.gameObject.GetComponent<Enemy>());
        } 
        else if(TowerType == TowerType.Wind)
        {
            WindGustEffect();
        } 
        else {
            if (towerProjectile == null)
            {
                i5Debug.LogError($"No projectile given for tower of type {TowerType}", this);
                return;
            }
            Projectile spawnedProjectile = SpawnProjectileForTower(towerType, towerProjectile, projectileSpawner, EffectRange);
            // Initialize the projectile object, so that it knows what his parent is
            spawnedProjectile.Initialize(this);
        }
    }

    /// <summary>
    /// Adds entering enemies to the collider list
    /// </summary>
    /// <param name="other">collider of the enemy</param>
    private void OnTriggerEnter(Collider other)
    {
        // Make sure it is a game object with enemy tag
        if(other.gameObject.CompareTag("Enemy") && !enemyColliders.Contains(other) )
        {
            enemyColliders.Add(other);
        }
    }

    /// <summary>
    /// Removes exiting enemies of the collider list
    /// </summary>
    /// <param name="other">collider of the enemy</param>
    private void OnTriggerExit (Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            enemyColliders.Remove(other);
        }
    }

    //-----------------------------------------------------------------------------------------------------------
    // The effect of towers that do not use projectiles that need to get spawned
    //-----------------------------------------------------------------------------------------------------------

    // Produces the effect of a lightning strike arriving at destination
    private void LightningStrikeEffect(int numberOfStrikes, Enemy targetEnemy)
    {

        // Do the damage
        int damage = Projectile.CalculateDamage(Damage, WeaknessMultiplier, TowerType, targetEnemy, damageWetMultiplier);
        targetEnemy.TakeDamage(damage);
        List<GameObject> enemies = new(GameObject.FindGameObjectsWithTag("Enemy"));
        enemies.Remove(targetEnemy.gameObject);
        for (int i = 0; i < numberOfStrikes; i++)
        {
            // initialize the closestEnemy to null to check whether there is an enemy in the range later.
            GameObject closestEnemy = null;
            float closestDistance = EffectRange;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(targetEnemy.gameObject.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            if (closestEnemy != null)
            {
                closestEnemy.GetComponent<Enemy>().TakeDamage((int)(damage * Mathf.Pow(damageReducingFactor, i + 1)));
                enemies.Remove(closestEnemy);
                targetEnemy = closestEnemy.GetComponent<Enemy>();
            }
            else
            {
                return;
            }
        }
    }

    // Produces the effect of a gust of wind arriving at destination
    private void WindGustEffect()
    {
        Enemy targetEnemy = target.GetComponent<Enemy>();
        int damage = Projectile.CalculateDamage(Damage, WeaknessMultiplier, TowerType, targetEnemy, damageWetMultiplier);
        targetEnemy.TakeDamage(damage);
        // blow the enemy
        targetEnemy.transform.position = targetEnemy.transform.position - EffectRange * targetEnemy.transform.forward;
    }

    /// <summary>
    /// Activated when clicking on the hidden button on towers to upgrade them
    /// </summary>
    public void TryOpeningUpgradeTowerMenu()
    {
        if(GameAdvancement.gamePaused == false)
        {
            UpgradeTower.OpenUpgradeTowerMenu(this);
        }
    }

    //-----------------------------------------------------------------------------------------------
    // Upgrade tower methods
    //-----------------------------------------------------------------------------------------------

    /// <summary>
    /// Upgrade an archer tower
    /// </summary>
    public void UpgradeArcherTower(float damageMultiplicator, float attackCooldownMultiplicator, float attackRangeMultiplicator)
    {
        level++;
        damage = (int)(damage * damageMultiplicator);
        attackCooldown *= attackCooldownMultiplicator;
        attackRange *= attackRangeMultiplicator;
        AdjustTowerRange(attackRangeMultiplicator);
    }

    /// <summary>
    /// Upgrade a fire tower
    /// </summary>
    public void UpgradeFireTower(float damageMultiplicator, float attackCooldownMultiplicator)
    {
        level++;
        damage = (int)(damage * damageMultiplicator);
        attackCooldown *= attackCooldownMultiplicator;
    }

    /// <summary>
    /// Upgrade an earth tower
    /// </summary>
    public void UpgradeEarthTower(float damageMultiplicator, float sizeMultiplicator)
    {
        level++;
        damage = (int)(damage * damageMultiplicator);
        effectRange *= sizeMultiplicator;
    }

    /// <summary>
    /// Upgrade a lightning tower
    /// </summary>
    public void UpgradeLightningTower(float damageMultiplicator, float jumpRangeMultiplicator, float attackRangeMultiplicator)
    {
        level++;
        damage = (int)(damage * damageMultiplicator);
        effectRange *= jumpRangeMultiplicator;
        attackRange *= attackRangeMultiplicator;
        AdjustTowerRange(attackRangeMultiplicator);
    }

    /// <summary>
    /// Upgrade a wind tower
    /// </summary>
    public void UpgradeWindTower(float attackCooldownMultiplicator, float dropBackRangeMultiplicator)
    {
        level++;
        attackCooldown *= attackCooldownMultiplicator;
        effectRange *= dropBackRangeMultiplicator;
    }

    // Adjust the tower range (size of the light blue collider)
    private void AdjustTowerRange(float attackRangeMultiplier)
    {
        // Set the radius of the sphere collider on the tower range component with the tower script to the attack range
        gameObject.transform.localScale *= attackRangeMultiplier;
    }

    public void ResetTowerProperties(Tower towerPrefab)
    {
        Debug.Log("The tower statistics were reset");
        level = 1;
        cost = towerPrefab.Cost;
        attackRange = towerPrefab.AttackRange;
        damage = towerPrefab.Damage;
        attackCooldown = towerPrefab.AttackCooldown;
        projectileSpeed = towerPrefab.ProjectileSpeed;
        effectRange = towerPrefab.EffectRange;
        numberOfEffect = towerPrefab.NumberOfEffect;
        weaknessMultiplier = towerPrefab.WeaknessMultiplier;
    }
}