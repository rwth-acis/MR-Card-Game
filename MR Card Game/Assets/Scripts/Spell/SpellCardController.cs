using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SpellCardController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    private GameObject spellCardCanvas;

    [SerializeField]
    private Button drawSpellButton;

    [SerializeField]
    private Button playSpellButton;

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

    /// <summary>
    /// If the card is drawn
    /// </summary>
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
            HideSpellCanvas();
        }
        else
        {
            // Check if there is at least one drawn spell card on the game board
            if (SpellCardManager.DrawnSpellsOnBoard > 0 && !cardDrawn)
            {
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
                DisplayDrawSpell();
            }
            // Make drawn spells appear if the wave is ongoing
            if (visible && cardDrawn)
            {
                if (onBoard)
                {
                    DisplayPlaySpell();
                }
                else
                {
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

    /// <summary>
    /// Activated when the spell image target enters the camera field
    /// </summary>
    /// <param name="imageTargetBehaviour">The ImageTargetBehaviour script, should be passed in the inspector</param>
    public void SpellCardEnteredCameraField(ImageTargetBehaviour imageTargetBehaviour)
    {
        Debug.Log(imageTargetBehaviour.TargetName);
        spellType = GetSpellTypeWithImageTargetName(imageTargetBehaviour.TargetName);
        visible = true;
        // Check if that card was already drawn
        if (cardDrawn)
        {
            if (onBoard)
            {
                SpellCardManager.DrawnSpellsOnBoard++;
                DisplayPlaySpell();
            }
            else
            {
                DisplaySpellImage();
            }
        }
        else
        {
            // Check that the game is not paused
            if (GameAdvancement.gamePaused == false && LevelInfo.waveOngoing == true && Questions.numberOfQuestionsNeededToAnswer == 0)
            {
                DisplayDrawSpell();
            }
        }
    }

    /// <summary>
    /// Activated when the spell image target leaves the camera field
    /// </summary>
    public void SpellCardLeftCameraField()
    {
        visible = false;
        HideSpellCanvas();
        if (cardDrawn && onBoard)
        {
            SpellCardManager.DrawnSpellsOnBoard--;
        }
    }

    // Display the canvas on which the draw spell button is
    private void DisplayDrawSpell()
    {
        spellCardCanvas.SetActive(true);
        drawSpellButton.gameObject.SetActive(true);
        playSpellButton.gameObject.SetActive(false);
        spellImage.gameObject.SetActive(false);
    }

    // Deactivates the canvas on which the draw spell button is
    private void HideSpellCanvas()
    {
        spellCardCanvas.SetActive(false);
    }

    /// <summary>
    /// Activated when click on the draw spell button
    /// </summary>
    public void InitiateDrawSpell()
    {
        GameAdvancement.gamePaused = true;
        // Check if the player has free draws
        if (SpellCardManager.FreeDraws == 0)
        {
            Questions.questionRequestingImageTarget = gameObject;
            GameSceneManager.DeactivateGameOverlay();
            SpellCardManager.AnswerQuestions.SetActive(true);
            ActivateQuestions.IncreaseNumberOfQuestionsThatNeedToBeAnswered(1);
            HideSpellCanvas();
            StartCoroutine(DrawSpell());
        }
        else
        {
            // Reduce the number of free draws by one
            SpellCardManager.FreeDraws--;
            cardDrawn = true;
            RevealSpell();
        }
    }

    // The method that builds an archer tower over the image target
    IEnumerator DrawSpell()
    {
        // Wait until no questions left
        yield return new WaitUntil(GameSceneManager.NoMoreQuestionsNeeded);
        GameSceneManager.ActivateGameOverlay();
        cardDrawn = true;
        RevealSpell();
    }

    // Reveals the spell card
    private void RevealSpell()
    {
        // Set the right sprite to the image target image component
        SpellImages.DisplaySpell(gameObject, spellType);
        DisplaySpellImage();
        // Check if the spell card is on the game board
        if (onBoard)
        {
            SpellCardManager.DrawnSpellsOnBoard++;
            DisplayPlaySpell();
            GameAdvancement.gamePaused = true;
        }
        Debug.Log("The spell card that was drawn was: " + spellType);
    }

    //---------------------------------------------------------------------------------------------------------------
    // Displaying spell on target image
    //---------------------------------------------------------------------------------------------------------------

    // Show the spell image after revealing the spell
    private void DisplaySpellImage()
    {
        spellCardCanvas.SetActive(true);
        drawSpellButton.gameObject.SetActive(false);
        spellImage.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------------------
    // Playing spell cards
    //---------------------------------------------------------------------------------------------------------------

    // Displays the spell card canvas correctly so that the play spell button is enabled
    private void DisplayPlaySpell()
    {
        // Check that the wave is ongoing and the card was already drawn
        if (LevelInfo.waveOngoing == true && cardDrawn == true)
        {
            spellCardCanvas.SetActive(true);
            playSpellButton.gameObject.SetActive(true);
            // Check if there are still this type of card in card deck
            if (SpellCardManager.CardDeck[spellType] > 0)
            {
                playSpellButton.GetComponentInChildren<TMP_Text>().text = $"Play {spellType} ({SpellCardManager.CardDeck[spellType]})";
            }
            else
            {
                playSpellButton.GetComponentInChildren<TMP_Text>().text = "No Card In Deck";
                PlaySpell();
            }
            drawSpellButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Check how many spell cards are currently drawn
    /// </summary>
    public int DrawnSpellCards()
    {
        // Get all spell cards with tag
        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("Spell Card");
        int count = 0;
        foreach (GameObject card in spellcards)
        {
            if (card.GetComponent<SpellCardController>().cardDrawn == true)
            {
                count++;
            }
        }
        return count;
    }

    //---------------------------------------------------------------------------------------------------------------
    // The spell card helper methods
    //---------------------------------------------------------------------------------------------------------------

    public void ResetSpellCard()
    {
        cardDrawn = false;
        visible = false;
        onBoard = false;
        spellType = 0;
        HideSpellCanvas();
    }

    private SpellType GetSpellTypeWithImageTargetName(string imageTargetName)
    {
        return imageTargetName switch
        {
            "Draw" => SpellType.Draw,
            "Armor" => SpellType.Armor,
            "ArrowRain" => SpellType.ArrowRain,
            "Healing" => SpellType.Healing,
            "Meteor" => SpellType.Meteor,
            "Obliteration" => SpellType.Obliteration,
            "Rain" => SpellType.Rain,
            "SlowTime" => SpellType.SlowTime,
            "SpaceDistortion" => SpellType.SpaceDistortion,
            "StopTime" => SpellType.StopTime,
            "ThunderStrike" => SpellType.ThunderStrike,
            "Teleport" => SpellType.Teleport,
            _ => SpellType.ArrowRain
        };
    }
}
