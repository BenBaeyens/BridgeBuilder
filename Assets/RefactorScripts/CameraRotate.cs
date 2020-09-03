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

        // If mouse button down, turn. (Could change this to cursorlockmode.confined)
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Add smoothing via external object
            cameraSmoother.transform.RotateAround(centerPoint, Vector3.up, horizontal);
            cameraSmoother.transform.RotateAround(centerPoint, cameraSmoother.transform.right, vertical);
        }


        // If no longer turning, set cursor to visible again 
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // TODO: ADD CLAMP TO THIS
        cameraSmoother.transform.position += Vector3.back * back;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraSmoother.transform.position, cameraLerpSpeed * Time.deltaTime);
        cameraSmoother.transform.LookAt(centerPoint);
        Camera.main.transform.LookAt(centerPoint);

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
