using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Option : MonoBehaviour
{
    PlayerInputAction inputAction;

    Transform optionPanel;

    Player player;

    public Action DisplayOption;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        //optionPanel = transform.GetChild(2).GetComponent<Transform>();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Option.performed += OnOption;
    }

    private void OnDisable()
    {
        inputAction.UI.Option.canceled -= OnOption;
        inputAction.UI.Disable();
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }


    private void OnOption(InputAction.CallbackContext _)
    {
        Display();
    }

    void Display()
    {
        Debug.Log("HI");
        DisplayOption?.Invoke();
        gameObject.SetActive(true);
    }
}
