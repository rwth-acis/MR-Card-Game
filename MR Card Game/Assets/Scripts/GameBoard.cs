using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// The class for flags of the anchor points of the game board
static class Anchors
{
    // The truth value of if the top left corner is visible at the moment or not
    public static bool topLeftCornerVisible = false;

    // The truth value of if the bottm right corner is visible at the moment or not
    public static bool bottomRightCornerVisible = false;

    public static Vector3 intersection = new Vector3(0, 0, 0);
}

public class GameBoard : MonoBehaviour
{
    // Define the two corner target images
    public GameObject topLeftCorner;
    public GameObject bottomRightCorner;

    // Define the game board object
    public GameObject gameBoard;

    // Define the model storage object to make models disapear
    public GameObject saveModelObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that is activated when the top left corner image target becomes visible
    public void TopLeftBecameVisible()
    {
        // Set the flag that the top left corner is in view
        Anchors.topLeftCornerVisible = true;

        // Check if the other corner is visible
        if(Anchors.bottomRightCornerVisible == true)
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
        Anchors.bottomRightCornerVisible = true;

        // Check if the other corner is visible
        if(Anchors.topLeftCornerVisible == true)
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
        Anchors.topLeftCornerVisible = false;

        // Remove the game board
        RemoveGameBoard();

        Debug.Log("Top left corner image target left the camera field!");
    }

    // Method that is activated when the bottom right corner image target leaves the camera field
    public void BottomRightLeftCameraField()
    {
        // Set the flag that the bottom right corner is not in view anymore
        Anchors.bottomRightCornerVisible = false;

        // Remove the game board
        RemoveGameBoard();

        Debug.Log("Bottom right corner image target left the camera field!");
    }

