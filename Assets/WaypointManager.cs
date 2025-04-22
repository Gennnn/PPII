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

    public List<Waypoint> inputWaypoints = new List<Waypoint>();

    public Dictionary<WaypointCategory, List<Waypoint>> waypoints = new Dictionary<WaypointCategory, List<Waypoint>>();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach (Waypoint w in inputWaypoints)
        {
            if (!waypoints.ContainsKey(w.category))
            {
                waypoints.Add(w.category, new List<Waypoint> { w });
            } else
            {
                waypoints[w.category].Add(w);
            }
        }
    }

    public List<Waypoint> RetrieveWaypoints(WaypointCategory category)
    {
        return waypoints[category];
    }

    public Waypoint RetrieveNearestWaypoint(WaypointCategory category, Vector3 position, float maxRange = 0.0f, string layerMask = "Player") {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        } else if (cWaypoints.Count == 1)
        {
            if (maxRange <= 0) { return cWaypoints[0]; }
            if (ValidRaycast(cWaypoints[0].position, position, maxRange, layerMask)) { return cWaypoints[0]; }
            return null;
        } else
        {
            Waypoint nearestWaypoint = null;
            for (int i = 0; i < cWaypoints.Count; i++)
            {
                if (maxRange <= 0) { nearestWaypoint = cWaypoints[i]; break; }
                if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { nearestWaypoint = cWaypoints[i]; break; }
            }
            if (nearestWaypoint == null) { return null; }
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) < Vector3.Distance(position, nearestWaypoint.position))
                {
                    if (maxRange <= 0) { nearestWaypoint = cWaypoints[i]; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { nearestWaypoint = cWaypoints[i]; }
                }
            }
            return nearestWaypoint;
        }
    }

    public Waypoint RetrieveFurthestWaypoint(WaypointCategory category, Vector3 position, float maxRange = 0.0f, string layerMask = "Player")
    {
        List<Waypoint> cWaypoints = waypoints[category];
        if (cWaypoints == null || cWaypoints.Count == 0)
        {
            return null;
        }
        else if (cWaypoints.Count == 1)
        {
            if (maxRange <= 0) { return cWaypoints[0]; }
            if (ValidRaycast(cWaypoints[0].position, position, maxRange, layerMask)) { return cWaypoints[0]; }
            return null;
        }
        else
        {
            Waypoint furthestWaypoint = null;
            for (int i = 0; i < cWaypoints.Count; i++)
            {
                if (maxRange <= 0) { furthestWaypoint = cWaypoints[i]; break; }
                if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { furthestWaypoint = cWaypoints[i]; break; }
            }
            if (furthestWaypoint == null)
            {
                return null;
            }
            for (int i = 1; i < cWaypoints.Count; i++)
            {
                if (Vector3.Distance(position, cWaypoints[i].position) > Vector3.Distance(position, furthestWaypoint.position))
                {
                    if (maxRange <= 0) { furthestWaypoint = cWaypoints[i]; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { furthestWaypoint = cWaypoints[i]; }
                    
                }
            }
            return furthestWaypoint;
        }
    }

    public Waypoint RetrieveFurthestWaypointInRange(WaypointCategory category, Vector3 position, float maxDistance, float maxRange = 0.0f, string layerMask = "Player")
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
                if (maxRange <= 0) { return cWaypoints[0]; }
                if (ValidRaycast(cWaypoints[0].position, position, maxRange, layerMask)) { return cWaypoints[0]; }
                
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
                    if (maxRange <= 0) { furthestWaypoint = cWaypoints[i]; break; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { furthestWaypoint = cWaypoints[i];  break; }                    
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
                    if (maxRange <= 0) { furthestWaypoint = cWaypoints[i]; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { furthestWaypoint = cWaypoints[i]; }
                }
            }
            return furthestWaypoint;
        }
    }

    public Waypoint RetrieveNearestWaypointWithMin(WaypointCategory category, Vector3 position, float minDistance, float maxRange = 0.0f, string layerMask = "Player")
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
                if (maxRange <= 0) { return cWaypoints[0]; }
                if (ValidRaycast(cWaypoints[0].position, position, maxRange, layerMask)) { return cWaypoints[0]; }
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
                    if (maxRange <= 0) { nearestWaypoint = cWaypoints[i]; break; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { nearestWaypoint = cWaypoints[i]; break; }
                    
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
                    if (maxRange <= 0) { nearestWaypoint = cWaypoints[i]; }
                    if (ValidRaycast(cWaypoints[i].position, position, maxRange, layerMask)) { nearestWaypoint = cWaypoints[i]; }
                    
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

    bool ValidRaycast(Vector3 waypointPosition, Vector3 targetPosition, float maxRange, string layerMask)
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask(layerMask);
        Vector3 rayDir = (targetPosition - waypointPosition).normalized;
        //Debug.DrawRay(transform.position, rayDir * shootRange, Color.red, 1.0f);
        Vector3 castPoint = new Vector3(waypointPosition.x, waypointPosition.y + 3, waypointPosition.z);
        if (Physics.Raycast(castPoint, rayDir, out hit, maxRange, mask))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
