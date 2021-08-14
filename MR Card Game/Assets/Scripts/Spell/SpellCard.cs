using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}

public class SpellCard : MonoBehaviour
{

    // The boolean variable that states that the image target is on or off the game board
    private bool onGameBoard = false;

    // The game overlay object
    [SerializeField]
    private GameObject gameOverlay;

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

    // The boolean variable that states that the image target is in the camera field
    private bool cardVisibleButNotDisplayed = false;

    // The boolean variable that states that the image target with a drawn spell is in the game board field but was not displayed
    private bool cardDrawnButNotDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

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
            // Check if the spell card was already drawn
            if(spellType != "")
            {
                // Hide the play spell button
                HideSpellCanvas();

                // Set the variable that states that the spell card is visible but the overlay not displayed to true
                cardDrawnButNotDisplayed = true;

            } else {

                // Hide the spell card canvas
                HideSpellCanvas();

                // Set the variable that states that the spell card is visible but the overlay not displayed to true
                cardVisibleButNotDisplayed = true;
            }

        } else {

            // Check if the reveal spell card overlay is not displayed while the image target is in the camera field and the game is not paused
            if(cardVisibleButNotDisplayed == true && GameAdvancement.gamePaused == false)
            {
                // Set the variable that states that the spell card is visible but the overlay not displayed to false
                cardVisibleButNotDisplayed = false;

                // Display the reveal spell menu
                DisplayDrawSpell();
            }

            // Check if the reveal spell card overlay is not displayed while the image target is in the camera field and the game is not paused
            if(cardDrawnButNotDisplayed == true && onGameBoard == true)
            {
                // Set the variable that states that the spell card is visible but the overlay not displayed to false
                cardDrawnButNotDisplayed = false;

                // Display the reveal spell menu
                DisplayPlaySpell();
            }
        }
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

        // Add three armor cards
        Cards.cardDeck[13] = "Armor";
        Cards.cardDeck[14] = "Armor";
        Cards.cardDeck[15] = "Armor";

        // Add three heal cards
        Cards.cardDeck[16] = "Heal";
        Cards.cardDeck[17] = "Heal";
        Cards.cardDeck[18] = "Heal";

        // Add three plague cards
        Cards.cardDeck[19] = "Plague";
        Cards.cardDeck[20] = "Plague";
        Cards.cardDeck[21] = "Plague";

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
    private readonly System.Random random = new System.Random();

    // Method that shuffles the card deck
    private void ShuffleCardDeck(string[] array)
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
        // Check that the game is not paused
        if(GameAdvancement.gamePaused == false && LevelInfo.waveOngoing == true)
        {
            // Display the reveal spell menu
            DisplayDrawSpell();

        } else {

            // Set the variable that the spell card is visible but the overlay not displayed to true
            cardVisibleButNotDisplayed = true;
        }
    }

    // Method that is activated when the spell image target leaves the camera field
    public void SpellCardLeftCameraField()
    {
        // Hide the reveal spell menu
        HideSpellCanvas();
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
    }

    // The method that deactivates the canvas on which the reveal spell button is
    private void HideSpellCanvas()
    {
        spellCardCanvas.SetActive(false);
    }

    // The method that is activated when the user clicks on the draw spell button
    public void InitiateDrawSpell()
    {
        if(Cards.freeDraws == 0)
        {
            // Disable the game overlay
            gameOverlay.SetActive(false);

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

            // Reveal the spell card
            RevealSpell();
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

        // Enable the game overlay
        gameOverlay.SetActive(true);

        // Reveal the spell card
        RevealSpell();
    }

    // The method that reveals the spell card that was just drawn
    private void RevealSpell()
    {
        // Depending on the type of card that is next in the card deck, make the right overlay appear and set the spell type variable to the right type
        switch(Cards.cardDeck[Cards.currentCardIndex])
        {
            case "Meteor":
                MakeMeteorCardAppear();
                spellType = "meteor";
            break;

            case "Arrow rain":
                MakeArrowRainCardAppear();
                spellType = "arrow rain";
            break;

            case "Thunder strike":
                MakeThunderStrikeCardAppear();
                spellType = "thunder strike";
            break;

            case "Armor":
                MakeArmorCardAppear();
                spellType = "armor";
            break;

            case "Heal":
                MakeHealCardAppear();
                spellType = "heal";
            break;

            case "Plague":
                MakePlagueCardAppear();
                spellType = "plague";
            break;

            case "Obliteration":
                MakeObliterationCardAppear();
                spellType = "obliteration";
            break;

            case "Draw":
                MakeDrawCardAppear();
                spellType = "draw";
            break;

            case "Teleport":
                MakeTeleportCardAppear();
                spellType = "teleport";
            break;

            case "Space distortion":
                MakeSpaceDistortionCardAppear();
                spellType = "space distortion";
            break;

            case "Slow time":
                MakeSlowTimeCardAppear();
                spellType = "slow time";
            break;

            case "Stop time":
                MakeStopTimeCardAppear();
                spellType = "stop time";
            break;

            case "Rain":
                MakeRainCardAppear();
                spellType = "rain";
            break;
        }

        // Check if the spell card is on the game board
        if(onGameBoard == true)
        {
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

    // Method used to make the meteor spell card appearance appear on the image target
    private void MakeMeteorCardAppear()
    {
        //
    }

    // Method used to make the arrow rain spell card appearance appear on the image target
    private void MakeArrowRainCardAppear()
    {
        //
    }

    // Method used to make the thunder strike spell card appearance appear on the image target
    private void MakeThunderStrikeCardAppear()
    {
        //
    }

    // Method used to make the armor spell card appearance appear on the image target
    private void MakeArmorCardAppear()
    {
        //
    }

    // Method used to make the heal spell card appearance appear on the image target
    private void MakeHealCardAppear()
    {
        //
    }

    // Method used to make the plague spell card appearance appear on the image target
    private void MakePlagueCardAppear()
    {
        //
    }

    // Method used to make the obliteration spell card appearance appear on the image target
    private void MakeObliterationCardAppear()
    {
        //
    }

    // Method used to make the draw spell card appearance appear on the image target
    private void MakeDrawCardAppear()
    {
        //
    }

    // Method used to make the teleport spell card appearance appear on the image target
    private void MakeTeleportCardAppear()
    {
        //
    }

    // Method used to make the space distortion spell card appearance appear on the image target
    private void MakeSpaceDistortionCardAppear()
    {
        //
    }

    // Method used to make the slow time spell card appearance appear on the image target
    private void MakeSlowTimeCardAppear()
    {
        //
    }

    // Method used to make the stop time spell card appearance appear on the image target
    private void MakeStopTimeCardAppear()
    {
        //
    }

    // Method used to make the rain spell card appearance appear on the image target
    private void MakeRainCardAppear()
    {
        //
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
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.tag == "Board")
        {
            // Set the variable that states that the spell card is on the board to true
            onGameBoard = true;

            // Check if the wave is ongoing
            if(LevelInfo.waveOngoing == true)
            {
                // Pause the game
                GameAdvancement.gamePaused = true;

                // Display the play spell button
                DisplayPlaySpell();

            } else {

                // Set the flag that the card is drawn but not displayed
                cardDrawnButNotDisplayed = true;
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

            // Un-pause the game
            GameAdvancement.gamePaused = false;
        }
    }

    // The method that displays the spell card canvas correctly so that the play spell button is enabled
    private void DisplayPlaySpell()
    {
        // Enable the spell card canvas game object
        spellCardCanvas.SetActive(true);

        // Enable the play spell button
        playSpellButton.gameObject.SetActive(true);

        // Rename the button accordingly to the current spell
        playSpellButton.GetComponentInChildren<TMP_Text>().text = "Play " + spellType;

        // Disable the draw spell button
        drawSpellButton.gameObject.SetActive(false);
    }

    // The method that displays the spell card canvas correctly so that the play spell button is enabled
    public void PlaySpell()
    {
        // Un-pause the game
        GameAdvancement.gamePaused = false;

        // Depending on the type of card that is next in the card deck, make the right overlay appear and set the spell type variable to the right type
        switch(Cards.cardDeck[Cards.currentCardIndex])
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

            case "Plague":
                PlayPlague();
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

        // Make sure a new spell card can be drawn on this card
        DisplayDrawSpell();
    }

    // Method that wait the appropriate time before launching the spell
    IEnumerator PrepareForLaunchingSpell()
    {
        // Initialize the time of wait variable
        float waitTime = 0.5f;

        // Initialize the flag that states if the spell can be launched or not
        bool canLaunchSpell = false;

        // Set the last position to the position of the card
        lastPosition = this.gameObject.transform.position;

        // Wait for some time
        yield return new WaitForSeconds(waitTime);

        // Set the current position to the position of the card
        currentPosition = this.gameObject.transform.position;

        // As long as the spell card is on the board, this method continues to try to launch the spell
        while(onGameBoard == true && canLaunchSpell == false)
        {
            // Check if the current and last position are the same
            if(lastPosition == currentPosition)
            {
                // If yes then add the wait time to the time the card was immobile
               timeCardImmobile = timeCardImmobile + waitTime;

            } else {

                // Reset the wait time
                timeCardImmobile = 0;
            }

            // Check if the time the card was immobile exceeds the time it must be immobile before the spell can be launched
            if(timeCardImmobile >= timeBeforeSpellLaunch)
            {
                // Set the can launch spell to true
                canLaunchSpell = true;
            }
        }

        // Depending on the type of card that is next in the card deck, make the right overlay appear and set the spell type variable to the right type
        switch(Cards.cardDeck[Cards.currentCardIndex])
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

            case "Plague":
                PlayPlague();
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
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlayDamageInRadiusSpell(spellCard, meteorRadius, meteorDamage);
    }

    // The method used to make the arrow rain spell card take effect
    private void PlayArrowRain(GameObject spellCard)
    {
        // Make the damage in radius take effect in the meteor radius and with the meteor damage
        PlayDamageInRadiusSpell(spellCard, arrowRainRadius, arrowRainDamage);
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
            Debug.Log("The enemies were not null");

            // Kill this enemy by making it take more damage than the maximum number of health points that exist
            closestEnemy.GetComponent<Enemy>().TakeDamage(1000);
        }
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

    // The method used to make the plague card take effect
    private void PlayPlague()
    {
        // Get a random number
        int newCategoryIndex = random.Next(0, 8);

        // Depending on the random number, set a category of enemy as plagued
        switch(newCategoryIndex)
        {
            case 0:
                // Set the type of plagued enemy to normal enemy
                GameAdvancement.plaguedEnemyType = "Normal Enemy";
            break;

            case 1:
                // Set the type of plagued enemy to fast enemy
                GameAdvancement.plaguedEnemyType = "Fast Enemy";
            break;

            case 2:
                // Set the type of plagued enemy to super fast enemy
                GameAdvancement.plaguedEnemyType = "Super Fast Enemy";
            break;

            case 3:
                // Set the type of plagued enemy to flying enemy
                GameAdvancement.plaguedEnemyType = "Flying Enemy";
            break;

            case 4:
                // Set the type of plagued enemy to tank enemy
                GameAdvancement.plaguedEnemyType = "Tank Enemy";
            break;

            case 5:
                // Set the type of plagued enemy to slow enemy
                GameAdvancement.plaguedEnemyType = "Slow Enemy";
            break;

            case 6:
                // Set the type of plagued enemy to berzerker enemy
                GameAdvancement.plaguedEnemyType = "Berzerker Enemy";
            break;

            case 7:
                // Set the type of plagued enemy to berzerkerflying enemy
                GameAdvancement.plaguedEnemyType = "Berzerker Flying Enemy";
            break;

            case 8:
                // Set the type of plagued enemy to berzerker tank enemy
                GameAdvancement.plaguedEnemyType = "Berzerker Tank Enemy";
            break;
        }
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

        // Check if the castle maximum health points is over 10
        if(GameAdvancement.castleMaxHP <= 10)
        {
            // Set the castle maximum health points to 0
            GameAdvancement.castleMaxHP = 0;

        } else {

            // Make the castle maximum health points lose 5 points
            GameAdvancement.castleMaxHP = GameAdvancement.castleMaxHP - 10;
        }

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
        float distance = 0;

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
        float distance = 0;

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

        // Wait for the duration of the space distortion
        yield return new WaitForSeconds(spaceDistortionDuration);

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

        // Wait for the duration of the stop time
        yield return new WaitForSeconds(stopTimeDuration);

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

        // Wait for the duration of the stop time
        yield return new WaitForSeconds(rainDuration);

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
        float distance = 0;

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
}