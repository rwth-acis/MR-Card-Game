using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemies target;

    private Tower parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    //
    public void Initialize(Tower parent)
    {
        // Set the parent tower of the projectile as the parent of the projectile
        this.target = parent.Target.gameObject.GetComponent<Enemies>();
        this.parent = parent;

        // Set the game object as child under the tower
        transform.parent = parent.transform;
    }

    private void MoveToTarget()
    {
        // Check that the target is not null
        if(target != null && target.isAlive == true)
        {
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
