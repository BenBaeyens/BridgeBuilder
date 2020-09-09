using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    public float moveSpeed = 26f;
    public float lerpSpeed = 1f;

    public Vector3 maxLimit = new Vector3(0, -0.5f, 0);
    public Vector3 minLimit = new Vector3(0, -7.3f, 0);

    public Vector3 startDistanceOffset = new Vector3(0, -5, 0); // The distance at which the object spawns and has to travel up to
    public Vector3 randomOffsetMargin = new Vector3(0, 0.7f, 0); // How much randomness the block can have on startup

    public float checkDistance = 0.01f;
    public float snapCheckDistance = 0.1f;


    public enum MoveDirection
    {
        x,
        y,
        z
    }

    public MoveDirection direction = MoveDirection.y;

    public List<Vector3> snapDistances;


    Vector3 destination;
    Vector3 originalPos;
    GameManager gameManager;

    public bool hasPlayerOnTop; // Set externally by player script
    public bool isLerpingMaterials; // Temporary in-between material used for lerping

    float destinationX;
    float destinationY;
    float destinationZ;

    Renderer thisRenderer;
    Material tempMat;
    Material targetMat;
    Material originalMat;
    float lerpTime;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        originalPos = transform.position;
        destination = transform.position + new Vector3(Random.Range(-randomOffsetMargin.x, randomOffsetMargin.x), Random.Range(-randomOffsetMargin.y, randomOffsetMargin.y), Random.Range(-randomOffsetMargin.z, randomOffsetMargin.z)); // Apply the random offset
        transform.position += startDistanceOffset; // Set the block position down by the offset
        gameManager = GameObject.FindObjectOfType<GameManager>(); // The game manager

    }

    private void Update()
    {

        if (hasPlayerOnTop)
        {
            thisRenderer.material = gameManager.unmovableBlockMaterial;
        }
        else
        {
            if (gameManager.lastSelectedObject == gameObject)
            {
                thisRenderer.material = gameManager.selectedMaterial;
            }
            else
            {
                thisRenderer.material = gameManager.defaultMaterial;
            }
        }

        if (!HasReachedDestination())
        {
            if (snapDistances.Count > 0 && !gameManager.isMovingObject)
            {
                for (int i = 0; i < snapDistances.Count; i++)
                {
                    if (Vector3.Distance(transform.position, snapDistances[i]) < snapCheckDistance)
                    {
                        destination = snapDistances[i];
                    }
                }
            }
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * lerpSpeed);
        }

        if (isLerpingMaterials && tempMat.color != targetMat.color)
        {
            thisRenderer.material.Lerp(originalMat, targetMat, Time.deltaTime / lerpTime * 0.1f);
            lerpTime *= Time.deltaTime;
        }
        else if (thisRenderer.material.color == targetMat.color)
        {
            isLerpingMaterials = false;
            thisRenderer.material = targetMat;
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

    public void MoveCall(float mouseInputX, float mouseInputY)
    {
        // INPUTS ARE NOT RELATIVE
        switch (direction)
        {
            case MoveDirection.x:
                destination += new Vector3(moveSpeed * mouseInputX * Time.deltaTime, 0, 0);
                destinationX = Mathf.Clamp(destination.x, minLimit.x, maxLimit.x);
                destinationY = transform.position.y;
                destinationZ = transform.position.z;
                break;

            case MoveDirection.y:
                destination += new Vector3(0, moveSpeed * mouseInputY * Time.deltaTime, 0);
                destinationX = transform.position.x;
                destinationY = Mathf.Clamp(destination.y, minLimit.y, maxLimit.y);
                destinationZ = transform.position.z;
                break;

            case MoveDirection.z:
                destination += new Vector3(0, 0, moveSpeed * mouseInputY * Time.deltaTime);
                destinationX = transform.position.x;
                destinationY = transform.position.y;
                destinationZ = Mathf.Clamp(destination.z, minLimit.z, maxLimit.z);
                break;
        }
        destination = new Vector3(destinationX, destinationY, destinationZ);
    }

    // Draw the snap points
    private void OnDrawGizmosSelected()
    {
        if (snapDistances.Count > 0)
        {
            for (int i = 0; i < snapDistances.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(snapDistances[i], 0.05f);
            }
        }
    }

    public void MaterialLerp(Material targetMat)
    {
        lerpTime = 0;
        originalMat = thisRenderer.material;
        this.targetMat = targetMat;
        isLerpingMaterials = true;
    }

    /* FUN TESTING

    public void ShootOffCall()
    {
        destination = transform.position * 3 / 2;
    }

    */
}
