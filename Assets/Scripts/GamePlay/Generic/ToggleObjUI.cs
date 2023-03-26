using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleObjUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject uiObj;

    public void OnPointerEnter(PointerEventData eventData) => uiObj.SetActive(true);

    public void OnPointerExit(PointerEventData eventData) => uiObj.SetActive(false);

    public void ManuallyDisableElement() => uiObj.SetActive(false);

    public void ManuallyEnableElement() => uiObj.SetActive(true);
}
