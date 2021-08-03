using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static i5.Toolkit.Core.Examples.Spawners.SpawnTower;
using UnityEngine.EventSystems;

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

    [SerializeField]
    private int holeCost;

    [SerializeField]
    private int swampCost;

    // Define the individual canvas for the build tower button
    [SerializeField]
    private GameObject buildUI;

    // Define the canvas of the build tower menu
    [SerializeField]
    private GameObject buildTowerCanvas;

    // Define the build trap window
    [SerializeField]
    private GameObject buildTrapWindow;

    // Define the build tower window
    [SerializeField]
    private GameObject buildTowerWindow;

    // Define the answer questions menu
    [SerializeField]
    private GameObject answerQuestions;

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

    // Define the build wind tower button
    [SerializeField]
    private Button buildHole;

    // Define the build wind tower button
    [SerializeField]
    private Button buildSwamp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the image target just entered the game board or left it
        if(makeBuildAppear == true && GameAdvancement.gamePaused == false)
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

        // Check if the build UI is active and if the game was paused
        if(GameAdvancement.gamePaused == true && buildUI.activeSelf == true)
        {
            // Make the ui button disappear
            buildUI.SetActive(false);

            // Set the flag to false
            makeBuildAppear = true;
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
    public void OpenBuildTowerMenu()
    {
        // Pause the game
        GameAdvancement.gamePaused = true;

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
    }

    // The method that opens the build tower menu
    public void OpenBuildTrapMenu()
    {
        // Pause the game
        GameAdvancement.gamePaused = true;

        // Set the canvas as active
        buildTowerCanvas.SetActive(true);

        // Set the build trap menu as active
        buildTrapWindow.SetActive(true);

        // Make sure the build tower menu is inactive
        buildTowerWindow.SetActive(false);

        // Disable the trap buttons that cannot be bought, and enable the trap buttons that can be bought

        // Check if the player has enough coin to build a hole
        if(GameAdvancement.currencyPoints >= holeCost)
        {
            // Enable the hole button
            buildHole.interactable = true;

        } else {

            // Disable the hole button
            buildHole.interactable = false;
        }

        // Check if the player has enough coin to build a swamp
        if(GameAdvancement.currencyPoints >= swampCost)
        {
            // Enable the swamp button
            buildSwamp.interactable = true;

        } else {

            // Disable the swamp button
            buildSwamp.interactable = false;
        }
    }

    // The method that activates when the player wants to build an archer tower by pressing on the archer tower button in the build menu
    private void InitiateArcherTowerBuild()
    {
        // Enable the answer question menu
        answerQuestions.SetActive(true);

        // Set the number of questions that are needed to answer to 1
        Questions.numberOfQuestionsNeededToAnswer = 1;

        StartCoroutine(BuildArcherTower());
    }

    // The method that activates when the player wants to build an fire tower by pressing on the fire tower button in the build menu
    private void InitiateFireTowerBuild()
    {
        // Enable the answer question menu
        answerQuestions.SetActive(true);

        // Set the number of questions that are needed to answer to 1
        Questions.numberOfQuestionsNeededToAnswer = 1;

        StartCoroutine(BuildFireTower());
    }

    // The method that activates when the player wants to build an earth tower by pressing on the earth tower button in the build menu
    private void InitiateEarthTowerBuild()
    {
        // Enable the answer question menu
        answerQuestions.SetActive(true);

        // Set the number of questions that are needed to answer to 1
        Questions.numberOfQuestionsNeededToAnswer = 1;

        StartCoroutine(BuildEarthTower());
    }

    // The method that activates when the player wants to build an lightning tower by pressing on the lightning tower button in the build menu
    private void InitiateLightningTowerBuild()
    {
        // Enable the answer question menu
        answerQuestions.SetActive(true);

        // Set the number of questions that are needed to answer to 1
        Questions.numberOfQuestionsNeededToAnswer = 1;

        StartCoroutine(BuildLightningTower());
    }

    // The method that activates when the player wants to build an wind tower by pressing on the wind tower button in the build menu
    private void InitiateWindTowerBuild()
    {
        // Enable the answer question menu
        answerQuestions.SetActive(true);

        // Set the number of questions that are needed to answer to 1
        Questions.numberOfQuestionsNeededToAnswer = 1;

        StartCoroutine(BuildWindTower());
    }

    // Function that is used to test when all questions that were needed to be answered were answered correctly
    private bool NoMoreQuestionsNeeded()
    {
        return Questions.numberOfQuestionsNeededToAnswer == 0;
    }

    // The method that builds an archer tower over the image target
    IEnumerator BuildArcherTower()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Spawn the archer tower or extract it from the object pool
        SpawnArcherTower(this.gameObject);
    }


    // The method that builds a fire tower over the image target
    IEnumerator BuildFireTower()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Spawn the fire tower or extract it from the object pool
        SpawnFireTower(this.gameObject);
    }

    // The method that builds a earth tower over the image target
    IEnumerator BuildEarthTower()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Spawn the earth tower or extract it from the object pool
        SpawnEarthTower(this.gameObject);
    }

    // The method that builds a lightning tower over the image target
    IEnumerator BuildLightningTower()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Spawn the lightning tower or extract it from the object pool
        SpawnLightningTower(this.gameObject);
    }

    // The method that builds a wind tower over the image target
    IEnumerator BuildWindTower()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Spawn the wind tower or extract it from the object pool
        SpawnWindTower(this.gameObject);
    }
}
