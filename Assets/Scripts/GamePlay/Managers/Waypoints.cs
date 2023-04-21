using UnityEngine;

/// <summary>
/// This class manages the set of waypoints that an game object follows in the game
/// </summary>
public class Waypoints : Singleton<Waypoints>
{
    [SerializeField] private Transform[] _points;

    /// <summary>
    /// Gets a list of the points that make up the waypoint path
    /// </summary>
    public Transform[] Points { get => _points; }

    protected override void Awake()
    {
        base.Awake();

        GetWaypointChildren();
    }

    /// <summary>
    /// Gets all children's tranform to add them to a list of points which creates the waypoint path
    /// </summary>
    private void GetWaypointChildren()
    {
        if (_points.Length == 0)
        {
            _points = new Transform[this.transform.childCount];
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i] = transform.GetChild(i);
            }
        }
    }

    /// <summary>
    /// Draws a line between the waypoints in the game editor to help visualize the path that enemies will follow.
    /// </summary>
    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        for (int i = Points.Length - 1; i > 0; i--)
        {
            Gizmos.DrawLine(Points[i].position, Points[i - 1].position);
        }
    }
}
