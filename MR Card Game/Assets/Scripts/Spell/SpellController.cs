using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    private GameObject spellCardCanvas;

    // The draw spell button 
    [SerializeField]
    private Button drawSpellButton;

    // The play spell button 
    [SerializeField]
    private Button playSpellButton;

    // The spell image
    [SerializeField]
    private UnityEngine.UI.Image spellImage;

    // The spell type got from the image target
    private SpellType spellType;

    // The flag that states that the card was drawn
    private bool cardDrawn = false;

    // The flag that states if the card is currently visible
    private bool visible = false;

    //The projected position on ground plane
    private Vector3 projectedPos;

    // The boolean variable that states that the image target is on or off the game board
    private bool onBoard = false;

    private GameObject groundPlane;

    private GameObject gameBoard;

    public bool CardDrawn
    {
        get => cardDrawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = SpellCardManager.GroundPlane;
        gameBoard = SpellCardManager.GameBoard;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the wave is not ongoing anymore
        if (LevelInfo.waveOngoing == false)
        {
            // Hide the play spell button
            HideSpellCanvas();

        }
        else
        {
            // Check if there is at least one drawn spell card on the game board
            if (SpellCardManager.DrawnSpellsOnBoard > 0 && !cardDrawn)
            {
                // Hide the reveal spell menu
                HideSpellCanvas();
            }

            if (visible)
            {
                projectedPos = ProjectPositionOnGroundPlane();
                spellCardCanvas.transform.SetParent(groundPlane.transform, true);
                spellCardCanvas.transform.localPosition = new Vector3(projectedPos.x, 0.01f, projectedPos.z);
                if (OverlapWithGameBoard(projectedPos))
                {
                    onBoard = true;
                }
                else
                {
                    onBoard = false;
                }
            }
            else
            {
                onBoard = false;
            }

            // Check if the card is visible but not drawn while the game is not paused and no other spell card is beeing drawn
            if (visible && !cardDrawn && !GameAdvancement.gamePaused && Questions.numberOfQuestionsNeededToAnswer == 0)
            {
                // Display the reveal spell menu
                DisplayDrawSpell();
            }

            // Make drawn spells appear if the wave is ongoing
            if (visible && cardDrawn)
            {
                if (onBoard)
                {
                    // Display the reveal spell menu
                    DisplayPlaySpell();

                }
                else
                {

                    // Display the reveal spell menu
                    DisplaySpellImage();
                }
            }
        }
    }

    // Project the position of the image target (gameObject) to the ground plane with position.y = 0, using similar triangles
    // return the projected position
    private Vector3 ProjectPositionOnGroundPlane()
    {
        Vector3 cameraPos = GetRelativePosition(groundPlane.transform, Camera.main.transform.position);
        Vector3 imageTargetPos = GetRelativePosition(groundPlane.transform, gameObject.transform.position);
        Vector3 cameraToCard = imageTargetPos - cameraPos;
        float similarityRatio = cameraPos.y / (cameraPos.y - imageTargetPos.y);
        Vector3 cameraToProjectedPos = cameraToCard * similarityRatio;
        Vector3 projectedPos = cameraPos + cameraToProjectedPos;
        return projectedPos;
    }

    private Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
        return relativePosition;
    }

    //get a position vector in gameboard coordinate.
    private bool OverlapWithGameBoard(Vector3 pos)
    {
        Vector3 gameBoardMin = GetRelativePosition(groundPlane.transform, gameBoard.GetComponentInChildren<BoxCollider>().bounds.min);
        Vector3 gameBoardMax = GetRelativePosition(groundPlane.transform, gameBoard.GetComponentInChildren<BoxCollider>().bounds.max);
        if (pos.x > gameBoardMin.x && pos.z > gameBoardMin.z && pos.x < gameBoardMax.x && pos.z < gameBoardMax.z)
        {
            return true;
        }
        //if the game board is rotated 180 degrees
        else if (pos.x < gameBoardMin.x && pos.z < gameBoardMin.x && pos.x > gameBoardMax.x && pos.z > gameBoardMax.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlaySpell()
    {
        if(SpellCardManager.Instance.TryPlaySpell(spellType, spellCardCanvas.transform.position))
        {
            cardDrawn = false;
            DisplayDrawSpell();
        }
        else
        {
            Invoke(nameof(ResetCardDrawn), 2f);
        }
    }
    private void ResetCardDrawn()
    {
        cardDrawn = false;
    }

    //---------------------------------------------------------------------------------------------------------------
    // Drawing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // Method that is activated when the spell image target enters the camera field
    public void SpellCardEnteredCameraField(ImageTargetBehaviour imageTargetBehaviour)
    {
        Debug.Log(imageTargetBehaviour.TrackableName);
        spellType = GetSpellTypeWithImageTargetName(imageTargetBehaviour.TrackableName);
        // Set the flag that the card is now visible
        visible = true;

        // Check if that card was already drawn
        if (cardDrawn)
        {
            if (onBoard)
            {
                // Increase the number of drawn spells on the board by one
                SpellCardManager.DrawnSpellsOnBoard++;
                DisplayPlaySpell();
            }
            else
            {
                // Display the drawn spell card
                DisplaySpellImage();
            }
        }
        else
        {
            // Check that the game is not paused
            if (GameAdvancement.gamePaused == false && LevelInfo.waveOngoing == true && Questions.numberOfQuestionsNeededToAnswer == 0)
            {
                // Display the reveal spell menu
                DisplayDrawSpell();

            }
        }
    }

    // Method that is activated when the spell image target leaves the camera field
    public void SpellCardLeftCameraField()
    {
        // Set the flag that the card is not visible anymore
        visible = false;

        // Hide the reveal spell menu
        HideSpellCanvas();

        // Check if the card was drawn
        if (cardDrawn && onBoard)
        {
            // Decrease the number of drawn spells on the game board by one
            SpellCardManager.DrawnSpellsOnBoard--;
        }
    }

    // The method that activates the canvas on which the reveal spell button is
    private void DisplayDrawSpell()
    {
        // Enable the spell card canvas
        spellCardCanvas.SetActive(true);

        // Enable the draw spell button
        drawSpellButton.gameObject.SetActive(true);

        // Disable the play spell button
        playSpellButton.gameObject.SetActive(false);

        // Make sure the spell image is disabled
        spellImage.gameObject.SetActive(false);
    }

    // The method that deactivates the canvas on which the reveal spell button is
    private void HideSpellCanvas()
    {
        spellCardCanvas.SetActive(false);
    }

    // The method that is activated when the user clicks on the draw spell button
    public void InitiateDrawSpell()
    {
        GameAdvancement.gamePaused = true;
        // Check if the player has free draws
        if (SpellCardManager.FreeDraws == 0)
        {
            // Set the question requesting image target correctly
            Questions.questionRequestingImageTarget = this.gameObject;

            // Disable the game overlay
            GameSceneManager.DeactivateGameOverlay();

            // Enable the answer question menu
            SpellCardManager.AnswerQuestions.SetActive(true);

            // Set the number of questions that are needed to answer to 1
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);

            // Make the canvas disappear
            HideSpellCanvas();

            // Start the routine that waits for the questions to be answered
            StartCoroutine(DrawSpell());

        }
        else
        {

            // Reduce the number of free draws by one
            SpellCardManager.FreeDraws--;

            // Set the flag that states that the card was drawn
            cardDrawn = true;

            // Reveal the spell card
            RevealSpell();
        }
    }

    // The method that builds an archer tower over the image target
    IEnumerator DrawSpell()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);

        // Enable the game overlay
        GameSceneManager.ActivateGameOverlay();

        // Set the flag that states that the card was drawn
        cardDrawn = true;

        // Reveal the spell card
        RevealSpell();
    }

    // The method that reveals the spell card that was just drawn
    private void RevealSpell()
    {
        // Set the right sprite to the image target image component
        SpellImages.DisplaySpell(gameObject, spellType);

        // Display the spell image
        DisplaySpellImage();

        // Check if the spell card is on the game board
        if (onBoard)
        {
            // Increase the number of drawn spells that are on the board by one
            SpellCardManager.DrawnSpellsOnBoard++;

            // Display the play spell button
            DisplayPlaySpell();

            // Pause the game
            GameAdvancement.gamePaused = true;
        }

        Debug.Log("The spell card that was drawn was: " + spellType);
    }

    //---------------------------------------------------------------------------------------------------------------
    // Displaying spell on target image
    //---------------------------------------------------------------------------------------------------------------

    // The method that activates the canvas on which the reveal spell button is
    private void DisplaySpellImage()
    {
        // Enable the spell card canvas
        spellCardCanvas.SetActive(true);

        // Enable the draw spell button
        drawSpellButton.gameObject.SetActive(false);

        // Make sure the spell image is disabled
        spellImage.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------------------
    // Playing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // The method that displays the spell card canvas correctly so that the play spell button is enabled
    private void DisplayPlaySpell()
    {
        // Check that the wave is ongoing and the card was already drawn
        if (LevelInfo.waveOngoing == true && cardDrawn == true)
        {
            // Debug.Log("Currently, the wave is ongoing is: " + LevelInfo.waveOngoing + " and the card was drawn is: " + cardDrawn);
            // Enable the spell card canvas game object
            spellCardCanvas.SetActive(true);

            // Debug.Log("Spell card canvas should be active");

            // Enable the play spell button
            playSpellButton.gameObject.SetActive(true);

            // Rename the button accordingly to the current spell
            if (SpellCardManager.CardDeck[spellType] > 0)
            {
                playSpellButton.GetComponentInChildren<TMP_Text>().text = $"Play {spellType} ({SpellCardManager.CardDeck[spellType]})";
            }
            else
            {
                playSpellButton.GetComponentInChildren<TMP_Text>().text = "No Card In Deck";
                // Directly play the spell to reset.
                PlaySpell();
            }
             
            // Disable the draw spell button
            drawSpellButton.gameObject.SetActive(false);
        }
    }

    // Method used to check how many spell cards are currently drawn
    public int DrawnSpellCards()
    {
        // Get all spell cards with tag
        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("Spell Card");

        // Initialize the count
        int count = 0;

        foreach (GameObject card in spellcards)
        {
            if (card.GetComponent<SpellController>().cardDrawn == true)
            {
                // Count one up
                count++;
            }
        }

        return count;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell card helper methods
    //---------------------------------------------------------------------------------------------------------------
    // The method used to make the damage in radius effect take place
    public void ResetSpellCard()
    {
        // Reset the spell card so that it was not drawn and cannot be played
        cardDrawn = false;
        visible = false;
        onBoard = false;

        // Reset the spell type
        spellType = 0;

        // Hide the play spell button
        HideSpellCanvas();
    }

    private SpellType GetSpellTypeWithImageTargetName(string imageTargetName)
    {
        switch (imageTargetName)
        {
            case "Spell3":
                return SpellType.Meteor;
            case "Spell4":
                return SpellType.ArrowRain;
            default:
                return SpellType.ThunderStrike;
        }
    }
}
