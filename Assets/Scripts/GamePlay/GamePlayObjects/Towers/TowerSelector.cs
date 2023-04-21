using System;
using UnityEngine;

/// <summary>
/// Handles selecting and deselecting a tower in the game.
/// </summary>
public class TowerSelector : MonoBehaviour
{
    [SerializeField] GameObject _selectedIcon;
    
    private bool _selected;

    /// <summary>
    /// Event that is triggered when a tower wants to be updated.
    /// </summary>
    public static event Action<TowerSelector> OnTowerWantsUpdate;

    /// <summary>
    /// Event that is triggered when a tower does not want to be updated.
    /// </summary>
    public static event Action OnNoUpdateNeeded;

    /// <summary>
    /// Called when the tower is selected or deselected.
    /// </summary>
    public void Selected()
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (!_selected) OnTowerWantsUpdate?.Invoke(this);
        else OnNoUpdateNeeded?.Invoke();

        _selected = !_selected;
        
        if (_selectedIcon != null) _selectedIcon.SetActive(_selected);
    }

    /// <summary>
    /// Checks whether the tower has been clicked on.
    /// </summary>
    public void CheckSelect()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                Selected();
                return;
            }
        }
    }
}
