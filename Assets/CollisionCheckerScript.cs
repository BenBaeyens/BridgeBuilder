using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerScript : MonoBehaviour
{

    public bool isInCollision;

    private void OnCollisionEnter(Collision other)
    {
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
