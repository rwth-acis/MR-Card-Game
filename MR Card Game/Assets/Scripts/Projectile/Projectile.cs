using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // A projectile instance needed to access non static variables in a static way
    public static Projectile Instance;

    // The target enemy of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

    /// <summary>
    /// Get the tower, which is the parent of the projectile
    /// </summary>
    public static Tower Parent
    {
        get { return Instance.parent; }
    }

    // The last position of the target is saved so that the projectile can continue travelling after the enemy is dead
    private Vector3 lastPosition;

    // The list of colliders that enter the range of the tower
    private List<Collider> enemyColliders = new List<Collider>();

    /// <summary>
    /// The list of enemies' colliders
    /// </summary>
    public List<Collider> EnemyColliders
    {
        get => enemyColliders;
        set => enemyColliders = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
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
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
            if(parent.TowerType == TowerType.Archer)
            {
                transform.LookAt(target.transform.position);
            }

            // Check if the projectile reached the destination
            if(ReachPosition(target.transform.position))
            {

                // Here, depending on the tower type, the enemy or enemies need to take damage depending on the type of projectile of the tower
                // Wind tower and lightning tower don't have projectiles.
                if(parent.TowerType == TowerType.Archer)
                {
                    ArrowEffect();
                } else {
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
        int damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, target.GetComponent<Enemy>(), parent.DamageRainingMultiplier);
        target.TakeDamage(damage);
    }

    // Produces the effect of a fire ball and rock arriving at destination
    // Wind and lightning tower don't have projectiles
    private void ProjectileThatCanHitMultipleEnemiesEffect()
    {

        // Initialize the list of dead enemies
        List<Collider> listOfDeadEnemies = new List<Collider>();

        // For each enemy in the collider, calculate the damage they should take
        foreach(var targetEnemy in enemyColliders)
        {
            Debug.Log("Currently, the target enemy is null is: " + (targetEnemy == null));
            if(targetEnemy != null)
            {
                // Initialize the damage variable
                // Calculate the damage
                int damage = CalculateDamage(parent.Damage, parent.WeaknessMultiplier, parent.TowerType, targetEnemy.GetComponent<Enemy>(), parent.DamageRainingMultiplier);

                // Check that the damage is not 0
                if (damage != 0)
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

        // Remove the dead enemies, if any
        if(listOfDeadEnemies != null)
        {
            foreach(var enemy in listOfDeadEnemies)
            {
                enemyColliders.Remove(enemy);
            }
        }
    }

    /// <summary>
    /// Calculates the damage a unit should take depending on the enemy, tower and tower attack type
    /// </summary>
    public static int CalculateDamage(int damage, float weaknessMultiplier, TowerType towerType, Enemy target, float damageWetMultiplier)
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
                        additionalDamageMultiplier = 1;
                    } else if(target.Resistance == ResistenceAndWeaknessType.Lighting)
                    {
                        additionalDamageMultiplier = -1;
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
            // Also adapt the damageWetMultiplier
            if (GameAdvancement.raining || target.IsWet)
            {
                return (int)((damage + additionalDamageMultiplier * damage * weaknessMultiplier) * damageWetMultiplier);
            }
            else
            {
                return (int)(damage + additionalDamageMultiplier * damage * weaknessMultiplier);
            }
        } else {

            return 0;
        }
    }

    /// <summary>
    /// Reinitialize the colliders list
    /// </summary>
    public void ClearCollidersList()
    {
        // Reinitialize colliders list as a new list
        enemyColliders = new List<Collider>();
    }

    // Adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is already in the list or not
        if(!enemyColliders.Contains(other))
        {
            // Add colliders that enter the projectile collider
            enemyColliders.Add(other);
        }
    }

    // Removes exiting enemies of the collider list
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
