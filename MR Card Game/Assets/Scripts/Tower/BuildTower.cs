using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static build.BuildTowerMenu;
using Vuforia;

// Define a static class that saves the image target on which the current tower is beeing built
public static class TowerImageTarget
{
    // The image target game object that opened the build tower menu
    public static GameObject currentImageTarget;
}

public class BuildTower : MonoBehaviour
{
    [Tooltip("The minimum distance between the image target and buildings for the build menu to appear")]
    public float minimumDistanceBase = 0.1f;

    [Tooltip("The flag that states if the build building image target is on the game board or not")]
    public bool onBoard = false;

    public bool visible = false;

    [SerializeField] private GameObject groundPlane;

    [SerializeField] private GameObject towerImageTarget;

    [SerializeField] private GameObject gameBoard;
    // Define the individual canvas for the build tower button
    [SerializeField] private GameObject buildUI;

    [SerializeField] private GameObject buildPositionIndicator;

    //The projected position on ground plane
    private Vector3 projectedPos = Vector3.zero; 

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the box collider is enabled
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure the box collider is enabled
        GetComponent<BoxCollider>().enabled = true;
        if (visible && !GameAdvancement.gamePaused)
        {
            projectedPos = ProjectPositionOnGroundPlane();
            buildPositionIndicator.transform.SetParent(groundPlane.transform, true);
            buildPositionIndicator.transform.localPosition = projectedPos;
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

        // Check if the image target just entered the game board or left it
        if(onBoard && !GameAdvancement.gamePaused && CheckDistanceToTowers() && visible)
        {
            // Make the ui button appear that should be clickable to construct a tower
            buildUI.SetActive(true);

            // Activate the canvas
            buildUI.GetComponent<Canvas>().enabled = true;

            // Activate the billboard script
            buildUI.GetComponent<Billboard>().enabled = true;


        } else if(!onBoard || !CheckDistanceToTowers() || !visible)
        {
            // Make the ui button disappear
            buildUI.SetActive(false);

        }

        // Check if the build UI is active and if the game was paused
        if(GameAdvancement.gamePaused == true && buildUI.activeSelf == true)
        {
            // Deactivate the billboard script
            buildUI.SetActive(false);
        }
    }

    // Methods activated when the image target enters the field
    public void ImageTargetVisible()
    {
        visible = true;
    }

    // Methods activated when the image target leaves the field
    public void ImageTargetLost()
    {
        visible = false;
    }

    // The method that adds entering enemies to the collider list
/*    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter was activated");

        // Check if the collider that entered the box collider of the image target is the game board
        if(other.gameObject.CompareTag("Board"))
        {
            onBoard = true;
        }
    }*/

    // The method that removes exiting enemies of the collider list
/*    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that left the box collider of the image target is the game board
        if(other.gameObject.CompareTag("Board"))
        {
            onBoard = false;
        }
    }*/

    // The method that checks the distance between the towers and the image target to see if a tower can be built here or not
    public bool CheckDistanceToTowers()
    {
        // Get the tower and trap array
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag("Trap");

        // Initialize the distance variable
        float distance = 0;

        // Initialize the minimum distance variable
        float minimumDistance = 1f * Board.greatestBoardDimension;

        // Initialize the distance ok flag that states that the minimum distance was kept
        bool distanceOK = true;

        // Check if a tower is too close to the image target
        foreach(GameObject tower in towerArray)
        {
            // Get the distance between the image target and the tower
            distance = Vector3.Distance(GetRelativePosition(groundPlane.transform, tower.transform.position), projectedPos);

            // Check if the distance is too short
            if(distance < minimumDistance)
            {
                distanceOK = false;
            }
        }

        // Check if a trap is too close to the image target
        foreach(GameObject trap in trapArray)
        {
            // Get the distance between the image target and the trap
            distance = Vector3.Distance(GetRelativePosition(groundPlane.transform, trap.transform.position), projectedPos);

            // Check if the distance is too short
            if (distance < minimumDistance)
            {
                distanceOK = false;
            }
        }

/*        Debug.Log("The distance ok flag is: " + distanceOK);*/

        // Return the distance ok flag
        return distanceOK;
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTower()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build tower menu of the build tower menu script
        OpenBuildTowerMenu();

        // Save the position of the building in the build position vector in the gameboard coordinate (assume the localPosition of gameBoard on Ground Plane is 0,0,0
        TowerEnhancer.buildPosition = new Vector3(projectedPos.x / gameBoard.transform.localScale.x, projectedPos.y / gameBoard.transform.localScale.y, projectedPos.z / gameBoard.transform.localScale.z);
    }

    // The method used to begin to build a tower on an image target when pressing on the build tower button
    public void BeginBuildingTrap()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;

        // Use the open build trap menu of the build tower menu script
        OpenBuildTrapMenu();

        // Save the position of the building in the build position vector in the gameboard coordinate (assume the localPosition of gameBoard on Ground Plane is 0,0,0
        TowerEnhancer.buildPosition = new Vector3(projectedPos.x / gameBoard.transform.localScale.x, projectedPos.y / gameBoard.transform.localScale.y, projectedPos.z / gameBoard.transform.localScale.z);
    }


    // Project the position of the image target GameObject to the ground plane with position.y=0, using similar triangles
    // return the projected position
    private Vector3 ProjectPositionOnGroundPlane()
    {
        Vector3 cameraPos = GetRelativePosition(groundPlane.transform, Camera.main.transform.position);
        Vector3 imageTargetPos = GetRelativePosition(groundPlane.transform, towerImageTarget.transform.position);
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
        if(pos.x > gameBoardMin.x && pos.z > gameBoardMin.z && pos.x < gameBoardMax.x && pos.z < gameBoardMax.z)
        {
            return true;
        }
        //if the game board is rotated 180 degrees
        else if(pos.x < gameBoardMin.x && pos.z < gameBoardMin.x && pos.x > gameBoardMax.x  && pos.z > gameBoardMax.z)
        {
            return true;
        }
        else
        {
            return false;
        }       
    }
}
