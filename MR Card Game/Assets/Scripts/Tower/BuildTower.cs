using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static build.BuildTowerMenu;

// Define a static class that saves the image target on which the current tower is beeing built
public static class TowerImageTarget
{
    // The image target game object that opened the build tower menu
    public static GameObject currentImageTarget;
}

public class BuildTower : MonoBehaviour
{
    // // The variable stating that the image target is currently on the field and the build UI button should appear
    // private bool makeBuildAppear = false;

    // // The variable stating that the image target is currently not the field and the build UI button should disappear
    // private bool makeBuildDisappear = false;

    // // The variables stating if a tower was already built on this image target
    // public bool towerBuiltCorrectly;

    // The minimum distance between the image target and buildings for the build menu to appear
    public float minimumDistanceBase = 0.1f;

    // The flag that states if the build building image target is on the game board or not
    public bool onBoard = false;

    public bool visible = false;

    // Define the individual canvas for the build tower button
    [SerializeField]
    private GameObject buildUI;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the box collider is enabled
        this.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // // Check that no tower was built on the image target
        // if(towerBuiltCorrectly == false)
        // {

        // Make sure the box collider is enabled
        this.GetComponent<BoxCollider>().enabled = true;

        // Check if the image target just entered the game board or left it
        if(onBoard == true && GameAdvancement.gamePaused == false && CheckDistanceToTowers() == true && visible == true)
        {
            // Make the ui button appear that should be clickable to construct a tower
            buildUI.SetActive(true);

            // Activate the canvas
            buildUI.GetComponent<Canvas>().enabled = true;

            // Activate the billboard script
            buildUI.GetComponent<Billboard>().enabled = true;

            // // Set the flag to false
            // makeBuildAppear = false;

        } else if(onBoard == false || CheckDistanceToTowers() == false || visible == false)
        {
            // Make the ui button disappear
            buildUI.SetActive(false);

            // // Set the flag to false
            // makeBuildDisappear = false;
        }

        // Check if the build UI is active and if the game was paused
        if(GameAdvancement.gamePaused == true && buildUI.activeSelf == true)
        {
            // Deactivate the billboard script
            buildUI.SetActive(false);

            // // Set the flag to false
            // makeBuildAppear = true;

            // }
        }
    }

    // Methods activated when the image target enters the field
    public void ImageTargetVisible()
    {
        visible = true;
    }

    // Methods activated when the image target leaves the field
    public void ImageTargetLost()
    {
        visible = false;
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter was activated");

        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            // Debug.Log("Build should appear now.");
            // makeBuildAppear = true;

            onBoard = true;
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            // makeBuildDisappear = true;

            onBoard = false;
        }
    }

    // The method that checks the distance between the towers and the image target to see if a tower can be built here or not
    public bool CheckDistanceToTowers()
    {
        // Get the tower and trap array
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag("Trap");

        // Initialize the distance variable
        float distance = 0;

        // Initialize the minimum distance variable
        float minimumDistance = 1f * Board.greatestBoardDimension;

        Debug.Log("The board dimension is: " + Board.greatestBoardDimension);
        Debug.Log("The minimum distance is: " + minimumDistance);

        // Initialize the distance ok flag that states that the minimum distance was kept
        bool distanceOK = true;

        // Check if a tower is too close to the image target
        foreach(GameObject tower in towerArray)
        {
            // Get the distance between the image target and the tower
            distance = Vector3.Distance(tower.transform.position, this.transform.position);

            // Check if the distance is too short
            if(distance < minimumDistance)
            {
                distanceOK = false;
            }
        }

        // Check if a trap is too close to the image target
        foreach(GameObject trap in trapArray)
        {
            // Get the distance between the image target and the trap
            distance = Vector3.Distance(trap.transform.position, this.transform.position);

            // Check if the distance is too short
            if(distance < minimumDistance)
            {
                distanceOK = false;
            }
        }

        Debug.Log("The distance ok flag is: " + distanceOK);

        // Return the distance ok flag
        return distanceOK;
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTower()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build tower menu of the build tower menu script
        OpenBuildTowerMenu();

        // Save the position of the building in the build position vector
        TowerEnhancer.buildPosition = gameObject.transform.position;
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTrap()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build trap menu of the build tower menu script
        OpenBuildTrapMenu();

        // Save the position of the building in the build position vector
        TowerEnhancer.buildPosition = gameObject.transform.position;
    }
}
