using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    public float maxTimer = 60.0f;

    PlayerInputAction inputAction;

    Transform optionPanel;

    Slider sliderX;
    Slider sliderY;

    TextMeshPro sliderX_InputText;
    TextMeshPro sliderY_InputText;

    TextMeshPro sliderX_OutputText;
    TextMeshPro sliderY_OutputText;

    TextMeshProUGUI fireMode;
    TextMeshProUGUI timer;

    Player player;

    Restart restart;

    float currentTimer;

    float pauseTimer;

    bool isDisplayingOption = false;

    public Action onEndTimer;

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
            
        Transform child = transform.GetChild(2);            // OptionPause
        optionPanel = child.GetComponent<Transform>();

        child = transform.GetChild(2);
        child = child.GetChild(0);                          // SensitivityX
        sliderX = child.GetComponent<Slider>();

        child = transform.GetChild(2);
        child = child.GetChild(1);                          // SensitivityY
        sliderY = child.GetComponent<Slider>();

        child = transform.GetChild(2);
        child = child.GetChild(3);
        restart = child.GetComponent<Restart>();            // Restart

        child = transform.GetChild(4);
        fireMode = child.GetComponent<TextMeshProUGUI>();   // FireMode(Auto Fire, Single Fire)

        child = transform.GetChild(5);
        timer = child.GetComponent<TextMeshProUGUI>();      // Timer

        player = FindAnyObjectByType<Player>();             // Player
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

        player.onFireMode += DisplayFireMode;

        currentTimer = maxTimer;

        restart.onTurnOffOption += TurnOff;
    }

    private void Update()
    {
        if (isDisplayingOption)
        {
            DisplayTimer();
        }
        else
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                DisplayTimer();
            }
            else
            {
                currentTimer = 0;
                DisplayTimer();
                onEndTimer?.Invoke();
            }
        }
    }

    public void Initialize()
    {
        currentTimer = maxTimer;
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

    void DisplayFireMode(bool isAutoFire)
    {
        if(isAutoFire)
        {
            fireMode.text = "Auto";
        }
        else
        {
            fireMode.text = "Single";
        }
    }

    void DisplayTimer()
    {
        timer.text = $"Remain Time : {currentTimer:f1}";
    }

    void TurnOff()
    {
        optionPanel.gameObject.SetActive(false);
        player.OnOption(false);
        isDisplayingOption = false;
    }
}
