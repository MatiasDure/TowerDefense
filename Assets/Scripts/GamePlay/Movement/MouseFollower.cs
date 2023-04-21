using UnityEngine;

/// <summary>
/// Follows the mouse cursor in the world space.
/// </summary>
public class MouseFollower : MonoBehaviour
{
    private bool _followMouse;

    private void Start() => _followMouse = true;

    private void Update()
    {
        if (_followMouse) FollowMouse();
    }

    /// <summary>
    /// Positions the game object to the current mouse position in the world.
    /// </summary>
    private void FollowMouse()
    {
        Cursor.visible = false;
        gameObject.transform.position = GetMousePosition();
    }

    /// <summary>
    /// Returns the mouse position in the world space.
    /// </summary>
    /// <returns> The mouse position in the world space. </returns>
    private Vector3 GetMousePosition()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        pos = Camera.main.ScreenToWorldPoint(pos);
        return new Vector3(pos.x, .5f, pos.z);
    }

    /// <summary>
    /// Updates whether or not the game object should follow the mouse cursor.
    /// </summary>
    /// <param name="followMouse">Whether or not the game object should follow the mouse cursor.</param>
    public void UpdateFollowingMouse(bool followMouse) => this._followMouse = followMouse;
}
