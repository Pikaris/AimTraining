using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    int hitCount = 0;

    PlayerInputAction inputAction;

    Transform optionPanel;

    Slider sliderX;
    Slider sliderY;

    TextMeshPro sliderX_InputText;
    TextMeshPro sliderY_InputText;

    TextMeshPro sliderX_OutputText;
    TextMeshPro sliderY_OutputText;

    TextMeshPro hitCountText;

    Player player;

    bool isDisplayingOption = false;

    float SliderX_Value
    {
        get { return sliderX.value; }
        set { sliderX.value = value; }
    }

    float SliderY_Value
    {
        get { return sliderY.value ; }
        set { sliderY.value = value; }
    }



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

        child = transform.GetChild(3);
        hitCountText = child.GetComponent<TextMeshPro>();

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

    private void HitCountText()
    {
        player.onHit += () =>
        {
            hitCount++;
            //Debug.Log(hitCount);
            //hitCountText.text = hitCount.ToString();
        };
    }


    private void OnOption(InputAction.CallbackContext _)
    {
        if (isDisplayingOption == false)
        {
            player.OnOption(true);
            optionPanel.gameObject.SetActive(true);
            isDisplayingOption = true;
            
            sliderX.value = player.SensitivityX / player.MaxSensitivityX;
            sliderY.value = player.SensitivityY / player.MaxSensitivityY;
            sliderX.onValueChanged.AddListener(delegate { ChangeXSensitivity(); });
            sliderY.onValueChanged.AddListener(delegate { ChangeYSensitivity(); });
        }
        else
        {
            player.OnOption(false);
            optionPanel.gameObject.SetActive(false);
            isDisplayingOption = false;
        }
    }

    

    private void ChangeXSensitivity()
    {
        player.SensitivityX = sliderX.value * player.MaxSensitivityX;
    }

    private void ChangeYSensitivity()
    {
        player.SensitivityY = sliderY.value * player.MaxSensitivityY;
    }
}
