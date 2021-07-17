using i5.Toolkit.Core.Spawners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using i5.Toolkit.Core.Utilities;
using static i5.Toolkit.Core.Examples.Spawners.SpawnProjectile;

public class Tower : MonoBehaviour
{
    // // Type of projectile
    // [SerializeField]
    // private string projectileType;

    // The projectile object
    [SerializeField]
    private Projectile projectile;

    // The additional height where the projectile should be shooted from
    [SerializeField]
    private float additionalShootingHeight;

    // The attack range of the tower
    [SerializeField]
    private float attackRange;

    // The damage of the tower
    [SerializeField]
    private int damage;

    public int Damage
    {
        get { return damage; }
    }

    // The flag that states if the tower can attack right now or not
    private bool canAttack;

    // The attack timer that is counted up after an attack
    private float attackTimer;

    // The attack cooldown between the attacks of the tower
    public float attackCooldown;

    // The projectile speed of the projectiles of this tower
    [SerializeField]
    private float projectileSpeed;

    // The variable used to access the value of the projectile speed from the projectile class
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    // The level of the tower
    [SerializeField]
    private int level;

    // 
    private Vector3 vector;

    // The current target of the tower
    private Collider target;

    // The variable used to access the value of the target from the projectile class
    public Collider Target
    {
        get { return target; }
    }

    private List<Collider> colliders = new List<Collider>();

    public List<Collider> GetColliders()
    {
        return colliders;
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

        // Check that the enemy did not die already
        // if(target != null && !IsValid(target.GetComponent<Enemies>().gameObject))
        if(target == null && !IsValid(target.GetComponent<Enemy>().gameObject))
        {
            // If the target is already dead, remove the target from the colliders array
            colliders.Remove(target);
            target = GetColliders()[0];
            Debug.Log("The target was dead and was removed");
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

    // Method that checks if a game object still exists
    public static bool IsValid(GameObject enemy)
    {
        // Try to access it
        try
        {
            // If it is null, return false
            if (enemy.gameObject == null) return false;

        }
        // Check if you catch an exception
        catch(Exception)
        {
            // If an exception was thrown, then the object was destroyed
            return false;
        }
        return true;
    }

    // Method that shoots projectiles at enemies
    private void Shoot()
    {
        // Find the spawner object
        Spawner spawner = GameObject.Find("ProjSpawn").GetComponent<Spawner>();

        // Get a projectile form the object pool
        Projectile spawnedProjectile = ObjectPool<Projectile>.RequestResource(() => {return new Projectile();});

        // Spawn the projectile
        // Projectile spawnedProjectile = SpawnProjectileForTower(spawner).GetComponent<Projectile>();

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
}
