using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraType : MonoBehaviour
{
    public bool isFacingBackward;
    public bool showPlayer;

    public Quaternion GetDirection(Quaternion forward)
    {
        if (isFacingBackward)
            forward *= Quaternion.Euler(0f, 180f, 0f);

        if (showPlayer)
            EventsProvider.Instance.OnShowPlayer.Invoke();
        else    
            EventsProvider.Instance.OnHidePlayer.Invoke();
        
        return forward;
    }
}
