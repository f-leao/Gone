using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : SingletonMonoBehaviour<PlayerCam>
{
    public float sensX;
    public float sensY;

    public Transform pivot;
    public GameObject[] cameras;
    private int currentCameraIndex;
    private GameObject currentCamera;

    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCameraIndex = 0;
        UpdateCamera();
    }

    // Update is called once per frame
    void Update()
    {
        //get the current camera
        // currentCamera = cameras[currentCameraIndex];

        //get the mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //player orientation
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        pivot.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        //camera orientation
        transform.rotation = currentCamera.GetComponent<CameraType>().GetDirection(pivot.rotation);
        transform.position = currentCamera.transform.position;
    }

    public void SetPlayerRotation(float yRotation)
    {
        this.yRotation = yRotation;
    }

    public void LookAtPoint(Vector3 point)
    {
        Vector3 direction = point;
        float y = Quaternion.LookRotation(direction).eulerAngles.y;
        yRotation = y;
    }
    

    public void NextCameraPosition()
    {
        currentCameraIndex = ++currentCameraIndex % cameras.Length;
        UpdateCamera();
    }

    private void UpdateCamera() => currentCamera = cameras[currentCameraIndex];
}
