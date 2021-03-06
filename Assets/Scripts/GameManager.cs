﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    public Material defaultMaterial; // The material the gameobject will have when not selected
    public Material selectedMaterial; // The material the gameobject will have when selected
    public Material unmovableBlockMaterial; // The material when the player is on top of it
    public string blockTag = "Block"; // The tag to compare with
    public string collisionDetectionTag = "ColDet";
    public float materialLerpTime = 0.1f;

    // PRIVATE VARIABLES
    [HideInInspector] public GameObject lastSelectedObject;
    Renderer lastSelectedObjectRenderer;
    BlockScript lastSelectedBlockScript;

    RaycastHit hit;
    Ray ray;
    CameraRotate camScript; // The camera rotation script
    Material lerpMaterial; // The material that lerps between the two states
    [HideInInspector] public bool isMovingObject;
    bool hasSelectedObject;
    public List<Collider> colliders;

    private void Start()
    {
        camScript = Camera.main.GetComponent<CameraRotate>();
        Collider[] tempCollider = GameObject.FindObjectsOfType<Collider>();
        for (int i = 0; i < tempCollider.Length; i++)
        {
            if (tempCollider[i].transform.tag == collisionDetectionTag)
            {
                colliders.Add(tempCollider[i]);
            }
        }
    }

    private void Update()
    {

        #region InputFunction

        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");
        // If the left mouse button gets clicked on an object, raise it
        if (Input.GetMouseButton(0) && LastSelectedObjectIsValid() && !camScript.isRotating)
        {
            if (!lastSelectedBlockScript.hasPlayerOnTop)
            {
                // Change this to the corresponding script
                isMovingObject = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMovingObject = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isMovingObject && !camScript.isRotating && !lastSelectedBlockScript.hasPlayerOnTop)
        {
            lastSelectedBlockScript.MoveCall(mouseX, mouseY);
        }
        #endregion
        #region RaycastFunctions

        // Set the ray details
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Execute the physics raycast
        if (Physics.Raycast(ray, out hit))
        {
            // First, check if we hit a clickable object
            if (hit.transform.CompareTag(blockTag) && !camScript.isRotating && !isMovingObject)
            {
                // Check if the we have selected an object before and change its material
                ResetMaterial();

                // Set the variables for the newly selected component
                lastSelectedObject = hit.transform.gameObject;
                lastSelectedObjectRenderer = hit.transform.GetComponent<Renderer>();
                lastSelectedBlockScript = hit.transform.GetComponent<BlockScript>();

                if (!hit.transform.GetComponent<BlockScript>().hasPlayerOnTop)
                {
                    hasSelectedObject = true;
                    lastSelectedBlockScript.MaterialLerpSetup(selectedMaterial);
                }
                else
                {
                    hasSelectedObject = false;
                    lastSelectedBlockScript.MaterialLerpSetup(unmovableBlockMaterial);
                }
            }
            else
            {
                hasSelectedObject = false;
                ResetMaterial();
            }
        }
        else
        {
            hasSelectedObject = false;
            ResetMaterial();
        }
        #endregion

    }

    // Reset the material to its original color when the gameobject is no longer selected
    public void ResetMaterial()
    {
        if (LastSelectedObjectIsValid())
        {
            if (lastSelectedBlockScript.hasPlayerOnTop)
            {
                isMovingObject = false;
            }

            else if (!isMovingObject && !hasSelectedObject)
            {
                // Can be changed by lastselectedobjectrenderer
                lastSelectedBlockScript.MaterialLerpSetup(defaultMaterial);
                lastSelectedObject = null;
            }
            else if (isMovingObject || hasSelectedObject)
            {
                lastSelectedBlockScript.MaterialLerpSetup(selectedMaterial);
            }
        }
    }


    // A function to determine if the last selected object is true or false
    bool LastSelectedObjectIsValid()
    {
        if (lastSelectedObject != null)
            return true;
        return false;
    }


    // Call to the PlayerController to start the game from the UI prefab
    public void StartGame()
    {
        GameObject.FindObjectOfType<PlayerController>().StartPathFind();
    }
}
