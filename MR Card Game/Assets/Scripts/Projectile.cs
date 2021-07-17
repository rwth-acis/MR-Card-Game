using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // The target of the projectile
    private Enemy target;

    // The parent tower of the projectile
    private Tower parent;

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
}
