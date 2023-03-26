using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }

    public bool ClickedRightMouse { get; private set; }
    public bool ClickedLeftMouse { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        ClickedLeftMouse = Input.GetMouseButtonDown(0);
        ClickedRightMouse = Input.GetMouseButtonDown(1);
    }
}
