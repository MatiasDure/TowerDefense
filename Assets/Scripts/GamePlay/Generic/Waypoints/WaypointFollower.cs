using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private float _startingSpeed;
    
    private int currentIndex;

    public float Speed { get; private set; }
    public float StartingSpeed => _startingSpeed;    

    private void Update()
    {
        //return if no waypoints passed or object reached final waypoint
        if (currentIndex == Waypoints.Instance.Points.Length) return;

        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        Vector3 direction = FindDirection();
        float distance = direction.magnitude;
        if(distance > 0.5f)
        {
            this.transform.position += direction.normalized * Speed * Time.deltaTime; 
        }
        else currentIndex++;
    }

    private Vector3 FindDirection() => Waypoints.Instance.Points[currentIndex].position - this.transform.position;

    private void ResetFollower()
    {
        ResetSpeed();
        currentIndex = 0;
    }

    private void OnEnable() => ResetFollower();

    void ResetSpeed() => SetSpeed(StartingSpeed);

    public void SetSpeed(float value) => this.Speed = value > 0 ? value : 0;

}
