using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerScript : MonoBehaviour
{

    public bool isInCollision;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        for (int i = 0; i < gameManager.colliders.Length; i++)
        {

            if (GetComponent<Collider>().bounds.Intersects(gameManager.colliders[i].bounds))
            {
                isInCollision = true;
                break;
            }
            isInCollision = false;
        }
    }
}
