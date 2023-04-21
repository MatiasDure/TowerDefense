using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Component that allows toggling a UI object's visibility by mouse hovering.
/// </summary>
public class ToggleObjUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _uiObj;

    /// <summary>
    /// Called when the mouse pointer enters the object's bounding box.
    /// Displays the UI object.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData) => _uiObj.SetActive(true);

    /// <summary>
    /// Called when the mouse pointer exits the object's bounding box.
    /// Hides the UI object.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData) => _uiObj.SetActive(false);

    /// <summary>
    /// Manually hides the UI object.
    /// </summary>
    public void ManuallyDisableElement() => _uiObj.SetActive(false);
}
