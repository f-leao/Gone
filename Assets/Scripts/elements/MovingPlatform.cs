using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private PlatformPathFinder path;
    // [SerializeField] private float speed;
    [SerializeField] private int targetWaypointIndex;
    [SerializeField] private float timeBetweenWaypoints;
    [SerializeField] private bool isMovementSmooth;
    [SerializeField] private bool isRotationSmooth;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeBetweenWaypoints;

        float smoothenedElapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

        transform.position = MoveToTargetPosition(isMovementSmooth ? smoothenedElapsedPercentage : elapsedPercentage);
        transform.rotation = RotateToTargetRotation(isRotationSmooth ? smoothenedElapsedPercentage : elapsedPercentage);

        if (elapsedPercentage >= 1)
            TargetNextWaypoint();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = path.getWaypoint(targetWaypointIndex);
        targetWaypointIndex = path.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = path.getWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        // float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        // timeToWaypoint = distanceToWaypoint / speed;
    }

    private Vector3 MoveToTargetPosition(float percentage)
    {
        Vector3 res = transform.position;

        if (res != targetWaypoint.position)
            res = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, percentage);

        return res;
    }

    private Quaternion RotateToTargetRotation(float percentage)
    {
        Quaternion res = transform.rotation;

        if (res != targetWaypoint.rotation)
            res = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, percentage);

        return res;
    }
}
