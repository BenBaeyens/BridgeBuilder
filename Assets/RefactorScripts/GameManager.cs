using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Material defaultMaterial; // The material the gameobject will have when not selected
    public Material selectedMaterial; // The material the gameobject will have when selected
    public string blockTag = "Block"; // The tag to compare with

    GameObject lastSelectedObject;
    Renderer lastSelectedObjectRenderer;
    RaycastHit hit;
    Ray ray;

    private void Update()
    {

        #region RaycastFunctions

        // Set the ray details
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Execute the physics raycast
        if (Physics.Raycast(ray, out hit))
        {
            // First, check if we hit a clickable object
            if (hit.transform.CompareTag(blockTag))
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
                ResetMaterial();
        }
        else
            ResetMaterial();

        #endregion
        #region InputFunction
        // If the left mouse button gets clicked on an object, raise it
        if (Input.GetMouseButton(0) && LastSelectedObjectIsValid())
            // Change this to the corresponding script
            lastSelectedObject.GetComponent<BlockScript>().MoveUpCall();


        // If the right mouse button gets clicked on an object, raise it
        if (Input.GetMouseButton(1) && LastSelectedObjectIsValid())
            // Change this to the corresponding script  
            lastSelectedObject.GetComponent<BlockScript>().MoveDownCall();

        #endregion

    }

    // Reset the material to its original color when the gameobject is no longer selected
    void ResetMaterial()
    {
        if (LastSelectedObjectIsValid())
        {
            lastSelectedObject.GetComponent<Renderer>().material = defaultMaterial;
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
