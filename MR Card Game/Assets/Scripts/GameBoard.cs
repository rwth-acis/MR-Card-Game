using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class for flags of the anchor points of the game board
static class Anchors
{
    // The truth value of if the top left corner is visible at the moment or not
    public static bool topLeftCornerVisible = false;

    // The truth value of if the bottm right corner is visible at the moment or not
    public static bool bottomRightCornerVisible = false;
}

public class GameBoard : MonoBehaviour
{
    // Define the two corner target images
    public GameObject topLeftCorner;
    public GameObject bottomRightCorner;

    // Define the game board object
    public GameObject gameBoard;

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
    }

    // Method that is activated when the top left corner image target leaves the camera field
    public void TopLeftLeftCameraField()
    {
        // Set the flag that the top left corner is not in view anymore
        Anchors.topLeftCornerVisible = false;

        // Remove the game board
        RemoveGameBoard();
    }

    // Method that is activated when the bottom right corner image target leaves the camera field
    public void BottomRightLeftCameraField()
    {
        // Set the flag that the bottom right corner is not in view anymore
        Anchors.bottomRightCornerVisible = true;

        // Remove the game board
        RemoveGameBoard();
    }

    // Method that is used to display the game board
    public void DisplayGameBoard()
    {
        // Get the position of the top left corner target image
        Vector3 positionTopLeftCorner = topLeftCorner.transform.position;

        // Get the position the bottom right corner target image
        Vector3 positionBottomRightCorner = bottomRightCorner.transform.position;

        // Set the game board as child of the top left corner
        gameBoard.transform.parent = topLeftCorner.transform;

        // Define the scale vector
        Vector3 scale = new Vector3(1, 1, 1);

        // Initialize the position vector
        Vector3 position = positionTopLeftCorner;

        // Find the distance in x direction between the corners
        if(positionTopLeftCorner.x >= positionBottomRightCorner.x)
        {
            // Case the top left corner has a greater x position than the bottom left corner
            scale.x = positionTopLeftCorner.x - positionBottomRightCorner.x;

            // Change the position vector accordingly
            position = position - new Vector3(scale.x, 0, 0);

        } else {

            // Case the top left corner has a smaller x position than the bottom left corner
            scale.x = positionBottomRightCorner.x - positionTopLeftCorner.x;

            // Change the position vector accordingly
            position = position + new Vector3(scale.x, 0, 0);
        }

        // Find the distance in y direction between the corners
        if(positionTopLeftCorner.y >= positionBottomRightCorner.y)
        {
            // Case the top left corner has a greater y position than the bottom left corner
            scale.x = positionTopLeftCorner.y - positionBottomRightCorner.y;

            // Change the position vector accordingly
            position = position - new Vector3(0, scale.y, 0);
            
        } else {

            // Case the top left corner has a smaller y position than the bottom left corner
            scale.x = positionBottomRightCorner.y - positionTopLeftCorner.y;

            // Change the position vector accordingly
            position = position + new Vector3(0, scale.y, 0);
        }

         // Find the distance in z direction between the corners
        if(positionTopLeftCorner.z >= positionBottomRightCorner.z)
        {
            // Case the top left corner has a greater z position than the bottom left corner
            scale.x = positionTopLeftCorner.z - positionBottomRightCorner.z;

            // Change the position vector accordingly
            position = position - new Vector3(0, 0, scale.z);
            
        } else {

            // Case the top left corner has a smaller z position than the bottom left corner
            scale.x = positionBottomRightCorner.z - positionTopLeftCorner.z;

            // Change the position vector accordingly
            position = position +  new Vector3(0, 0, scale.z);
        }

        // Scale the game board correctly
        gameBoard.transform.localScale = scale;

        // Set the position of the game board between the two corner markers
        gameBoard.transform.position = position;

        // Set the correct rotation

    }

    // Method that is used to remove the game board
    public void RemoveGameBoard()
    {

    }
}
