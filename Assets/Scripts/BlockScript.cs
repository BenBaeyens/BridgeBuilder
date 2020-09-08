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


    public List<Vector3> snapDistances;


    Vector3 destination;
    Vector3 originalPos;
    GameManager gameManager;

    public bool hasPlayerOnTop; // Set externally by player script


    void Start()
    {
        originalPos = transform.position;
        destination = transform.position + new Vector3(Random.Range(-randomOffsetMargin.x, randomOffsetMargin.x), Random.Range(-randomOffsetMargin.y, randomOffsetMargin.y), Random.Range(-randomOffsetMargin.z, randomOffsetMargin.z)); // Apply the random offset
        transform.position += startDistanceOffset; // Set the block position down by the offset
        gameManager = GameObject.FindObjectOfType<GameManager>(); // The game manager

    }

    private void Update()
    {

        if (hasPlayerOnTop)
        {
            GetComponent<Renderer>().material = gameManager.unmovableBlockMaterial;
        }
        else
        {
            if (gameManager.lastSelectedObject == gameObject)
            {
                GetComponent<Renderer>().material = gameManager.selectedMaterial;
            }
            else
            {
                GetComponent<Renderer>().material = gameManager.defaultMaterial;
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

    public void MoveVerticalCall(float mouseInput)
    {

        // Check this when changing blocks to sideways movement
        destination += new Vector3(0, moveSpeed * mouseInput * Time.deltaTime, 0);
        float destinationX = Mathf.Clamp(destination.x, minLimit.x + originalPos.x, maxLimit.x + originalPos.x);
        float destinationY = Mathf.Clamp(destination.y, minLimit.y, maxLimit.y);
        float destinationZ = Mathf.Clamp(destination.z, minLimit.z + originalPos.z, maxLimit.z + originalPos.z);

        destination = new Vector3(destinationX, destinationY, destinationZ);
    }

    public void MoveHorizontalCall(float mouseInput)
    {
        // Check this when changing blocks to sideways movement
        destination += new Vector3(moveSpeed * mouseInput * Time.deltaTime, 0, 0);
        float destinationX = Mathf.Clamp(destination.x, originalPos.x, originalPos.x);
        float destinationY = Mathf.Clamp(destination.y, minLimit.y + originalPos.y, maxLimit.y + originalPos.y);
        float destinationZ = Mathf.Clamp(destination.z, minLimit.z + originalPos.z, maxLimit.z + originalPos.z);

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

    /* FUN TESTING

    public void ShootOffCall()
    {
        destination = transform.position * 3 / 2;
    }

    */
}
