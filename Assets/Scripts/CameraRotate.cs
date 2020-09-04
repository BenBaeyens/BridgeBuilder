using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public Vector3 centerPoint; // The point at which the camera rotates
    public float cameraSpeed = 250f; // The speed at which the camera moves
    public float cameraZoomSpeed = 100f; // The speed at which the camera zooms in and out
    public float cameraLerpSpeed = 6f; // The speed at which the camera follows the smoother object
    public GameObject centerPointObject; // Optional center point object

    public float minBackMove; // How much the camera can move from its starting position
    public float maxBackMove; // ^^
    public float backMove = 0;

    public float minDegree;

    [HideInInspector] public bool isRotating; // Communicate with the game manager script;
    GameObject cameraSmoother;

    private void Start()
    {
        // Find the camera smoother gameobject
        cameraSmoother = GameObject.Find("CameraSmoother");

        // If the centerPointObject is not empty,set it as centerpoint
        if (centerPointObject != null)
        {
            centerPoint = centerPointObject.transform.position;
        }
    }

    private void Update()
    {
        // Get the inputs from the user

        float horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed;
        float vertical = -Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;
        float back = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * cameraZoomSpeed;
        backMove += back;

        // If mouse button down, turn. (Could change this to cursorlockmode.confined)
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isRotating = true;

            // Add smoothing via external object
            // Clamp code
            if (cameraSmoother.transform.localEulerAngles.x > minDegree && cameraSmoother.transform.localEulerAngles.x < 89 || vertical > 0)
                cameraSmoother.transform.RotateAround(centerPoint, cameraSmoother.transform.right, vertical);

            else if (cameraSmoother.transform.localEulerAngles.x > 89 && cameraSmoother.transform.localEulerAngles.x < 300)
                cameraSmoother.transform.RotateAround(centerPoint, -cameraSmoother.transform.right, 0.1f);

            // Always rotate horizontally
            cameraSmoother.transform.RotateAround(centerPoint, Vector3.up, horizontal);
        }


        // If no longer turning, set cursor to visible again 
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isRotating = false;
        }

        if (backMove >= minBackMove && backMove <= maxBackMove)
            cameraSmoother.transform.position += Vector3.back * back;
        else
            backMove = Mathf.Clamp(backMove, minBackMove, maxBackMove);



        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, cameraSmoother.transform.position, cameraLerpSpeed * Time.deltaTime);
        cameraSmoother.transform.LookAt(centerPoint);
        Camera.main.transform.LookAt(centerPoint);

        // When camera is too low, raise it up a little bit when the player lets go
        if (!Input.GetMouseButton(1))
        {
            if (cameraSmoother.transform.localEulerAngles.x < minDegree || cameraSmoother.transform.localEulerAngles.x > 300)
                cameraSmoother.transform.position += new Vector3(0, 0.1f, 0);
        }



        // Debugging rays
        Debug.DrawRay(centerPoint, cameraSmoother.transform.position, Color.red);
        Debug.DrawRay(centerPoint, Camera.main.transform.position, Color.green);
    }

    private void OnDrawGizmos()
    {
        // Draw a gizmo at the centerpoint
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPoint, .1f);
    }
}
