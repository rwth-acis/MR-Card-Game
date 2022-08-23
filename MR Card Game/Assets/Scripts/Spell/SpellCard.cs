using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// List of spell cards:     Status:     Comments:               Tested:
// - Meteor                 Done        animation done!         working
// - Arrow rain             Done        animation done!         working
// - Thunder strike         Done        animation done!         working
// - Armor                  Done        display is there        working
// - Heal                   Done                                working
// - Obliteration           Done                                working
// - Draw                   Done                                working
// - Telport                Done                                working
// - Space distortion       Done                                working
// - Slow time              Done                                working
// - stop time              Done                                working
// - rain                   Done        double damage working   working

public static class Cards
{
    // The number of free cards that can be drawn without answering questions
    public static int freeDraws = 0;

    // The shuffled cards array
    public static string[] cardDeck;

    // The index of the current card in the card array
    public static int currentCardIndex;

    // The last index of the array
    public static int lastCardIndex;

    // The number of drawn spell cards that are on the board
    public static int drawnSpellsOnBoard = 0;
}

public class SpellCard : MonoBehaviour
{

    // The answer question overlay object
    [SerializeField]
    private GameObject answerQuestions;

    // The canvas game object on which the reveal spell button is 
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
    private Image spellImage;

    [SerializeField]
    private GameObject groundPlane;

    [SerializeField]
    private GameObject spellImageTarget;

    [SerializeField]
    private GameObject gameBoard;

    [SerializeField]
    private GameObject spellPositionIndicator;

    // // The boolean variable that states that the image target is in the camera field
    // private bool cardVisibleButNotDisplayed = false;

    // // The boolean variable that states that the image target with a drawn spell is in the game board field but was not displayed
    // private bool cardDrawnButNotDisplayed = false;

    // The flag that states that the card was drawn
    private bool cardDrawn = false;

    // The flag that states if the card is currently visible
    private bool visible = false;

    //The projected position on ground plane
    private Vector3 projectedPos;

    // Define the currency display button
    [SerializeField]
    private Button currencyDisplay;

    // The method used to access to the currency display button as a static object
    public static Button CurrencyDisplay
    {
        get { return instance.currencyDisplay; }
    }

    // Define the wave display button
    [SerializeField]
    private Button waveDisplay;

    // The boolean variable that states that the image target is on or off the game board
    private bool onBoard = false;

    // The method used to access to the wave display button as a static object
    public static Button WaveDisplay
    {
        get { return instance.waveDisplay; }
    }

    // Define the start next wave button
    [SerializeField]
    private Button startNextWave;

