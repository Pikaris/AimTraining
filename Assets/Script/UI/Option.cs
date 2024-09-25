using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Option : MonoBehaviour
{
    PlayerInputAction inputAction;

    Transform optionPanel;

    public Action DisplayOption; 

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        Transform child = transform.GetChild(2);
        optionPanel = child.GetComponent<Transform>();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.UI.Option.performed += OnOption;
    }

    private void OnDisable()
    {
        inputAction.UI.Option.canceled -= OnOption;
        inputAction.Disable();
    }
    private void OnOption(InputAction.CallbackContext context)
    {
        DisplayOption?.Invoke();
        optionPanel.gameObject.SetActive(true);
    }
}
