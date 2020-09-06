using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    public Material defaultMaterial; // The material the gameobject will have when not selected
    public Material selectedMaterial; // The material the gameobject will have when selected
    public Material unmovableBlockMaterial; // The material when the player is on top of it
    public string blockTag = "Block"; // The tag to compare with

    // PRIVATE VARIABLES
    GameObject lastSelectedObject;
    Renderer lastSelectedObjectRenderer;
    RaycastHit hit;
    Ray ray;
    CameraRotate camScript; // The camera rotation script
    [HideInInspector] public bool isMovingObject;

    private void Start()
    {
        camScript = Camera.main.GetComponent<CameraRotate>();
    }

    private void Update()
    {

        #region RaycastFunctions

        // Set the ray details
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Execute the physics raycast
        if (Physics.Raycast(ray, out hit))
        {
            // First, check if we hit a clickable object
            if (hit.transform.CompareTag(blockTag) && !camScript.isRotating)
            {
                if (!hit.transform.GetComponent<BlockScript>().hasPlayerOnTop)
                {
                    // Check if the we have selected an object before and change its material
                    ResetMaterial();

                    // Set the variables for the newly selected component
                    lastSelectedObject = hit.transform.gameObject;
                    lastSelectedObjectRenderer = hit.transform.GetComponent<Renderer>();

                    // Set the material to selected
                    lastSelectedObjectRenderer.material = selectedMaterial;
                }
                else
                {
                    ResetMaterial();
                    lastSelectedObject = hit.transform.gameObject;
                    lastSelectedObjectRenderer = hit.transform.GetComponent<Renderer>();

                    lastSelectedObjectRenderer.material = unmovableBlockMaterial;
                }
            }
            else
                ResetMaterial();
        }
        else
            ResetMaterial();

        #endregion
        #region InputFunction

        float mouseY = Input.GetAxis("Mouse Y");
        // If the left mouse button gets clicked on an object, raise it
        if (Input.GetMouseButton(0) && LastSelectedObjectIsValid() && !camScript.isRotating)
        {
            // Change this to the corresponding script
            isMovingObject = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMovingObject = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isMovingObject && !camScript.isRotating)
        {
            lastSelectedObject.GetComponent<BlockScript>().MoveVerticalCall(mouseY);
        }
        #endregion

    }

    // Reset the material to its original color when the gameobject is no longer selected
    void ResetMaterial()
    {
        if (LastSelectedObjectIsValid() && !isMovingObject)
        {
            lastSelectedObject.GetComponent<Renderer>().material = defaultMaterial;
            lastSelectedObject = null;
        }
    }


    // A function to determine if the last selected object is true or false
    bool LastSelectedObjectIsValid()
    {
        if (lastSelectedObject != null)
            return true;
        return false;
    }
}
