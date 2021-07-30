using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTower : MonoBehaviour
{
    // The variable stating if the image target is currently on the field
    private bool currentlyOnField = false;

    // The variable that tells if the image target is in the camera field or not
    private bool imageTargetVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // // Check if the image target is visible
        // if(imageTargetVisible == true)
        // {
        //     // Check if the image target is inside the board
        // }

        // Check if the image target is on the game board
        if(currentlyOnField == true)
        {
            // Make the ui button appear that should be clickable to construct a tower
        }
    }

    // // Method that is activated when the image target becomes visible
    // public void TowerImageTargetBecameVisible()
    // {
    //     // Set the image target visible variable to true
    //     imageTargetVisible = true;
    // }

    // // Method that is activated when image target leaves the camera field
    // public void TowerImageTargetNotVisible()
    // {
    //     // Set the image target visible variable to false
    //     imageTargetVisible = false;
    // }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that enetered the box collider of the image target is the game board
        if(collider.gameObject.name = "Board")
        {
            currentlyOnField = true;
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(collider.gameObject.name = "Board")
        {
            currentlyOnField = true;
        }
    }
}
