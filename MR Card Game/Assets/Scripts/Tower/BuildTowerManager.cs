using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static build.BuildTowerMenu;
using Vuforia;
using UnityEngine.PlayerLoop;

/// <summary>
/// A static class that saves the image target on which the current tower is beeing built
/// </summary>
public static class TowerImageTarget
{
    /// <summary>
    /// The image target game object that opened the build tower menu
    /// </summary>
    public static GameObject currentImageTarget;
}

public class BuildTowerManager : MonoBehaviour
{
    [SerializeField] private GameObject groundPlane;

    [SerializeField] private GameObject towerImageTarget;

    [SerializeField] private GameObject gameBoard;

    [SerializeField] private GameObject buildUI;

    [SerializeField] private GameObject buildPositionIndicator;

    // if the build building image target is on the game board or not
    private bool onBoard = false;
    private bool visible = false;
    //The projected position on ground plane
    private Vector3 projectedPos = Vector3.zero;

    //Multiply with the alpha of materials on the tower indicator.
    private float indicatorAlphaMultiplyFactor = 1;
    // Arrays used to changing the alpha value of the indicator
    private Renderer[] indicatorRenderers;
    private float[] initialAlphas;

    // Start is called before the first frame update
    void Start()
    {
        indicatorRenderers = buildPositionIndicator.GetComponentsInChildren<Renderer>();
        initialAlphas = new float[indicatorRenderers.Length];
        for(int i = 0; i < indicatorRenderers.Length; i++)
        {
            if (indicatorRenderers[i].gameObject.name != "ImageTargetPicture")
            {
                initialAlphas[i] = indicatorRenderers[i].material.color.a;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //onBoard check
        if (visible && !GameAdvancement.gamePaused)
        {
            projectedPos = ProjectPositionOnGroundPlane();
            buildPositionIndicator.transform.SetParent(groundPlane.transform, true);
            buildPositionIndicator.SetActive(true);
            if (OverlapWithGameBoard(projectedPos))
            {              
                onBoard = true;
            }
            else
            {
                buildPositionIndicator.SetActive(false);
                onBoard = false;
            }
        }
        else
        {
            onBoard = false;
        }

        if (!visible)
        {
            buildPositionIndicator.SetActive(false);
        }

        // Check if the image target just entered the game board or left it
        if(onBoard && !GameAdvancement.gamePaused && !BuildingIndicatorOverlap() && visible)
        {
            buildUI.SetActive(true);
            buildUI.GetComponent<Canvas>().enabled = true;
            buildUI.GetComponent<Billboard>().enabled = true;
            buildPositionIndicator.SetActive(true);
            buildUI.transform.position = buildPositionIndicator.transform.position;
        } else if(!onBoard || BuildingIndicatorOverlap() || !visible)
        {
            buildUI.SetActive(false);
        }

        // Check if the build UI is active and if the game was paused
        if (GameAdvancement.gamePaused == true && buildUI.activeSelf == true)
        {
            buildUI.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (onBoard)
        {
            // Set the position of the indicator correctly, not influenced by image target
            // But the rotation will remain as identity
            buildPositionIndicator.transform.localPosition = projectedPos;
            buildPositionIndicator.transform.rotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        if (onBoard)
        {
            FlashIndicator();
        }
    }

    /// <summary>
    /// Activated when the image target enters the field
    /// </summary>
    public void ImageTargetVisible()
    {
        visible = true;
    }

    /// <summary>
    /// Activated when the image target leaves the field
    /// </summary>
    public void ImageTargetLost()
    {
        visible = false;
        buildPositionIndicator.GetComponentInChildren<BuildTowerIndicator>().OverlapWithTowerOrTrap = false;
    }

    /// <summary>
    /// Begin to build a tower on an image target when pressing on the build tower button
    /// </summary>
    public void BeginBuildingTower()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;
        OpenBuildTowerMenu();
        // Save the position of the building in the build position vector in the gameboard coordinate (assume the localPosition of gameBoard on Ground Plane is (0,0,0)
        TowerEnhancer.buildPosition = new Vector3(projectedPos.x / gameBoard.transform.localScale.x, projectedPos.y / gameBoard.transform.localScale.y, projectedPos.z / gameBoard.transform.localScale.z);
    }

    /// <summary>
    /// Begin to build a tower on an image target when pressing on the build tower button
    /// </summary>
    public void BeginBuildingTrap()
    {
        // Set this image target as the currently building one
        TowerImageTarget.currentImageTarget = gameObject;
        OpenBuildTrapMenu();
        // Save the position of the building in the build position vector in the gameboard coordinate (assume the localPosition of gameBoard on Ground Plane is (0,0,0)
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

    //change the alpha value of the indicator
    //only use in FixedUpdate
    private void FlashIndicator()
    {
        for(int i = 0; i < indicatorRenderers.Length; i++)
        {
            if (indicatorRenderers[i].gameObject.name != "ImageTargetPicture")
            {
                indicatorRenderers[i].material.color = new Color(indicatorRenderers[i].material.color.r, indicatorRenderers[i].material.color.g, indicatorRenderers[i].material.color.b, initialAlphas[i] * indicatorAlphaMultiplyFactor);
            }
        }
        indicatorAlphaMultiplyFactor -= 0.01f;
        if(indicatorAlphaMultiplyFactor < 0)
        {
            indicatorAlphaMultiplyFactor = 1;
        }
    }

    private bool BuildingIndicatorOverlap()
    {
        return buildPositionIndicator.GetComponentInChildren<BuildTowerIndicator>().OverlapWithTowerOrTrap;
    }
}
