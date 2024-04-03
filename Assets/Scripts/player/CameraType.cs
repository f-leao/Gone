using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraType : MonoBehaviour
{
    public bool isFacingBackward;

    public Quaternion GetDirection(Quaternion forward)
    {
        if (isFacingBackward)
            forward *= Quaternion.Euler(0f, 180f, 0f);
        
        return forward;
    }
}
