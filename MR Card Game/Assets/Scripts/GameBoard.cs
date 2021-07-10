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

        float xDifferenceTargetImages = 0;
        float zDifferenceTargetImages = 0;

        // Find the distance in x direction between the corners
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

        // Find the distance in z direction between the corners
        if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        {
            // Case the top left corner has a greater z position than the bottom left corner
            zDifferenceTargetImages = positionTopLeftCorner.z - positionBottomRightCorner.z;

            // Set the z position to the center of the diagonal of the two markers
            position.z = position.z - xDifferenceTargetImages / 2;

        } else {

            // Case the top left corner has a smaller z position than the bottom left corner
            zDifferenceTargetImages = positionBottomRightCorner.z - positionTopLeftCorner.z;

            // Set the z position to the center of the diagonal of the two markers
            position.z = position.z + xDifferenceTargetImages / 2;
        }

        // // Get the length of the diagonal between the two corners
        // float diagonalLength = (float) Math.Sqrt((double)(xDifferenceTargetImages * xDifferenceTargetImages + zDifferenceTargetImages * zDifferenceTargetImages));

        // Create the right position vectors in the 0-y-plane
        Vector3 firstPointLine1 = new Vector3(positionTopLeftCorner.x, 0, positionTopLeftCorner.z);
        Vector3 firstPointLine2 = new Vector3(positionBottomRightCorner.x, 0, positionBottomRightCorner.z);

        // The direction vectors going out the position vectors
        Vector3 secondPointLine1 = new Vector3(1, 0, (float) (1 * Math.Tan(boardAngle)));
        Vector3 secondPointLine2 = new Vector3(-1, 0, (float) (-1 * Math.Tan(90 - boardAngle)));

        // Initialize the intersection vector
        Vector3 intersection;

        if(Math3d.LineLineIntersection(out intersection, firstPointLine1, secondPointLine1, firstPointLine2, secondPointLine2))
        {
            // Initialize the x and z difference
            float xDiff = 0;
            float zDiff = 0;

            // Get the right x difference
            if(positionTopLeftCorner.x >= intersection.x)
            {
                xDiff = positionTopLeftCorner.x - intersection.x;

            } else {

                xDiff = intersection.x - positionTopLeftCorner.x;
            }

            // Get the right z difference
            if(positionBottomRightCorner.z >= intersection.z)
            {
                zDiff = positionBottomRightCorner.z - intersection.z;

            } else {

                zDiff = intersection.z - positionBottomRightCorner.z;
            }

            // With the board angle, get the right upper scale of the plane (add the size of the target image to it so that the plane covers the target images)
            scale.x = xDiff / (float)Math.Cos(boardAngle) + (float)0.15;

            // With the board angle, get the right side scale of the plane (add the size of the target image to it so that the plane covers the target images)
            scale.z = zDiff / (float)Math.Sin(90 - boardAngle) + (float)0.15;

            // Since the ratio should be 2:1 get the smalles scale and scale the plane accordingly
            if(scale.x < scale.z * 2)
            {
                // Case the z scale is too big, scale it down
                scale.z = scale.x  / 2;

            } else {

                // Case the x scale is too big, scale it down
                scale.x = scale.z  * 2;
            }

            // Scale the board correctly, a plane is 10 units big so divide the scale by 10
            gameBoard.transform.localScale = (scale / 10);

            // Make sure the mesh renderer is enabled, or the game board could disapear
            gameBoard.GetComponent<Renderer>().enabled = true;

            // Set the position of the game board between the two corner markers and set it a bit higher so that it is not covered by the target images
            gameBoard.transform.position = position + new Vector3(0, (float)0.002, 0);

        } else {

            // If there was no intersection, print it in the log
            Debug.Log("There was no intersection");
        }

        // -------------------------------------------------------------------


        // // If the x scaling is negative, revert this
        // if(scale.x < 0)
        // {
        //     scale.x = - scale.x;
        // }

        // // Add the size of the target image to it so that the target image is covered
        // scale.x = scale.x + (float)0.152;

        //  // Find the distance in z direction between the corners
        // if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        // {
        //     // Case the top left corner has a greater z position than the bottom left corner
        //     scale.z = positionTopLeftCorner.z - positionBottomRightCorner.z;

        //     // Change the position vector accordingly
        //     position = position - new Vector3(0, 0, scale.z/2);
            
        // } else {

        //     // Case the top left corner has a smaller z position than the bottom left corner
        //     scale.z = positionBottomRightCorner.z - positionTopLeftCorner.z;

        //     // Change the position vector accordingly
        //     position = position + new Vector3(0, 0, scale.z/2);

        // }

        // // If the z scaling is negative, revert this
        // if(scale.z < 0)
        // {
        //     scale.z = - scale.z;
        // }

        // // Add the size of the target image to it so that the target image is covered
        // scale.z = scale.z + (float)0.152;

        // // Scale the game board correctly, a plane is 10 units large and wide, so divide by 10
        // gameBoard.transform.localScale = (scale / 10);

        // // Set the position of the game board between the two corner markers
        // gameBoard.transform.position = position + new Vector3(0, (float)0.002, 0);


        // // Make sure the mesh renderer is enabled, or the game board could disapear
        // gameBoard.GetComponent<Renderer>().enabled = true;

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
