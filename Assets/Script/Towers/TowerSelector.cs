using System;
using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    [SerializeField] GameObject selectedIcon;
    private bool selected;

    public static event Action<TowerSelector> OnTowerWantsUpdate;
    public static event Action OnNoUpdateNeeded;

    private void Awake()
    {
        if (!selectedIcon) Debug.LogWarning("The exclamation icon gameobject was not passed to the TowerManager script!");
    }

    public void Selected()
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (!selected) OnTowerWantsUpdate?.Invoke(this);
        else OnNoUpdateNeeded?.Invoke();
        selected = !selected;
        if (selectedIcon != null) selectedIcon.SetActive(selected);
    }

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
