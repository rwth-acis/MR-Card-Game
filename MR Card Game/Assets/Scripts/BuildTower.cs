using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static i5.Toolkit.Core.Examples.Spawners.SpawnTower;

public class BuildTower : MonoBehaviour
{
    // The variable stating that the image target is currently on the field and the build UI button should appear
    private bool makeBuildAppear = false;

    // The variable stating that the image target is currently not the field and the build UI button should disappear
    private bool makeBuildDisappear = false;

    [SerializeField]
    private int archerTowerCost;

    [SerializeField]
    private int fireTowerCost;

    [SerializeField]
    private int earthTowerCost;

    [SerializeField]
    private int lightningTowerCost;

    [SerializeField]
    private int windTowerCost;

    // Define the individual canvas for the build tower button
    [SerializeField]
    private GameObject buildUI;

    // Define the canvas of the build tower menu
    [SerializeField]
    private GameObject buildTowerCanvas;

    // Define the build tower window
    [SerializeField]
    private GameObject buildTowerWindow;

    // Define the build trap window
    [SerializeField]
    private GameObject buildTrapWindow;

    // Define the build archer tower button
    [SerializeField]
    private Button buildArcherTower;

    // Define the build fire tower button
    [SerializeField]
    private Button buildFireTower;

    // Define the build earth tower button
    [SerializeField]
    private Button buildEarthTower;

    // Define the build Lightning tower button
    [SerializeField]
    private Button buildLightningTower;

    // Define the build wind tower button
    [SerializeField]
    private Button buildWindTower;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the image target just entered the game board or left it
        if(makeBuildAppear == true)
        {
            // Make the ui button appear that should be clickable to construct a tower
            buildUI.SetActive(true);

            // Set the flag to false
            makeBuildAppear = false;

        } else if(makeBuildDisappear == true)
        {
            // Make the ui button disappear
            buildUI.SetActive(false);

            // Set the flag to false
            makeBuildDisappear = false;
        }
    }

    // The method that adds entering enemies to the collider list
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that enetered the box collider of the image target is the game board
        if(other.gameObject.name == "Board")
        {
            makeBuildAppear = true;
        }
    }

    // The method that removes exiting enemies of the collider list
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.name == "Board")
        {
            makeBuildDisappear = true;
        }
    }

    // The method that opens the build tower menu
    private void OpenBuildTowerMenu()
    {
        // Set the canvas as active
        buildTowerCanvas.SetActive(true);

        // Set the build tower menu as active
        buildTowerWindow.SetActive(true);

        // Make sure the build trap menu is inactive
        buildTrapWindow.SetActive(false);

        // Disable the tower buttons that cannot be bought, and enable the tower buttons that can be bought

        // Check if the player has enough coin to build an archer tower
        if(GameAdvancement.currencyPoints >= archerTowerCost)
        {
            // Enable the archer tower
            buildArcherTower.interactable = true;

        } else {

            // Disable the archer tower
            buildArcherTower.interactable = false;
        }

        // Check if the player has enough coin to build a fire tower
        if(GameAdvancement.currencyPoints >= fireTowerCost)
        {
            // Enable the fire tower
            buildFireTower.interactable = true;

        } else {

            // Disable the fire tower
            buildFireTower.interactable = false;
        }

        // Check if the player has enough coin to build an earth tower
        if(GameAdvancement.currencyPoints >= earthTowerCost)
        {
            // Enable the earth tower
            buildEarthTower.interactable = true;

        } else {

            // Disable the earth tower
            buildEarthTower.interactable = false;
        }

        // Check if the player has enough coin to build a lightning tower
        if(GameAdvancement.currencyPoints >= lightningTowerCost)
        {
            // Enable the lightning tower
            buildLightningTower.interactable = true;

        } else {

            // Disable the lightning tower
            buildLightningTower.interactable = false;
        }

        // Check if the player has enough coin to build a wind tower
        if(GameAdvancement.currencyPoints >= windTowerCost)
        {
            // Enable the wind tower
            buildWindTower.interactable = true;

        } else {

            // Disable the wind tower
            buildWindTower.interactable = false;
        }

        // Here the game should be paused
    }

    // The method that builds an archer tower over the image target
    private void BuildArcherTower()
    {
        // Spawn the archer tower or extract it from the object pool
        SpawnArcherTower(this.gameObject);
    }

    // The method that builds a fire tower over the image target
    private void BuildFireTower()
    {
        // Spawn the fire tower or extract it from the object pool
        SpawnFireTower(this.gameObject);
    }

    // The method that builds a earth tower over the image target
    private void BuildEarthTower()
    {
        // Spawn the earth tower or extract it from the object pool
        SpawnEarthTower(this.gameObject);
    }

    // The method that builds a lightning tower over the image target
    private void BuildLightningTower()
    {
        // Spawn the lightning tower or extract it from the object pool
        SpawnLightningTower(this.gameObject);
    }

    // The method that builds a wind tower over the image target
    private void BuildWindTower()
    {
        // Spawn the wind tower or extract it from the object pool
        SpawnWindTower(this.gameObject);
    }

}
