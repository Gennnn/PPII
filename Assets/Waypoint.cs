using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Waypoint")]
public class Waypoint : ScriptableObject
{
    public Vector3 position;
    public Vector3 dispersion;
    public WaypointCategory category;

    public Vector3 GetLocation()
    {
        return new Vector3(UnityEngine.Random.Range(position.x - dispersion.x, position.x + dispersion.x), position.y, UnityEngine.Random.Range(position.z - dispersion.z, position.z + dispersion.z));
    }
}