    // The method used to access to the start next wave button as a static object
    public static Button StartNextWave
    {
        get { return instance.startNextWave; }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Debug.Log("Start method was run");

        // Initialize the deck of cards array in the Cards class
        Cards.cardDeck = new string[35];

        // Fill the card deck
        FillCardDeck();

        // Shuffle the card deck
        ShuffleCardDeck(Cards.cardDeck);

        // Set the current card index to 0
        Cards.currentCardIndex = 0;

        // Set the last card index to length - 1
        Cards.lastCardIndex = Cards.cardDeck.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the wave is not ongoing anymore
        if(LevelInfo.waveOngoing == false)
        {
            // Hide the play spell button
            HideSpellCanvas();

        } else {
            // Check if there is at least one drawn spell card on the game board
            if(Cards.drawnSpellsOnBoard > 0 && !cardDrawn)
            {
                // Hide the reveal spell menu
                HideSpellCanvas();
            }

            if (visible)
            {
                projectedPos = ProjectPositionOnGroundPlane();
                spellPositionIndicator.transform.SetParent(groundPlane.transform, true);
                spellPositionIndicator.transform.localPosition = new Vector3(projectedPos.x, 0.005f, projectedPos.z);
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
            if (visible&& !cardDrawn && !GameAdvancement.gamePaused && Questions.numberOfQuestionsNeededToAnswer == 0)
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

                } else {

                    // Display the reveal spell menu
                    DisplaySpellImage();
                }
            }
        }
    }

    // Project the position of the image target GameObject to the ground plane with position.y=0, using similar triangles
    // return the projected position
    private Vector3 ProjectPositionOnGroundPlane()
    {
        Vector3 cameraPos = GetRelativePosition(groundPlane.transform, Camera.main.transform.position);
        Vector3 imageTargetPos = GetRelativePosition(groundPlane.transform, spellImageTarget.transform.position);
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

    //---------------------------------------------------------------------------------------------------------------
    // Helper methods to activate / deactivate the game overlay
    //---------------------------------------------------------------------------------------------------------------

    // Method that activates the components of the game overlay
    public static void ActivateGameOverlay()
    {
        // Activate the currency display button
        CurrencyDisplay.gameObject.SetActive(true);

        // Activate the wave display button
        WaveDisplay.gameObject.SetActive(true);

        // Check if the wave is currently ongoing
        if(LevelInfo.waveOngoing == false || (LevelInfo.numberOfUndefeatedEnemies == 0 && GameAdvancement.currentWave < LevelInfo.numberOfWaves))
        {
            // If it is not the case, activate the start next wave button
            StartNextWave.gameObject.SetActive(true);
        }
    }

    // Method that deactivates the components of the game overlay
    public static void DeactivateGameOverlay()
    {
        // Deactivate the currency display button
        CurrencyDisplay.gameObject.SetActive(false);

        // Deactivate the wave display button
        WaveDisplay.gameObject.SetActive(false);

        // Deactivate the start next wave button
        StartNextWave.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------------------
    // Initializing the deck of cards
    //---------------------------------------------------------------------------------------------------------------

    // Method used to put the right amount of cards in the deck of cards
    private void FillCardDeck()
    {
        // Add five meteor cards
        Cards.cardDeck[0] = "Meteor";
        Cards.cardDeck[1] = "Meteor";
        Cards.cardDeck[2] = "Meteor";
        Cards.cardDeck[3] = "Meteor";
        Cards.cardDeck[4] = "Meteor";

        // Add five arrow rain cards
        Cards.cardDeck[5] = "Arrow rain";
        Cards.cardDeck[6] = "Arrow rain";
        Cards.cardDeck[7] = "Arrow rain";
        Cards.cardDeck[8] = "Arrow rain";
        Cards.cardDeck[9] = "Arrow rain";

        // Add three thunder strike cards
        Cards.cardDeck[10] = "Thunder strike";
        Cards.cardDeck[11] = "Thunder strike";
        Cards.cardDeck[12] = "Thunder strike";
        Cards.cardDeck[13] = "Thunder strike";

        // Add three armor cards
        Cards.cardDeck[14] = "Armor";
        Cards.cardDeck[15] = "Armor";
        Cards.cardDeck[16] = "Armor";
        Cards.cardDeck[17] = "Armor";

        // Add three heal cards
        Cards.cardDeck[18] = "Heal";
        Cards.cardDeck[19] = "Heal";
        Cards.cardDeck[20] = "Heal";
        Cards.cardDeck[21] = "Heal";

        // Add one obliteration cards
        Cards.cardDeck[22] = "Obliteration";

        // Add one draw cards
        Cards.cardDeck[23] = "Draw";

        // Add three teleport cards
        Cards.cardDeck[24] = "Teleport";
        Cards.cardDeck[25] = "Teleport";
        Cards.cardDeck[26] = "Teleport";

        // Add three space distortion cards
        Cards.cardDeck[27] = "Space distortion";
        Cards.cardDeck[28] = "Space distortion";
        Cards.cardDeck[29] = "Space distortion";

        // Add two slow time cards
        Cards.cardDeck[30] = "Slow time";
        Cards.cardDeck[31] = "Slow time";

        // Add one stop time cards
        Cards.cardDeck[32] = "Stop time";

        // Add two rain cards
        Cards.cardDeck[33] = "Rain";
        Cards.cardDeck[34] = "Rain";
    }

    // Initialize random number generator
    private static readonly System.Random random = new System.Random();

    // Method that shuffles the card deck
    public static void ShuffleCardDeck(string[] array)
    {
        // Get the length of the question array
        int length = array.Length;

        // Initialize the swap index
        int swapIndex = 0;

        // Initialize the loop index
        int index = length - 1;

        // Shuffle the card deck
        while(index >= 0)
        {
            // Get a random number
            swapIndex = random.Next(0, index);

            // Copy entry at swapIndex to the entry index of the array
            string value = array[swapIndex];
            array[swapIndex] = array[index];
            array[index] = value;

            // Reduce the index by one
            index = index - 1;
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    // Drawing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // Initialize the spell type
    private string spellType;

    // Method that is activated when the spell image target enters the camera field
    public void SpellCardEnteredCameraField()
    {
        // Set the flag that the card is now visible
        visible = true;

        // Check if that card was already drawn
        if(cardDrawn == true)
        {
            if(onBoard == true)
            {
                // Increase the number of drawn spells on the board by one
                Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard + 1;

                DisplayPlaySpell();

            } else {

                // Display the drawn spell card
                DisplaySpellImage();
            }

        } else {

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false && LevelInfo.waveOngoing == true && Questions.numberOfQuestionsNeededToAnswer == 0)
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
        if(cardDrawn == false && onBoard == true)
        {
            // Decrease the number of drawn spells on the game board by one
            Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard - 1;
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
        // Check if the player has free draws
        if(Cards.freeDraws == 0)
        {
            // Set the question requesting image target correctly
            Questions.questionRequestingImageTarget = this.gameObject;

            // Disable the game overlay
            DeactivateGameOverlay();

            // Enable the answer question menu
            answerQuestions.SetActive(true);

            // Set the number of questions that are needed to answer to 1
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);

            // Make the canvas disappear
            HideSpellCanvas();

            // Start the routine that waits for the questions to be answered
            StartCoroutine(DrawSpell());

        } else {

            // Reduce the number of free draws by one
            Cards.freeDraws = Cards.freeDraws - 1;

            // Set the spell type
            spellType = Cards.cardDeck[Cards.currentCardIndex];

            // increase the current card index
            IncreaseCurrentCardIndex();

            // Set the flag that states that the card was drawn
            cardDrawn = true;

            // Reveal the spell card
            RevealSpell();
        }
    }

    // Method used to increase the current card index
    private void IncreaseCurrentCardIndex()
    {
        // The current card index needs to be changed, check if the end of the array was reached
        if(Cards.currentCardIndex < Cards.lastCardIndex)
        {
            // Increase the current card index by one
            Cards.currentCardIndex = Cards.currentCardIndex + 1;

        } else {

            // Shuffle the card deck
            ShuffleCardDeck(Cards.cardDeck);

            // Set the current card index to 0
            Cards.currentCardIndex = 0;
        }
    }

    // Function that is used to test when all questions that were needed to be answered were answered correctly
    private bool NoMoreQuestionsNeeded()
    {
        return Questions.numberOfQuestionsNeededToAnswer == 0;
    }

    // The method that builds an archer tower over the image target
    IEnumerator DrawSpell()
    {
        // Wait until the number of questions that need to be answered is 0
        yield return new WaitUntil(NoMoreQuestionsNeeded);

        // Set the spell type
        spellType = Cards.cardDeck[Cards.currentCardIndex];

        // Increase the current card index
        IncreaseCurrentCardIndex();

        // Enable the game overlay
        ActivateGameOverlay();

        // Set the flag that states that the card was drawn
        cardDrawn = true;

        // Reveal the spell card
        RevealSpell();
    }

    // The method that reveals the spell card that was just drawn
    private void RevealSpell()
    {
        // Set the right sprite to the image target image component
        SpellImages.DisplaySpell(this.gameObject, spellType);

        // Display the spell image
        DisplaySpellImage();

        // Check if the spell card is on the game board
        if(onBoard == true)
        {
            // Increase the number of drawn spells that are on the board by one
            Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard + 1;

            // Display the play spell button
            DisplayPlaySpell();

            // Debug.Log("Trying to display the spell");

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

    // Initialize the last position variable
    private Vector3 lastPosition;

    // Initialize the current position variable
    private Vector3 currentPosition;

    // Initialize the time the card is immobile on the board
    private float timeCardImmobile;

    // The time the card should be immobile on the game board before the spell is launched
    [SerializeField]
    private float timeBeforeSpellLaunch;

    // The method used to detect that the image target entered the game board space
/*    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.CompareTag("Board"))
        {
            // Set the variable that states that the spell card is on the board to true
            onGameBoard = true;

            // Check if the wave is ongoing
            if(LevelInfo.waveOngoing == true)
            {
                if(cardDrawn == true)
                {
                    // Debug.Log("Entered the display spell if statement");
                    // Pause the game
                    GameAdvancement.gamePaused = true;

                    // Increase the number of drawn spells that are on the board by one
                    Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard + 1;

                    // Display the play spell button
                    DisplayPlaySpell();
                }

            }
        }
    }

    // The method used to detect that the image target left the game board space
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            // Set the variable that states that the spell card is on the board to false
            onGameBoard = false;

            // Check if the card was drawn
            if(cardDrawn == true)
            {
                // Decrease the number of drawn spells that are on the board by one
                Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard - 1;
            }

            // Check if the number of drawn spell cards that are on the board is 0
            if(Cards.drawnSpellsOnBoard == 0)
            {
                // Un-pause the game
                GameAdvancement.gamePaused = false;
            }            
        }
    }*/

    // The method that displays the spell card canvas correctly so that the play spell button is enabled
    private void DisplayPlaySpell()
    {
        // Check that the wave is ongoing and the card was already drawn
        if(LevelInfo.waveOngoing == true && cardDrawn == true)
        {
            // Debug.Log("Currently, the wave is ongoing is: " + LevelInfo.waveOngoing + " and the card was drawn is: " + cardDrawn);
            // Enable the spell card canvas game object
            spellCardCanvas.SetActive(true);

            // Debug.Log("Spell card canvas should be active");

            // Enable the play spell button
            playSpellButton.gameObject.SetActive(true);

            // Rename the button accordingly to the current spell
            playSpellButton.GetComponentInChildren<TMP_Text>().text = "Play " + spellType;

            // Disable the draw spell button
            drawSpellButton.gameObject.SetActive(false);
        }
    }

    // The method that displays the spell card canvas correctly so that the play spell button is enabled
    public void PlaySpell()
    {
        // Depending on the type of card that is next in the card deck, make the right overlay appear and set the spell type variable to the right type
        switch(spellType)
        {
            case "Meteor":
                PlayMeteor(this.gameObject);
            break;

            case "Arrow rain":
                PlayArrowRain(this.gameObject);
            break;

            case "Thunder strike":
                PlayThunderStrike(this.gameObject);
            break;

            case "Armor":
                PlayArmor();
            break;

            case "Heal":
                PlayHeal();
            break;

            case "Obliteration":
                PlayObliteration();
            break;

            case "Draw":
                PlayDraw();
            break;

            case "Teleport":
                PlayTeleport(this.gameObject);
            break;

            case "Space distortion":
                PlaySpaceDistortion(this.gameObject);
            break;

            case "Slow time":
                PlaySlowTime();
            break;

            case "Stop time":
                PlayStopTime();
            break;

            case "Rain":
                PlayRain();
            break;
        }

        // Decrease the number of drawn spells that are on the board by one
        Cards.drawnSpellsOnBoard = Cards.drawnSpellsOnBoard - 1;

        Debug.Log("The number of drawn card spells that are on the board is: " + Cards.drawnSpellsOnBoard);

        // Make sure a new spell card can be drawn on this card
        DisplayDrawSpell();

        // Lower the flag that states that the card was drawn
        cardDrawn = false;

        // Check if the number of drawn spell cards that are on the board is 0
        if(Cards.drawnSpellsOnBoard <= 0)
        {
            Debug.Log("The game is being unpaused after a spell card has been played");

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }

        // Check if no spell card is drawn
        if(DrawnSpellCards() == 0)
        {
            // Unpause the game
            GameAdvancement.gamePaused = false;
        }
    }

    // Method used to check how many spell cards are currently drawn
    public int DrawnSpellCards()
    {
        // Get all spell cards with tag
        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("Spell Card");

        // Initialize the count
        int count = 0;

        foreach(GameObject card in spellcards)
        {
            if(card.GetComponent<SpellCard>().cardDrawn == true)
            {
                // Count one up
                count = count + 1;
            }
        }

        return count;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell cards effect
    //---------------------------------------------------------------------------------------------------------------

    // The instance of this class to access the static value of certain variables
    public static SpellCard instance;

    // The meteor radius
    [SerializeField]
    private float meteorRadius;

    // The meteor damage
    [SerializeField]
    private int meteorDamage;

    // The arrow rain radius
    [SerializeField]
    private float arrowRainRadius;

    // The arrow rain damage
    [SerializeField]
    private int arrowRainDamage;

    // The teleport radius
    [SerializeField]
    private float teleportRadius;

    // The space distortion radius
    [SerializeField]
    private float spaceDistortionRadius;

    // The space distortion radius
    [SerializeField]
    private float spaceDistortionDuration;

    // The space distortion factor
    [SerializeField]
    private float spaceDistortionFactor;

    // The duration the time should be stopped when the stop time card is played
    [SerializeField]
    private float stopTimeDuration;

    // The static variable used by other classes to access the stop time duration
    public static float getStopTimeDuration
    {
        get { return instance.stopTimeDuration; }
    }

    // The duration the time should be slowed when the slow time card is played
    [SerializeField]
    private float slowTimeDuration;

    // The factor by which the time should be slowed when the slow time card is played
    [SerializeField]
    private float slowTimeFactor;

    // The duration the rain should last when the rain card is played
    [SerializeField]
    private float rainDuration;

    // The factor by which the time should be slowed when the rain card is played
    [SerializeField]
    private float rainFactor;

    // The method used to make the meteor spell card take effect
    private void PlayMeteor(GameObject spellCard)
    {
        // Calculate the position
        positionObject.transform.position = spellCard.transform.position;
        positionObject.transform.parent = Board.gameBoard.transform;

        // Make sure the position is on the game board
        Vector3 newPosition = positionObject.transform.localPosition;
        newPosition.y = 0.1f;
        positionObject.transform.localPosition = newPosition;

        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlayDamageInRadiusSpell(positionObject, meteorRadius, meteorDamage);

        // Make the meteor animation
        StartCoroutine(ActivateMeteorAnimation());
    }

    IEnumerator ActivateMeteorAnimation()
    {
        // Make the arrow rain prefab appear at the position of the image target
        GameObject meteorImpact = SpawnSpellEffect.SpawnAMeteorImpact();

        // Set the position of the spell effect to the position of the image target
        meteorImpact.transform.position = positionObject.transform.position;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of one seconds
        while(timeWaited <= 1)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Release the arrow rain object
        ObjectPools.ReleaseSpellEffect(meteorImpact, "Meteor Impact");
    }

    [SerializeField]
    private GameObject positionObject;

    // The method used to make the arrow rain spell card take effect
    private void PlayArrowRain(GameObject spellCard)
    {
        // Calculate the position
        positionObject.transform.position = spellCard.transform.position;
        positionObject.transform.parent = Board.gameBoard.transform;

        // Make sure the position is on the game board
        Vector3 newPosition = positionObject.transform.localPosition;
        newPosition.y = 0.1f;
        positionObject.transform.localPosition = newPosition;

        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlayDamageInRadiusSpell(positionObject, arrowRainRadius, arrowRainDamage);

        // Make the arrow rain animation
        StartCoroutine(ActivateArrowRainAnimation());
    }

    IEnumerator ActivateArrowRainAnimation()
    {
        // Make the arrow rain prefab appear at the position of the image target
        GameObject arrowRain = SpawnSpellEffect.SpawnAnArrowRain();

        // Set the position of the spell effect to the position of the image target
        arrowRain.transform.position = positionObject.transform.position;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of one second
        while(timeWaited <= 1)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Release the arrow rain object
        ObjectPools.ReleaseSpellEffect(arrowRain, "Arrow Rain");
    }

    // The method used to make the thunder strike spell card take effect
    private void PlayThunderStrike(GameObject spellCard)
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Initialize the closest distance variable
            float closestDistance = 1000;

            // Initialize the distance variable
            float distance = 0;

            // Initialize the closest enemy variable
            GameObject closestEnemy = null;

            // Find the enemy that is the closest to the spell card
            foreach(GameObject enemy in enemies)
            {
                // Calculate the distance between the spell cards location and this enemy
                distance = Vector3.Distance(spellCard.transform.position, enemy.transform.position);

                // Check if this distance is smaller than the current smallest enemy
                if(distance < closestDistance)
                {
                    // Set the closest distance to this distance
                    closestDistance = distance;

                    // Set the closest enemy to this enemy
                    closestEnemy = enemy;
                    
                }
            }
            // Debug.Log("The enemies were not null");

            // Kill this enemy by making it take more damage than the maximum number of health points that exist
            closestEnemy.GetComponent<Enemy>().TakeDamage(1000);

            // Make the arrow rain animation
            StartCoroutine(ActivateThunderStrikeAnimation(closestEnemy));
        }
    }

    // The coroutine that spawns the thunder strike animation and returns it to the object pool
    IEnumerator ActivateThunderStrikeAnimation(GameObject enemy)
    {
        // Make the thunder strike prefab appear at the position of the image target
        GameObject thunderStrike = SpawnSpellEffect.SpawnAThunderStrike();

        // Set the position of the spell effect to the position of the image target
        thunderStrike.transform.position = enemy.transform.position;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of 0.2 seconds
        while(timeWaited <= 0.2f)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Release the thunder strike object
        ObjectPools.ReleaseSpellEffect(thunderStrike, "Thunder Strike");
    }

    // The method used to make the armor spell card take effect
    private void PlayArmor()
    {
        // Add three armor points to the castle armor
        GameAdvancement.castleCurrentAP = GameAdvancement.castleCurrentAP + 3;

        // Actualize the castle health points (and armor points)
        GameSetup.ActualizeCastleHealthPoints();
    }

    // The method used to make the heal spell card take effect
    private void PlayHeal()
    {
        // Check if the plus five health points heal would exceed the castle's maximum health points
        if(GameAdvancement.castlecurrentHP + 5 > GameAdvancement.castleMaxHP)
        {
            // If yes, set the castle current health points to the castle max health points
            GameAdvancement.castlecurrentHP = GameAdvancement.castleMaxHP;

        } else {

            // If not, add five health points to the castle current health points
            GameAdvancement.castlecurrentHP = GameAdvancement.castlecurrentHP + 5;
        }

        // Actualize the castle health points
        GameSetup.ActualizeCastleHealthPoints();
    }

    // The method used to make the obliteration spell card take effect
    private void PlayObliteration()
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Go over all enemies
            foreach(GameObject enemy in enemies)
            {
                // Kill this enemy by making it take more damage than the maximum number of health points that exist
                enemy.GetComponent<Enemy>().TakeDamage(1000);
            }
        }

        // Reduce the castle maximum health points by 30%
        GameAdvancement.castleMaxHP = (int)((float)GameAdvancement.castleMaxHP * (float)0.7);

        // Check that the castle current health points are not exceeding the castle maximum health points
        if(GameAdvancement.castlecurrentHP > GameAdvancement.castleMaxHP)
        {
            // Set the castle current health points to the castle maximun health points
            GameAdvancement.castlecurrentHP = GameAdvancement.castleMaxHP;
        }

        // Actualize the castle health points
        GameSetup.ActualizeCastleHealthPoints();
    }

    // The method used to make the draw card take effect
    private void PlayDraw()
    {
        // Increase the number of free card draws by three
        Cards.freeDraws = Cards.freeDraws + 3;
    }

    // The method used to make the teleport spell card take effect
    private void PlayTeleport(GameObject spellCard)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float greatestDistance = teleportRadius * Board.greatestBoardDimension;

        // Initialize the distance variable
        //float distance = 0;

        // Initialize the closest enemy variable
        List<GameObject> enemiesInRange = EnemiesInRange(spellCard, greatestDistance);

        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            // Go through all enemies in the list
            foreach(GameObject enemy in enemiesInRange)
            {
                // Set the position of the enemy to the position of the enemy spawn
                enemy.transform.position = Waypoints.enemySpawn.transform.position;

                // Set the waypoint index of the enemy to 0
                enemy.GetComponent<Enemy>().waypointIndex = 0;
            }
        }
    }

    // The method used to make the space distortion card take effect
    private void PlaySpaceDistortion(GameObject spellCard)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float closestDistance = spaceDistortionRadius * Board.greatestBoardDimension;

        // Initialize the distance variable
        //float distance = 0;

        // Initialize the closest enemy variable
        List<GameObject> enemiesInRange = EnemiesInRange(spellCard, closestDistance);


        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            StartCoroutine(SlowEnemies(enemiesInRange));
        }
    }

    // Coroutine that makes the space distortion spell card take effect
    IEnumerator SlowEnemies(List<GameObject> enemies)
    {
        // Go through all enemies in the list
        foreach(GameObject enemy in enemies)
        {
            // Set the personal slow factor of the enemies to the space distortion factor
            enemy.GetComponent<Enemy>().personalSlowFactor = spaceDistortionFactor;
        }

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= spaceDistortionDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Go through all enemies in the list
        foreach(GameObject enemy in enemies)
        {
            // Reset the personal slow factor of the enemies
            enemy.GetComponent<Enemy>().personalSlowFactor = 1;
        }
    }

    // The method used to make the slow time spell card take effect
    private void PlaySlowTime()
    {
        StartCoroutine(SlowTime());
    }

    // Coroutine that makes the slow time spell card take effect
    IEnumerator SlowTime()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = slowTimeFactor;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= slowTimeDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Wait for the duration of the slow time card
        yield return new WaitForSeconds(slowTimeDuration);

        // Remove the slow time effect
        GameAdvancement.globalSlow = 1;
    }

    // The method used to make the stop time spell card take effect
    private void PlayStopTime()
    {
        StartCoroutine(StopTime());
    }

    // Coroutine that makes the stop time spell card take effect
    IEnumerator StopTime()
    {
        // Stop time
        GameAdvancement.timeStopped = true;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= stopTimeDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Remove the stop time
        GameAdvancement.timeStopped = false;
    }

    // The method used to make the rain card take effect
    private void PlayRain()
    {
        StartCoroutine(Rain());
    }

    // Coroutine that makes the rain spell card take effect
    IEnumerator Rain()
    {
        // Slow enemies down
        GameAdvancement.globalSlow = rainFactor;

        // Set the raining flag to true
        GameAdvancement.raining = true;

        // Initialize the time waited variable
        float timeWaited = 0;

        // Wait until the time waited equals the duration of the slow time card
        while(timeWaited <= rainDuration)
        {
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Check that the game is not paused
            if(GameAdvancement.gamePaused == false)
            {
                // Increase the time waited by 0.1 seconds
                timeWaited = timeWaited + 0.1f;
            }
        }

        // Remove the slow effect
        GameAdvancement.globalSlow = 1;

        // Set the raining flag to false
        GameAdvancement.raining = false;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell card helper methods
    //---------------------------------------------------------------------------------------------------------------

    // The method used to get the list of objects in a certain range of a game object
    private List<GameObject> EnemiesInRange(GameObject spellCard, float radius)
    {
        // Initialize and fill the enemies array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check that there are enemies on the board
        if(enemies != null)
        {
            // Initialize the maximum distance where the meteor damage should still happen
            float closestDistance = teleportRadius * Board.greatestBoardDimension;

            // Initialize the distance variable
            float distance = 0;

            // Initialize the closest enemy variable
            List<GameObject> enemiesInRange = new List<GameObject>();

            // Find the enemy that is the closest to the spell card
            foreach(GameObject enemy in enemies)
            {
                // Calculate the distance between the spell cards location and this enemy
                distance = Vector3.Distance(spellCard.transform.position, enemy.transform.position);

                // Check if this distance is smaller than the current smallest enemy
                if(distance < closestDistance)
                {
                    // Add this enemy to the enemies in range array
                    enemiesInRange.Add(enemy);              
                }
            }

            // Return the list of the enemies in range
            return enemiesInRange;

        } else {

            // Return nul if there are no enemies in range
            return null;
        }
    }

    // The method used to make the damage in radius effect take place
    private void PlayDamageInRadiusSpell(GameObject spellCard, float radius, int damage)
    {
        // Initialize the maximum distance where the meteor damage should still happen
        float closestDistance = radius * Board.greatestBoardDimension;

        // Initialize the distance variable
        // float distance = 0;

        // Initialize the closest enemy variable
        List<GameObject> enemiesInRange = EnemiesInRange(spellCard, closestDistance);

        // Check that the enemies in range list is not empty
        if(enemiesInRange != null)
        {
            // Go through all enemies in the list
            foreach(GameObject enemy in enemiesInRange)
            {
                // Damage this enemy by the meteor damage
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
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
        spellType = "";

        // Hide the play spell button
        HideSpellCanvas();
    }

    // The method used to reset the spell card deck
    public static void ResetSpellCardDeck()
    {
        // Reset the number of free draws
        Cards.freeDraws = 0;

        // Set the current card index to 0
        Cards.currentCardIndex = 0;

        // Set the number of drawn spells on the board to 0
        Cards.drawnSpellsOnBoard = 0;

        // Shuffle the card deck
        ShuffleCardDeck(Cards.cardDeck);
    }
}
