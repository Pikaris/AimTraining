using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    PlayerInputAction inputAction;

    Transform optionPanel;

    Slider sliderX;
    Slider sliderY;

    Player player;

    bool isDisplayingOption = false;


    private void Awake()
    {
        inputAction = new PlayerInputAction();

        Transform child = transform.GetChild(2);        // OptionPause
        optionPanel = child.GetComponent<Transform>();

        child = transform.GetChild(2);
        child = child.GetChild(0);                      // SensitivityX
        sliderX = child.GetComponent<Slider>();

        child = transform.GetChild(2);
        child = child.GetChild(1);                      // SensitivityY
        sliderY = child.GetComponent<Slider>();       

        player = FindAnyObjectByType<Player>();
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
        if (isDisplayingOption == false)
        {
            player.OnOption(true);
            optionPanel.gameObject.SetActive(true);
            isDisplayingOption = true;
            ChangeSensitivity();
        }
        else
        {
            player.OnOption(false);
            optionPanel.gameObject.SetActive(false);
            isDisplayingOption = false;
        }
    }

    private void ChangeSensitivity()
    {
        sliderX.value = player.SensitivityX / player.MaxSensitivityX;
        sliderY.value = player.SensitivityY / player.MaxSensitivityY;

        player.SensitivityX = sliderX.value * player.MaxSensitivityX;
        player.SensitivityY = sliderY.value * player.MaxSensitivityY;
    }
}
