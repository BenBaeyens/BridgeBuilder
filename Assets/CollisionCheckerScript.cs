using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerScript : MonoBehaviour
{

    public bool isInCollision;

    private void Start()
    {
        //Physics.IgnoreCollision(transform.parent.GetChild(0).GetComponent<Collider>(), GetComponent<Collider>(), true);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("COLLISION");
        if (!other.transform.CompareTag("Block"))
            isInCollision = true;
    }
    private void OnCollisionStay(Collision other)
    {
        if (!other.transform.CompareTag("Block"))
            isInCollision = true;
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Block"))
            isInCollision = false;
    }

}
