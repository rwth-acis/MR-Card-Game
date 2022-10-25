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
    // Define the two corner target images
    public GameObject topLeftCorner;
    public GameObject bottomRightCorner;

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

    public GameObject saveModelObject;

    [SerializeField]
    private Button startNextWave;

    [Tooltip("The real length of the board in meter")]
    [SerializeField]
    private float boardLength;

    [Tooltip("The real height of the board in meter")]
    [SerializeField]
    private float boardWidth;

    [SerializeField]
    private float greatestBoardDimension = 0.03f;

    private readonly float originalXScale = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        Board.boardVisible = false;
        Board.gameBoard = gameBoard;
        Board.gameBoardGroundObject = gameBoardGroundObject;
        Board.gameBoardImageTarget = topLeftCorner;
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
            startNextWave.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Activated when the top left corner image target becomes visible
    /// </summary>
    public void TopLeftBecameVisible()
    {

        // If the other corner is visible, then display the game board
        DisplayGameBoard();

        if(LevelInfo.waveOngoing == false)
        {
            EnableStartNextWave();
        }
    }

    /// <summary>
    /// Activated when the top left corner image target leaves the camera field
    /// </summary>
    public void TopLeftLeftCameraField()
    {
        // Check if the wave is already ongoing
        if(LevelInfo.waveOngoing == false)
        {

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
    /// Set the rotation of the game board to the same rotation as the top left corner
    /// </summary>
    public void SetBoardRotationCorrectly()
    {
        Quaternion cornerRotation = topLeftCorner.transform.rotation;
        Vector3 cornerEulerAngles = cornerRotation.eulerAngles;
        Quaternion boardRotation = gameBoard.transform.rotation;
        // Set the x rotation of the game board to the rotation of the corner
        Vector3 boardEulerAngles = cornerEulerAngles;
        boardRotation.eulerAngles = boardEulerAngles;

        // Set the rotation of the game board correctly
        gameBoard.transform.rotation = boardRotation;
    }

    /// <summary>
    /// Set the position of the game board correctly
    /// </summary>
    public void SetBoardPositionCorrectly()
    {
        Vector3 positionTopLeftCorner = topLeftCorner.transform.position;

        // Set the position of the game board
        gameBoard.transform.position = positionTopLeftCorner + Camera.main.transform.forward * 0.01f;

    }

    public void SetRotationCorrectly()
    {
        // Get the rotation of one of the image targets
        Quaternion cornerRotation = topLeftCorner.transform.rotation;
        Vector3 cornerEulerAngles = cornerRotation.eulerAngles;

        // Get the rotation of the game board 
        Quaternion boardRotation = gameBoard.transform.rotation;
        Vector3 boardEulerAngles = boardRotation.eulerAngles;

        // Set the x rotation of the game board to the rotation of the corner
        boardEulerAngles.x = cornerEulerAngles.x;
        boardRotation.eulerAngles = boardEulerAngles;

        gameBoard.transform.rotation = boardRotation;
    }

    /// <summary>
    /// Reduce or increase a rotation by 180
    /// </summary>
    public float ReverseAngle(float angle)
    {
        if(angle < 180)
        {
            // Case the angle needs to be increased
            angle += 180;
        } else {
            // Case the angle needs to be decreased
            angle -= 180;
        }

        return angle;
    }

    /// <summary>
    /// Returns the rotation that the board should have given the position and rotation of the two board corners
    /// </summary>
    public float GetTheEndRotation(float rotationTLC, float rotationBRC, float xPositionTLC, float xPositionBRC, float zPositionTLC, float zPositionBRC)
    {
        float newAngle = 0;

        if(xPositionTLC >= xPositionBRC)
        {
            if(zPositionTLC >= zPositionBRC)
            {
                // Case TLC has a greater x and z position than BRC, both target images should have an angle between 90 and 180 degrees
                newAngle = ChangeAnglesAndReturnAverage(rotationTLC, rotationBRC, 90, 180);

                if(newAngle == 90)
                {
                    return 270;
                } else {
                    return newAngle;
                }

            } else {

                // Case TLC has a greater x but smaller z position than BRC, both target images should have an angle between 180 and 270 degrees
                newAngle = ChangeAnglesAndReturnAverage(rotationTLC, rotationBRC, 0, 90);

                if(newAngle == 0)
                {
                    return 180;
                } else {
                    return newAngle;
                }
            }
        } else {
            if(zPositionTLC < zPositionBRC)
            {
                // Case TLC has a smaller x and z position than BRC, both target images should have an angle between 270 and 0 degrees
                newAngle = ChangeAnglesAndReturnAverage(rotationTLC, rotationBRC, 270, 360);

                if(newAngle == 270)
                {
                    return 90;
                } else {
                    return newAngle;
                }

            } else {
                // Case TLC has a smaller x and greater z position than BRC, both target images should have an angle between 0 and 90 degrees
                newAngle = ChangeAnglesAndReturnAverage(rotationTLC, rotationBRC, 180, 270);

                if(newAngle == 180)
                {
                    return 0;
                } else {
                    return newAngle;
                }
            }
        }
    }

    /// <summary>
    /// Changes angles such that they are between the two given limits
    /// </summary>
    public float ChangeAnglesAndReturnAverage(float angle1, float angle2, int limit1, int limit2)
    {
        // Rectify the first angle correctly
        while(angle1 > limit2 || angle1 < limit1)
        {
            if(angle1 > limit2)
            {
                angle1 = angle1 - 90;
            } else {
                angle1 = angle1 + 90;
            }
        }
        // Rectify the second angle correctly
        while(angle2 > limit2 || angle2 < limit1)
        {
            if(angle2 > limit2)
            {
                angle2 = angle2 - 90;
            } else {
                angle2 = angle2 + 90;
            }
        }

        // Get the average of both and return it
        return ((angle1 + angle2) / 2);
    }

    /// <summary>
    /// Returns the correct scale vector given the angle in degrees
    /// </summary>
    public Vector3 GetTheCorrectScaleFormAngle(float angleDegree, float xDiff, float zDiff, float intersectionX, float intersectionZ)
    {
        float angle = 0;
        Vector3 scale = new Vector3(0, 0, 0);
        if(angleDegree <= 0 && angleDegree < 90)
        {
            // Convert the angle to radian
            angle = (float)((double)angleDegree * Math.PI / 180);

            if(xDiff != 0){
                // With the board angle, get the right upper scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.x = (float)((double)xDiff / Math.Cos(angle) + Board.overlayLength);

            } else {

                scale.x = intersectionX;
            }
            if(zDiff != 0)
            {
                // With the board angle, get the right side scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.z = (float)((double)zDiff / Math.Sin(Math.PI/2 - angle) + Board.overlayLength);

            } else {

                scale.z = intersectionZ;
            }

        } else if(angleDegree <= 90 && angleDegree < 180)
        {
            // Convert the angle - 90 to radian
            angle = (float)(((double)angleDegree - 90) * Math.PI / 180);

            if(xDiff != 0){
                // With the board angle, get the right upper scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.x = (float)((double)xDiff / Math.Sin(angle) + Board.overlayLength);

            } else {
                
                scale.x = intersectionX;
            }

            if(zDiff != 0)
            {
                // With the board angle, get the right side scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.z = (float)((double)zDiff / Math.Sin(angle) + Board.overlayLength);

            } else {

                scale.z = intersectionZ;
            }

        } else if(angleDegree <= 180 && angleDegree < 270)
        {
            // Convert the angle - 180 to radian
            angle = (float)(((double)angleDegree - 180) * Math.PI / 180);
            if(xDiff != 0){
                // With the board angle, get the right upper scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.x = (float)((double)xDiff / Math.Cos(angle) + Board.overlayLength);

            } else {
                
                scale.x = intersectionX;
            }

            if(zDiff != 0)
            {
                // With the board angle, get the right side scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.z = (float)((double)zDiff / Math.Sin(Math.PI/2 - angle) + Board.overlayLength);

            } else {

                scale.z = intersectionZ;
            }

            Debug.Log("The current angle in radian is: " + angle);
            Debug.Log("The current scale in x direction is: " + scale.x);

        } else {

            // Case the angle is between 270 and 360 degrees
            // Convert the angle - 270 to radian
            angle = (float)(((double)angleDegree - 270) * Math.PI / 180);

            if(xDiff != 0){
                // With the board angle, get the right upper scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.x = (float)((double)xDiff / Math.Cos(angle) + Board.overlayLength);

            } else {
                
                scale.x = intersectionX;
            }

            if(zDiff != 0)
            {
                // With the board angle, get the right side scale of the plane (add the size of the target image to it so that the plane covers the target images)
                scale.z = (float)((double)zDiff / Math.Sin(angle) + Board.overlayLength);

            } else {

                scale.z = intersectionZ;
            }
        }

        scale.y = (float)0.01;
        Debug.Log("Scale is: " + scale.x + " in x direction and " + scale.z + " in z direction.");
        return scale;
    }

    // Method that gives the direction in which the line vector of the border of the board goes to, so that the intersection point can be found
    public Vector3 CreateNewLine1Vector(float angleDegree)
    {
        // Initialize the radian angle variable
        float angleRadian = 0;

        // Check in what case we are
        if(angleDegree <= 0 && angleDegree < 90)
        {
            // Convert the angle to radian
            angleRadian = (float)((double)angleDegree * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)1, (float)0, (float)(-1 * Math.Tan(angleRadian)));

        } else if(angleDegree <= 90 && angleDegree < 180)
        {
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 90) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1 * Math.Tan(angleRadian)), (float)0, (float)(-1));


        } else if(angleDegree <= 180 && angleDegree < 270)
        {
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 180) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1), (float)0, (float)(1 * Math.Tan(angleRadian)));
        } else {
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree -270) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(1 * Math.Tan(angleRadian)), (float)0, (float)1);
        }
    }

    // Method that gives the direction in which the line vector of the border of the board goes to, so that the intersection point can be found
    public Vector3 CreateNewLine2Vector(float angleDegree)
    {
        // Initialize the radian angle variable
        float angleRadian = 0;

        // Check in what case we are
        if(angleDegree <= 0 && angleDegree < 90)
        {
            // Case the angle is between 0 and 90 degrees
            // Convert the angle to radian
            angleRadian = (float)((double)angleDegree * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(float)(1 * Math.Tan(angleRadian)), (float)0, (float)1);

        } else if(angleDegree <= 90 && angleDegree < 180)
        {
            // Case the angle is between 90 and 180 degrees
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 90) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)1, (float)0, (float)(-1 * Math.Tan(angleRadian)));


        } else if(angleDegree <= 180 && angleDegree < 270)
        {
            // Case the angle is between 180 and 270 degrees
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 180) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1 * Math.Tan(angleRadian)), (float)0, (float)(-1));

        } else {

            // Case the angle is between 270 and 360 degrees
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree -270) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1), (float)0, (float)(1 * Math.Tan(angleRadian)));
        }
    }


    // ---------------------------------------------------------------------------------------------------
    // Method used to remove the game board
    // ---------------------------------------------------------------------------------------------------

    // Method that is used to remove the game board
    public void RemoveGameBoard()
    {
        // Set the game board as child of the save model object so that it disapears
        gameBoard.SetActive(false);

        // Set the flag that states that the game board is visible to false
        Board.boardVisible = false;
    }
}
