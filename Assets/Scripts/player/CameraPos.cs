using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : SingletonMonoBehaviour<CameraPos>
{
    public Transform[] cameraPositions;

    private int currentCameraIndex;

    void Start()
    {
        currentCameraIndex = 0;
    }

    void Update()
    {
        transform.position = cameraPositions[currentCameraIndex].position;
    }

    public void NextCameraPosition()
    {
        currentCameraIndex = ++currentCameraIndex % cameraPositions.Length;
    }
}
