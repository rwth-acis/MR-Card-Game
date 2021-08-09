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
    // The variable stating that the image target is currently on the field and the build UI button should appear
    private bool makeBuildAppear = false;

    // The variable stating that the image target is currently not the field and the build UI button should disappear
    private bool makeBuildDisappear = false;

    // The variables stating if a tower was alread built on this image target
    public bool towerBuiltCorrectly;

    // Define the individual canvas for the build tower button
    [SerializeField]
    private GameObject buildUI;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check that no tower was built on the image target
        if(towerBuiltCorrectly == false)
        {
            // Check if the image target just entered the game board or left it
            if(makeBuildAppear == true && GameAdvancement.gamePaused == false)
            {
                // Make the ui button appear that should be clickable to construct a tower
                buildUI.SetActive(true);

                // Activate the canvas
                buildUI.GetComponent<Canvas>().enabled = true;

                // Activate the billboard script
                buildUI.GetComponent<Billboard>().enabled = true;

                // Set the flag to false
                makeBuildAppear = false;

            } else if(makeBuildDisappear == true)
            {
                // Make the ui button disappear
                buildUI.SetActive(false);

                // Set the flag to false
                makeBuildDisappear = false;
            }

            // Check if the build UI is active and if the game was paused
            if(GameAdvancement.gamePaused == true && buildUI.activeSelf == true)
            {
                // Deactivate the billboard script
                buildUI.SetActive(false);

                // Set the flag to false
                makeBuildAppear = true;
            }
        }
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter was activated");

        // Check if the collider that enetered the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            Debug.Log("Build should appear now.");
            makeBuildAppear = true;
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            makeBuildDisappear = true;
        }
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTower()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build tower menu of the build tower menu script
        OpenBuildTowerMenu();
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTrap()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build trap menu of the build tower menu script
        OpenBuildTrapMenu();
    }
}
