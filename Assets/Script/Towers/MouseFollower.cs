using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private bool followMouse;

    private void Start() => followMouse = true;

    private void Update()
    {
        if (followMouse) FollowMouse();
    }

    private void FollowMouse()
    {
        Cursor.visible = false;
        gameObject.transform.position = GetMousePosition();
    }

    //Gets mouse position in the world
    private Vector3 GetMousePosition()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        pos = Camera.main.ScreenToWorldPoint(pos);
        return new Vector3(pos.x, .5f, pos.z);
    }

    public void UpdateFollowingMouse(bool followMouse) => this.followMouse = followMouse;
}
