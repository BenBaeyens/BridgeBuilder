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
        if (GetComponent<Collider>().bounds.Intersects())
    }
}
