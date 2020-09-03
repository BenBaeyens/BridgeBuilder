using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScriptOld : MonoBehaviour
{

    public float speedIncrements = 10f;
    public float startYDistanceOffset = 5f; // The distance at which the object spawns and has to travel up to
    public float randomOffsetY = .7f;

    public float topCap = 7f;
    public float bottomCap = -3f;

    public float checkDistance = 0.01f;

    bool loadingAnimation = true;

    Vector3 destination;


    void Start()
    {
        topCap = transform.position.y + topCap;
        bottomCap = transform.position.y + bottomCap;

        destination = transform.position + new Vector3(0, Random.Range(-randomOffsetY, randomOffsetY), 0); // Apply the random offset
        transform.position -= new Vector3(0, startYDistanceOffset, 0); // Set the block position down by the offset
    }

    private void Update()
    {
        if (loadingAnimation) // Loading animation Code
        {
            if (!HasReachedDestination())
            {
                transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
            }
            else
            {
                loadingAnimation = false;
            }
        }
        if (!HasReachedDestination())
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
        }
    }

    public bool HasReachedDestination()
    {
        if (Vector3.Distance(transform.position, destination) <= checkDistance)
        {
            transform.position = destination;
            return true;
        }
        return false;
    }

    public bool HasReachedTop()
    {
        if (destination.y >= topCap)
        {
            destination = new Vector3(destination.x, topCap, destination.z);
            return true;
        }
        return false;
    }

    public bool HasReachedBottom()
    {
        if (destination.y <= bottomCap)
        {
            destination = new Vector3(destination.x, bottomCap, destination.z);
            return true;
        }
        return false;

    }

    public void MoveUpCall(float mouseInput)
    {

        destination += new Vector3(0, speedIncrements * Time.deltaTime * mouseInput / 10, 0);

    }
    public void ShootOffCall()
    {
        destination = transform.position * 3 / 2;
    }
}
