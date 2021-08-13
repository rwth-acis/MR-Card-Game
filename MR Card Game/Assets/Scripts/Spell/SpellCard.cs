using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrawCards
{
    // The number of free cards that can be drawn without answering questions
    public static int freeDraws = 0;
}

public class SpellCard : MonoBehaviour
{

    // The boolean variable that states that the image target is on or off the game board
    private bool onGameBoard = false;

    // The canvas game object on which the reveal spell button is 
    [SerializeField]
    private GameObject revealSpellCanvas;

    // The boolean variable that states that the image target is in the camera field
    private bool cardVisibleButNotDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the reveal spell card overlay is not displayed while the image target is in the camera field and the game is not paused
        if(cardVisibleButNotDisplayed == true && GameAdvancement.gamePaused == false)
        {
            // Set the variable that the spell card is visible but the overlay not displayed to false
            cardVisibleButNotDisplayed = false;

            // Display the reveal spell menu
            DisplayRevealSpell();
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    // Drawing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // Method that is activated when the spell image target enters the camera field
    public void SpellCardEnteredCameraField()
    {
        // Hide the reveal spell menu
        HideRevealSpell();
    }

    // Method that is activated when the spell image target leaves the camera field
    public void SpellCardLeftCameraField()
    {
        // Check that the game is not paused
        if(GameAdvancement.gamePaused == false)
        {
            // Display the reveal spell menu
            DisplayRevealSpell();

        } else {

            // Set the variable that the spell card is visible but the overlay not displayed to true
            cardVisibleButNotDisplayed = true;
        }
    }

    // The method that activates the canvas on which the reveal spell button is
    private void DisplayRevealSpell()
    {
        revealSpellCanvas.SetActive(true);
    }

    // The method that deactivates the canvas on which the reveal spell button is
    private void HideRevealSpell()
    {
        revealSpellCanvas.SetActive(false);
    }

    // The method that is activated when the user clicks on the reveal spell button
    public void RevealSpell()
    {

    }

    //---------------------------------------------------------------------------------------------------------------
    // Playing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // The method used to detect that the image target entered the game board space
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            onGameBoard = true;
        }
    }

    // The method used to detect that the image target left the game board space
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            onGameBoard = true;
        }
    }
}
