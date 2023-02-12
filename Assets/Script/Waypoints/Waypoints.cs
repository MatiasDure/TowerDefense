using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    public Transform[] Points { get => _points; }
    public static Waypoints Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if(_points.Length == 0)
        {
            _points = new Transform[this.transform.childCount];
            for(int i = 0; i < _points.Length; i++)
            {
                _points[i] = transform.GetChild(i);
            }
        }
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        for (int i = Points.Length - 1; i > 0; i--)
        {
            Gizmos.DrawLine(Points[i].position, Points[i - 1].position);
        }
    }
}
