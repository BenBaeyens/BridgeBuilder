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


    Transform destination;
    CollisionCheckerScript destinationCheckerScript;
    Vector3 originalPos;
    GameManager gameManager;

    public bool hasPlayerOnTop; // Set externally by player script
    public bool isLerpingMaterials; // Temporary in-between material used for lerping

    float destinationX;
    float destinationY;
    float destinationZ;

    Renderer thisRenderer;
    Material targetMaterial;
    Material originalMat;
    float t;
    float startTime;
    bool isInCollision;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        originalPos = transform.position;
        destination = transform.parent.GetChild(1);
        destinationCheckerScript = destination.GetComponent<CollisionCheckerScript>();
        destination.position = transform.position + new Vector3(Random.Range(-randomOffsetMargin.x, randomOffsetMargin.x), Random.Range(-randomOffsetMargin.y, randomOffsetMargin.y), Random.Range(-randomOffsetMargin.z, randomOffsetMargin.z)); // Apply the random offset
        transform.position += startDistanceOffset; // Set the block position down by the offset
        gameManager = GameObject.FindObjectOfType<GameManager>(); // The game manager
        startTime = Time.time;
    }

    private void Update()
    {
        CollisionDetection();

        PlayerOnTopMaterialChange();

        MoveBlock();

        MaterialLerp();

    }

    private void CollisionDetection()
    {
        for (int i = 0; i < gameManager.colliders.Count; i++)
        {
            if (GetComponent<Collider>().bounds.Intersects(gameManager.colliders[i].bounds) && gameManager.colliders[i] != destination.GetComponent<Collider>() && destination.GetComponent<CollisionCheckerScript>().isInCollision)
            {
                isInCollision = true;
                break;
            }
            else if (!destinationCheckerScript.isInCollision)
                isInCollision = false;
        }
    }

    public bool HasReachedDestination()
    {
        if (Vector3.Distance(transform.position, destination.position) <= checkDistance)
        {
            transform.position = destination.position;
            return true;
        }
        return false;
    }

    public void MoveCall(float mouseInputX, float mouseInputY)
    {
        // INPUTS ARE NOT RELATIVE
        if (!destinationCheckerScript.isInCollision)
        {
            switch (direction)
            {
                case MoveDirection.x:
                    destination.position += new Vector3(moveSpeed * mouseInputX * Time.deltaTime, 0, 0);
                    destinationX = Mathf.Clamp(destination.position.x, minLimit.x, maxLimit.x);
                    destinationY = transform.position.y;
                    destinationZ = transform.position.z;
                    break;
                case MoveDirection.y:
                    destination.position += new Vector3(0, moveSpeed * mouseInputY * Time.deltaTime, 0);
                    destinationX = transform.position.x;
                    destinationY = Mathf.Clamp(destination.position.y, minLimit.y, maxLimit.y);
                    destinationZ = transform.position.z;
                    break;

                case MoveDirection.z:
                    destination.position += new Vector3(0, 0, moveSpeed * mouseInputY * Time.deltaTime);
                    destinationX = transform.position.x;
                    destinationY = transform.position.y;
                    destinationZ = Mathf.Clamp(destination.position.z, minLimit.z, maxLimit.z);
                    break;
            }
            destination.position = new Vector3(destinationX, destinationY, destinationZ);
        }
    }

    private void MoveBlock()
    {
        if (!HasReachedDestination() && !isInCollision)
        {
            if (snapDistances.Count > 0 && !gameManager.isMovingObject)
            {
                for (int i = 0; i < snapDistances.Count; i++)
                {
                    if (Vector3.Distance(transform.position, snapDistances[i]) < snapCheckDistance)
                    {
                        destination.position = snapDistances[i];
                    }
                }
            }
            transform.position = Vector3.Lerp(transform.position, destination.position, Time.deltaTime * lerpSpeed);
        }
    }

    public void MaterialLerpSetup(Material targetMat)
    {
        if (!isLerpingMaterials && targetMat != targetMaterial)
        {
            startTime = Time.time;
            isLerpingMaterials = true;
            originalMat = thisRenderer.material;
            targetMaterial = targetMat;
        }
    }

    private void MaterialLerp()
    {
        if (isLerpingMaterials)
        {
            t = (Time.time - startTime) * 10f;
            thisRenderer.material.Lerp(originalMat, targetMaterial, t);
            if (t >= 1f)
            {
                thisRenderer.material = targetMaterial;
                isLerpingMaterials = false;
            }
        }
    }

    private void PlayerOnTopMaterialChange()
    {
        if (hasPlayerOnTop)
        {
            MaterialLerpSetup(gameManager.unmovableBlockMaterial);
        }
        else
        {
            if (gameManager.lastSelectedObject == gameObject)
            {
                MaterialLerpSetup(gameManager.selectedMaterial);
            }
            else
            {
                MaterialLerpSetup(gameManager.defaultMaterial);
            }
        }
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

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(maxLimit, 0.1f);
        Gizmos.DrawWireSphere(minLimit, 0.1f);
        Gizmos.DrawLine(minLimit, maxLimit);
    }
}
