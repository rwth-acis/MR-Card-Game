using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// The class for flags of the anchor points of the game board
static class Board
{
    // The truth value of if the top left corner is visible at the moment or not
    public static bool topLeftCornerVisible = false;

    // The truth value of if the bottm right corner is visible at the moment or not
    public static bool bottomRightCornerVisible = false;

    // Define the additional length that the board can be over the target images
    public static float overlayLength = (float)0.1;

    public static float boardAngle;

    // Define the dimension of the longer side of the board
    public static float greatestBoardDimension;

    // Define the height of the board
    public static float boardHeight;

    // The gameboard object
    public static GameObject gameBoard;

    // The castle game object
    public static GameObject castle;

    // The game object that should contain all towers (child of the game board)
    public static GameObject buildingStorage;

    // Define the camera so that UI elements on units can be oriented towards it
    public static Transform camera;
}

public class GameBoard : MonoBehaviour
{
    // Define the two corner target images
    public GameObject topLeftCorner;
    public GameObject bottomRightCorner;

    // Define the game board object
    [SerializeField]
    private GameObject gameBoard;

    // Define the castle object
    [SerializeField]
    private GameObject castle;

    // Define the camera object
    [SerializeField]
    private Transform camera;

    // Define the building storage object
    [SerializeField]
    private GameObject buildingStorage;

    // Define the model storage object to make models disapear
    public GameObject saveModelObject;

    // public static GameObject GetGameBoard()
    // {
    //     return gameBoard;
    // }

