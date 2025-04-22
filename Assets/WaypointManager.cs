using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum WaypointCategory
{
    THROWER
}
public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;
    public Dictionary<WaypointCategory, List<Waypoint>> waypoints = new Dictionary<WaypointCategory, List<Waypoint>>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach (Transform child in  transform)
        {
            if (child.GetComponent<Waypoint>() != null)
            {
                if (TryGetComponent<Waypoint>(out Waypoint waypoint))
                {
                    if (!waypoints.ContainsKey(waypoint.category))
                    {
                        waypoints.Add(waypoint.category, new List<Waypoint> { waypoint});
                    } else
                    {
                        waypoints[waypoint.category].Add(waypoint);
                    }
                }
            }
        }
    }

    public List<Waypoint> RetrieveWaypoints(WaypointCategory category)
    {
        return waypoints[category];
    }

    public Waypoint RetrieveNearestWaypoint(WaypointCategory category, Vector3 position) {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        } else if (cWaypoints.Count == 1)
        {
            return cWaypoints[0];
        } else
        {
            Waypoint nearestWaypoint = cWaypoints[0];
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) < Vector3.Distance(position, nearestWaypoint.position))
                {
                    nearestWaypoint = cWaypoints[i];
                }
            }
            return nearestWaypoint;
        }
    }

    public Waypoint RetrieveFurthestWaypoint(WaypointCategory category, Vector3 position)
    {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        }
        else if (cWaypoints.Count == 1)
        {
            return cWaypoints[0];
        }
        else
        {
            Waypoint furthestWaypoint = cWaypoints[0];
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) > Vector3.Distance(position, furthestWaypoint.position))
                {
                    furthestWaypoint = cWaypoints[i];
                }
            }
            return furthestWaypoint;
        }
    }

    public Waypoint RetrieveFurthestWaypointInRange(WaypointCategory category, Vector3 position, float maxDistance)
    {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        }
        else if (cWaypoints.Count == 1)
        {
            if (Vector3.Distance(position, cWaypoints[0].position) <= maxDistance)
            {
                return cWaypoints[0];
            }
            return null;
        }
        else
        {
            Waypoint furthestWaypoint = null;
            for (int i = 0; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) <= maxDistance)
                {
                    furthestWaypoint = cWaypoints[i];
                    break;
                }
            }
            if (furthestWaypoint == null)
            {
                return null;
            }
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) > Vector3.Distance(position, furthestWaypoint.position) && Vector3.Distance(position, cWaypoints[i].position) <= maxDistance)
                {
                    furthestWaypoint = cWaypoints[i];
                }
            }
            return furthestWaypoint;
        }
    }

    public Waypoint RetrieveNearestWaypointWithMin(WaypointCategory category, Vector3 position, float minDistance)
    {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        }
        else if (cWaypoints.Count == 1)
        {
            if (Vector3.Distance(position, cWaypoints[0].position) >= minDistance)
            {
                return cWaypoints[0];
            }
            return null;
        }
        else
        {
            Waypoint nearestWaypoint = null;
            for (int i = 0; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) >= minDistance)
                {
                    nearestWaypoint = cWaypoints[i];
                    break;
                }
            }
            if (nearestWaypoint == null)
            {
                return null;
            }
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) < Vector3.Distance(position, nearestWaypoint.position) && Vector3.Distance(position, cWaypoints[i].position) >= minDistance)
                {
                    nearestWaypoint = cWaypoints[i];
                }
            }
            return nearestWaypoint;
        }
    }

    public Waypoint GetWaypointPercentile(WaypointCategory category, Vector3 position, float percentile)
    {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        }
        percentile = Mathf.Clamp01(percentile);
        List<Waypoint> sortedWaypoints = cWaypoints.OrderBy(waypoint => Vector3.Distance(position, waypoint.position)).ToList();
        int index = Mathf.FloorToInt(percentile * (sortedWaypoints.Count - 1));

        return sortedWaypoints[index];
    }
}
