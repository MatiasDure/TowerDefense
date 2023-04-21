using UnityEngine;

/// <summary>
/// This class keeps track of the user's input.
/// </summary>
/// <remarks> Can be expanded to incorporate more complex cases i.e combination of multiple inputs </remarks>
public class InputManager : Singleton<InputManager>
{
    /// <summary>
    /// Gets whether the right click was pressed during the current frame
    /// </summary>
    public bool ClickedRightMouse { get; private set; }

    /// <summary>
    /// Gets whether the left click was pressed during the current frame
    /// </summary>
    public bool ClickedLeftMouse { get; private set; }

    protected override void Awake() => base.Awake();

    void Update()
    {
        ClickedLeftMouse = Input.GetMouseButtonDown(0);
        ClickedRightMouse = Input.GetMouseButtonDown(1);
    }
}