    // Method that is used to display the game board
    public void DisplayGameBoard()
    {
        // Get the position of the top left corner target image
        Vector3 positionTopLeftCorner = topLeftCorner.transform.position;

        // Get the position of the bottom right corner target image
        Vector3 positionBottomRightCorner = bottomRightCorner.transform.position;

        // Get the rotation of the top left corner target image
        Vector3 rotationVectorTopLeftCorner = topLeftCorner.transform.rotation.eulerAngles;
        
        // Get the rotation of the bottom right corner target image
        Vector3 rotationVectorBottomRightCorner = bottomRightCorner.transform.rotation.eulerAngles;

        float angleDifference = 0;

        // Check if one target image is in the wrong direction, by checking it the anlge gets smaller if one number is reversed
        // Check which angle is greater
        if(rotationVectorTopLeftCorner.y >= rotationVectorBottomRightCorner.y)
        {
            angleDifference = rotationVectorTopLeftCorner.y - rotationVectorBottomRightCorner.y;
        } else {
            angleDifference = rotationVectorBottomRightCorner.y - rotationVectorTopLeftCorner.y;
        }

        // Check if the angle difference is greater than 90 degrees, if yes one of the rotations has to be turned around
        if(angleDifference > 90)
        {
            // Reverse one of the rotations
            if(positionTopLeftCorner.x >= positionBottomRightCorner.x)
            {
                if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
                {
                    // Case the top left corner has a greater x and z possition than the bottom left corner
                    if(0 <= rotationVectorTopLeftCorner.y && rotationVectorTopLeftCorner.y<= 90)
                    {
                        // The rotation of the top left corner is correct, reverse the other
                        rotationVectorBottomRightCorner.y = ReverseAngle(rotationVectorBottomRightCorner.y);
                    } else {
                        // The rotation of the top left corner is incorrect, reverse it
                        rotationVectorTopLeftCorner.y = ReverseAngle(rotationVectorTopLeftCorner.y);
                    }

                } else {
                    // Case the top left corner has a greater x position but smaller z possition than the bottom left corner
                    if(90 <= rotationVectorTopLeftCorner.y && rotationVectorTopLeftCorner.y <= 180)
                    {
                        // The rotation of the top left corner is correct, reverse the other
                        rotationVectorBottomRightCorner.y = ReverseAngle(rotationVectorBottomRightCorner.y);
                    } else {
                        // The rotation of the top left corner is incorrect, reverse it
                        rotationVectorTopLeftCorner.y = ReverseAngle(rotationVectorTopLeftCorner.y);
                    }

                }
            } else {
                if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
                {
                    // Case the top left corner has a smaller x but greather z possition than the bottom left corner
                    if(180 <= rotationVectorTopLeftCorner.y && rotationVectorTopLeftCorner.y <= 270)
                    {
                        // The rotation of the top left corner is correct, reverse the other
                        rotationVectorBottomRightCorner.y = ReverseAngle(rotationVectorBottomRightCorner.y);
                    } else {
                        // The rotation of the top left corner is incorrect, reverse it
                        rotationVectorTopLeftCorner.y = ReverseAngle(rotationVectorTopLeftCorner.y);
                    }

                } else {
                    // Case the top left corner has a smaller x and z possition than the bottom left corner
                    if(270 <= rotationVectorTopLeftCorner.y && rotationVectorTopLeftCorner.y <= 360)
                    {
                        // The rotation of the top left corner is correct, reverse the other
                        rotationVectorBottomRightCorner.y = ReverseAngle(rotationVectorBottomRightCorner.y);
                    } else {
                        // The rotation of the top left corner is incorrect, reverse it
                        rotationVectorTopLeftCorner.y = ReverseAngle(rotationVectorTopLeftCorner.y);
                    }
                }
            }
        }
        // Angles are now correct, get the angle in the middle of the two as approximation
        float boardAngle = (rotationVectorTopLeftCorner.y + rotationVectorBottomRightCorner.y) / 2;

        // Get the rotation vector
        Vector3 rotationVectorBoard = new Vector3(0, boardAngle, 0);

        // Set the rotation of the board
        gameBoard.transform.rotation = Quaternion.Euler(rotationVectorBoard);

        // Resize the board using the angle



        // Set the game board as child of the top left corner
        gameBoard.transform.parent = topLeftCorner.transform;

        // Initialize the scale vector
        Vector3 scale = new Vector3(1, 1, 1);

        // Initialize the position vector
        Vector3 position = positionTopLeftCorner;

        float xDifference = 0;
        float zDifference = 0;

        // Find the distance in x direction between the corners
        if(positionTopLeftCorner.x >= positionBottomRightCorner.x)
        {
            // Case the top left corner has a greater x position than the bottom left corner
            xDifference = positionTopLeftCorner.x - positionBottomRightCorner.x;

        } else {

            // Case the top left corner has a smaller x position than the bottom left corner
            xDifference = positionBottomRightCorner.x - positionTopLeftCorner.x;
        }

        // Find the distance in z direction between the corners
        if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        {
            // Case the top left corner has a greater z position than the bottom left corner
            zDifference = positionTopLeftCorner.z - positionBottomRightCorner.z;

        } else {

            // Case the top left corner has a smaller z position than the bottom left corner
            zDifference = positionBottomRightCorner.z - positionTopLeftCorner.z;
        }

        // Get the length of the diagonal between the two corners
        float diagonalLength = (float) Math.Sqrt((double)(xDifference * xDifference + zDifference * zDifference));

        // Second point BUT here there is still an Y component, need to destroy it
        Vector3 secondPointLine1 = new Vector3(1 + positionTopLeftCorner.x, 0, (float) (1 * Math.Tan(boardAngle)) + positionTopLeftCorner.z);
        Vector3 secondPointLine2 = new Vector3(positionBottomRightCorner.x - 1, 0, positionBottomRightCorner.z - (float) (1 * Math.Tan(90 - boardAngle)));

        Vector3 intersection;

        // if(LineLineIntersection(out intersection, positionTopLeftCorner, secondPointLine1, positionBottomRightCorner, secondPointLine2))
        // {
        //     // Here we get the intersection
        // }

        // -------------------------------------------------------------------


        // If the x scaling is negative, revert this
        if(scale.x < 0)
        {
            scale.x = - scale.x;
        }

        // Add the size of the target image to it so that the target image is covered
        scale.x = scale.x + (float)0.152;

         // Find the distance in z direction between the corners
        if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        {
            // Case the top left corner has a greater z position than the bottom left corner
            scale.z = positionTopLeftCorner.z - positionBottomRightCorner.z;

            // Change the position vector accordingly
            position = position - new Vector3(0, 0, scale.z/2);
            
        } else {

            // Case the top left corner has a smaller z position than the bottom left corner
            scale.z = positionBottomRightCorner.z - positionTopLeftCorner.z;

            // Change the position vector accordingly
            position = position + new Vector3(0, 0, scale.z/2);

        }

        // If the z scaling is negative, revert this
        if(scale.z < 0)
        {
            scale.z = - scale.z;
        }

        // Add the size of the target image to it so that the target image is covered
        scale.z = scale.z + (float)0.152;

        // Scale the game board correctly, a plane is 10 units large and wide, so divide by 10
        gameBoard.transform.localScale = (scale / 10);

        // Set the position of the game board between the two corner markers
        gameBoard.transform.position = position + new Vector3(0, (float)0.002, 0);


        // Make sure the mesh renderer is enabled, or the game board could disapear
        gameBoard.GetComponent<Renderer>().enabled = true;

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

    // // Method that finds the point in space where two straight lines meet given the coordinate of two points and an angle
    // public void FindWhereTwoLinesMeet(float pos1X, float pos1Z, float pos2X, float pos2Z, float angle)
    // {
    //     // Create the ray for the first position and angle
    //     Ray ray = new Ray();
    //     ray.origin = new Vector3(pos1X, 0, Pos1Z);
    //     ray.direction = new Vector3(0, angle, 0);

    //     // Create the plan for the second position and anlge + 90
    //     Plane plane = new Plane();
    //     plane.normal = new Vector3();

    // }

    // Method that is used to remove the game board
    public void RemoveGameBoard()
    {
        // Set the game board as child of the save model object so that it disapears
        gameBoard.transform.parent = saveModelObject.transform;
    }
}
