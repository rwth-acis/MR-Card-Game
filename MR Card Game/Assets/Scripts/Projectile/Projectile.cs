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

    // The last position of the target is saved so that the projectile can continue travelling after the enemy is dead
    private Vector3 lastPosition;

    // The list of colliders that enter the range of the tower
    private List<Collider> enemyColliders = new List<Collider>();

    // Access the list of colliders
    public List<Collider> EnemyColliders
    {
        get => enemyColliders;
        set => enemyColliders = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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

    /// <summary>
    /// Set the parent and target objects
    /// </summary>
    /// <param name="parent"></param>
    public void Initialize(Tower parent)
    {
        // Set the parent tower of the projectile as the parent of the projectile
        this.target = parent.Target.gameObject.GetComponent<Enemy>();
        this.parent = parent;
    }

    // Make a projectile move to a target
    private void MoveToTarget()
    {
        // Check that the target is not null
        if(target != null && target.IsAlive == true)
        {
            // Update the target last position
            lastPosition = target.transform.position;

            // If the target is not null and alive, move the projectile in the direction of the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed * 14 * Board.greatestBoardDimension);
            if(parent.TowerType == TowerType.Archer)
            {
                transform.LookAt(target.transform.position);
            }

            // Check if the projectile reached the destination
            if(ReachPosition(target.transform.position))
            {

                // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                if(parent.TowerType == TowerType.Archer)
                {
                    // Make a single enemy take damage
                    ArrowEffect();

                } else {
                    
                    // Make all enemies inside the projectile collider take damage
                    ProjectileThatCanHitMultipleEnemiesEffect();
                }
                ObjectPools.ReleaseProjectile(this, parent);
            }

        } else {

            // Check if it is not a wind tower or lightning tower that do not have a projectile
            if(parent.TowerType != TowerType.Wind && parent.TowerType != TowerType.Lightning && parent.TowerType != TowerType.Archer)
            {
                // Make the projectile move to the last position
                transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * parent.ProjectileSpeed);

                // Check if the projectile reached the destination
                if(transform.position == target.transform.position)
                {
                    ProjectileThatCanHitMultipleEnemiesEffect();
                    ObjectPools.ReleaseProjectile(this, parent);
                }
            } else {
                // Case it is an arrow and needs to despawn
                ObjectPools.ReleaseProjectile(this, parent);
            }
        }
    }

    private bool ReachPosition(Vector3 position)
    {
        if (Vector3.Magnitude(transform.position - position) <= 0.002f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // The effect of an arrow arriving at destination
    private void ArrowEffect()
    {
        // Calculate the damage
        int damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, target.GetComponent<Enemy>());
        target.TakeDamage(damage);
    }

    // The method that produces the effect of a fire ball and rock arriving at destination
    private void ProjectileThatCanHitMultipleEnemiesEffect()
    {
        // Initialize the damage variable
        int damage = 0;

        // Initialize the list of dead enemies
        List<Collider> listOfDeadEnemies = new List<Collider>();

        // For each enemy in the collider, calculate the damage they should take
        foreach(var targetEnemy in enemyColliders)
        {
            Debug.Log("Currently, the target enemy is null is: " + (targetEnemy == null));
            if(targetEnemy != null)
            {
                // Calculate the damage
                damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, targetEnemy.GetComponent<Enemy>());

                // Check that the damage is not null
                if(damage != 0)
                {
                    // Check if the enemy will die from this attack
                    if(targetEnemy != null && damage >= targetEnemy.GetComponent<Enemy>().CurrentHP)
                    {
                        listOfDeadEnemies.Add(targetEnemy);
                    }

                    if(targetEnemy != null)
                    {
                        targetEnemy.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
        }

        // Check that the list of dead enemies is not empty
        if(listOfDeadEnemies != null)
        {
            // Go over the list of dead enemies
            foreach(var enemy in listOfDeadEnemies)
            {
                // Remove the enemies that are dead from the colliders list
                enemyColliders.Remove(enemy);
            }
        }
    }

    // The method that calculates the damage a unit should take depending on the enemy, tower and tower attack type
    public static int CalculateDamage(int damage, float weaknessMultiplier, TowerType towerType, Enemy target)
    {

        Debug.Log("The enemy for which damage is calculated is: " + (target == null));
        // Initialize the additional damage multiplier
        int additionalDamageMultiplier = 0;

        if(target != null)
        {
            switch(towerType)
            {
                case TowerType.Archer:

                    if(target.Weakness == ResistenceAndWeaknessType.Archer)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Archer)
                    {
                        additionalDamageMultiplier = -1;
                    }

                break;
                
                case TowerType.Fire:
                    if(target.Weakness == ResistenceAndWeaknessType.Fire)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Fire)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;

                case TowerType.Lightning:
                    if(target.Weakness == ResistenceAndWeaknessType.Lighting)
                    {
                        if(GameAdvancement.raining == true || target.IsWet == true)
                        {
                            additionalDamageMultiplier = 2;
                        } else {
                            additionalDamageMultiplier = 1;
                        }
                    } else if(target.Resistance == ResistenceAndWeaknessType.Lighting)
                    {
                        if(GameAdvancement.raining == true)
                        {
                            additionalDamageMultiplier = 0;
                        } else {
                            additionalDamageMultiplier = -1;
                        }
                    } else {
                        if(GameAdvancement.raining == true || target.IsWet == true)
                        {
                            additionalDamageMultiplier = 1;
                        }
                    }
                break;

                case TowerType.Earth:
                    if(target.Weakness == ResistenceAndWeaknessType.Earth)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Earth)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;

                case TowerType.Wind:
                    if(target.Weakness == ResistenceAndWeaknessType.Wind)
                    {
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Wind)
                    {
                        additionalDamageMultiplier = -1;
                    }
                break;
            }

            // Return the damage with a bonus, a malus or flat depending on if a weakness or resistance was found
            return (int) (damage + additionalDamageMultiplier * damage * weaknessMultiplier);

        } else {

            return 0;
        }
    }

    // Reinitialize the colliders list
    public void ClearCollidersList()
    {
        // Reinitialize colliders list as a new list
        enemyColliders = new List<Collider>();
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is already in the list or not
        if(!enemyColliders.Contains(other))
        {
            // Add colliders that enter the projectile collider
            enemyColliders.Add(other);
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit (Collider other)
    {
        // Remove colliders that leave the collider of the projectile collider
        enemyColliders.Remove(other);
    }

    public void ReturnProjectileToObjectPool()
    {
        // Call the release enemy of the object pool class
        ObjectPools.ReleaseProjectile(this, parent);
    }
}
