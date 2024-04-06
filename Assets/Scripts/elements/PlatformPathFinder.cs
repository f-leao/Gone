using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPathFinder : MonoBehaviour
{
    public Transform getWaypoint(int positionIndex)
    {
        return transform.GetChild(positionIndex);
    }

    public int GetNextWaypointIndex(int current)
    {
        return (current + 1) % transform.childCount;
    }
}
