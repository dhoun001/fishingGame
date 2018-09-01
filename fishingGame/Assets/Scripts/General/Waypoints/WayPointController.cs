using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls group of way points, allowing gameobjects to move between them
/// Assumes all children are waypoints.
/// You can't change waypoints durign runtime
/// </summary>
public class WayPointController : MonoBehaviour
{
    /// <summary>
    /// How long to spend when reaching a waypoint
    /// </summary>
    [SerializeField]
    private float stayAtWaypointDuration = 1f;

    [Space(10)]

    [SerializeField]
    private bool useStartingPositionAsWayPoint = false;

    [SerializeField]
    private bool loopMovement = true;

    private Vector2[] wayPointPositions;
    private int wayPointIndex = 0;
    private Vector2 nextWayPoint;
    private int numbWaypoints = 0;
    private void Awake()
    {
        numbWaypoints = transform.childCount;
        if (useStartingPositionAsWayPoint)
            numbWaypoints++;

        if (numbWaypoints == 0)
            return;

        //Some bs for getting vector positions
        wayPointPositions = new Vector2[numbWaypoints];
        for(int i = 0; i < numbWaypoints; ++i)
        {
            if (i == 0 && useStartingPositionAsWayPoint)
                wayPointPositions[i] = transform.position;
            else if (!useStartingPositionAsWayPoint)
                wayPointPositions[i] = transform.GetChild(i).position;
            else
                wayPointPositions[i] = transform.GetChild(i - 1).position;
        }

        nextWayPoint = wayPointPositions[wayPointIndex];
    }

    private void OnDisable()
    {
        wayPointIndex = 0;
        nextWayPoint = wayPointPositions[wayPointIndex];
    }

    public void BeginTravel(float speed, MovementController movement)
    {
        if (numbWaypoints == 0)
            return;

        StartCoroutine(Traveling(speed, movement));
    }

    public void StopTraveling()
    {
        StopAllCoroutines();
    }

    private IEnumerator Traveling(float speed, MovementController movement)
    {
        do
        {
            //Go to next waypoint (approx. equal)
            while (Vector2.Distance(movement.transform.position, nextWayPoint) > 0.01f)
            {
                movement.MoveTowardsPosition(nextWayPoint, speed);
                yield return null;
            }

            //Get next waypoint
            wayPointIndex++;
            if (wayPointIndex >= numbWaypoints)
            {
                if (!loopMovement)
                    break;

                wayPointIndex = 0;
            }
            nextWayPoint = wayPointPositions[wayPointIndex];

            //Wait
            yield return new WaitForSeconds(stayAtWaypointDuration);

        } while (loopMovement);
    }


}
