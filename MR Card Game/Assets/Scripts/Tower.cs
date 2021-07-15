using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Type of projectile
    [SerializeField]
    private string projectileType;

    public Projectile projectile;

    // The attack range of the tower
    public float attackRange;

    // The damage of the tower
    public int damage;

    // The attack speed of the tower
    public float attackSpeed;

    // The level of the tower
    public int level;

    // The projectile type of the tower
    public string projectileName;

    // 
    public Vector3 vector;

    // 
    private Collider target;

    private List<Collider> colliders = new List<Collider>();

    public List<Collider> GetColliders()
    {
        return colliders;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        if(GetColliders().Contains(target) == false)
        {
            target = null;
        }

        if(target == null && colliders.Count > 0)
        {
            target = GetColliders()[0];
        }

        // if(target != null && target.IsActive() == true)
        if(target != null)
        {
            // Attack
            Debug.Log("The current enemey that is targeted is named: " + target.name);
            Shoot();
        }
    }

    // Method that shoots projectiles at enemies
    private void Shoot()
    {
        // Get the projectile of the tower
        // Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).getComponent<Projectile>();
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
