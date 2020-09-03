using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatricesScript : MonoBehaviour
{

    [Range(0, 25)] public int x = 10;
    [Range(0, 25)] public int y = 10;
    public Vector3 objectSize;
    public Vector3 offset;
    [Range(0.5f, 10f)] public float spaceBetweenObjects = 1f;

    public PrimitiveType type = PrimitiveType.Sphere;

    List<List<GameObject>> grid = new List<List<GameObject>>();

    public float speed = 29f;
    GameObject cameraSmoother;

    [HideInInspector] int originalx;
    [HideInInspector] int originaly;
    [HideInInspector] PrimitiveType originalType;
    [HideInInspector] Vector3 originalObjectSize;
    [HideInInspector] float originalSpaceBetweenObjects;
    [HideInInspector] Vector3 originalOffset;

    private void Start()
    {
        cameraSmoother = GameObject.FindGameObjectWithTag("CameraSmoother");
        originalx = x;
        originaly = y;
        originalType = type;
        originalObjectSize = objectSize;
        originalOffset = offset;
        CreateGrid();
    }

    private void Update()
    {
        if (x != originalx || y != originaly || type != originalType || objectSize != originalObjectSize || spaceBetweenObjects != originalSpaceBetweenObjects || originalOffset != offset)
        {
            ClearGrid();
            CreateGrid();
            originalx = x;
            originaly = y;
            originalType = type;
            originalObjectSize = objectSize;
            originalSpaceBetweenObjects = spaceBetweenObjects;
            originalOffset = offset;
        }

        float horizontal = -Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;


        // Add smoothing using external object
        cameraSmoother.transform.RotateAround(new Vector3(x / 2, 0, y / 2), Vector3.up, horizontal);
        cameraSmoother.transform.RotateAround(new Vector3(x / 2, 0, y / 2), cameraSmoother.transform.right, vertical);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraSmoother.transform.position, 6f * Time.deltaTime);
        Camera.main.transform.LookAt(new Vector3(x / 2, 0, y / 2));
    }

    private void ClearGrid()
    {
        for (int r = 0; r < originalx; r++)
        {
            for (int c = 0; c < originaly; c++)
            {
                Destroy(grid[r][c]);
            }
        }
        grid.Clear();
    }

    private void CreateGrid()
    {
        for (int r = 0; r < x; r++)
        {
            grid.Add(new List<GameObject>());
            for (int c = 0; c < y; c++)
            {
                grid[r].Add(GameObject.CreatePrimitive(type));
                grid[r][c].transform.position = new Vector3(c * spaceBetweenObjects, 0, r * spaceBetweenObjects) + offset;
                grid[r][c].transform.localScale = objectSize;
                grid[r][c].AddComponent<BlockScript>();
                grid[r][c].tag = "Block";
            }
        }
    }
}