    // Start is called before the first frame update
    void Start()
    {
        // Set the game board of the static class Board to the given game board object
        Board.gameBoard = gameBoard;

        // Set the camera of the static class Board to the given camera object
        Board.camera = camera;

        // Set the building storage of the static class Board to the given building storage object
        Board.buildingStorage = buildingStorage;

        // Set the castle of the static class Board to the given castle object
        Board.castle = castle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that is activated when the top left corner image target becomes visible
    public void TopLeftBecameVisible()
    {
        // Set the flag that the top left corner is in view
        Board.topLeftCornerVisible = true;

        // Check if the other corner is visible
        if(Board.bottomRightCornerVisible == true)
        {
            // If the other corner is visible, then display the game board
            DisplayGameBoard();
        }

        Debug.Log("Top left corner image target has entered the camera field!");
    }

    // Method that is activated when the bottom right corner image target becomes visible
    public void BottomRightBecameVisible()
    {
        // Set the flag that the bottom right corner is in view
        Board.bottomRightCornerVisible = true;

        // Check if the other corner is visible
        if(Board.topLeftCornerVisible == true)
        {
            // If the other corner is visible, then display the game board
            DisplayGameBoard();
        }

        Debug.Log("Bottom right corner image target has entered the camera field!");
    }

    // Method that is activated when the top left corner image target leaves the camera field
    public void TopLeftLeftCameraField()
    {
        // Set the flag that the top left corner is not in view anymore
        Board.topLeftCornerVisible = false;

        // Remove the game board
        RemoveGameBoard();

        Debug.Log("Top left corner image target left the camera field!");
    }

    // Method that is activated when the bottom right corner image target leaves the camera field
    public void BottomRightLeftCameraField()
    {
        // Set the flag that the bottom right corner is not in view anymore
        Board.bottomRightCornerVisible = false;

        // Remove the game board
        RemoveGameBoard();

        Debug.Log("Bottom right corner image target left the camera field!");
    }

    // ---------------------------------------------------------------------------------------------------
    // Method used to display the game board
    // ---------------------------------------------------------------------------------------------------

    // Method that is used to display the game board
    public void DisplayGameBoard()
    {
        // Set the game board active
        gameBoard.SetActive(true);
        
        // Get the positions and rotations of the two corners
        Vector3 positionTopLeftCorner = topLeftCorner.transform.position;
        Vector3 positionBottomRightCorner = bottomRightCorner.transform.position;
        Vector3 rotationVectorTopLeftCorner = topLeftCorner.transform.rotation.eulerAngles;
        Vector3 rotationVectorBottomRightCorner = bottomRightCorner.transform.rotation.eulerAngles;

        // Get the end rotation. This is important in case that one or two corners are in the wrong direction
        float boardAngle = GetTheEndRotation(rotationVectorTopLeftCorner.y, rotationVectorBottomRightCorner.y, positionTopLeftCorner.x, positionBottomRightCorner.x, positionTopLeftCorner.z, positionBottomRightCorner.z);
        
        // Set the board angle in the Board class
        Board.boardAngle = boardAngle;

        // Get the rotation vector
        Vector3 rotationVectorBoard = new Vector3(0, boardAngle, 0);

        // Set the rotation of the board
        gameBoard.transform.rotation = Quaternion.Euler(rotationVectorBoard);

        // Set the game board as child of the top left corner
        gameBoard.transform.parent = topLeftCorner.transform;

        // Resize the board using the angle

        // Initialize the position vector
        Vector3 position = positionTopLeftCorner;

        // Initialize the x and z variables of the differences in position of the two corners
        float xDifferenceTargetImages = 0;
        float zDifferenceTargetImages = 0;

        // Find the distance in x difference between the corners and set the position of the board in the middle of the two corners
        if(positionTopLeftCorner.x >= positionBottomRightCorner.x)
        {
            // Case the top left corner has a greater x position than the bottom left corner
            xDifferenceTargetImages = positionTopLeftCorner.x - positionBottomRightCorner.x;

            // Set the x position to the center of the diagonal of the two markers
            position.x = position.x - xDifferenceTargetImages / 2;

        } else {

            // Case the top left corner has a smaller x position than the bottom left corner
            xDifferenceTargetImages = positionBottomRightCorner.x - positionTopLeftCorner.x;

            // Set the x position to the center of the diagonal of the two markers
            position.x = position.x + xDifferenceTargetImages / 2;
        }

        // Find the distance in z difference between the corners and set the position of the board in the middle of the two corners
        if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        {
            // Case the top left corner has a greater z position than the bottom left corner
            zDifferenceTargetImages = positionTopLeftCorner.z - positionBottomRightCorner.z;

            // Set the z position to the center of the diagonal of the two markers
            position.z = position.z - zDifferenceTargetImages / 2;

        } else {

            // Case the top left corner has a smaller z position than the bottom left corner
            zDifferenceTargetImages = positionBottomRightCorner.z - positionTopLeftCorner.z;

            // Set the z position to the center of the diagonal of the two markers
            position.z = position.z + zDifferenceTargetImages / 2;
        }

        // Set the y position to the maximum value of the y position of the two corners
        if(positionTopLeftCorner.y < positionBottomRightCorner.y)
        {
            position.y = positionBottomRightCorner.y - positionTopLeftCorner.y;

        }
        //  else {

        //     position.y = positionBottomRightCorner.y;
        // }

        // Set the board height variable correctly
        Board.boardHeight = position.y;

        // Create the right position vectors in the 0-y-plane
        Vector3 firstPointLine1 = new Vector3(positionTopLeftCorner.x, 0, positionTopLeftCorner.z);
        Vector3 firstPointLine2 = new Vector3(positionBottomRightCorner.x, 0, positionBottomRightCorner.z);

        // The direction vectors going out the of the position vectors
        Vector3 secondPointLine1 = CreateNewLine1Vector(boardAngle);
        Vector3 secondPointLine2 = CreateNewLine2Vector(boardAngle);

        // Initialize the intersection vector
        Vector3 intersection = new Vector3(0, 0, 0);

        if(Math3d.LineLineIntersection(out intersection, firstPointLine1, secondPointLine1, firstPointLine2, secondPointLine2))
        {
            // Initialize the x and z difference
            float xDiffTopLeft = 0;
            float zDiffBottomRight = 0;

            // Get the right x difference between the top left corner and the intersection
            if(positionTopLeftCorner.x >= intersection.x)
            {
                xDiffTopLeft = positionTopLeftCorner.x - intersection.x;

            } else {

                xDiffTopLeft = intersection.x - positionTopLeftCorner.x;
            }

            // Get the right z difference between the bottom right corner and the intersection
            if(positionBottomRightCorner.z >= intersection.z)
            {
                zDiffBottomRight = positionBottomRightCorner.z - intersection.z;

            } else {

                zDiffBottomRight = intersection.z - positionBottomRightCorner.z;
            }

            // Depending on the anle, resize the board correctly
            Vector3 correctScale = GetTheCorrectScaleFormAngle(boardAngle, xDiffTopLeft, zDiffBottomRight, intersection.x, intersection.z);

            // Since the ratio should be 1:1 get the smalles scale and scale the plane accordingly
            if(correctScale.x < correctScale.z)
            {
                // Case the z scale is too big, scale it down
                correctScale.z = correctScale.x;

            } else {

                // Case the x scale is too big, scale it down
                correctScale.x = correctScale.z;
            }

            // Set the y scale to the x scale so that the height of things is correct
            correctScale.y = correctScale.x;

            Board.greatestBoardDimension = correctScale.x / 10;

            // Scale the board correctly, a plane is 10 units big so divide the scale by 10
            gameBoard.transform.localScale = (correctScale / 10);

            // // Make sure the mesh renderer is enabled, or the game board could disapear // This was necessary before I change the board model
            // gameBoard.GetComponent<Renderer>().enabled = true;

            // Set the position of the game board between the two corner markers and set it a bit higher so that it is not covered by the target images
            gameBoard.transform.position = position + new Vector3(0, (float)0.002 + Board.greatestBoardDimension * (float)1, 0);

            Debug.Log("The board height is: " + Board.boardHeight);

        } else {

            // If there was no intersection, print it in the log
            Debug.Log("There was no intersection");
        }
    }

    // Helper method used to reduce or increase a rotation by 180
    public float ReverseAngle(float angle)
    {
        // Check if the angle is smaller or greater 180
        if(angle < 180)
        {
            // Case the angle needs to be increased
            angle = angle + 180;
        } else {
            // Case the angle needs to be decreased
            angle = angle - 180;
        }

        return angle;
    }

    // Method that returns the rotation that the board should have given the position and rotation of the two board corners
    public float GetTheEndRotation(float rotationTLC, float rotationBRC, float xPositionTLC, float xPositionBRC, float zPositionTLC, float zPositionBRC)
    {
        // Initialize the new angle variable
        float newAngle = 0;

        // Check in what case we are
        if(xPositionTLC >= xPositionBRC)
        {
            if(zPositionTLC >= zPositionBRC)
            {
                // Case TLC has a greater x and z position than BRC, both target images should have an angle between 90 and 180 degrees
                newAngle = ChangeAnglesAndReturnAverage(rotationTLC, rotationBRC, 90, 180); // correct?

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
            //
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

    // Method that changes angles such that they are between the two given limits
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

    // Method that returns the correct scale vector given the angle in degrees
    public Vector3 GetTheCorrectScaleFormAngle(float angleDegree, float xDiff, float zDiff, float intersectionX, float intersectionZ)
    {
        // Initialize an angle variale
        float angle = 0;

        // Initialize the scale vector
        Vector3 scale = new Vector3(0, 0, 0);

        // Check in what case we are
        if(angleDegree <= 0 && angleDegree < 90)
        {
            // Case the angle is between 0 and 90 degrees
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
            // Case the angle is between 90 and 180 degrees
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
            // Case the angle is between 180 and 270 degrees
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

        // Return the scale
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
            // Case the angle is between 0 and 90 degrees
            // Convert the angle to radian
            angleRadian = (float)((double)angleDegree * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)1, (float)0, (float)(-1 * Math.Tan(angleRadian)));

        } else if(angleDegree <= 90 && angleDegree < 180)
        {
            // Case the angle is between 90 and 180 degrees
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 90) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1 * Math.Tan(angleRadian)), (float)0, (float)(-1));


        } else if(angleDegree <= 180 && angleDegree < 270)
        {
            // Case the angle is between 180 and 270 degrees
            // Convert the angle to radian
            angleRadian = (float)(((double)angleDegree - 180) * Math.PI / 180);

            // Return the right direction vector
            return new Vector3((float)(-1), (float)0, (float)(1 * Math.Tan(angleRadian)));


        } else {

            // Case the angle is between 270 and 360 degrees
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
        // gameBoard.transform.parent = saveModelObject.transform;
        gameBoard.SetActive(false);
    }
}
