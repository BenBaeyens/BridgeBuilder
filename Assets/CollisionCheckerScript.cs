using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerScript : MonoBehaviour
{

    public bool isInCollision;
    GameManager gameManager;
    Vector3 lastPosition;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        if (!isInCollision)
        {
            lastPosition = transform.position;
        }
        else
        {
            transform.position = lastPosition;
        }

        for (int i = 0; i < gameManager.colliders.Count; i++)
        {

            if (GetComponent<Collider>().bounds.Intersects(gameManager.colliders[i].bounds) && gameManager.colliders[i] != GetComponent<Collider>())
            {
                isInCollision = true;
                break;
            }
            isInCollision = false;
        }
    }
}
