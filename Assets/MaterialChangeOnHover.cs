using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add more variety

public class MaterialChangeOnHover : MonoBehaviour
{
    public Material originalMat;
    public Material selectedMat;

    GameObject lastSelected;
    bool hasSelected;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Block"))
            {
                if (lastSelected != null)
                {
                    lastSelected.GetComponent<Renderer>().material = originalMat;
                }
                lastSelected = hit.transform.gameObject;
                lastSelected.GetComponent<Renderer>().material = selectedMat;
            }
            else if (lastSelected != null)
            {
                lastSelected.GetComponent<Renderer>().material = originalMat;
            }
        }
        else if (lastSelected != null)
        {
            lastSelected.GetComponent<Renderer>().material = originalMat;
            lastSelected = null;
        }

        if (Input.GetMouseButton(0) && lastSelected != null)
        {
            lastSelected.GetComponent<BlockScript>().MoveUpCall();
        }
        if (Input.GetMouseButton(1) && lastSelected != null)
        {
            lastSelected.GetComponent<BlockScript>().MoveDownCall();
        }
    }
}
