using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
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
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        vector =  transform.parent.gameObject.transform.localScale;
        transform.localScale = transform.localScale + new Vector3(0, 1 / Board.greatestBoardDimension, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.gameObject.transform.localScale = vector + new Vector3(0, 1 / Board.greatestBoardDimension * 20, 0);
    }

    // Function that is called when somethin enters the sphere collider of the tower
    public void OnTriggerEnter3D(SphereCollider collider)
    {
        // Check the tag of the object that entered the sphere collider
        if(collider.tag == "Enemy")
        {
            // Get the gameObject that entered the tower range
            target = collider.gameObject;
            Debug.Log("The enemey is named: " + target.name);
        }
    }
}
