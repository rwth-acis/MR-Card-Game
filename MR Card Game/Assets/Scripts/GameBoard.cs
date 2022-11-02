using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// The class for flags of the anchor points of the game board
static class Board
{
    /// <summary>
    /// the additional length that the board can be over the target images
    /// </summary>
    public static float overlayLength = 0.1f;

    /// <summary>
    /// How much is the board scaled regarding its original scale on X dimension.
    /// </summary>
    public static float boardScalingFactor;

    /// <summary>
    /// The gameboard object
    /// </summary>
    public static GameObject gameBoard;

    public static GameObject gameBoardGroundObject;

    public static GameObject gameBoardImageTarget;

    /// <summary>
    /// The castle game object
    /// </summary>
    public static GameObject castle;

    /// <summary>
    /// The game object contain all towers (child of the game board)
    /// </summary>
    public static GameObject buildingStorage;

    /// <summary>
    /// the camera so that UI elements on units can be oriented towards it
    /// </summary>
    public static Transform camera;

    /// <summary>
    /// if the game board is visible or not
    /// </summary>
    public static bool boardVisible;

    public static bool activateGameBoard;

    /// <summary>
    /// The width of the board in meter.
    /// </summary>
    public static float boardWidth;

    /// <summary>
    /// The length of the board in meter.
    /// </summary>
    public static float boardLength;
}

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject gameBoard;

    [SerializeField]
    private GameObject gameBoardGroundObject;

    [SerializeField]
    private GameObject castle;

    [SerializeField]
    private Transform arCamera;

    [SerializeField]
    private GameObject buildingStorage;

    [SerializeField]
    private GameObject saveModelObject;

    [SerializeField]
    private Button startNextWave;

    [Tooltip("The real length of the board in meter")]
    [SerializeField]
    private float boardLength;

    [Tooltip("The real height of the board in meter")]
    [SerializeField]
    private float boardWidth;

    // Start is called before the first frame update
    void Start()
    {
        Board.boardVisible = false;
        Board.gameBoard = gameBoard;
        Board.gameBoardGroundObject = gameBoardGroundObject;
        Board.camera = arCamera;
        Board.buildingStorage = buildingStorage;
        Board.castle = castle;
        Board.boardLength = boardLength;
        Board.boardWidth = boardWidth;
    }

    /// <summary>
    /// Enable the start next wave button while the game board is in view
    /// </summary>
    public void EnableStartNextWave()
    {
        // Check if the wave is currently ongoing
        if(LevelInfo.waveOngoing == false)
        {
            startNextWave.interactable = true;
        }
    }

    /// <summary>
    /// Activated when the top left corner image target becomes visible
    /// </summary>
    public void GameBoardPlaced()
    {

        // If the other corner is visible, then display the game board
        DisplayGameBoard();

        if(LevelInfo.waveOngoing == false)
        {
            EnableStartNextWave();
        }
    }

    /// <summary>
    /// ground the game board on wave begin (so that the board cannot dissapear and stays in place)
    /// </summary>
    public static void GroundGameBoard()
    {
        Board.gameBoard.transform.parent = Board.gameBoardGroundObject.transform;
    }

    /// <summary>
    /// Ground the game board on wave begin (so that the board cannot dissapear and stays in place)
    /// </summary>
    public static void UngroundGameBoard()
    {
        Board.gameBoard.transform.parent = Board.gameBoardImageTarget.transform;
    }

    /// <summary>
    /// Display the game board
    /// </summary>
    public void DisplayGameBoard()
    {
        gameBoard.SetActive(true);
        Board.boardVisible = true;
    }

    public void SetBoardScalingCorrectly()
    {
        // Set the board to a proper scale, a little bit magic
        gameBoard.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
    }

    /// <summary>
    /// Remove the game board
    /// </summary>
    public void RemoveGameBoard()
    {
        // Set the game board as child of the save model object so that it disapears
        gameBoard.SetActive(false);

        // Set the flag that states that the game board is visible to false
        Board.boardVisible = false;
    }
}
