using UnityEngine;

/// <summary>
/// This class allows an object to follow a series of waypoints defined by the Waypoints class.
/// </summary>
public class WaypointFollower : MonoBehaviour
{
    private const float DISTANCE_LENIENCY = 0.5f;

    [SerializeField] private float _startingSpeed;
    
    private int _currentIndex;
    private float _speed;

    /// <summary>
    /// Gets or sets the speed of the game object following the waypoint system.
    /// </summary>
    public float Speed 
    {
        get => _speed; 
        set => _speed = Mathf.Max(0, value); 
    }

    private void OnEnable() => ResetFollower();

    private void Update()
    {
        if (ReachedFinalPoint()) return;

        MoveTowardsWaypoint();
    }

    /// <summary>
    /// Check for whether the game object has reached the final point of the waypoint system
    /// </summary>
    /// <returns> True if the game object reached the final point, false otherwise </returns>
    private bool ReachedFinalPoint() => _currentIndex == Waypoints.Instance.Points.Length;

    /// <summary>
    /// Move the game object towards the current point
    /// </summary>
    private void MoveTowardsWaypoint()
    {
        Vector3 direction = FindDirection();
        float distance = direction.magnitude;

        if (CanMoveTowardsPoint(distance))
        {
            this.transform.position += direction.normalized * Speed * Time.deltaTime;
            return;
        }

        _currentIndex++;
    }

    /// <summary>
    /// Check for whether there is still some distance between the game object and the point in question
    /// </summary>
    /// <param name="distance"> The current distance between the game object and the point in question </param>
    /// <returns> True if there is still some more distance to move, false otherwise </returns>
    private static bool CanMoveTowardsPoint(float distance) => distance > DISTANCE_LENIENCY;

    /// <summary>
    /// Finds the direction from the game object to the current point its moving towards
    /// </summary>
    /// <returns> A vector3 representing the direction to move towards to reach the point in question </returns>
    private Vector3 FindDirection() => Waypoints.Instance.Points[_currentIndex].position - this.transform.position;

    /// <summary>
    /// Resets the follower's speed and waypoint index to the starting values.
    /// </summary>
    private void ResetFollower()
    {
        ResetSpeed();
        _currentIndex = 0;
    }

    /// <summary>
    /// Resets the speed to the initial starting speed passed through the inspector
    /// </summary>
    private void ResetSpeed() => _speed = _startingSpeed;
}
